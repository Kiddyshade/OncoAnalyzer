using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using OncoAnalyzer.Models;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OncoAnalyzer.Services
{
    public class BiomarkerService
    {

        //private readonly IDatabaseService databaseService;
        private readonly IDbExecutor dbExecutor;


        private Dictionary<int, List<Biomarker>> biomarkerRecords = new Dictionary<int, List<Biomarker>>();
        //private DatabaseService databaseService;
        

        public BiomarkerService(IDbExecutor dbExecutor)
        {
            this.dbExecutor = dbExecutor;
        }


        public void RecordTest()
        {
            // 1. Accept Patient ID and validate
            Console.Write("Enter Patient ID: ");
            if (!int.TryParse(Console.ReadLine(), out int patientId) || patientId<=0)
            {
                Console.WriteLine("Invalid Patient ID. Please enter a valid ID.");
                return;
            }

            // 2. Validate and Accept Biomarker Data 
            Console.Write("Enter Biomarker Name (e.g., PSA): ");
            var biomarkerName = Console.ReadLine();
            Console.Write("Enter Test Value: ");
            if (string.IsNullOrWhiteSpace(biomarkerName))
            {
                Console.WriteLine("Biomarker name cannot be empty.");
                return;
            }

            // **Validate biomarker value**
            double value;
            Console.Write("Enter Test Value (positive number required): ");
            if (!double.TryParse(Console.ReadLine(),out value) || value<0)
            {
                Console.WriteLine("Invalid biomarker value. Please enter a positive number.");
                return;
            }


            // Call the overloaded method with the gathered input - for unit testing purpose
            RecordTest(biomarkerName, value, DateTime.Now, patientId);
       

        }

        // Over loaded RecordTest method for parameter input (used for testing)
        public void RecordTest(string biomarkerName, double value, DateTime testDate, int patientId)
        {

            string insertTest = @"INSERT INTO BiomarkerResults (BiomarkerName, Value, TestDate, PatientId)
                                VALUES (@BiomarkerName, @Value, @TestDate, @PatientId);";

            dbExecutor.ExecuteNonQuery(insertTest, command =>
            {
                command.Parameters.Add(new SqlParameter("@BiomarkerName", biomarkerName));
                command.Parameters.Add(new SqlParameter("@Value", value));
                command.Parameters.Add(new SqlParameter("@TestDate", testDate));
                command.Parameters.Add(new SqlParameter("@PatientId", patientId));
            }
            );

   
            Console.WriteLine($"Biomarker test '{biomarkerName}' recorded for Patient ID {patientId}.");
        }


        public void GenerateReport()
        {
            // 1. Accept Patient ID
            Console.Write("Enter Patient ID: ");
            var patientId = int.Parse(Console.ReadLine() ?? "0");

            // 2. Generate Report
            if (biomarkerRecords.TryGetValue(patientId, out var records))
            {
                Console.WriteLine($"\nReport for Patient ID {patientId}: ");
                foreach (var record in records)
                {
                    Console.WriteLine($"- {record.Name}: {record.Value} (Date: {record.TestDate})");

                }
            }
            else
            {
                Console.WriteLine($"No biomarker records found for Patient ID {patientId}.");
            }
        }
    }
}
