## Features
- Add patient details
- Record biomarker test results
- Generate a summary report for patients
- **Search for a Patient**: Find a patient by ID or Name and view their details.

## How to Run
1. Clone the repository:
git clone https://github.com/<YourGitHubUsername>/OncoAnalyzer.git
2. Open the solution in Visual Studio.
3. Run the application using `Ctrl + F5`.

## Testing the Application
### Search for a Patient
- **Test Case 1**: Search for an existing patient by ID.
- **Test Case 2**: Search for an existing patient by Name.
- **Test Case 3**: Handle cases where no match is found.

-------------------------------------------------------------------------------------

## Enhancements
The following improvements have been made to the project:

1. **Data Validation for Adding Patients**:
   - Ensures that name, age, and diagnosis inputs are valid.
   - Prevents null or invalid data entries.

2. **Validate Biomarker Test Results**:
   - Ensures biomarker values are positive numbers.
   - Avoids invalid or meaningless test results.

3. **Prevent Searching Nonexistent Patients**:
   - Handles cases where no matching patient ID or name is found.
   - Prevents null reference errors.

4. **View All Patients**:
   - Displays a list of all patients in the system.
   - Useful for getting a quick overview of the data.

## Final Menu Options
1. Add Patient Details
2. Record Biomarker Test Results
3. Generate Patient Report
4. Search for a Patient
5. View All Patients
6. Exit
