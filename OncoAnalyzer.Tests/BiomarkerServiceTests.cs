using Moq;
using System.Data;
using System.Data.SqlClient;
using Xunit;
using OncoAnalyzer.Services;


namespace OncoAnalyzer.Tests
{
    public class BiomarkerServiceTests
    {
        [Fact]
        public void RecordTest_ValidInput_ExecuteInsertQuery()
        {
            //Arrange
            var mockDbExecutor = new Mock<IDbExecutor>();


            // Mock the ExecuteNonQuery method to simulate a successful query execution
            mockDbExecutor.Setup(exec => exec.ExecuteNonQuery(It.IsAny<string>(),
                It.IsAny<Action<IDbCommand>>())).Returns(1);


            // Inject the mocked database Executor into the BiomarkerService
            var biomarkerService = new BiomarkerService(mockDbExecutor.Object);

            //Act
            biomarkerService.RecordTest("PSA", 12.5, DateTime.Now, 1);

            // Assert
            //Verify that ExecuteNonQuery was called exactly once
            mockDbExecutor.Verify(exec => exec.ExecuteNonQuery(It.IsAny<string>(),
                It.IsAny<Action<IDbCommand>>()), Times.Once);

        }

    }
}
