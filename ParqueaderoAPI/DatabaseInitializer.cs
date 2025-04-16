using System;
using System.IO;
using Microsoft.Data.SqlClient; // Cambio aquí

public class DatabaseInitializer
{
    public static void InitializeDatabase()
    {
        string connectionString = "Server=localhost\\SQLEXPRESS;Database=master;Trusted_Connection=True;TrustServerCertificate=True;";
        string scriptPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scripts", "ParqueaderoDB.sql");

        if (File.Exists(scriptPath))
        {
            try
            {
                string script = File.ReadAllText(scriptPath);

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(script, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
                Console.WriteLine("✅ Base de datos creada con éxito.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error ejecutando el script: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("⚠️ No se encontró el archivo SQL.");
        }
    }
}

