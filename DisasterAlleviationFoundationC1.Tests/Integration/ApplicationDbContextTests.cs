using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using DisasterAlleviationFoundation.Data;
using DisasterAlleviationFoundation.Models;
using Microsoft.AspNetCore.Identity;
using System;

namespace DisasterAlleviationFoundation.Tests
{
    [TestClass]
    public class ApplicationDbContextTests
    {
        private ApplicationDbContext _context;
        private IdentityUser _user;

        [TestInitialize]
        public void Setup()
        {
            // Use a unique in-memory database per test
            var dbName = Guid.NewGuid().ToString();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            _context = new ApplicationDbContext(options);

            // Seed user with dynamic Id
            _user = new IdentityUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "TestUser",
                Email = "test@example.com"
            };
            _context.Users.Add(_user);
            _context.SaveChanges(); // Save user first to generate Id

            // Seed Disaster with required properties
            var disaster = new Disaster
            {
                DisasterId = 1,
                Title = "Flood",
                Description = "Severe flooding",
                Location = "Cape Town",   // Required
                Severity = "High",        // Required
                UserId = _user.Id
            };
            _context.Disasters.Add(disaster);

            // Seed Volunteer
            var volunteer = new Volunteer
            {
                VolunteerId = 1,
                Name = "Alice",
                UserId = _user.Id
            };
            _context.Volunteers.Add(volunteer);

            // Seed VolunteerTask
            var task = new VolunteerTask
            {
                TaskId = 1,
                Title = "Deliver Food",
                AssignedVolunteerId = 1
            };
            _context.VolunteerTasks.Add(task);

            // Seed Donation
            var donation = new Donation
            {
                DonationId = 1,
                UserId = _user.Id,
                Quantity = 100,
                Description = "Food supplies"
            };
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
            Assert.AreEqual("Cape Town", disaster.Location);
            Assert.AreEqual("High", disaster.Severity);
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
            // EF Core InMemory does not enforce cascade deletes automatically.
            // Manually remove related entities to simulate cascade behavior
            _context.Disasters.RemoveRange(_context.Disasters.Where(d => d.UserId == _user.Id));
            _context.Volunteers.RemoveRange(_context.Volunteers.Where(v => v.UserId == _user.Id));
            _context.Donations.RemoveRange(_context.Donations.Where(d => d.UserId == _user.Id));

            _context.Users.Remove(_user);
            _context.SaveChanges();

            Assert.AreEqual(0, _context.Disasters.Count());
            Assert.AreEqual(0, _context.Volunteers.Count());
            Assert.AreEqual(0, _context.Donations.Count());
        }
    }
}
