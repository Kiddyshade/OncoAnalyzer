using System;
using OncoAnalyzer.Services;// Importing Services Namespace

namespace OncoAnalyzer
{
    class program
    {
        static void Main(string[] args)
        {
            //1. Entry point of the program
            Console.WriteLine("Welcome to OncoAnalyzer - Oncology Data Tracker");
            var patientService = new PatientService(); // Initialize Service
            var biomarkerService = new BiomarkerService(); // Initialize service

            while (true)
            {
                // 2. Display Menu
                Console.WriteLine("\nMain Menu");
                Console.WriteLine("1. Add Patient Details");
                Console.WriteLine("2. Record Biomarker Test Results");
                Console.WriteLine("3. Generate Patient Report");
                Console.WriteLine("4. Search for a Patient");
                Console.WriteLine("5. Exit");
                Console.WriteLine("Select an option: ");

                var input = Console.ReadLine();

                // 3. Handle User Input
                switch (input)
                {
                    case "1":
                        patientService.AddPatient();
                    break;
                     case "2":
                     biomarkerService.RecordTest();
                     break;
                     case "3":
                     biomarkerService.GenerateReport();
                     break;
                     case "4":
                        patientService.SearchPatient(); // Patient Search functionality
                     break;
                     case "5":
                     Console.WriteLine("Exiting application. Goodbye!");
                     break;
                    default: Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }
    }
}