using DisasterAlleviationFoundation.Controllers;
using DisasterAlleviationFoundation.Data;
using DisasterAlleviationFoundation.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DisasterAlleviationFoundation.Tests
{
    [TestClass]
    public class DisastersControllerIntegrationTests
    {
        private ApplicationDbContext _context;
        private DisastersController _controller;
        private Mock<UserManager<IdentityUser>> _mockUserManager;
        private Mock<IWebHostEnvironment> _mockEnv;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("DisasterTestDb")
                .Options;

            _context = new ApplicationDbContext(options);

            // Clear existing data
            _context.Disasters.RemoveRange(_context.Disasters);
            _context.DisasterFiles.RemoveRange(_context.DisasterFiles);
            _context.SaveChanges();

            // Seed test disaster
            _context.Disasters.Add(new Disaster
            {
                DisasterId = 1,
                Title = "Flood",
                Description = "Severe flooding",
                Location = "Cape Town",
                Severity = "High",
                DateReported = DateTime.Now
            });
            _context.SaveChanges();

            // Mock UserManager
            var store = new Mock<IUserStore<IdentityUser>>();
            _mockUserManager = new Mock<UserManager<IdentityUser>>(
                store.Object, null, null, null, null, null, null, null, null
            );
            _mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>()))
                            .ReturnsAsync(new IdentityUser { Id = "user123", UserName = "testuser" });

            // Mock IWebHostEnvironment
            _mockEnv = new Mock<IWebHostEnvironment>();
            _mockEnv.Setup(e => e.WebRootPath).Returns(System.IO.Path.GetTempPath());

            // Controller
            _controller = new DisastersController(_context, _mockUserManager.Object, _mockEnv.Object);

            // Mock HttpContext
            var httpContext = new DefaultHttpContext();
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
        }

        [TestMethod]
        public async Task Index_ReturnsView_WithDisasters()
        {
            var result = await _controller.Index() as ViewResult;
            Assert.IsNotNull(result);

            var model = result.Model as List<Disaster>;
            Assert.IsNotNull(model);
            Assert.AreEqual(1, model.Count);
            Assert.AreEqual("Flood", model[0].Title);
        }

        [TestMethod]
        public async Task Details_ReturnsView_WithCorrectDisaster()
        {
            var result = await _controller.Details(1) as ViewResult;
            Assert.IsNotNull(result);

            var model = result.Model as Disaster;
            Assert.IsNotNull(model);
            Assert.AreEqual("Flood", model.Title);
        }

        [TestMethod]
        public void Create_GET_ReturnsView_WithSeverityOptions()
        {
            var result = _controller.Create() as ViewResult;
            Assert.IsNotNull(result);
            Assert.IsTrue(result.ViewData.ContainsKey("SeverityOptions"));
        }

        [TestMethod]
        public async Task Create_POST_AddsDisasterAndRedirects()
        {
            var disaster = new Disaster
            {
                Title = "Fire",
                Description = "Wildfire in forest",
                Location = "Knysna",
                Severity = "High"
            };

            var result = await _controller.Create(disaster, null) as RedirectToActionResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);

            var saved = _context.Disasters.FirstOrDefault(d => d.Title == "Fire");
            Assert.IsNotNull(saved);
            Assert.AreEqual("Knysna", saved.Location);
        }

        [TestMethod]
        public async Task Edit_POST_UpdatesExistingDisaster()
        {
            var disaster = _context.Disasters.First();
            disaster.Title = "Updated Flood";

            var result = await _controller.Edit(disaster.DisasterId, disaster, null) as RedirectToActionResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);

            var updated = _context.Disasters.Find(disaster.DisasterId);
            Assert.AreEqual("Updated Flood", updated.Title);
        }

        [TestMethod]
        public async Task DeleteConfirmed_RemovesDisaster()
        {
            var result = await _controller.DeleteConfirmed(1) as RedirectToActionResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);

            var deleted = _context.Disasters.Find(1);
            Assert.IsNull(deleted);
        }
    }
}
