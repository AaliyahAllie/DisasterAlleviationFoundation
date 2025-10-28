using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DisasterAlleviationFoundation.Tests
{
    [TestClass]
    public class ProfileViewModelTests
    {
        private bool ValidateModel(object model, out List<ValidationResult> results)
        {
            var context = new ValidationContext(model, null, null);
            results = new List<ValidationResult>();
            return Validator.TryValidateObject(model, context, results, true);
        }

        [TestMethod]
        public void Properties_CanBeAssignedAndRead()
        {
            // Arrange
            var profile = new ProfileViewModel
            {
                Email = "user@example.com",
                UserName = "TestUser",
                Password = "Password123"
            };

            // Assert
            Assert.AreEqual("user@example.com", profile.Email);
            Assert.AreEqual("TestUser", profile.UserName);
            Assert.AreEqual("Password123", profile.Password);
        }

        [TestMethod]
        public void Validation_Succeeds_WithValidData()
        {
            // Arrange
            var profile = new ProfileViewModel
            {
                Email = "valid@example.com",
                UserName = "ValidUser",
                Password = "StrongPassword"
            };

            // Act
            var isValid = ValidateModel(profile, out var results);

            // Assert
            Assert.IsTrue(isValid, "Model should be valid when all fields are correct");
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void Validation_Fails_WhenEmailIsMissing()
        {
            // Arrange
            var profile = new ProfileViewModel
            {
                UserName = "User1",
                Password = "Password123"
            };

            // Act
            var isValid = ValidateModel(profile, out var results);

            // Assert
            Assert.IsFalse(isValid, "Model should be invalid when Email is missing");
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains("Email")));
        }

        [TestMethod]
        public void Validation_Fails_WhenUserNameIsMissing()
        {
            // Arrange
            var profile = new ProfileViewModel
            {
                Email = "user@example.com",
                Password = "Password123"
            };

            // Act
            var isValid = ValidateModel(profile, out var results);

            // Assert
            Assert.IsFalse(isValid, "Model should be invalid when UserName is missing");
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains("UserName")));
        }

        [TestMethod]
        public void Validation_Fails_WhenEmailIsInvalid()
        {
            // Arrange
            var profile = new ProfileViewModel
            {
                Email = "not-an-email",
                UserName = "User1",
                Password = "Password123"
            };

            // Act
            var isValid = ValidateModel(profile, out var results);

            // Assert
            Assert.IsFalse(isValid, "Model should be invalid with incorrect email format");
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains("Email")));
        }
    }

    // Mock class (for testing directly here)
    public class ProfileViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
