using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

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
            try
            {
               connection.Open();

               string sql = @"
            DECLARE @TargetBrowserId INT;
            
            -- 1. Zkusíme najít ID. 
            SELECT @TargetBrowserId = BrowserId FROM BrowserOneTab WHERE BrowserName = @BrowserName;

            -- 2. KONTROLA: Pokud jsme ID nenašli, nemůžeme pokračovat.
            IF @TargetBrowserId IS NULL
            BEGIN
               -- Vyhodíme chybu, kterou chytíme v C#
               RAISERROR('Prohlížeč s názvem ""%s"" nebyl v databázi nalezen. Zkontrolujte tabulku BrowserOneTab.', 16, 1, @BrowserName);
               RETURN;
            END

            -- 3. Pokud existuje config -> UPDATE
            IF EXISTS (SELECT 1 FROM ImageRecognition WHERE BrowserId = @TargetBrowserId AND ConfigName = @ConfigName)
            BEGIN
                UPDATE ImageRecognition
                SET 
                    ScreenStartX = @ScreenStartX,
                    ScreenStartY = @ScreenStartY,
                    ScreenWidth = @ScreenWidth,
                    ScreenHeight = @ScreenHeight,
                    RecognitionStartX = @RecognitionStartX,
                    RecognitionStartY = @RecognitionStartY,
                    SampleHash = @SampleHash
                WHERE BrowserId = @TargetBrowserId AND ConfigName = @ConfigName;
            END
            -- 4. Jinak -> INSERT
            ELSE
            BEGIN
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
            END";

               using (SqlCommand command = new SqlCommand(sql, connection))
               {
                  // PŘIDÁNO .Trim() - odstraní případné mezery na začátku/konci, které způsobují neshodu
                  command.Parameters.AddWithValue("@BrowserName", browserName);

                  command.Parameters.AddWithValue("@ConfigName", configName);
                  command.Parameters.AddWithValue("@ScreenStartX", refRect.X);
                  command.Parameters.AddWithValue("@ScreenStartY", refRect.Y);
                  command.Parameters.AddWithValue("@ScreenWidth", refRect.Width);
                  command.Parameters.AddWithValue("@ScreenHeight", refRect.Height);
                  command.Parameters.AddWithValue("@RecognitionStartX", searchStart.X);
                  command.Parameters.AddWithValue("@RecognitionStartY", searchStart.Y);
                  command.Parameters.AddWithValue("@SampleHash", sampleHash);

                  command.ExecuteNonQuery();
               }
            }

            catch (SqlException ex)
            {
               // Pokud SQL vyhodí RAISERROR, zobrazí se text zde
               MessageBox.Show("Chyba DB: " + ex.Message);
            }
         }
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="selectedBrowser"></param>
      /// <param name="configName"></param>
      /// <returns></returns>
      public static (Rectangle refRect, Point searchStart, string sampleHash) LoadImageRecognitionConfig(string selectedBrowser, string configName)
      {
         using (SqlConnection connection = new SqlConnection(connectionString))
         {
            try
            {
               connection.Open();
               string sql = @"
                SELECT 
                    ir.ScreenStartX, ir.ScreenStartY, ir.ScreenWidth, ir.ScreenHeight,
                    ir.RecognitionStartX, ir.RecognitionStartY,
                    ir.SampleHash
                FROM ImageRecognition ir
                INNER JOIN BrowserOneTab bot ON ir.BrowserId = bot.BrowserId
                WHERE bot.BrowserName = @BrowserName AND ir.ConfigName = @ConfigName";
               using (SqlCommand cmd = new SqlCommand(sql, connection))
               {
                  cmd.Parameters.AddWithValue("@BrowserName", selectedBrowser);
                  cmd.Parameters.AddWithValue("@ConfigName", configName);
                  using (SqlDataReader reader = cmd.ExecuteReader())
                  {
                     if (reader.Read())
                     {
                        Rectangle refRect = new Rectangle(
                           reader.GetInt32(0),
                           reader.GetInt32(1),
                           reader.GetInt32(2),
                           reader.GetInt32(3)
                        );
                        Point searchStart = new Point(
                           reader.GetInt32(4),
                           reader.GetInt32(5)
                        );
                        string sampleHash = reader.GetString(6);
                        return (refRect, searchStart, sampleHash);
                     }
                  }
               }
            }
            catch (SqlException ex)
            {
               MessageBox.Show("Chyba při práci s databází: " + ex.Message);
            }
         }
         return (Rectangle.Empty, Point.Empty, string.Empty);
      }

      public static string GetImageRecognitionConfigSampleHash(string selectedBrowser, string configName)
      {
         string sampleHash = null; // Výchozí hodnota, pokud se nic nenajde

         using (SqlConnection connection = new SqlConnection(connectionString))
         {
            try
            {
               connection.Open();

               string query = @"
                SELECT ir.SampleHash 
                FROM ImageRecognition ir
                INNER JOIN BrowserOneTab bot ON ir.BrowserId = bot.BrowserId
                WHERE bot.BrowserName = @BrowserName 
                  AND ir.ConfigName = @ConfigName";

               using (SqlCommand cmd = new SqlCommand(query, connection))
               {
                  cmd.Parameters.AddWithValue("@BrowserName", selectedBrowser.Trim());
                  cmd.Parameters.AddWithValue("@ConfigName", configName.Trim());

                  // ExecuteScalar vrátí první sloupec prvního řádku (nebo null)
                  object result = cmd.ExecuteScalar();

                  if (result != null && result != DBNull.Value)
                  {
                     sampleHash = result.ToString();
                  }
               }
            }
            catch (SqlException ex)
            {
               MessageBox.Show("Chyba při načítání hashe: " + ex.Message);
            }
         }

         return sampleHash;
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


      #endregion


   }
}