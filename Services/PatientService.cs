using System;
using System.Collections.Generic;
using OncoAnalyzer.Models;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OncoAnalyzer.Services
{
    public class PatientService
    {
        private List<Patient> patients = new List<Patient>(); // In-memory storage

        public void AddPatient()
        {
            // 1. Accept patient details
            Console.Write("Enter Patient Name: ");
            var name = Console.ReadLine();
            Console.WriteLine("Enter Age: ");
            var age = int.Parse(Console.ReadLine() ?? "0");
            Console.Write("Enter Diagnosis (e.g., Lung Cancer): ");
            var diagnosis = Console.ReadLine();

            // 2. Add to patient list
            var patient = new Patient
            {
                Id = patients.Count + 1,
                Name = name,
                Age = age,
                Diagnosis = diagnosis
            };

            patients.Add(patient);
            Console.WriteLine($"Patient '{name}' added successfully with ID {patient.Id}. ");

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

