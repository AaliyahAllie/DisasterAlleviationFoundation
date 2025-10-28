using Microsoft.VisualStudio.TestTools.UnitTesting;
using DisasterAlleviationFoundation.Models;
using System;
using System.Collections.Generic;

namespace DisasterAlleviationFoundation.Tests
{
    [TestClass]
    public class HomeDashboardViewModelTests
    {
        [TestMethod]
        public void Volunteers_List_ShouldInitializeEmpty()
        {
            // Arrange
            var dashboard = new HomeDashboardViewModel();

            // Assert
            Assert.IsNotNull(dashboard.Volunteers, "Volunteers list should be initialized");
            Assert.AreEqual(0, dashboard.Volunteers.Count, "Volunteers list should start empty");
        }

        [TestMethod]
        public void VolunteerTasks_List_ShouldInitializeEmpty()
        {
            // Arrange
            var dashboard = new HomeDashboardViewModel();

            // Assert
            Assert.IsNotNull(dashboard.VolunteerTasks, "VolunteerTasks list should be initialized");
            Assert.AreEqual(0, dashboard.VolunteerTasks.Count, "VolunteerTasks list should start empty");
        }

        [TestMethod]
        public void Can_Add_Volunteer_And_Task()
        {
            // Arrange
            var dashboard = new HomeDashboardViewModel();

            var volunteer = new Volunteer
            {
                VolunteerId = 1,
                Name = "John Doe"
            };

            var task = new VolunteerTask
            {
                TaskId = 101,
                Title = "Deliver Supplies",
                Description = "Deliver food parcels to affected families",
                DateTime = DateTime.Now,
                Location = "Cape Town"
            };

            // Act
            dashboard.Volunteers.Add(volunteer);
            dashboard.VolunteerTasks.Add(task);

            // Assert
            Assert.AreEqual(1, dashboard.Volunteers.Count, "One volunteer should be added");
            Assert.AreEqual("John Doe", dashboard.Volunteers[0].Name);

            Assert.AreEqual(1, dashboard.VolunteerTasks.Count, "One task should be added");
            Assert.AreEqual("Deliver Supplies", dashboard.VolunteerTasks[0].Title);
            Assert.AreEqual("Deliver food parcels to affected families", dashboard.VolunteerTasks[0].Description);
            Assert.AreEqual("Cape Town", dashboard.VolunteerTasks[0].Location);
        }
    }
}
