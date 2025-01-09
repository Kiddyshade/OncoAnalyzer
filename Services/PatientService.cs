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
    }
}
