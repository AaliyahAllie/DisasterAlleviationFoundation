using Microsoft.VisualStudio.TestTools.UnitTesting;
using DisasterAlleviationFoundation.Models;

namespace DisasterAlleviationFoundation.Tests
{
    [TestClass]
    public class VolunteerAdminViewModelTests
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
                Skills = "First Aid",
                Availability = "Weekends"
            };

            var assignment = new VolunteerAssignment
            {
                AssignmentId = 101,
                VolunteerId = 1,
                TaskDescription = "Deliver Supplies"
            };

            var viewModel = new VolunteerAdminViewModel
            {
                Volunteer = volunteer,
                Assignment = assignment
            };

            // Assert
            Assert.AreEqual(volunteer, viewModel.Volunteer);
            Assert.AreEqual(assignment, viewModel.Assignment);
        }

        [TestMethod]
        public void Assignment_CanBeNull()
        {
            // Arrange
            var volunteer = new Volunteer
            {
                VolunteerId = 2,
                Name = "Jane Smith"
            };

            var viewModel = new VolunteerAdminViewModel
            {
                Volunteer = volunteer,
                Assignment = null
            };

            // Assert
            Assert.AreEqual(volunteer, viewModel.Volunteer);
            Assert.IsNull(viewModel.Assignment);
        }
    }

   
}
