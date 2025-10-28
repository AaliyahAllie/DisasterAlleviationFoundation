using Microsoft.VisualStudio.TestTools.UnitTesting;
using DisasterAlleviationFoundation.Models;

namespace DisasterAlleviationFoundation.Tests
{
    [TestClass]
    public class AdminLoginViewModelTests
    {
        [TestMethod]
        public void Properties_CanBeAssignedAndRead()
        {
            // Arrange
            var model = new AdminLoginViewModel();
            string testUsername = "admin123";
            string testPassword = "password!";

            // Act
            model.Username = testUsername;
            model.Password = testPassword;

            // Assert
            Assert.AreEqual(testUsername, model.Username);
            Assert.AreEqual(testPassword, model.Password);
        }

        [TestMethod]
        public void DefaultValues_AreEmptyStrings()
        {
            // Arrange & Act
            var model = new AdminLoginViewModel();

            // Assert
            Assert.AreEqual(string.Empty, model.Username);
            Assert.AreEqual(string.Empty, model.Password);
        }
    }
}
