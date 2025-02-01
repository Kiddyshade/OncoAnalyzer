using Spectre.Console; //For enhanced console UI
using OncoAnalyzer.Services;// Importing Services Namespace
using OncoAnalyzer.Models; // Import application models
using Serilog; // used to log message to both the console and a file
using OncoAnalyzer.Helpers;
using CsvHelper;

namespace OncoAnalyzer
{
    class program
    {
        static void Main(string[] args)
        {

            // Configure spectre console here
            AnsiConsole.Markup("[bold green] Welcome to OncoAnalyzer - Oncology Data Tracker[/]\n");

            // Configure Serilog for logging application events
            ConfigureLogging();

            try
            {

                // Use DbExecutor (which implements IDbExecutor) and pass the connection string
                var dbExecutor = new DbExecutor("Server=DESKTOP-GDT78B8\\SQLEXPRESS;Database=OncoAnalyzerDB;Trusted_Connection=True;");

                // Initialize application services
                var patientService = new PatientService(dbExecutor); // Initialize Service
                var biomarkerService = new BiomarkerService(dbExecutor); // Initialize service
                var UserService = new UserService(dbExecutor);  // Initialize Service


                // Authenticate the user (login functionality)
                User currentUser = AuthenticateUser(UserService);


                // Start the main application loop (menu-driven interface)
                while(true)
                {
                    //Display the main menu based on the user's role
                    var selectedOption = DisplayMainMenu(currentUser);

                    // Handle the selected menu option
                    HandleMenuSelection(selectedOption, currentUser, patientService, biomarkerService, dbExecutor);
                }

            }
            catch (Exception ex)
            {
                // handle unexpected exceptions and Log them
                AnsiConsole.MarkupLine("[bold red] An Unexpected error occured: {0}{/}", ex.Message);
                Log.Error(ex, "An unhandled exception occurred.");
            }

            finally 
            {
                // Ensure the log is flushed before application exists
                Log.CloseAndFlush();
            }
            
        }

        //<summary>
        //    configures Serilog for logging application events and errors.
        //</summary>
        static void ConfigureLogging()
        {
            Log.Logger = new LoggerConfiguration()
                             .WriteTo.Console()    // Log to Console
                             .WriteTo.File("Logs/oncoanalyzer.log", rollingInterval: RollingInterval.Day) // Log to file with daily rolling
                             .CreateLogger();

            // Log application startup
            Log.Information("OncoAnalyzer application starting...");
        }

        //<summary>
        //    Handles user authentication by prompting for username and password.
        //</summary>
        //<param name="UserService"> The Service responsible for user Authentication.</param>
        //<returns> The authenticated user. </returns>
        static User AuthenticateUser(UserService userService)
        {
            User currentUser = null;

            // Loop until the user provides valid credentials
            while (currentUser == null)
            {
                AnsiConsole.MarkupLine("[bold yellow] Please log in:[/]");
                string username = AnsiConsole.Ask<string>("[green]Username:[/]");
                string password = AnsiConsole.Prompt(
                    new TextPrompt<string> ("[green]Password:[/]")
                        .PromptStyle("red") // Display password input in red
                        .Secret()); // Mask password input

                // Authenticate the user
                currentUser = userService.Authenticate(username, password);

                if (currentUser == null)
                {
                    // Display error message for invalid credentials
                    AnsiConsole.MarkupLine("[bold red]Invalid login credentials. Please try again. [/]");
                }
            }
            // Display welcome message for the authenticated user
            AnsiConsole.MarkupLine("[bold green]Welcome, {0}! You are logged in as {1}. [/]", currentUser.Username, currentUser.Role);
            return currentUser;

        }

        //<summary>
        //    Displays the main menu based on the user's role.
        //</summary>
        //<param name="currentUser"> The Authenticated user.</param>
        //<returns> The selected menu option. </returns>
        static string DisplayMainMenu(User currentUser)
        {
            // Get role-based menu options
            var menuOptions = MenuHelper.GetMenuOptionsForRole(currentUser.Role);

            //Use Spectre.Console to display the menu and capture user selection
            return AnsiConsole.Prompt
                (
                new SelectionPrompt<string>()
                .Title("[bold yellow] Main Menu[/]")  // Menu title
                .PageSize(7) // Number of options visible at a time
                .AddChoices(menuOptions) //Add role-based menu options
                );
        }


        //<summary>
        //    Handles the menu selection based on the selected option.
        //</summary>
        //<param name="selectedOption"> The selected menu option.</param>
        //<param name="currentUser"> The Authenticated user.</param>
        //<param name="patientService"> The patient service instance.</param>
        //<param name="biomarkerService"> The biomarker service instance.</param>
        //<param name="dbExecutor"> The database executor instance.</param>
        static void HandleMenuSelection(string selectedOption, User currentUser, PatientService patientService, BiomarkerService biomarkerService, DbExecutor dbExecutor)
        {

            // Use a switch statement to handle the selected menu option
            switch (selectedOption)
            {
                case "1. Add Patient Details":
                    if (currentUser.Role == "Admin")
                    {
                        patientService.AddPatient();
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[bold red] Access denied. Only Admins can add patients.[/]");

                    }
                    break;

                case "2. Record Biomarker Test Results":
                    if (currentUser.Role == "Staff")
                    {
                        AnsiConsole.MarkupLine("[bold red] Access denied. Staff cannot record biomarker tests.[/]");
                    }
                    else
                    {
                        biomarkerService.RecordTest();
                    }
                    break;

                case "3. View all Patients":
                    patientService.ViewAllPatients();
                    break;

                case "4. Search Patients":
                    patientService.SearchPatients();
                    break;

                case "5. Export all Patients to CSV":
                    patientService.ExportAllPatientstoCSV();
                    break;

                case "6. Export all Patients to PDF":
                    patientService.ExportAllPatientstoPDF();
                    break;

                case "7. Import Patients from CSV":
                    if (currentUser.Role == "Admin")
                    {
                        patientService.ImportFromCsv();
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[bold red] Access denied. Only Admins can import patients.[/]");

                    }
                    break;

                case "8. Add Treatment for a patient":
                    if (currentUser.Role == "Admin" || currentUser.Role == "Doctor")
                    {
                        var treatmentService = new TreatmentService(dbExecutor);
                        treatmentService.AddTreatment();
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[bold red] Access denied. Only Admins and Doctors can add treatments. [/]");
                    }
                    break;

                case "9. View Treatments for a Patient":
                    AnsiConsole.Markup("[green]Enter Patient ID to view treatments:[/]");
                    if (int.TryParse(Console.ReadLine(), out int patientId))
                    {
                        var treatmentService = new TreatmentService(dbExecutor);
                        treatmentService.ViewTreatments(patientId);
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[bold red]Invalid Patient ID.[/]");
                    }
                    break;

                case "10. Exit":
                    Log.Information("OncoAnalyzer application exiting...");
                    AnsiConsole.MarkupLine("[bold red]Exiting application. Goodbye![/]");
                    Environment.Exit(0);
                    break;

                default:
                    AnsiConsole.MarkupLine("[bold red]Invalid option. Please try again.[/]");
                    break;
            }
        }

    }
}