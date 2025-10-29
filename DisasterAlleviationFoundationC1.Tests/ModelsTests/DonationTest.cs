using Microsoft.VisualStudio.TestTools.UnitTesting;
using DisasterAlleviationFoundation.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DisasterAlleviationFoundation.Tests
{
    [TestClass]
    public class DonationTests
    {
        [TestMethod]
        public void Properties_CanBeAssignedAndRead()
        {
            // Arrange
            var donation = new Donation();
            int testId = 1;
            string testUserId = "user123";
            string testCategory = "Clothes";
            int testQuantity = 20;
            string testDescription = "Winter jackets";
            string testStatus = "Distributed";
            DateTime testDate = new DateTime(2025, 10, 28);

            // Act
            donation.DonationId = testId;
            donation.UserId = testUserId;
            donation.Category = testCategory;
            donation.Quantity = testQuantity;
            donation.Description = testDescription;
            donation.Status = testStatus;
            donation.DateDonated = testDate;

            // Assert
            Assert.AreEqual(testId, donation.DonationId);
            Assert.AreEqual(testUserId, donation.UserId);
            Assert.AreEqual(testCategory, donation.Category);
            Assert.AreEqual(testQuantity, donation.Quantity);
            Assert.AreEqual(testDescription, donation.Description);
            Assert.AreEqual(testStatus, donation.Status);
            Assert.AreEqual(testDate, donation.DateDonated);
        }

        [TestMethod]
        public void DefaultValues_AreSetCorrectly()
        {
            // Arrange
            var donation = new Donation();

            // Assert
            Assert.AreEqual("Pending", donation.Status, "Default status should be 'Pending'");
            Assert.AreEqual("", donation.Category, "Default category should be empty string");
            Assert.IsTrue((DateTime.Now - donation.DateDonated).TotalSeconds < 1,
                "DateDonated should default to the current time");
        }

        [TestMethod]
        public void Validation_FailsWhenCategoryIsMissing()
        {
            // Arrange
            var donation = new Donation
            {
                Category = null // Violates [Required]
            };

            var context = new ValidationContext(donation);
            var results = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateObject(donation, context, results, true);

            // Assert
            Assert.IsFalse(isValid, "Validation should fail when Category is null");
        }

        [TestMethod]
        public void Validation_FailsWhenCategoryTooLong()
        {
            // Arrange
            var donation = new Donation
            {
                Category = new string('A', 60) // More than 50 chars
            };

            var context = new ValidationContext(donation);
            var results = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateObject(donation, context, results, true);

            // Assert
            Assert.IsFalse(isValid, "Validation should fail when Category exceeds 50 characters");
        }

        [TestMethod]
        public void Validation_PassesForValidDonation()
        {
            // Arrange
            var donation = new Donation
            {
                Category = "Food",
                Quantity = 10,
                Description = "Canned goods",
                UserId = "user456"
            };

            var context = new ValidationContext(donation);
            var results = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateObject(donation, context, results, true);

            // Assert
            Assert.IsTrue(isValid, "Validation should pass for a valid donation");
        }
    }
}
