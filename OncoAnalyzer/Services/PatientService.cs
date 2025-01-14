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

        public void SearchPatient()
        {
            // 1. Ask user to search by ID or Name
            Console.WriteLine("Search by: 1. ID 2. Name");
            var option = Console.ReadLine();

            if (option == "1") //Search by ID
            {
                Console.WriteLine("Enter Patient ID: ");
                if (int.TryParse(Console.ReadLine(), out int id))
                {
                    var patient = patients.FirstOrDefault(p => p.Id == id);
                    if(patient != null) 
                    {
                        DisplayPatientDetails(patient);
                    }
                    else { Console.WriteLine("No patient found with the given ID."); }
                }
                else { Console.WriteLine("Invalid ID input. Please try again."); }
            }
            else if (option == "2") 
            {
                Console.WriteLine("Enter Patient Name: ");
                var name = Console.ReadLine();

                var patient = patients.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
              
                if(patient != null)
                {
                    DisplayPatientDetails(patient);
                }
                else
                {
                    Console.WriteLine("No patient found with the given name.");
                }
            }
            else
            {
                Console.WriteLine("Invalid option. Please choose 1 or 2.");
            }
        }

        private void DisplayPatientDetails(Patient patient)
        {
            // 2. Display the patient's details
            Console.WriteLine("\nPatient Details: ");
            Console.WriteLine($"ID: {patient.Id}");
            Console.WriteLine($"Name: {patient.Name}");
            Console.WriteLine($"Age: {patient.Age}");
            Console.WriteLine($"Diagnosis: {patient.Diagnosis}");
        }

        
    }

   
}

