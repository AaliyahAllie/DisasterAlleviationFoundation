using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DisasterAlleviationFoundation.Controllers;
using DisasterAlleviationFoundation.Data;
using DisasterAlleviationFoundation.Models;

namespace DisasterAlleviationFoundation.Tests
{
    [TestClass]
    public class VolunteersControllerTests
    {
        private ApplicationDbContext _context;
        private Mock<UserManager<IdentityUser>> _userManagerMock;
        private VolunteersController _controller;

        [TestInitialize]
        public void Setup()
        {
            // Use in-memory database for testing
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "VolunteersTestDB")
                .Options;

            _context = new ApplicationDbContext(options);

            // Mock UserManager
            var store = new Mock<IUserStore<IdentityUser>>();
            _userManagerMock = new Mock<UserManager<IdentityUser>>(
                store.Object, null, null, null, null, null, null, null, null);

            // Create controller
            _controller = new VolunteersController(_context, _userManagerMock.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [TestMethod]
        public void Index_ReturnsViewWithVolunteers()
        {
            // Arrange
            _context.Volunteers.AddRange(new List<Volunteer>
            {
                new Volunteer { VolunteerId = 1, Name = "Alice" },
                new Volunteer { VolunteerId = 2, Name = "Bob" }
            });
            _context.SaveChanges();

            // Act
            var result = _controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var model = result.Model as List<Volunteer>;
            Assert.AreEqual(2, model.Count);
            Assert.AreEqual("Alice", model[0].Name);
        }

        [TestMethod]
        public async Task Register_ValidVolunteer_SavesAndRedirects()
        {
            // Arrange
            var user = new IdentityUser { Id = "user123", Email = "user@example.com" };
            _userManagerMock.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);

            var model = new Volunteer
            {
                Name = "John",
                Skills = "Cooking",
                Availability = "Weekends"
            };

            // Act
            var result = await _controller.Register(model) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);

            var savedVolunteer = _context.Volunteers.FirstOrDefault();
            Assert.IsNotNull(savedVolunteer);
            Assert.AreEqual("user123", savedVolunteer.UserId);
            Assert.AreEqual("John", savedVolunteer.Name);
        }

        [TestMethod]
        public async Task Register_InvalidModel_ReturnsSameView()
        {
            // Arrange
            _controller.ModelState.AddModelError("Name", "Required");
            var model = new Volunteer();

            // Act
            var result = await _controller.Register(model) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(model, result.Model);
        }
    }
}
