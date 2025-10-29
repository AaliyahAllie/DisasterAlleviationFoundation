using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using DisasterAlleviationFoundation.Data;
using DisasterAlleviationFoundation.Models;
using Microsoft.AspNetCore.Identity;

namespace DisasterAlleviationFoundation.Tests
{
    [TestClass]
    public class ApplicationDbContextTests
    {
        private ApplicationDbContext _context;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "DbContextTestDB")
                .Options;

            _context = new ApplicationDbContext(options);

            // Seed some data
            var user = new IdentityUser { Id = "user1", UserName = "TestUser", Email = "test@example.com" };
            _context.Users.Add(user);

            var disaster = new Disaster { DisasterId = 1, Title = "Flood", Description = "Severe flooding", UserId = "user1" };
            _context.Disasters.Add(disaster);

            var volunteer = new Volunteer { VolunteerId = 1, Name = "Alice", UserId = "user1" };
            _context.Volunteers.Add(volunteer);

            var task = new VolunteerTask { TaskId = 1, Title = "Deliver Food", AssignedVolunteerId = 1 };
            _context.VolunteerTasks.Add(task);

            var donation = new Donation { DonationId = 1, UserId = "user1", Quantity = 100, Description = "Food supplies" };
            _context.Donations.Add(donation);

            _context.SaveChanges();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [TestMethod]
        public void Can_Add_And_Retrieve_Disaster()
        {
            var disaster = _context.Disasters.FirstOrDefault(d => d.DisasterId == 1);
            Assert.IsNotNull(disaster);
            Assert.AreEqual("Flood", disaster.Title);
        }

        [TestMethod]
        public void Can_Add_And_Retrieve_Volunteer()
        {
            var volunteer = _context.Volunteers.FirstOrDefault(v => v.VolunteerId == 1);
            Assert.IsNotNull(volunteer);
            Assert.AreEqual("Alice", volunteer.Name);
        }

        [TestMethod]
        public void Can_Add_And_Retrieve_VolunteerTask()
        {
            var task = _context.VolunteerTasks.FirstOrDefault(t => t.TaskId == 1);
            Assert.IsNotNull(task);
            Assert.AreEqual(1, task.AssignedVolunteerId);
        }

        [TestMethod]
        public void Can_Add_And_Retrieve_Donation()
        {
            var donation = _context.Donations.FirstOrDefault(d => d.DonationId == 1);
            Assert.IsNotNull(donation);
            Assert.AreEqual(100, donation.Quantity);
        }

        [TestMethod]
        public void Cascade_Delete_User_Deletes_Related_Entities()
        {
            // Arrange
            var user = _context.Users.First(u => u.Id == "user1");

            // Act
            _context.Users.Remove(user);
            _context.SaveChanges();

            // Assert
            Assert.AreEqual(0, _context.Disasters.Count());
            Assert.AreEqual(0, _context.Volunteers.Count());
            Assert.AreEqual(0, _context.Donations.Count());
        }
    }
}
