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
--------------------------------------------------------------------------------------------------
# 15-01-25 - Update (Integrating a Logging system using - Serilog)
--------------------------------------------------------------------------------------------------

# OncoAnalyzer - Oncology Data Tracker

OncoAnalyzer is a C# console application designed to track patient data and biomarker test results in an oncology-focused environment. This project showcases features like patient management, biomarker tracking, unit testing with mock services, database integration using **MS SQL Server**, and a persistent logging system for better debugging and monitoring.

---

## Features

1. **Patient Management**
   - Add new patients (name, age, diagnosis).
   - View all patients stored in the database.

2. **Biomarker Test Tracking**
   - Record biomarker test results (e.g., PSA levels) for patients.
   - Store and associate biomarker data with corresponding patient IDs.

3. **Persistent Logging System**
   - Uses **Serilog** to log important application events and errors to the console and a log file.
   - Logs are stored in the project root under the `Logs` folder.

4. **Unit Testing**
   - Comprehensive tests for services using **xUnit** and **Moq** for mocking.
   - Mock dependencies with `IDbExecutor` for better isolation and testability.

5. **Database Integration**
   - Persistent data storage with **Microsoft SQL Server**.
   - `IDbExecutor` for database abstraction, allowing flexibility in executing SQL commands.

---

## Technologies Used

- **C#** (.NET 6.0)
- **Microsoft SQL Server** (for database storage)
- **xUnit** (for unit testing)
- **Moq** (for mocking services in unit tests)
- **Serilog** (for structured and file-based logging)
- **Visual Studio 2022**

---

## How to Set Up the Project

### Prerequisites

1. Install **Visual Studio 2022** with the `.NET Desktop Development` workload.
2. Install **MS SQL Server** and **SQL Server Management Studio (SSMS)**.
3. Clone this repository:
   ```bash
   git clone https://github.com/Kiddyshade/OncoAnalyzer.git

--------------------------------------------------------------------------------------------------
# 19-01-25 - Update (OncoAnalyzer is a console-based application designed to help track patient data
in an oncology laboratory. It provides basic patient data management functionalities and now includes
user authentication with role-based access control, as well as advanced features like exporting data 
to CSV/PDF.)
--------------------------------------------------------------------------------------------------

# OncoAnalyzer - Oncology Data Tracker

OncoAnalyzer is a console-based application designed to help track patient data in an oncology laboratory. It provides basic patient data management functionalities and now includes user authentication with role-based access control, as well as advanced features like exporting data to CSV/PDF.

## Features

### Core Features:
- **Add Patient Details**: Add basic patient information (name, age, and diagnosis) into the system.
- **View All Patients**: List all the patients stored in the database.
- **Advanced Search**: Search patients using multiple criteria like name, age range, and diagnosis, with case-insensitive matching.
- **Record Biomarker Test Results**: Record test results for patients with details such as biomarker name, value, and date.
- **Export to CSV and PDF**:
  - Export all patient data to a CSV file.
  - Export patient data as a structured PDF file.

### New Features:
- **User Authentication and Role-Based Access Control**:
  - **Login System**: Users must log in using a username and password to access the application.
  - **Roles**: Each user has a specific role:
    - `Admin`: Full access (add, edit, delete, and view patient data).
    - `Doctor`: Can only view and search patient data.
    - `Staff`: Can only export patient data (CSV or PDF).
  - Passwords are securely hashed using SHA-256 with UTF-16 encoding.
  - Authentication errors are logged using **Serilog**.

### Persistent Logging:
- All application events (e.g., successful database operations, errors, login attempts) are logged to a file using **Serilog** for easy debugging and monitoring.

## Installation

### Prerequisites:
1. **.NET 6.0 SDK**: Ensure you have .NET 6.0 installed. Download it from [Microsoft .NET](https://dotnet.microsoft.com/).
2. **SQL Server**: The application uses Microsoft SQL Server for persistent storage.
3. **NuGet Packages**:
   - **Serilog**: For logging.
   - **PDFSharp**: For exporting patient data to PDF.
   - **Dapper**: For database interactions (via `IDbExecutor`).

### Clone the Repository:
```bash
git clone https://github.com/Kiddyshade/OncoAnalyzer.git
cd OncoAnalyzer

--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
# 24/01/25 Update
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

# OncoAnalyzer - Oncology Data Tracker

OncoAnalyzer is a console-based application designed to track oncology-related patient data, biomarker test results, and other critical information. It allows users to manage data securely, export reports, and perform advanced queries.

---

## **Features**

### 1. **User Authentication and Role-Based Access Control**
- Users must log in with valid credentials before accessing the system.
- Roles:
  - **Admin**: Full access to add, import, export, and manage data.
  - **Doctor**: Can view and search patient data.
  - **Staff**: Can export data to CSV or PDF.

---

### 2. **Patient Management**
- Add new patient details.
- View all registered patients.
- Perform advanced patient searches with filters like name, age range, and diagnosis.

---

### 3. **Biomarker Test Management**
- Record biomarker test results for patients.

---

### 4. **Data Export**
- **Export to CSV**: Save all patient data to a CSV file.
- **Export to PDF**: Generate a PDF report of patient data.

---

### 5. **Data Import from CSV**
- **Admin-only feature**: Import patient data from a properly formatted CSV file.
- **CSV Format**:
  - Must include the headers: `Name`, `Age`, `Diagnosis`.
  - Example:
    ```csv
    Name,Age,Diagnosis
    John Doe,45,Lung Cancer
    Jane Smith,34,Breast Cancer
    ```
- The system validates the CSV structure and data before importing.
- Skips invalid rows and logs errors.

---

## **Getting Started**

### **Prerequisites**
1. Install [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0).
2. Ensure you have access to an instance of Microsoft SQL Server.

---

### **Setup Instructions**

1. Clone this repository:
   ```bash
   git clone https://github.com/Kiddyshade/OncoAnalyzer.git
   cd OncoAnalyzer
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
# 25/01/25 Update
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

## New Features: Treatment Tracking

### **1. Add Treatment**
- **Access**: Admins and Doctors.
- **Description**: Allows recording of treatment details for a specific patient.
- **Usage**:
  - Select **Option 8** from the main menu.
  - Provide the patient ID, treatment type, start date, optional end date, and optional response.

### **2. View Treatments**
- **Access**: All roles.
- **Description**: Displays all treatments associated with a given patient ID.
- **Usage**:
  - Select **Option 9** from the main menu.
  - Enter the patient ID to view associated treatments.

### **Role-Based Access Control**
| Role    | Options Available                                   |
|---------|-----------------------------------------------------|
| Admin   | 1, 2, 3, 4, 5, 6, 7, 8, 9, 10                      |
| Doctor  | 2, 3, 4, 5, 6, 8, 9, 10                            |
| Staff   | 3, 4, 5, 6, 9, 10                                  |

---

## Testing the New Features

### **Scenarios**
1. **Admin User**:
   - Add a treatment for a patient and verify the database.
   - Import patients from a CSV file.
2. **Doctor User**:
   - Add treatments and view them.
   - Confirm restricted access to patient import.
3. **Staff User**:
   - View treatments and export data.
   - Confirm restricted access to adding/editing data.

---

## Example Commands and Outputs

### **Add Treatment**
Enter Patient ID: 1 Enter Treatment Type (e.g., Chemotherapy): Chemotherapy Enter Start Date (YYYY-MM-DD): 2025-01-01 Enter End Date (YYYY-MM-DD, optional): 2025-01-15 Enter Response (optional): Partial Response Treatment added successfully.


### **View Treatments**
Enter Patient ID: 1 Treatments for Patient ID 1: ID: 1, Type: Chemotherapy, Start Date: 2025-01-01, End Date: 2025-01-15, Response: Partial Response

---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

# Update on 01/02/2025

OncoAnalyzer - Oncology Data Tracker

OncoAnalyzer is a C# console application designed to track patient data, biomarker test results, and treatments in an oncology-focused environment. It features role-based authentication, data export (CSV & PDF), and Microsoft SQL Server integration for persistent storage.
Features
1. User Authentication & Role-Based Access Control

    Admin: Full access (add, edit, delete, and manage patient data).
    Doctor: Can record biomarker test results and manage treatments.
    Staff: Can only view and export data (CSV/PDF).
    Passwords are securely hashed to enhance security.

2. Patient Management

    Add new patients to the system.
    View all registered patients.
    Perform advanced search by:
        Name
        Age range
        Diagnosis (case-insensitive)

3. Biomarker Test Tracking

    Record biomarker test results (e.g., PSA levels).
    Associate biomarker data with the correct patient.

4. Treatment Management

    Admins and doctors can add treatments for a patient.
    View all treatments associated with a specific patient.

5. Data Export & Import

Export:

    Save all patient data to CSV (Excel-compatible).
    Generate PDF reports with structured formatting.
    Import:
    Admin-only feature: Import patient data from CSV with validation.

6. Persistent Logging System

    Uses Serilog for structured logging and error tracking.
    Logs events like user login attempts, database errors, and CSV imports.

Technologies Used

    C# (.NET 6.0)
    Microsoft SQL Server (Database)
    Spectre.Console (Enhanced CLI)
    Serilog (Logging)
    CsvHelper (CSV Import/Export)
    PDFSharp (PDF Generation)
    xUnit & Moq (Unit Testing)

How to Run the Project
Prerequisites

    Install .NET 6.0 SDK: Download Here
    Install Microsoft SQL Server and SQL Server Management Studio (SSMS)
    Clone the repository:

    git clone https://github.com/<your-github-username>/OncoAnalyzer.git
    cd OncoAnalyzer

Database Setup

    Open SSMS and create the database:

CREATE DATABASE OncoAnalyzerDB;
USE OncoAnalyzerDB;

Create the Patients table:

CREATE TABLE Patients (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Age INT NOT NULL,
    Diagnosis NVARCHAR(200) NOT NULL
);

Create the Users table for authentication:

    CREATE TABLE Users (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Username NVARCHAR(50) UNIQUE NOT NULL,
        PasswordHash VARBINARY(64) NOT NULL,
        Role NVARCHAR(20) NOT NULL
    );

Running the Application

    Open Visual Studio and load the project.
    Open the Package Manager Console and install dependencies:

dotnet restore

Run the application:

    dotnet run

Usage Guide
User Login

    Start the application.
    Enter username and password.
    The system grants access based on role-based authentication.

Main Menu (Admin)

1. Add Patient Details
2. Record Biomarker Test Results
3. View all Patients
4. Search Patients
5. Export Patients to CSV
6. Export Patients to PDF
7. Import Patients from CSV
8. Add Treatment for a Patient
9. View Treatments for a Patient
10. Exit

The menu dynamically adjusts based on user role.
Running Unit Tests

OncoAnalyzer includes unit tests using xUnit and Moq.

    Open a terminal in the project root.
    Run the tests using:

    dotnet test

    Test coverage:
        Authentication Tests
        Patient Service Tests
        Role-Based Menu Access Tests
        Database Mock Tests

Contributing
Steps to Contribute

    Fork the repository.
    Create a branch (feature/new-feature).
    Commit your changes:

git commit -m "Added new feature: XYZ"

Push the branch:

    git push origin feature/new-feature

    Create a Pull Request.

License

This project is licensed under the MIT License.
