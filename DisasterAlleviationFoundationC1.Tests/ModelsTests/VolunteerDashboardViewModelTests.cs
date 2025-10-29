using Microsoft.VisualStudio.TestTools.UnitTesting;
using DisasterAlleviationFoundation.Models;
using System;
using System.Collections.Generic;

namespace DisasterAlleviationFoundation.Tests
{
    [TestClass]
    public class VolunteerDashboardViewModelTests
    {
        [TestMethod]
        public void Properties_CanBeAssignedAndRead()
        {
            // Arrange
            var volunteer = new Volunteer
            {
                VolunteerId = 1,
                Name = "John Doe",
                UserId = "user123",
                Age = 30,
                Skills = "First Aid",
                Availability = "Weekends"
            };

            var task1 = new VolunteerTask
            {
                TaskId = 101,
                Title = "Deliver Supplies",
                Description = "Deliver food parcels",
                DateTime = DateTime.Now,
                AssignedVolunteerId = volunteer.VolunteerId,
                Location = "Cape Town"
            };

            var task2 = new VolunteerTask
            {
                TaskId = 102,
                Title = "Medical Aid",
                Description = "Provide first aid at shelter",
                DateTime = DateTime.Now,
                AssignedVolunteerId = volunteer.VolunteerId,
                Location = "Cape Town"
            };

            var dashboard = new VolunteerDashboardViewModel
            {
                Volunteer = volunteer
            };

            // Act
            dashboard.AssignedTasks.Add(task1);
            dashboard.AssignedTasks.Add(task2);

            // Assert
            Assert.AreEqual(volunteer, dashboard.Volunteer);
            Assert.AreEqual(2, dashboard.AssignedTasks.Count);
            Assert.AreEqual("Deliver Supplies", dashboard.AssignedTasks[0].Title);
            Assert.AreEqual("Medical Aid", dashboard.AssignedTasks[1].Title);
        }

        [TestMethod]
        public void AssignedTasks_ShouldInitializeEmpty()
        {
            // Arrange
            var dashboard = new VolunteerDashboardViewModel();

            // Assert
            Assert.IsNotNull(dashboard.AssignedTasks, "AssignedTasks list should be initialized");
            Assert.AreEqual(0, dashboard.AssignedTasks.Count, "AssignedTasks should start empty");
        }
    }
}
