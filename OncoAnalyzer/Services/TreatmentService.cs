using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using OncoAnalyzer.Models;
using Serilog;


namespace OncoAnalyzer.Services
{
    public class TreatmentService
    {
        private readonly IDbExecutor dbExecutor;

        public TreatmentService(IDbExecutor dbExecutor)
        {
            this.dbExecutor = dbExecutor;
        }


        //Add a new treatment
        public void AddTreatment()
        {
            try
            {
                Console.Write("Enter Patient ID: ");
                if (!int.TryParse(Console.ReadLine(), out int PatientId)) 
                {
                    Console.WriteLine("Invalid Patient ID.");
                    return;
                }

                Console.Write("Enter Treatment Type (e.g., Chemotheraphy): ");
                string treatmentType = Console.ReadLine();

                Console.Write("Enter Start Date (YYYY-MM-DD): ");
                if (!DateTime.TryParse(Console.ReadLine(),out DateTime startDate))
                {
                    Console.WriteLine("Invalid start Date. ");
                    return;
                }
                Console.Write("Enter End Date (YYYY-MM-DD, Optional): ");
                string endDateInput = Console.ReadLine();
                DateTime? endDate = string.IsNullOrWhiteSpace(endDateInput) ? null : DateTime.Parse(endDateInput);

                Console.Write("Enter Response (e.g., Complete Response, Optional): ");
                string response = Console.ReadLine();

                //Insert into database
                string query = @"INSERT INTO Treatments (PatientId, TreatmentType, StartDate, EndDate, Response)
                                VALUES (@PatientId, @TreatmentType, @StartDate, @EndDate, @Response);";

                dbExecutor.ExecuteNonQuery(query, command =>
                {
                    command.Parameters.Add(new SqlParameter("@PatientId", PatientId));
                    command.Parameters.Add(new SqlParameter("@TreatmentType", treatmentType));
                    command.Parameters.Add(new SqlParameter("@StartDate", startDate));
                    command.Parameters.Add(new SqlParameter("@EndDate", (object)endDate ?? DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@Response", (object)response ?? DBNull.Value));
                }
                );
                Console.WriteLine("Treatment added successfully.");
                Log.Information("Treatment added successfully for patient ID {PatientId}.", PatientId);

            }

            catch (Exception ex) 
            {
                Log.Error(ex, "Error adding treatment");
                Console.WriteLine("An error occured while adding treatment. ");
            }
        }

        // View all treatments for a patient
        public void ViewTreatments (int patientId)
        {
            try
            {
                Console.WriteLine($"Treatments for Patient ID {patientId}: ");

                // Query to fetch treatments for the given patient ID
                string query = "SELECT * FROM Treatments WHERE PatientId = @PatientId;";

                using (var reader = dbExecutor.ExecuteReader(query, command =>
                {
                    command.Parameters.Add(new SqlParameter("@PatientId", patientId));
                }))
                {
                    if (!reader.Read()) 
                    {
                        Console.WriteLine("No treatments found for this patitent. ");
                        Log.Information("No treatments found for Patient ID {PatientId}.", patientId);
                        return;
                    }

                    // Read and display each treatment record
                    do
                    {
                        Log.Debug("Treatment Record: ID={Id}, Type={Type}, StartDate = {StartDate}, EndDate = {EndDate}, Response={Response}"
                            , reader["Id"], reader["TreatmentType"], reader["StartDate"], reader["EndDate"] ?? "N/A", reader["Response"] ?? "N/A");

                        // Display the treatment details in the console
                        Console.WriteLine($"ID: {reader["Id"]}, Type: {reader["TreatmentType"]}," +
                            $" Start Date: {reader["StartDate"]}, End Date:{reader["EndDate"] ?? "N/A"} ," +
                            $"Response: {reader["Response"] ?? "N/A"}");
                    }
                    while (reader.Read()); // Continue reading if there ar more rows
                }

                Log.Information("Displayed treatments for Patient ID {PatientId}.", patientId);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error retriving treatments. ");
                Console.WriteLine("An error occurred while retriving treatements.");
            }
        }
    }
}
