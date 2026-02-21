using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneTab_Order
{
   class DB_Access
   {
      static readonly string binDir = AppDomain.CurrentDomain.BaseDirectory; // Běhový adresář (např. bin\Debug\net8.0)
      static readonly string projectDir = Directory.GetParent(binDir).Parent.Parent.Parent.FullName; // Projektová složka = 3 úrovně výš z bin\Debug\netX
      static readonly string dbFilePath = Path.Combine(projectDir, @"mssql_db.mdf");
      static readonly string connectionString = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={dbFilePath};Integrated Security=True";

      public static void ConnectionTest()
      {
         using (SqlConnection connection = new SqlConnection(connectionString))
         {
            try
            {
               connection.Open();

               string sql = "SELECT 1";
               using (SqlCommand command = new SqlCommand(sql, connection))
               {
                  object result = command.ExecuteScalar();
               }
               MessageBox.Show("DB connection is successful.");
            }
            catch (SqlException ex)
            {
               MessageBox.Show("Chyba při práci s databází: " + ex.Message);
            }
         }
      }

      public static void SaveBrowserOneTabUrl(string browserName, string oneTabUrl)
      {
         using (SqlConnection connection = new SqlConnection(connectionString))
         {
            try
            {
               connection.Open();
               string sql = @"IF EXISTS (SELECT BrowserName FROM BrowserOneTab WHERE BrowserName = @BrowserName)
                  BEGIN
                      UPDATE BrowserOneTab
                      SET OneTabUrl = @OneTabUrl
                      WHERE BrowserName = @BrowserName
                  END
                  ELSE
                  BEGIN
                      INSERT INTO BrowserOneTab (BrowserName, OneTabUrl)
                      VALUES (@BrowserName, @OneTabUrl)
                  END";

               using (SqlCommand command = new SqlCommand(sql, connection))
               {
                  command.Parameters.AddWithValue("@BrowserName", browserName);
                  command.Parameters.AddWithValue("@OneTabUrl", oneTabUrl);
                  command.ExecuteNonQuery();
               }
            }
            catch (SqlException ex)
            {
               MessageBox.Show("Chyba při práci s databází: " + ex.Message);
            }
         }
      }

      public static string LoadBrowserOneTabUrl(string browserName)
      {
         using (SqlConnection connection = new SqlConnection(connectionString))
         {
            try
            {
               connection.Open();
               string sql = @"SELECT OneTabUrl FROM BrowserOneTab WHERE BrowserName = @BrowserName";
               using (SqlCommand cmd = new SqlCommand(sql, connection))
               {
                  cmd.Parameters.AddWithValue("@BrowserName", browserName);
                  using (SqlDataReader reader = cmd.ExecuteReader())
                  {
                     while (reader.Read())
                     {
                        return reader.GetString(0);
                     }
                  }
               }
            }
            catch (SqlException ex)
            {
               MessageBox.Show("Chyba při práci s databází: " + ex.Message);
            }
         }
         return string.Empty;
      }


      #region ImageRecognition

      public static bool ConfigNameExist(string selectedBrowser, string configName)
      {
         using (SqlConnection connection = new SqlConnection(connectionString))
         {
            try
            {
               connection.Open();
               // SQL dotaz, který spojí obě tabulky a zjistí počet záznamů, 
               // které odpovídají zadanému BrowserName a ConfigName.
               string query = @"
                    SELECT COUNT(1)
                    FROM ImageRecognition ir
                    INNER JOIN BrowserOneTab bot ON ir.BrowserId = bot.BrowserId
                    WHERE bot.BrowserName = @BrowserName 
                      AND ir.ConfigName = @ConfigName";

               using (SqlCommand cmd = new SqlCommand(query, connection))
               {
                  // Použití parametrů zabrání SQL Injection a chybám se speciálními znaky
                  cmd.Parameters.AddWithValue("@BrowserName", selectedBrowser);
                  cmd.Parameters.AddWithValue("@ConfigName", configName);

                  // ExecuteScalar vrátí první buňku výsledku (počet nalezených řádků)
                  int count = (int)cmd.ExecuteScalar();

                  // Pokud je počet větší než 0, konfigurace existuje
                  return count > 0;
               }
            }
            catch (SqlException ex)
            {
               MessageBox.Show("Chyba při práci s databází: " + ex.Message);
               return false;
            }
         }
      }

      public static List<string> LoadBrowserConfigNames(string selectedBrowserName)
      {
         List<string> configNames = new List<string>();

         using (SqlConnection connection = new SqlConnection(connectionString))
         {
            try
            {
               connection.Open();

               // Spojíme tabulky, abychom získali ConfigName jen pro vybraný BrowserName
               string query = @"
                SELECT ir.ConfigName 
                FROM ImageRecognition ir
                INNER JOIN BrowserOneTab bot ON ir.BrowserId = bot.BrowserId
                WHERE bot.BrowserName = @BrowserName
                ORDER BY ir.ConfigName ASC"; // Seřadíme abecedně

               using (SqlCommand cmd = new SqlCommand(query, connection))
               {
                  cmd.Parameters.AddWithValue("@BrowserName", selectedBrowserName);

                  using (SqlDataReader reader = cmd.ExecuteReader())
                  {
                     while (reader.Read())
                     {
                        // Přečteme sloupec 0 (ConfigName) a přidáme do listu
                        if (!reader.IsDBNull(0))
                        {
                           configNames.Add(reader.GetString(0));
                        }
                     }
                  }
               }
            }
            catch (SqlException ex)
            {
               MessageBox.Show("Chyba při načítání konfigurací: " + ex.Message);
            }
         }
         return configNames;
      }

      public static void SaveImageRecognitionConfig(Rectangle refRect, Point searchStart, string sampleHash, string configName, string browserName)
      {
         using (SqlConnection connection = new SqlConnection(connectionString))
         {
            connection.Open();

            using (SqlTransaction transaction = connection.BeginTransaction())
            {
               try
               {
                  string sql = @"
                  DECLARE @TargetBrowserId INT;
                  DECLARE @RecognitionId INT;

                  -- 1. Najít BrowserId
                  SELECT @TargetBrowserId = BrowserId
                  FROM BrowserOneTab
                  WHERE BrowserName = @BrowserName;

                  IF @TargetBrowserId IS NULL
                  BEGIN
                      RAISERROR('Prohlížeč s názvem ""%s"" nebyl v databázi nalezen.', 16, 1, @BrowserName);
                      RETURN;
                  END

                  -- 2. Existuje config?
                  SELECT @RecognitionId = RecognitionId
                  FROM ImageRecognition
                  WHERE BrowserId = @TargetBrowserId
                    AND ConfigName = @ConfigName;

                  IF @RecognitionId IS NOT NULL
                  BEGIN
                      -- UPDATE
                      UPDATE ImageRecognition
                      SET 
                          ScreenStartX = @ScreenStartX,
                          ScreenStartY = @ScreenStartY,
                          ScreenWidth = @ScreenWidth,
                          ScreenHeight = @ScreenHeight,
                          RecognitionStartX = @RecognitionStartX,
                          RecognitionStartY = @RecognitionStartY
                      WHERE RecognitionId = @RecognitionId;
                  END
                  ELSE
                  BEGIN
                      -- INSERT
                      INSERT INTO ImageRecognition
                      (
                          BrowserId, ConfigName, SampleHash,
                          ScreenStartX, ScreenStartY, ScreenWidth, ScreenHeight,
                          RecognitionStartX, RecognitionStartY
                      )
                      VALUES
                      (
                          @TargetBrowserId, @ConfigName, @SampleHash,
                          @ScreenStartX, @ScreenStartY, @ScreenWidth, @ScreenHeight,
                          @RecognitionStartX, @RecognitionStartY
                      );

                      SET @RecognitionId = SCOPE_IDENTITY();
                  END

                  -- 3. Uložit SampleHash do Samples (pokud ještě není)
                  IF NOT EXISTS
                  (
                      SELECT 1
                      FROM Samples
                      WHERE RecognitionId = @RecognitionId
                        AND SampleHash = @SampleHash
                  )
                  BEGIN
                      INSERT INTO Samples (RecognitionId, SampleHash)
                      VALUES (@RecognitionId, @SampleHash);
                  END
                  ";

                  using (SqlCommand command = new SqlCommand(sql, connection, transaction))
                  {
                     command.Parameters.AddWithValue("@BrowserName", browserName.Trim());
                     command.Parameters.AddWithValue("@ConfigName", configName.Trim());
                     command.Parameters.AddWithValue("@ScreenStartX", refRect.X);
                     command.Parameters.AddWithValue("@ScreenStartY", refRect.Y);
                     command.Parameters.AddWithValue("@ScreenWidth", refRect.Width);
                     command.Parameters.AddWithValue("@ScreenHeight", refRect.Height);
                     command.Parameters.AddWithValue("@RecognitionStartX", searchStart.X);
                     command.Parameters.AddWithValue("@RecognitionStartY", searchStart.Y);
                     command.Parameters.AddWithValue("@SampleHash", sampleHash);

                     command.ExecuteNonQuery();
                  }

                  transaction.Commit();
               }
               catch (Exception ex)
               {
                  transaction.Rollback();
                  MessageBox.Show("Chyba DB: " + ex.Message);
               }
            }
         }
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="selectedBrowser"></param>
      /// <param name="configName"></param>
      /// <returns></returns>
      public static List<(Rectangle refRect, Point searchStart, string sampleHash)>
     LoadImageRecognitionConfig(string selectedBrowser, string configName)
      {
         var result = new List<(Rectangle, Point, string)>();

         using (SqlConnection connection = new SqlConnection(connectionString))
         {
            try
            {
               connection.Open();

               string sql = @"
                SELECT 
                    ir.ScreenStartX,
                    ir.ScreenStartY,
                    ir.ScreenWidth,
                    ir.ScreenHeight,
                    ir.RecognitionStartX,
                    ir.RecognitionStartY,
                    s.SampleHash
                FROM ImageRecognition ir
                INNER JOIN BrowserOneTab bot 
                    ON ir.BrowserId = bot.BrowserId
                INNER JOIN Samples s
                    ON ir.RecognitionId = s.RecognitionId
                WHERE bot.BrowserName = @BrowserName 
                  AND ir.ConfigName = @ConfigName";

               using (SqlCommand cmd = new SqlCommand(sql, connection))
               {
                  cmd.Parameters.AddWithValue("@BrowserName", selectedBrowser.Trim());
                  cmd.Parameters.AddWithValue("@ConfigName", configName.Trim());

                  using (SqlDataReader reader = cmd.ExecuteReader())
                  {
                     while (reader.Read())
                     {
                        Rectangle refRect = new Rectangle(
                            reader.GetInt32(0),
                            reader.GetInt32(1),
                            reader.GetInt32(2),
                            reader.GetInt32(3)
                        );

                        Point searchStart = new Point(
                            reader.IsDBNull(4) ? 0 : reader.GetInt32(4),
                            reader.IsDBNull(5) ? 0 : reader.GetInt32(5)
                        );

                        string sampleHash = reader.GetString(6);

                        result.Add((refRect, searchStart, sampleHash));
                     }
                  }
               }
            }
            catch (SqlException ex)
            {
               MessageBox.Show("Chyba při práci s databází: " + ex.Message);
            }
         }

         return result;
      }

      public static List<string> GetImageRecognitionConfigSampleHash(string selectedBrowser, string configName)
      {
         List<string> sampleHashes = new List<string>();

         using (SqlConnection connection = new SqlConnection(connectionString))
         {
            try
            {
               connection.Open();

               string query = @"
                SELECT s.SampleHash
                FROM ImageRecognition ir
                INNER JOIN BrowserOneTab bot 
                    ON ir.BrowserId = bot.BrowserId
                INNER JOIN Samples s
                    ON ir.RecognitionId = s.RecognitionId
                WHERE bot.BrowserName = @BrowserName
                  AND ir.ConfigName = @ConfigName";

               using (SqlCommand cmd = new SqlCommand(query, connection))
               {
                  cmd.Parameters.AddWithValue("@BrowserName", selectedBrowser.Trim());
                  cmd.Parameters.AddWithValue("@ConfigName", configName.Trim());

                  using (SqlDataReader reader = cmd.ExecuteReader())
                  {
                     while (reader.Read())
                     {
                        sampleHashes.Add(reader.GetString(0));
                     }
                  }
               }
            }
            catch (SqlException ex)
            {
               MessageBox.Show("Chyba při načítání hashů: " + ex.Message);
            }
         }

         return sampleHashes;
      }

      public static void DeleteImageRecognitionConfig(string selectedBrowser, string configName)
      {
         using (SqlConnection connection = new SqlConnection(connectionString))
         {
            try
            {
               connection.Open();
               string sql = @"
                DELETE ir
                FROM ImageRecognition ir
                INNER JOIN BrowserOneTab bot ON ir.BrowserId = bot.BrowserId
                WHERE bot.BrowserName = @BrowserName AND ir.ConfigName = @ConfigName";
               using (SqlCommand cmd = new SqlCommand(sql, connection))
               {
                  cmd.Parameters.AddWithValue("@BrowserName", selectedBrowser);
                  cmd.Parameters.AddWithValue("@ConfigName", configName);
                  cmd.ExecuteNonQuery();
               }
            }
            catch (SqlException ex)
            {
               MessageBox.Show("Chyba při práci s databází: " + ex.Message);
            }
         }
      }

      public static ImageRecognitionData LoadConfig(string configName, string browserName)
      {
         using (SqlConnection conn = new SqlConnection(connectionString))
         using (SqlCommand cmd = conn.CreateCommand())
         {
            cmd.CommandText = @"
         SELECT 
             ir.RecognitionId,
             ir.ScreenStartX,
             ir.ScreenStartY,
             ir.ScreenWidth,
             ir.ScreenHeight,
             ir.RecognitionStartX,
             ir.RecognitionStartY
         FROM ImageRecognition ir
         JOIN BrowserOneTab b ON b.BrowserId = ir.BrowserId
         WHERE ir.ConfigName = @ConfigName
           AND b.BrowserName = @BrowserName";

            cmd.Parameters.AddWithValue("@ConfigName", configName);
            cmd.Parameters.AddWithValue("@BrowserName", browserName);

            conn.Open();

            using (SqlDataReader r = cmd.ExecuteReader())
            {
               if (!r.Read())
                  throw new InvalidOperationException("Config not found.");

               int screenStartX = r.GetInt32(r.GetOrdinal("ScreenStartX"));
               int screenStartY = r.GetInt32(r.GetOrdinal("ScreenStartY"));
               int screenWidth = r.GetInt32(r.GetOrdinal("ScreenWidth"));
               int screenHeight = r.GetInt32(r.GetOrdinal("ScreenHeight"));

               int searchX = r.GetInt32(r.GetOrdinal("RecognitionStartX"));
               int searchY = r.GetInt32(r.GetOrdinal("RecognitionStartY"));

               return new ImageRecognitionData
               {
                  RecognitionId = r.GetInt32(r.GetOrdinal("RecognitionId")),

                  ScreenStart = new Point(screenStartX, screenStartY),
                  ScreenEnd = new Point(screenStartX + screenWidth, screenStartY + screenHeight),
                  SearchStartScreen = new Point(searchX, searchY)
               };
            }
         }
      }

      public static void AddSampleToConfig(string configName, string sampleHash, string browserName)
      {
         using (SqlConnection conn = new SqlConnection(connectionString))
         using (SqlCommand cmd = conn.CreateCommand())
         {
            conn.Open();

            // Najdi RecognitionId
            cmd.CommandText = @"
            SELECT ir.RecognitionId
            FROM ImageRecognition ir
            JOIN BrowserOneTab b ON b.BrowserId = ir.BrowserId
            WHERE ir.ConfigName = @ConfigName
              AND b.BrowserName = @BrowserName";

            cmd.Parameters.AddWithValue("@ConfigName", configName);
            cmd.Parameters.AddWithValue("@BrowserName", browserName);

            object result = cmd.ExecuteScalar();
            if (result == null)
               throw new InvalidOperationException("Config not found.");

            int recognitionId = (int)result;

            // Vlož sample
            cmd.Parameters.Clear();
            cmd.CommandText = @"
            INSERT INTO Samples (RecognitionId, SampleHash)
            VALUES (@RecognitionId, @SampleHash)";

            cmd.Parameters.AddWithValue("@RecognitionId", recognitionId);
            cmd.Parameters.AddWithValue("@SampleHash", sampleHash);

            cmd.ExecuteNonQuery();
         }
      }


      #endregion


   }
}