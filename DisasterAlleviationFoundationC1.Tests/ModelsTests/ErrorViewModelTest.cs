using Microsoft.VisualStudio.TestTools.UnitTesting;
using DisasterAlleviationFoundation.Models;

namespace DisasterAlleviationFoundation.Tests
{
    [TestClass]
    public class ErrorViewModelTests
    {
        [TestMethod]
        public void RequestId_CanBeAssignedAndRead()
        {
            // Arrange
            var model = new ErrorViewModel();
            string expectedId = "REQ123";

            // Act
            model.RequestId = expectedId;

            // Assert
            Assert.AreEqual(expectedId, model.RequestId);
        }

        [TestMethod]
        public void ShowRequestId_ReturnsTrue_WhenRequestIdIsNotEmpty()
        {
            // Arrange
            var model = new ErrorViewModel { RequestId = "REQ001" };

            // Act & Assert
            Assert.IsTrue(model.ShowRequestId, "ShowRequestId should return true when RequestId is not null or empty");
        }

        [TestMethod]
        public void ShowRequestId_ReturnsFalse_WhenRequestIdIsNull()
        {
            // Arrange
            var model = new ErrorViewModel { RequestId = null };

            // Act & Assert
            Assert.IsFalse(model.ShowRequestId, "ShowRequestId should return false when RequestId is null");
        }

        [TestMethod]
        public void ShowRequestId_ReturnsFalse_WhenRequestIdIsEmpty()
        {
            // Arrange
            var model = new ErrorViewModel { RequestId = string.Empty };

            // Act & Assert
            Assert.IsFalse(model.ShowRequestId, "ShowRequestId should return false when RequestId is empty");
        }
    }
}
