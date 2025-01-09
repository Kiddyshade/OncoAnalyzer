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
            // 1. Accept Patient ID
            Console.Write("Enter Patient ID: ");
            var patientId = int.Parse(Console.ReadLine() ?? "0");

            // 2. Accept Biomarker Data
            Console.Write("Enter Biomarker Name (e.g., PSA): ");
            var biomarkerName = Console.ReadLine();
            Console.Write("Enter Test Value: ");
            var value = double.Parse(Console.ReadLine() ?? "0");

            // 3. Record in dictionary
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
