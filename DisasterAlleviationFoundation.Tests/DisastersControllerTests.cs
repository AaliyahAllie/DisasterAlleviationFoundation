using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using DisasterAlleviationFoundation.Controllers;
using DisasterAlleviationFoundation.Data;
using DisasterAlleviationFoundation.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DisasterAlleviationFoundation.Tests
{
    public class DisastersControllerTests
    {
        // -------------------
        // Helper: InMemory DbContext
        // -------------------
        private ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new ApplicationDbContext(options);

            // Seed disasters
            context.Disasters.Add(new Disaster
            {
                DisasterId = 1,
                Title = "Flood",
                Description = "Severe flood",
                Location = "City A",
                Severity = "High",
                DateReported = DateTime.Now
            });

            context.SaveChanges();
            return context;
        }

        // -------------------
        // Helper: Mock UserManager
        // -------------------
        private Mock<UserManager<IdentityUser>> GetMockUserManager()
        {
            var store = new Mock<IUserStore<IdentityUser>>();
            return new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
        }

        // -------------------
        // Helper: Mock IWebHostEnvironment
        // -------------------
        private Mock<IWebHostEnvironment> GetMockEnvironment()
        {
            var env = new Mock<IWebHostEnvironment>();
            env.Setup(e => e.WebRootPath).Returns(Path.GetTempPath());
            return env;
        }

        // -------------------
        // Index test
        // -------------------
        [Fact]
        public async Task Index_ReturnsViewWithDisasters()
        {
            var context = GetInMemoryDbContext();
            var userManager = GetMockUserManager();
            var env = GetMockEnvironment();
            var controller = new DisastersController(context, userManager.Object, env.Object);

            var result = await controller.Index() as ViewResult;

            Assert.NotNull(result);
            var model = result.Model as List<Disaster>;
            Assert.NotNull(model);
            Assert.Single(model);
            Assert.Equal("Flood", model[0].Title);
        }

        // -------------------
        // Details test
        // -------------------
        [Fact]
        public async Task Details_ValidId_ReturnsViewWithDisaster()
        {
            var context = GetInMemoryDbContext();
            var userManager = GetMockUserManager();
            var env = GetMockEnvironment();
            var controller = new DisastersController(context, userManager.Object, env.Object);

            var result = await controller.Details(1) as ViewResult;

            Assert.NotNull(result);
            var model = result.Model as Disaster;
            Assert.NotNull(model);
            Assert.Equal(1, model.DisasterId);
        }

        [Fact]
        public async Task Details_InvalidId_ReturnsNotFound()
        {
            var context = GetInMemoryDbContext();
            var userManager = GetMockUserManager();
            var env = GetMockEnvironment();
            var controller = new DisastersController(context, userManager.Object, env.Object);

            var result = await controller.Details(999);

            Assert.IsType<NotFoundResult>(result);
        }

        // -------------------
        // Create GET test
        // -------------------
        [Fact]
        public void Create_Get_ReturnsView()
        {
            var context = GetInMemoryDbContext();
            var userManager = GetMockUserManager();
            var env = GetMockEnvironment();
            var controller = new DisastersController(context, userManager.Object, env.Object);

            var result = controller.Create() as ViewResult;

            Assert.NotNull(result);
            Assert.NotNull(result.ViewData["SeverityOptions"]);
        }

        // -------------------
        // Create POST test
        // -------------------
        [Fact]
        public async Task Create_Post_ValidModel_RedirectsToIndex()
        {
            var context = GetInMemoryDbContext();
            var userManager = GetMockUserManager();
            userManager.Setup(u => u.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>()))
                       .ReturnsAsync(new IdentityUser { Id = "user1" });
            var env = GetMockEnvironment();
            var controller = new DisastersController(context, userManager.Object, env.Object);

            var disaster = new Disaster
            {
                Title = "Earthquake",
                Description = "Severe earthquake",
                Location = "City B",
                Severity = "High"
            };

            var result = await controller.Create(disaster, null) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Single(context.Disasters.Where(d => d.Title == "Earthquake"));
        }

        // -------------------
        // Edit GET test
        // -------------------
        [Fact]
        public async Task Edit_Get_ValidId_ReturnsView()
        {
            var context = GetInMemoryDbContext();
            var userManager = GetMockUserManager();
            var env = GetMockEnvironment();
            var controller = new DisastersController(context, userManager.Object, env.Object);

            var result = await controller.Edit(1) as ViewResult;

            Assert.NotNull(result);
            var model = result.Model as Disaster;
            Assert.NotNull(model);
            Assert.Equal(1, model.DisasterId);
        }

        [Fact]
        public async Task Edit_Get_InvalidId_ReturnsNotFound()
        {
            var context = GetInMemoryDbContext();
            var userManager = GetMockUserManager();
            var env = GetMockEnvironment();
            var controller = new DisastersController(context, userManager.Object, env.Object);

            var result = await controller.Edit(999);

            Assert.IsType<NotFoundResult>(result);
        }

        // -------------------
        // Delete GET test
        // -------------------
        [Fact]
        public async Task Delete_Get_ValidId_ReturnsView()
        {
            var context = GetInMemoryDbContext();
            var userManager = GetMockUserManager();
            var env = GetMockEnvironment();
            var controller = new DisastersController(context, userManager.Object, env.Object);

            var result = await controller.Delete(1) as ViewResult;

            Assert.NotNull(result);
            var model = result.Model as Disaster;
            Assert.NotNull(model);
        }

        [Fact]
        public async Task Delete_Get_InvalidId_ReturnsNotFound()
        {
            var context = GetInMemoryDbContext();
            var userManager = GetMockUserManager();
            var env = GetMockEnvironment();
            var controller = new DisastersController(context, userManager.Object, env.Object);

            var result = await controller.Delete(999);

            Assert.IsType<NotFoundResult>(result);
        }

        // -------------------
        // DeleteConfirmed POST test
        // -------------------
        [Fact]
        public async Task DeleteConfirmed_RemovesDisaster()
        {
            var context = GetInMemoryDbContext();
            var userManager = GetMockUserManager();
            var env = GetMockEnvironment();
            var controller = new DisastersController(context, userManager.Object, env.Object);

            var result = await controller.DeleteConfirmed(1) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Empty(context.Disasters.ToList());
        }
    }
}
