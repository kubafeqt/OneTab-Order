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
               MessageBox.Show("Connection is successful.");
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


   }
}
