using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using OncoAnalyzer.Models;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OncoAnalyzer.Services
{
    public class PatientService
    {
        private List<Patient> patients = new List<Patient>(); // In-memory storage

        private DatabaseService databaseService;

        public PatientService(DatabaseService dbService)
        {
            databaseService = dbService;
        }

        public void AddPatient()
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
                if (!int.TryParse(ageInput,out age) || age<=0)
                {
                    Console.WriteLine("Invalid age. Please enter a positive integer.");
                }

            } while (age<=0);

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

            // Insert patient details into the database
            using (var connection = databaseService.GetConnection())
            {
                connection.Open();

                string insertPatient = @"INSERT INTO Patients (Name, Age, Diagnosis) VALUES (@Name, @Age, @Diagnosis);";

                using (var command = new SqlCommand(insertPatient, connection))
                {
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Age", age);
                    command.Parameters.AddWithValue("@Diagnosis", diagnosis);

                    command.ExecuteNonQuery();
                    Console.WriteLine($"Patient '{name}' added successfully.");
                }
            } 

        }

        // View all patients
        public void ViewAllPatients()
        {
            Console.WriteLine("\nList of All Patients: ");

            using (var connection = databaseService.GetConnection())
            {
                connection.Open();
                string query = "SELECT * FROM Patients;";
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            Console.WriteLine("No patients found.");
                            return;
                        }
                        while (reader.Read()) 
                        {
                            Console.WriteLine($"ID: {reader["Id"]}, Name: {reader["Name"]}, Age: {reader["Age"]}, Diagnosis: {reader["Diagnosis"]}");
                        }
                    }
                }
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

