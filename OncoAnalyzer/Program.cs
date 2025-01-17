using System;
using OncoAnalyzer.Services;// Importing Services Namespace
using Serilog; // used to log message to both the console and a file

namespace OncoAnalyzer
{
    class program
    {
        static void Main(string[] args)
        {
            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                             .WriteTo.Console()    // Logs to Console
                             .WriteTo.File(Path.Combine(AppContext.BaseDirectory,"../../../Logs/oncoanalyzer.log"),  // Logs in the Log folder in project folder 
                             rollingInterval: RollingInterval.Day, // Logs to a file(daily rolling logs)
                             retainedFileCountLimit: 7)  // Retain logs for the Last 7 days
                             .CreateLogger(); // logs can be found in "E:\Development\Language\C#\OncoAnalyzer\OncoAnalyzer\bin\Debug\net6.0\Logs"

            // Log startup message
            Log.Information("OncoAnalyzer application starting...");

            try
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
                    Console.WriteLine("4. Search Patients");  // Add this line for Advance patient search
                    Console.WriteLine("5. Export all Patients to CSV");
                    Console.WriteLine("6. Export all Patients to PDF");
                    Console.WriteLine("7. Exit");
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
                            patientService.SearchPatients(); // Calling advanced patient search
                            break;
                        case "5":
                            patientService.ExportAllPatientstoCSV(); // Calling export to CSV functionality
                            break;
                        case "6":
                            patientService.ExportAllPatientstoPDF(); // Calling export to PDF functionality
                            break;
                        case "7":
                            Log.Information("OncoAnalyzer applciation exiting...");
                            Console.WriteLine("Exiting application. Goodbye!");
                            break;

                        default:
                            Console.WriteLine("Invalid option. Please try again.");
                            break;
                    }
                }

            }
            catch (Exception ex)
            {
                // Log unhandled exceptions
                Log.Error(ex, "An unhandled exception occurred.");
            }

            finally 
            {
                // Ensure the log is flushed before application exists
                Log.CloseAndFlush();
            }
            
        }
    }
}