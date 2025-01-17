using System;
using System.Data.SqlClient;
using System.Reflection.PortableExecutable;
using OncoAnalyzer.Models;
using Serilog;

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
    }

   
}

