using OncoAnalyzer.Models;
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
                var UserService = new UserService(dbExecutor);  // Initialize Service


                // Authenticate the user
                User currentUser = null;
                while (currentUser  == null)
                {
                    Console.WriteLine("\n Please log in: ");
                    Console.Write("Username: ");
                    string username = Console.ReadLine();
                    Console.Write("Password: ");
                    string password = Console.ReadLine();

                    currentUser = UserService.Authenticate(username, password);

                    if (currentUser == null)
                    {
                        Console.WriteLine("Invalid Login credentials. Please try again."); // Current user null
                    }
                }

                Console.WriteLine($"Welcome, {currentUser.Username}! You are logged in as {currentUser.Role}.");

                // Main menu with role-based permissions
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

                    //Add Import from CSV as an Admin-only Feature
                    if (currentUser.Role == "Admin")
                    {
                        Console.WriteLine("7. Import Patients from CSV");
                    }
                    Console.WriteLine("8. Exit");
                    Console.WriteLine("Select an option: ");



                    var input = Console.ReadLine();

                    // 3. Handle User Input
                    switch (input)
                    {
                        case "1":
                            if (currentUser.Role != "Admin")
                            {
                                Console.WriteLine("Access denied. Only Admins can add patients.");
                                break;
                            }
                            //var patientService = new PatientService(dbExecutor);
                            patientService.AddPatient();
                            break;
                        case "2":

                            if (currentUser.Role == "Staff")
                            {
                                Console.WriteLine("Access denied. Staff cannot record biomarker tests.");
                                break;
                            }
                            //var biomarkerService = new BiomarkerService(dbExecutor);
                            biomarkerService.RecordTest();
                            break;
                        case "3":
                            var patientServiceView = new PatientService(dbExecutor);
                            patientServiceView.ViewAllPatients(); // view all patient from database functionality
                            break;
                        case "4":
                            var patientServiceSearch = new PatientService(dbExecutor);
                            patientServiceSearch.SearchPatients(); // Calling advanced patient search
                            break;
                        case "5":
                            var patientServiceExportCSV = new PatientService(dbExecutor);
                            patientServiceExportCSV.ExportAllPatientstoCSV(); // Calling export to CSV functionality
                            break;
                        case "6":
                            var patientServiceExportPDF = new PatientService(dbExecutor);
                            patientServiceExportPDF.ExportAllPatientstoPDF(); // Calling export to PDF functionality
                            break;
                        case "7":

                            if (currentUser.Role == "Admin")
                            {

                                patientService.ImportFromCsv();
                            }
                            else
                            {
                                Console.WriteLine("You do not have permission to access this feature.");
                            }
                            break;

                        case "8":
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