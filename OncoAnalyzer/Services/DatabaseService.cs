using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OncoAnalyzer.Services
{
    public class DatabaseService : IDatabaseService // interherting this interface service for mock testing purpose
    {
        // Connection string for the MS SQL Server database
        private readonly string connectionString;

       
        // Test database connection when the service is initialized
        public DatabaseService() 
        {
            // Update this with your actual connection string
            connectionString = "Server=DESKTOP-GDT78B8\\SQLEXPRESS;Database=OncoAnalyzerDB;Trusted_Connection=True;";
            TestDatabaseConnection();
        }

        private void TestDatabaseConnection()
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    Console.WriteLine("Connected to MS SQL Server database successfully!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to connect to the database.");
                Console.WriteLine($"Error: {ex.Message}");
                throw; // Re-throw exeception for debugging
            }
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
