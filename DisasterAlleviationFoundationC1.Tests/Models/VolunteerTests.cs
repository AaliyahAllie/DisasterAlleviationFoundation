using Microsoft.VisualStudio.TestTools.UnitTesting;
using DisasterAlleviationFoundation.Models;

namespace DisasterAlleviationFoundation.Tests
{
    [TestClass]
    public class VolunteerTests
    {
        [TestMethod]
        public void Properties_CanBeAssignedAndRead()
        {
            // Arrange
            var volunteer = new Volunteer
            {
                VolunteerId = 1,
                UserId = "user123",
                Name = "John Doe",
                Age = 30,
                Skills = "First Aid, Logistics",
                Availability = "Weekends"
            };

            // Assert
            Assert.AreEqual(1, volunteer.VolunteerId);
            Assert.AreEqual("user123", volunteer.UserId);
            Assert.AreEqual("John Doe", volunteer.Name);
            Assert.AreEqual(30, volunteer.Age);
            Assert.AreEqual("First Aid, Logistics", volunteer.Skills);
            Assert.AreEqual("Weekends", volunteer.Availability);
        }

        [TestMethod]
        public void DefaultValues_AreEmptyStrings()
        {
            // Arrange
            var volunteer = new Volunteer();

            // Assert
            Assert.AreEqual(string.Empty, volunteer.UserId);
            Assert.AreEqual(string.Empty, volunteer.Name);
            Assert.AreEqual(string.Empty, volunteer.Skills);
            Assert.AreEqual(string.Empty, volunteer.Availability);
        }
    }
}
