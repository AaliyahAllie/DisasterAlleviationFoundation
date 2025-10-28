using DisasterAlleviationFoundation.Controllers;
using DisasterAlleviationFoundation.Data;
using DisasterAlleviationFoundation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace DisasterAlleviationFoundation.Tests
{
    [TestClass]
    public class HomeControllerIntegrationTests
    {
        private ApplicationDbContext _context;
        private HomeController _controller;

        [TestInitialize]
        public void Setup()
        {
            // In-memory DbContext
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("HomeControllerTestDb")
                .Options;

            _context = new ApplicationDbContext(options);

            // Seed test data
            var volunteer1 = new Volunteer { VolunteerId = 1, Name = "John Doe" };
            var volunteer2 = new Volunteer { VolunteerId = 2, Name = "Jane Smith" };
            _context.Volunteers.AddRange(volunteer1, volunteer2);

            var disaster = new Disaster
            {
                DisasterId = 1,
                Title = "Flood",
                Description = "Severe flooding",
                Location = "Cape Town",
                Severity = "High",
                DateReported = DateTime.Now
            };
            _context.Disasters.Add(disaster);

            var assignment = new VolunteerAssignment
            {
                AssignmentId = 1,
                VolunteerId = volunteer1.VolunteerId,
                Volunteer = volunteer1,
                DisasterId = disaster.DisasterId,
                Disaster = disaster,
                TaskDescription = "Deliver Supplies",
                Location = "Cape Town",
                DateAssigned = DateTime.Now
            };
            _context.VolunteerAssignments.Add(assignment);

            var donation = new Donation
            {
                DonationId = 1,
                Quantity = 100,
                DateDonated = DateTime.Now,
                UserId = "user123",
                Status = "Pending"
            };
            _context.Donations.Add(donation);

            _context.SaveChanges();

            // Setup controller
            _controller = new HomeController(_context);

            // Mock HttpContext
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            // Mock UrlHelper so Url.Action() works
            var mockUrlHelper = new Mock<IUrlHelper>();
            mockUrlHelper
                .Setup(u => u.Action(It.IsAny<UrlActionContext>()))
                .Returns("/Admin/Login");
            _controller.Url = mockUrlHelper.Object;
        }

        [TestMethod]
        public void Index_ReturnsView_WithAssignmentsAndStats()
        {
            // Act
            var result = _controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);

            var model = result.Model as System.Collections.Generic.List<VolunteerAssignment>;
            Assert.IsNotNull(model);
            Assert.AreEqual(1, model.Count);
            Assert.AreEqual("Deliver Supplies", model[0].TaskDescription);

            Assert.AreEqual(2, _controller.ViewData["TotalVolunteers"]);
            Assert.AreEqual(1, _controller.ViewData["TotalDisasters"]);
            Assert.AreEqual(1, _controller.ViewData["TotalDonations"]);
            Assert.AreEqual("/Admin/Login", _controller.ViewData["AdminLoginUrl"]);
        }

        [TestMethod]
        public void About_ReturnsView_WithMissionAndFeatures()
        {
            // Act
            var result = _controller.About() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("About Disaster Alleviation", result.ViewData["Title"]);
            var features = result.ViewData["Features"] as System.Collections.Generic.List<string>;
            Assert.IsTrue(features.Count > 0);
            var benefits = result.ViewData["Benefits"] as System.Collections.Generic.List<string>;
            Assert.IsTrue(benefits.Count > 0);
        }

        [TestMethod]
        public void Error_ReturnsView_WithRequestId()
        {
            // Act
            var result = _controller.Error() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var model = result.Model as ErrorViewModel;
            Assert.IsNotNull(model);
            Assert.IsFalse(string.IsNullOrEmpty(model.RequestId));
        }
    }
}
