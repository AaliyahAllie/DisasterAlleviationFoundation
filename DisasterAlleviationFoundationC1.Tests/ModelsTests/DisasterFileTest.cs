using Microsoft.VisualStudio.TestTools.UnitTesting;
using DisasterAlleviationFoundation.Models;
using System;

namespace DisasterAlleviationFoundation.Tests
{
    [TestClass]
    public class DisasterFileTests
    {
        [TestMethod]
        public void Properties_CanBeAssignedAndRead()
        {
            // Arrange
            var disaster = new Disaster { DisasterId = 1, Title = "Flood" };
            var file = new DisasterFile();

            int testFileId = 10;
            int testDisasterId = 1;
            string testFileName = "report.pdf";
            string testFilePath = "/uploads/report.pdf";
            DateTime testDate = new DateTime(2025, 10, 28);

            // Act
            file.FileId = testFileId;
            file.DisasterId = testDisasterId;
            file.FileName = testFileName;
            file.FilePath = testFilePath;
            file.UploadedAt = testDate;
            file.Disaster = disaster;

            // Assert
            Assert.AreEqual(testFileId, file.FileId);
            Assert.AreEqual(testDisasterId, file.DisasterId);
            Assert.AreEqual(testFileName, file.FileName);
            Assert.AreEqual(testFilePath, file.FilePath);
            Assert.AreEqual(testDate, file.UploadedAt);
            Assert.AreEqual(disaster, file.Disaster);
        }

        [TestMethod]
        public void UploadedAt_HasDefaultValue()
        {
            // Arrange & Act
            var file = new DisasterFile();

            // Assert
            Assert.IsTrue((DateTime.Now - file.UploadedAt).TotalSeconds < 1,
                "UploadedAt should default to current time");
        }
    }
}
