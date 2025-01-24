using System.Globalization;  // For CSV parsing Culture
using System.Data.SqlClient;
using OncoAnalyzer.Models;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using Serilog;
using CsvHelper;

namespace OncoAnalyzer.Services
{
    public class PatientService  
    {
        //private readonly IDatabaseService databaseService;
        private readonly IDbExecutor dbExecutor;

        private List<Patient> patients = new List<Patient>(); // In-memory storage

        //private DatabaseService databaseService;

        public PatientService(IDbExecutor dbExecutor)
        {
            this.dbExecutor = dbExecutor;
        }

        public void AddPatient()
        {

            try
            {
                // 0.1 Updated in Patientservice.cs
                string name;
                do
                {
                    // 1. Accept patient details
                    Console.Write("Enter Patient Name: ");
                    name = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(name))
                    {
                        Console.WriteLine("Patient name cannot be empty. Please try again.");
                    }
                } while (string.IsNullOrWhiteSpace(name));

                int age;

                do
                {
                    Console.WriteLine("Enter Age: ");

                    var ageInput = Console.ReadLine();
                    if (!int.TryParse(ageInput, out age) || age <= 0)
                    {
                        Console.WriteLine("Invalid age. Please enter a positive integer.");
                    }

                } while (age <= 0);

                string diagnosis;

                do
                {
                    Console.Write("Enter Diagnosis (required!) (e.g., Lung Cancer): ");
                    diagnosis = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(diagnosis))
                    {
                        Console.WriteLine("Diagnosis cannot be empty. Please try again.");
                    }
                } while (string.IsNullOrWhiteSpace(diagnosis));


                // Call the overloaded method with the gathered input - for unit testing purpose
                AddPatient(name, age, diagnosis);
            }
            catch (Exception ex)
            {

                Log.Error(ex, "Failed to add a new patient. ");
                Console.WriteLine("ERROR: Could not add patient. Please try again. ");
            }


        }

        // Over loaded AddPatient method for parameter input (used for testing)
        public void AddPatient(string name, int age, string diagnosis)
        {

            string insertPatient = @"INSERT INTO Patients (Name, Age, Diagnosis) 
                                                VALUES (@Name, @Age, @Diagnosis);";

            dbExecutor.ExecuteNonQuery(insertPatient, command =>
            {
                command.Parameters.Add(new SqlParameter("@Name", name));
                command.Parameters.Add(new SqlParameter("@Age", age));
                command.Parameters.Add(new SqlParameter("@Diagnosis", diagnosis));
            }
            );
            
            Console.WriteLine($"Patient '{name}' added successfully.");
        }

        // View all patients
        public void ViewAllPatients()
        {

            try
            {
                Console.WriteLine("\nList of All Patients: ");

                // Define the SQL query to fetch all patients
                string query = "SELECT * FROM Patients;";

                // Use the IDbExecutor to execute the query
                using (var reader = dbExecutor.ExecuteReader(query, null))
                {
                    // Check if there are rows
                    if (!reader.Read())  // If Read() returns false, there are no rows
                    {
                        Console.WriteLine("No Patients found");
                        return;
                    }


                    // Process the first row
                    do
                    {
                        Console.WriteLine($"ID: {reader["Id"]}, Name: {reader["Name"]}," +
                           $"Age: {reader["Age"]}, Diagnosis: {reader["Diagnosis"]}");
                    } while (reader.Read());

                    Log.Information("Displayed all patients successfully.");
                }
            }
            catch (Exception ex)
            {

                Log.Error(ex, "Failed to retrieve patient data.");
                Console.WriteLine("ERROR: Could not retrieve patients.");
            }

            
        }

        // Advanced search functionality - case insensitive to handle large dataset
        public void SearchPatients()
        {
            try
            {
                Console.WriteLine("Search patients: ");
                Console.WriteLine("Leave fields empty to skip filters.");

                //Input for filters
                Console.Write("Enter Patient Name (optional): ");
                string name = Console.ReadLine()?.Trim();

                Console.Write("Enter Minimum Age (optional): ");
                string minAgeInput = Console.ReadLine();
                int? minAge = int.TryParse(minAgeInput, out int min) ? min : null;

                Console.Write("Enter Maximum Age (optional): ");
                string maxAgeInput = Console.ReadLine();
                int? maxAge = int.TryParse(maxAgeInput, out int max) ? max : null;

                Console.Write("Enter Diagnosis (optional): ");
                string diagnosis = Console.ReadLine()?.Trim();

                // Construct the query
                string query = "SELECT * FROM Patients WHERE 1=1";
                var parameters = new List<SqlParameter>();

                if (!string.IsNullOrWhiteSpace(name))
                {
                    query += " AND LOWER(Name) LIKE LOWER(@Name)"; // case insensitive
                    parameters.Add(new SqlParameter("@Name", $"%{name}%"));
                }

                if (minAge.HasValue)
                {
                    query += " AND Age >= @MinAge";
                    parameters.Add(new SqlParameter("@MinAge", minAge.Value));
                }

                if (maxAge.HasValue)
                {
                    query += " AND age <= @MaxAge";
                    parameters.Add(new SqlParameter("@MaxAge", maxAge.Value));
                }

                if (!string.IsNullOrWhiteSpace(diagnosis))
                {
                    query += " AND LOWER(Diagnosis) LIKE LOWER(@Diagnosis)"; // case insensitive
                    parameters.Add(new SqlParameter("@Diagnosis", $"%{diagnosis}%"));
                }

                // Log the query and parameters for debugging
                Log.Debug("Executing query: {Query}", query);

                foreach (var param in parameters)
                {
                    Log.Debug("Parameter: {Name} = {Value}", param.ParameterName, param.Value);
                }

                // Execute the query

                using (var reader = dbExecutor.ExecuteReader(query, command =>
                    {
                    foreach (var param in parameters)
                    {
                        command.Parameters.Add(param);
                        
                    }
                    }))
                {
                    // Check if there are any results

                    if (!reader.Read())
                    {
                        Console.WriteLine("No patients found matching the given criteria.");
                        return;
                    }

                    // Display the first result (since Read() advances the cursor)
                    Console.WriteLine("\n Search Results: ");

                    do
                    {
                        Console.WriteLine($"ID: {reader["Id"]}, Name: {reader["Name"]}, Age: {reader["Age"]}, Diagnosis: {reader["Diagnosis"]}");

                    }
                    while (reader.Read());
                    

                    Log.Information("Advanced search completed successfully with filters: Name={Name}, MinAge={MinAge}, MaxAge={MaxAge}, Diagnosis={Diagnosis}. ",
                        name, minAge, maxAge, diagnosis);
                }
            }
            catch (Exception ex)
            {

                Log.Error(ex, "Failed to execute advanced patient search. ");
                Console.WriteLine("ERROR: Could not complete search. Please try again.");
            }
        }

        // Export to CSV file logic
        public void ExportAllPatientstoCSV()
        {
            try
            {
                // Define the SQL query to fetch all patients
                string query = "SELECT * FROM Patients";

                // Execute the query and fetch data
                var patients = new List<string>();
                using (var reader = dbExecutor.ExecuteReader(query, null))
                {

                    // check if there are any patients
                    if (!reader.Read())
                    {
                        Console.WriteLine("No patients to export");
                        return;
                    }

                    // Write CSV header
                    patients.Add("ID, Name, Age, Diagnosis");

                    // Loop through the results and add to the list

                    do
                    {
                        string row = $"{reader["Id"]},{reader["Name"]},{reader["Age"]},{reader["Diagnosis"]}";
                        patients.Add(row);
                    } while (reader.Read());
                }

                // define the file path for the CSV
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "patients.csv");

                // Write all lines to the CSV file
                File.WriteAllLines(filePath, patients);

                Console.WriteLine($"Patients exported successfully to {filePath}");
                Log.Information("Exported all patients to CSV at {FilePath}", filePath);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to export patients to CSV.");
                Console.WriteLine("ERROR: Could not export patients. Please try again");
                throw;
            }
        }

        //Export to PDF file logic

        public void ExportAllPatientstoPDF()
        {
            try
            {
                // Define the SQL queryt to fetch all patients
                string query = "SELECT * FROM Patients;";

                // Fetch data from database
                var patients = new List<(int Id, string Name, int Age, string Diagnosis)>();
                using (var reader = dbExecutor.ExecuteReader(query, null))
                {
                    if (!reader.Read())
                    {
                        Console.WriteLine("No patients to export");
                        return;
                    }

                    do
                    {
                        patients.Add((
                            Convert.ToInt32(reader["Id"]),
                            reader["Name"].ToString(),
                            Convert.ToInt32(reader["Age"]),
                            reader["Diagnosis"].ToString()
                            ));

                    } while (reader.Read());
                }

                // Create a new PDF document
                PdfDocument document = new PdfDocument();
                document.Info.Title = "Patient Data Report";

                // Create a new page
                PdfPage page = document.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);

                // Use the correct constructor for fonts in PDFSharp 6.1.1
                XFont titleFont = new XFont("Arial", 20);
                XFont contentFont = new XFont("Verdana", 12);

                // Add the title

                gfx.DrawString("Patient Data Report", titleFont, XBrushes.Black,
                    new XRect(0,0,page.Width,50), XStringFormats.TopCenter);

                // Add patient data
                int yPosition = 60; // starting position for patient data
                gfx.DrawString("ID      Name                       Age       Diagnosis", contentFont, XBrushes.Black, 20, yPosition);
                gfx.DrawLine(XPens.Black, 20, yPosition + 5, page.Width - 20, yPosition + 5); //Add a horizontal line
                yPosition += 20;

                foreach (var patient in patients)
                {
                    string patientInfo = $"{patient.Id,-5} {patient.Name,-18} {patient.Age,-6} {patient.Diagnosis}";
                    gfx.DrawString(patientInfo, contentFont, XBrushes.Black, 20, yPosition);
                    yPosition += 20;

                    // Add a new page if the content overflows the current page
                    if (yPosition > page.Height - 50)
                    {
                        page = document.AddPage();
                        gfx = XGraphics.FromPdfPage(page);
                        yPosition = 20;
                    }
                }

                // Define the file path for the PDF
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "patient.pdf");

                // Save the PDF document
                document.Save(filePath);

                Console.WriteLine($"Patients exported successfully to {filePath}");
                Log.Information("Exported all patients to PDF at {FilePath}", filePath);

            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to export patients to PDF.");
                Console.WriteLine("ERROR: Could not export patients. Please try again.");
                
            }
        }

        public void ImportFromCsv()
        {
            try
            {
                Console.WriteLine("\n Enter the full path of the CSV file to Import");
                string filePath = Console.ReadLine();

                if (!File.Exists(filePath))
                {
                    Console.WriteLine("File does not exist. Please provide a valid path.");
                    return;
                }

                //Read and parse the CSV header
                var patients = new List<Patient>();
                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    // validate the CSV header
                    if (csv.Context.Reader.HeaderRecord == null)
                    {
                        csv.Read();  // Read the first Row
                        csv.ReadHeader(); // Read the header rows
                    }


                    var csvHeader = csv.Context.Reader.HeaderRecord;

                    if(csvHeader == null || !csvHeader.Contains("Name") || !csvHeader.Contains("Age") || !csvHeader.Contains("Diagnosis"))
                    {
                        Console.WriteLine("Invalid CSV Format. Ensure the file contains the header: Name, Age, Diagnosis.");
                        return;
                    }

                    // Configure CSV mapping (if needed)
                    csv.Context.RegisterClassMap<PatientCsvMap>();

                    // Read the records
                    patients = csv.GetRecords<Patient>().ToList();
                }

                // Validate and insert each record
                foreach (var patient in patients)
                {
                    if (string.IsNullOrWhiteSpace(patient.Name) || patient.Age <=0 || string.IsNullOrWhiteSpace(patient.Diagnosis))
                    {
                        Console.WriteLine($"Skipping invalid record: {patient}");
                        continue;
                    }

                    // Insert into the database
                    string query = @"INSERT INTO Patients (Name, Age, Diagnosis) VALUES (@Name, @Age, @Diagnosis);";
                    dbExecutor.ExecuteNonQuery( query, command =>
                    {
                        command.Parameters.Add(new SqlParameter("@Name", patient.Name));
                        command.Parameters.Add(new SqlParameter("@Age", patient.Age));
                        command.Parameters.Add(new SqlParameter("@Diagnosis", patient.Diagnosis));
                    });
                    Console.WriteLine($"Imported patient: {patient.Name}, {patient.Age}, {patient.Diagnosis}");
                }

                Log.Information("CSV Imported completed successfully");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error during CSV import.");
                Console.WriteLine("An Error occured during CSV import. Please check the logs for details.");
                
            }
        }

    }

   
}

