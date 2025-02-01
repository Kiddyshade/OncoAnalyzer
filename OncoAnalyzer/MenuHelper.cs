
namespace OncoAnalyzer.Helpers
{
    public static class MenuHelper
    {
        //<summary>
        //    Returns the list of menu options based on the user's role.
        //</summary>
        //<param name="currentUser"> The role of the Authenticated user.</param>
        //<returns> The list of menu options. </returns>
        public static List<string> GetMenuOptionsForRole(string Role)
        {
            // Base menu options (common to all roles)
            var options = new List<string>
            {
                "3. View all Patients",
                "4. Search all Patients",
                "5. Export all Patients to CSV",
                "6. Export all Patients to PDF",
                "9. View Treatments for a Patient",
                "10. Exit"
            };

            // Additional options for Admin users
            if (Role == "Admin")
            {
                options.InsertRange(0, new[] { "1. Add Patient Details", "7. Import Patient Details", "7. Import Patients from CSV" });

            }

            // Additional optoins for Admin and Doctor roles
            if (Role == "Admin" || Role == "Doctor")
            {
                options.Insert(2, "2. Record Biomarker Test Results");
                options.Insert(5, "8. Add Treatment for a patient");
            }

            return options;
        }

    }
}
