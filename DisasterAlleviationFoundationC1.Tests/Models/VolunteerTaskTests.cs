using Microsoft.VisualStudio.TestTools.UnitTesting;
using DisasterAlleviationFoundation.Models;
using System;

namespace DisasterAlleviationFoundation.Tests
{
    [TestClass]
    public class VolunteerTaskTests
    {
        [TestMethod]
        public void Properties_CanBeAssignedAndRead()
        {
            // Arrange
            var volunteer = new Volunteer
            {
                VolunteerId = 1,
                Name = "John Doe"
            };

            var task = new VolunteerTask
            {
                TaskId = 101,
                Title = "Deliver Supplies",
                Description = "Deliver food parcels to shelter",
                DateTime = DateTime.Now,
                AssignedVolunteerId = volunteer.VolunteerId,
                AssignedVolunteer = volunteer,
                Location = "Cape Town"
            };

            // Assert
            Assert.AreEqual(101, task.TaskId);
            Assert.AreEqual("Deliver Supplies", task.Title);
            Assert.AreEqual("Deliver food parcels to shelter", task.Description);
            Assert.AreEqual(volunteer.VolunteerId, task.AssignedVolunteerId);
            Assert.AreEqual(volunteer, task.AssignedVolunteer);
            Assert.AreEqual("Cape Town", task.Location);
        }

        [TestMethod]
        public void NullableProperties_CanBeNull()
        {
            // Arrange
            var task = new VolunteerTask
            {
                TaskId = 102,
                Title = "Medical Aid",
                Description = "Provide first aid at shelter",
                DateTime = DateTime.Now,
                AssignedVolunteerId = null,
                AssignedVolunteer = null,
                Location = null
            };

            // Assert
            Assert.IsNull(task.AssignedVolunteerId);
            Assert.IsNull(task.AssignedVolunteer);
            Assert.IsNull(task.Location);
        }

        [TestMethod]
        public void DefaultValues_AreCorrect()
        {
            // Arrange
            var task = new VolunteerTask();

            // Assert default strings
            Assert.AreEqual(string.Empty, task.Title);
            Assert.AreEqual(string.Empty, task.Description);

            // Assert nullable properties are null
            Assert.IsNull(task.AssignedVolunteerId);
            Assert.IsNull(task.AssignedVolunteer);
            Assert.IsNull(task.Location);

            // DateTime default is DateTime.MinValue
            Assert.AreEqual(DateTime.MinValue, task.DateTime);
        }
    }
}
