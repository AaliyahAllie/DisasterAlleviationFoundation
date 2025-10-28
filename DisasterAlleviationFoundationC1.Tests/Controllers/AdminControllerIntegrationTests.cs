using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DisasterAlleviationFoundation.Controllers;
using DisasterAlleviationFoundation.Data;
using DisasterAlleviationFoundation.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace DisasterAlleviationFoundation.Tests
{
    [TestClass]
    public class AdminControllerIntegrationTests
    {
        private ApplicationDbContext _context;
        private AdminController _controller;

        [TestInitialize]
        public void Setup()
        {
            // Setup in-memory database
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            _context = new ApplicationDbContext(options);

            // Clear any existing data
            _context.Volunteers.RemoveRange(_context.Volunteers);
            _context.VolunteerTasks.RemoveRange(_context.VolunteerTasks);
            _context.Disasters.RemoveRange(_context.Disasters);
            _context.VolunteerAssignments.RemoveRange(_context.VolunteerAssignments);
            _context.SaveChanges();

            // Seed test data
            _context.Volunteers.Add(new Volunteer { VolunteerId = 1, Name = "John Doe" });
            _context.Volunteers.Add(new Volunteer { VolunteerId = 2, Name = "Jane Smith" });

            _context.VolunteerTasks.Add(new VolunteerTask
            {
                TaskId = 1,
                Title = "Deliver Supplies",
                Description = "Deliver food parcels to shelter",
                DateTime = DateTime.Now
            });

            _context.Disasters.Add(new Disaster
            {
                DisasterId = 1,
                Title = "Flood",
                Description = "Severe flooding in Cape Town",
                Location = "Cape Town",
                Severity = "High",
                DateReported = DateTime.Now
            });

            _context.SaveChanges();

            // Setup controller
            _controller = new AdminController(_context);

            // Mock HttpContext with session
            var httpContext = new DefaultHttpContext();
            httpContext.Session = new DummySession();
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
        }

        [TestMethod]
        public void Login_Post_ValidAdmin_RedirectsToDashboard()
        {
            var model = new AdminLoginViewModel
            {
                Username = "admin",
                Password = "adminPassKey12@"
            };

            var result = _controller.Login(model) as RedirectToActionResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Dashboard", result.ActionName);
            Assert.AreEqual("true", _controller.HttpContext.Session.GetString("IsAdmin"));
        }

        [TestMethod]
        public void Dashboard_ReturnsView_WithVolunteersAndAssignments()
        {
            _controller.HttpContext.Session.SetString("IsAdmin", "true");

            var result = _controller.Dashboard() as ViewResult;

            Assert.IsNotNull(result);
            var model = result.Model as List<VolunteerAdminViewModel>;
            Assert.AreEqual(2, model.Count);
            Assert.AreEqual("John Doe", model[0].Volunteer.Name);
            Assert.AreEqual("Jane Smith", model[1].Volunteer.Name);
        }

        [TestMethod]
        public void DeleteVolunteer_RemovesVolunteer()
        {
            _controller.HttpContext.Session.SetString("IsAdmin", "true");

            var result = _controller.DeleteVolunteer(1) as RedirectToActionResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Dashboard", result.ActionName);
            Assert.AreEqual(1, _context.Volunteers.Count()); // Only Jane Smith remains
            Assert.IsNull(_context.Volunteers.Find(1));
        }

        [TestMethod]
        public void EditAssignment_AddsNewAssignment_WhenNotExists()
        {
            _controller.HttpContext.Session.SetString("IsAdmin", "true");

            var assignment = new VolunteerAssignment
            {
                VolunteerId = 2,
                TaskDescription = "Provide medical aid",
                DisasterId = 1,
                Location = "Cape Town"
            };

            var result = _controller.EditAssignment(assignment) as RedirectToActionResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Dashboard", result.ActionName);

            var savedAssignment = _context.VolunteerAssignments.FirstOrDefault(a => a.VolunteerId == 2);
            Assert.IsNotNull(savedAssignment);
            Assert.AreEqual("Provide medical aid", savedAssignment.TaskDescription);
        }

        [TestMethod]
        public void EditAssignment_UpdatesExistingAssignment()
        {
            _controller.HttpContext.Session.SetString("IsAdmin", "true");

            var existing = new VolunteerAssignment
            {
                VolunteerId = 2,
                TaskDescription = "Initial Task",
                DisasterId = 1,
                Location = "Old Location"
            };
            _context.VolunteerAssignments.Add(existing);
            _context.SaveChanges();

            var updatedAssignment = new VolunteerAssignment
            {
                VolunteerId = 2,
                TaskDescription = "Updated Task",
                DisasterId = 1,
                Location = "Cape Town"
            };

            var result = _controller.EditAssignment(updatedAssignment) as RedirectToActionResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Dashboard", result.ActionName);

            var saved = _context.VolunteerAssignments.First(a => a.VolunteerId == 2);
            Assert.AreEqual("Updated Task", saved.TaskDescription);
            Assert.AreEqual("Cape Town", saved.Location);
        }
    }

    // Dummy session implementation for testing
    public class DummySession : ISession
    {
        private readonly Dictionary<string, byte[]> _sessionStorage = new();

        public IEnumerable<string> Keys => _sessionStorage.Keys;
        public string Id => Guid.NewGuid().ToString();
        public bool IsAvailable => true;

        public void Clear() => _sessionStorage.Clear();
        public Task CommitAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
        public Task LoadAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
        public void Remove(string key) => _sessionStorage.Remove(key);
        public void Set(string key, byte[] value) => _sessionStorage[key] = value;
        public bool TryGetValue(string key, out byte[] value) => _sessionStorage.TryGetValue(key, out value);
    }
}
