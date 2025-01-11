using System;
using System.Collections.Generic;
using OncoAnalyzer.Models;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OncoAnalyzer.Services
{
    public class BiomarkerService
    {
        private Dictionary<int, List<Biomarker>> biomarkerRecords = new Dictionary<int, List<Biomarker>>();

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

            // 3. Add biomarker record to dictionary
            if (!biomarkerRecords.ContainsKey(patientId))
                biomarkerRecords[patientId] = new List<Biomarker>();

            biomarkerRecords[patientId].Add(new Biomarker
            {
                Name = biomarkerName,
                Value = value,
                TestDate = DateTime.Now,
            });

            Console.WriteLine($"Recorded biomarker '{biomarkerName}' for patient ID {patientId}.");

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
