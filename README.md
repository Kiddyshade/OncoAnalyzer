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

--------------------------------------------------------------------------------------

# OncoAnalyzer - Enhancement SQL Server integration and store data in database

## Overview
OncoAnalyzer is a C# console application designed to simulate an Oncology-focused system for managing patient data and biomarker test results. Now enhanced with **MS SQL Server** integration for persistent data storage.

## Features
1. Add patient details.
2. Record biomarker test results.
3. View all patients and their details.
4. Data persistence using MS SQL Server.

## Setup Instructions
### Prerequisites
1. **MS SQL Server** installed on your machine.
2. **Visual Studio** with the .NET development workload installed.

### Database Setup
1. Create a new database `OncoAnalyzerDB` using the SQL script:
    ```sql
    CREATE DATABASE OncoAnalyzerDB;
    USE OncoAnalyzerDB;
    CREATE TABLE Patients (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(100) NOT NULL,
        Age INT NOT NULL,
        Diagnosis NVARCHAR(200) NOT NULL
    );
    CREATE TABLE BiomarkerResults (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        BiomarkerName NVARCHAR(100) NOT NULL,
        Value FLOAT NOT NULL,
        TestDate DATETIME NOT NULL,
        PatientId INT NOT NULL FOREIGN KEY REFERENCES Patients(Id)
    );
    ```
2. Update the `DatabaseService.cs` connection string with your SQL Server instance details.

### Running the Application
1. Clone the repository:
    ```bash
    git clone https://github.com/<YourGitHubUsername>/OncoAnalyzer.git
    ```
2. Open the solution in **Visual Studio**.
3. Run the application using `Ctrl + F5`.

## Testing the Application
- Add patients to the database.
- Record biomarker test results for a patient.
- View all patients and their details.
------------------------------------------------------------------------------
 ## 14/01/2024 - Update
------------------------------------------------------------------------------s
 # OncoAnalyzer - Oncology Data Tracker

OncoAnalyzer is a C# console application designed for tracking patient data and biomarker test results in an oncology-focused environment. This project showcases features like patient management, biomarker tracking, unit testing with mock services, and database integration using **MS SQL Server**.

This project also demonstrates the use of **interactive rebasing**, **mock testing** with **Moq**, and an organized service-oriented architecture—all key components for modern .NET application development.

---

## Features

1. **Patient Management**
   - Add new patients (name, age, diagnosis).
   - View all patients stored in the database.

2. **Biomarker Test Tracking**
   - Record biomarker test results (e.g., PSA levels) for patients.
   - Store and associate biomarker data with corresponding patient IDs.

3. **Unit Testing**
   - Comprehensive tests for `PatientService` and `BiomarkerService` using **xUnit** and **Moq**.
   - Mock dependencies with `IDbExecutor` for better isolation and testability.

4. **Database Integration**
   - Persistent data storage with **Microsoft SQL Server**.
   - `IDbExecutor` for database abstraction, allowing flexibility in executing SQL commands.

5. **Project Setup**
   - Organized project structure separating services, models, and tests.
   - A clean and extensible architecture for future scalability.

---

## Technologies Used

- **C#** (.NET 6.0)
- **Microsoft SQL Server** (for database storage)
- **xUnit** (for unit testing)
- **Moq** (for mocking services in unit tests)
- **Visual Studio 2022**

---

## How to Set Up the Project

### Prerequisites

1. Install **Visual Studio 2022** with the `.NET Desktop Development` workload.
2. Install **MS SQL Server** and **SQL Server Management Studio (SSMS)**.
3. Clone this repository:
   ```bash
   git clone https://github.com/Kiddyshade/OncoAnalyzer.git
