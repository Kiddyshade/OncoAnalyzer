using Moq;
using System.Data;
using System.Data.SqlClient;
using Xunit;
using OncoAnalyzer.Services;

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
    }
}