using Xunit;
using Moq;
using OncoAnalyzer.Services;
using OncoAnalyzer.Models;

namespace OncoAnalyzer.Tests
{
    public class AuthenticationTests
    {
        [Fact]

        public void Authenticate_ValidUser_ReturnUser() 
        {
            //Arrange
            var mockUserService = new Mock<UserService>(null);
            var expectedUser = new User { Username = "admin", Role = "Admin" };

            // Fix: Now Moq can override 'Authenticate'
            mockUserService.Setup(s => s.Authenticate("admin", "password")).Returns(expectedUser);


            //Act
            var result = mockUserService.Object.Authenticate("admin", "password");

            //Assert
            Assert.NotNull(result);
            Assert.Equal("admin",result.Username);
            Assert.Equal("Admin",result.Role);
        }

        [Fact]
        public void Authenticate_InavalidCrendentials_ReturnsNull()
        {
            //Arrange
            var mockUserService = new Mock<UserService>(null);

            // Fix: Return 'null' for invalid credentials
            mockUserService.Setup(s => s.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Returns((User)null);

            //Act
            var result = mockUserService.Object.Authenticate("WrongUser", "WrongPassword");

            //Assert
            Assert.Null(result);
        }
    }
}
