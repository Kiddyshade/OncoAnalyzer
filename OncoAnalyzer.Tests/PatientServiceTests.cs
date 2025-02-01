using Moq;
using System.Data;
using System.Data.SqlClient;
using Xunit;
using OncoAnalyzer.Services;
using OncoAnalyzer.Models;

namespace OncoAnalyzer.Tests
{
    public class PatientServiceTests
    {
        [Fact]
        public void Addpatient_ValidInput_ExecutesInserQuery()
        {
            //Arrange
            var mockDbExecutor = new Mock<IDbExecutor>();


            // Mock the ExecuteNonQuery method to simulate a successful query execution
            mockDbExecutor.Setup(exec => exec.ExecuteNonQuery(It.IsAny<string>(),
                It.IsAny<Action<IDbCommand>>())).Returns(1); // simulate successful execution

    
            //Inject the mocked IDbExecutor into the PatientService
            var patientService = new PatientService(mockDbExecutor.Object);

            //Act
            patientService.AddPatient("John Doe", 45, "Lung Cancer");

            //Assert
            //Verify that ExecuteNonQuery was called exactly once
            mockDbExecutor.Verify(exec => exec.ExecuteNonQuery(It.IsAny<string>(),
                It.IsAny<Action<IDbCommand>>()),Times.Once);

        }

        [Fact]
        public void AddPatient_ValidData_AddPatientSuccessfully()
        {
            //Arrange
            var mockDbExecutor = new Mock<IDbExecutor>();
            var patientService = new PatientService(mockDbExecutor.Object);

            //Correcting test to use the overloaded method with parameters
            string testName = "John Doe";
            int testAge = 45;
            string testDiagnosis = "Lung Cancer";


            //Ensure correct method is called
            mockDbExecutor.Setup(db => db.ExecuteNonQuery(It.IsAny<string>(), It.IsAny<Action<IDbCommand>>())).Returns(1);

            //Act
            patientService.AddPatient(testName, testAge, testDiagnosis);

            //Assert: Ensure ExecuteNonQuery was called once
            mockDbExecutor.Verify(db => db.ExecuteNonQuery(It.IsAny<string>(), It.IsAny<Action<IDbCommand>>()), Times.Once);
        }

        [Fact]

        public void AddPatient_InvalidData_ThrowsArgumentExeception()
        {
            //Arrange
            var mockDbExecutor = new Mock<IDbExecutor>();
            var patientService = new PatientService(mockDbExecutor.Object);

            // Providing invalid input (empty name, age = 0)
            string invalidName = " ";
            int invalidAge = 0;
            string invalidDiagnosis = " ";

            // Act & Assert: Ensure method throws ArgumentExeception for invalid input
            Assert.Throws<ArgumentException>(() => patientService.AddPatient(invalidName, invalidAge, invalidDiagnosis));
        }
    }
}