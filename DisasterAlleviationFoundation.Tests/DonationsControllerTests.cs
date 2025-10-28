using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using DisasterAlleviationFoundation.Controllers;
using DisasterAlleviationFoundation.Data;
using DisasterAlleviationFoundation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DisasterAlleviationFoundation.Tests
{
    public class DonationsControllerTests
    {
        private ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new ApplicationDbContext(options);
            context.Donations.AddRange(
                new Donation { DonationId = 1, Category = "Food", Quantity = 10, Status = "Pending", DateDonated = DateTime.Now.AddDays(-1) },
                new Donation { DonationId = 2, Category = "Clothes", Quantity = 5, Status = "Distributed", DateDonated = DateTime.Now }
            );
            context.SaveChanges();
            return context;
        }

        private Mock<UserManager<IdentityUser>> GetMockUserManager()
        {
            var store = new Mock<IUserStore<IdentityUser>>();
            var mgr = new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
            mgr.Setup(m => m.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
               .ReturnsAsync(new IdentityUser { Id = "user1" });
            return mgr;
        }

        private DonationsController GetController(ApplicationDbContext context, Mock<UserManager<IdentityUser>> userManager)
        {
            var controller = new DonationsController(context, userManager.Object);

            // Mock User in HttpContext
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "user1")
            }, "mock"));

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            return controller;
        }

        [Fact]
        public async Task Index_ReturnsViewWithDonations()
        {
            var context = GetInMemoryDbContext();
            var userManager = GetMockUserManager();
            var controller = GetController(context, userManager);

            var result = await controller.Index() as ViewResult;

            Assert.NotNull(result);
            var model = result.Model as List<Donation>;
            Assert.NotNull(model);
            Assert.Equal(2, model.Count);
            Assert.Equal(2, model[0].DonationId); // latest first
            Assert.Equal(1, model[1].DonationId);
        }

        [Fact]
        public async Task Create_Post_ValidDonation_RedirectsToIndex()
        {
            var context = GetInMemoryDbContext();
            var userManager = GetMockUserManager();
            var controller = GetController(context, userManager);

            var newDonation = new Donation
            {
                Category = "Medicine",
                Quantity = 20,
                Description = "First aid kits"
            };

            var result = await controller.Create(newDonation) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);

            var donationInDb = context.Donations.FirstOrDefault(d => d.Category == "Medicine");
            Assert.NotNull(donationInDb);
            Assert.Equal("user1", donationInDb.UserId);
        }

        [Fact]
        public async Task UpdateStatus_ValidId_UpdatesStatus()
        {
            var context = GetInMemoryDbContext();
            var userManager = GetMockUserManager();
            var controller = GetController(context, userManager);

            var result = await controller.UpdateStatus(1, "Distributed") as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);

            var donation = context.Donations.Find(1);
            Assert.NotNull(donation);
            Assert.Equal("Distributed", donation.Status);
        }
    }
}
