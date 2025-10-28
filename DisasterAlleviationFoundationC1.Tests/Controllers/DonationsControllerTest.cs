using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using DisasterAlleviationFoundation.Controllers;
using DisasterAlleviationFoundation.Data;
using DisasterAlleviationFoundation.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DisasterAlleviationFoundation.Tests
{
    [TestClass]
    public class DonationsControllerIntegrationTests
    {
        private ApplicationDbContext _context;
        private DonationsController _controller;
        private Mock<UserManager<IdentityUser>> _mockUserManager;

        [TestInitialize]
        public void Setup()
        {
            // Setup in-memory database
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("DonationsTestDb")
                .Options;

            _context = new ApplicationDbContext(options);

            // Clear existing data
            _context.Donations.RemoveRange(_context.Donations);
            _context.SaveChanges();

            // Mock UserManager
            var store = new Mock<IUserStore<IdentityUser>>();
            _mockUserManager = new Mock<UserManager<IdentityUser>>(
                store.Object, null, null, null, null, null, null, null, null
            );
            _mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>()))
                            .ReturnsAsync(new IdentityUser { Id = "user123", UserName = "testuser" });

            // Controller
            _controller = new DonationsController(_context, _mockUserManager.Object);

            // Mock HttpContext
            var httpContext = new DefaultHttpContext();
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
        }

        [TestMethod]
        public async Task Index_ReturnsView_WithDonations()
        {
            // Arrange
            _context.Donations.Add(new Donation { DonationId = 1, Quantity = 100, Status = "Pending", DateDonated = DateTime.Now });
            _context.SaveChanges();

            // Act
            var result = await _controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var model = result.Model as System.Collections.Generic.List<Donation>;
            Assert.IsNotNull(model);
            Assert.AreEqual(1, model.Count);
            Assert.AreEqual(100, model[0].Quantity);
        }

        [TestMethod]
        public void Create_GET_ReturnsView()
        {
            var result = _controller.Create() as ViewResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task Create_POST_AddsDonationAndRedirects()
        {
            // Arrange
            var donation = new Donation
            {
                Quantity = 200,
                Status = "Pending"
            };

            // Act
            var result = await _controller.Create(donation) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);

            var saved = _context.Donations.FirstOrDefault(d => d.Quantity == 200);
            Assert.IsNotNull(saved);
            Assert.AreEqual("Pending", saved.Status);
            Assert.AreEqual("user123", saved.UserId);
        }

        [TestMethod]
        public async Task UpdateStatus_ChangesDonationStatus()
        {
            // Arrange
            var donation = new Donation
            {
                DonationId = 1,
                Quantity = 300,
                Status = "Pending"
            };
            _context.Donations.Add(donation);
            _context.SaveChanges();

            // Act
            var result = await _controller.UpdateStatus(1, "Approved") as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);

            var updated = _context.Donations.Find(1);
            Assert.AreEqual("Approved", updated.Status);
        }
    }
}
