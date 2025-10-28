using Microsoft.VisualStudio.TestTools.UnitTesting;
using DisasterAlleviationFoundation.Models;
using System;

namespace DisasterAlleviationFoundation.Tests
{
    [TestClass]
    public class DisasterTests
    {
        [TestMethod]
        public void Properties_CanBeAssignedAndRead()
        {
            // Arrange
            var disaster = new Disaster();
            int testId = 1;
            string testTitle = "Flood";
            string testDescription = "Severe flooding in area.";
            string testLocation = "Cape Town";
            DateTime testDate = new DateTime(2025, 10, 28);
            string testSeverity = "High";
            string testUserId = "user123";

            // Act
            disaster.DisasterId = testId;
            disaster.Title = testTitle;
            disaster.Description = testDescription;
            disaster.Location = testLocation;
            disaster.DateReported = testDate;
            disaster.Severity = testSeverity;
            disaster.UserId = testUserId;

            // Assert
            Assert.AreEqual(testId, disaster.DisasterId);
            Assert.AreEqual(testTitle, disaster.Title);
            Assert.AreEqual(testDescription, disaster.Description);
            Assert.AreEqual(testLocation, disaster.Location);
            Assert.AreEqual(testDate, disaster.DateReported);
            Assert.AreEqual(testSeverity, disaster.Severity);
            Assert.AreEqual(testUserId, disaster.UserId);
        }

        [TestMethod]
        public void DisasterFiles_DefaultIsNotNull()
        {
            // Arrange & Act
            var disaster = new Disaster();

            // Assert
            Assert.IsNotNull(disaster.DisasterFiles);
            Assert.AreEqual(0, disaster.DisasterFiles.Count); // Should be empty by default
        }
    }
}
