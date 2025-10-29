using Microsoft.VisualStudio.TestTools.UnitTesting;
using DisasterAlleviationFoundation.Models;
using System;

namespace DisasterAlleviationFoundation.Tests
{
    [TestClass]
    public class VolunteerAssignmentTests
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

            var disaster = new Disaster
            {
                DisasterId = 10,
                Title = "Flood",
                Description = "Severe flooding in Cape Town",
                Location = "Cape Town",
                Severity = "High",
                DateReported = DateTime.Now
            };

            var assignment = new VolunteerAssignment
            {
                AssignmentId = 101,
                VolunteerId = volunteer.VolunteerId,
                Volunteer = volunteer,
                DisasterId = disaster.DisasterId,
                Disaster = disaster,
                TaskDescription = "Deliver Supplies",
                Location = "Cape Town",
                DateAssigned = DateTime.Now
            };

            // Assert
            Assert.AreEqual(101, assignment.AssignmentId);
            Assert.AreEqual(volunteer.VolunteerId, assignment.VolunteerId);
            Assert.AreEqual(volunteer, assignment.Volunteer);
            Assert.AreEqual(disaster.DisasterId, assignment.DisasterId);
            Assert.AreEqual(disaster, assignment.Disaster);
            Assert.AreEqual("Deliver Supplies", assignment.TaskDescription);
            Assert.AreEqual("Cape Town", assignment.Location);
            Assert.IsTrue((DateTime.Now - assignment.DateAssigned).TotalSeconds < 5, "DateAssigned should be close to now");
        }

        [TestMethod]
        public void DefaultValues_AreCorrect()
        {
            // Arrange
            var assignment = new VolunteerAssignment();

            // Assert default string values
            Assert.AreEqual(string.Empty, assignment.TaskDescription);
            Assert.AreEqual(string.Empty, assignment.Location);

            // Assert DateAssigned is initialized
            Assert.IsTrue((DateTime.Now - assignment.DateAssigned).TotalSeconds < 5, "DateAssigned should default to now");
        }
    }
}
