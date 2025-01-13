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

            // Use DbExecutor (which implements IDbExecutor) and pass the connection string
            var dbExecutor = new DbExecutor("Server=DESKTOP-GDT78B8\\SQLEXPRESS;Database=OncoAnalyzerDB;Trusted_Connection=True;");

            //var databaseService = new DatabaseService();
            var patientService = new PatientService(dbExecutor); // Initialize Service
            var biomarkerService = new BiomarkerService(dbExecutor); // Initialize service

            while (true)
            {
                // 2. Display Menu
                Console.WriteLine("\nMain Menu");
                Console.WriteLine("1. Add Patient Details");
                Console.WriteLine("2. Record Biomarker Test Results");
                Console.WriteLine("3. View all Patients");
                Console.WriteLine("4. Exit");
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
                        patientService.ViewAllPatients(); // view all patient from database functionality
                        break;
                     case "4":
                        Console.WriteLine("Exiting application. Goodbye!");
                        break;
                   
                    default: Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }
    }
}