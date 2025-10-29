using Microsoft.VisualStudio.TestTools.UnitTesting;
using DisasterAlleviationFoundation.Models;
using System;

namespace DisasterAlleviationFoundation.Tests
{
    [TestClass]
    public class VolunteerTaskViewModelTests
    {
        [TestMethod]
        public void Properties_CanBeAssignedAndRead()
        {
            // Arrange
            var taskVM = new VolunteerTaskViewModel
            {
                TaskId = 101,
                Title = "Deliver Supplies",
                Description = "Deliver food parcels",
                DateTime = DateTime.Now,
                AssignedVolunteerId = 1,
                AssignedVolunteerName = "John Doe",
                DisasterId = 10,
                DisasterTitle = "Flood"
            };

            // Assert
            Assert.AreEqual(101, taskVM.TaskId);
            Assert.AreEqual("Deliver Supplies", taskVM.Title);
            Assert.AreEqual("Deliver food parcels", taskVM.Description);
            Assert.AreEqual(1, taskVM.AssignedVolunteerId);
            Assert.AreEqual("John Doe", taskVM.AssignedVolunteerName);
            Assert.AreEqual(10, taskVM.DisasterId);
            Assert.AreEqual("Flood", taskVM.DisasterTitle);
        }

        [TestMethod]
        public void ComputedProperty_AssignedInfo_ReturnsNameOrUnassigned()
        {
            // Arrange: assigned
            var assignedTask = new VolunteerTaskViewModel
            {
                AssignedVolunteerName = "John Doe"
            };

            // Assert assigned
            Assert.AreEqual("John Doe", assignedTask.AssignedInfo);

            // Arrange: unassigned
            var unassignedTask = new VolunteerTaskViewModel
            {
                AssignedVolunteerName = null
            };

            // Assert unassigned
            Assert.AreEqual("Unassigned", unassignedTask.AssignedInfo);
        }

        [TestMethod]
        public void NullableProperties_CanBeNull()
        {
            // Arrange
            var taskVM = new VolunteerTaskViewModel
            {
                TaskId = 102,
                Title = "Medical Aid",
                Description = "Provide first aid",
                DateTime = DateTime.Now,
                AssignedVolunteerId = null,
                AssignedVolunteerName = null,
                DisasterId = null,
                DisasterTitle = null
            };

            // Assert
            Assert.IsNull(taskVM.AssignedVolunteerId);
            Assert.IsNull(taskVM.AssignedVolunteerName);
            Assert.IsNull(taskVM.DisasterId);
            Assert.IsNull(taskVM.DisasterTitle);
            Assert.AreEqual("Unassigned", taskVM.AssignedInfo);
        }
    }
}
