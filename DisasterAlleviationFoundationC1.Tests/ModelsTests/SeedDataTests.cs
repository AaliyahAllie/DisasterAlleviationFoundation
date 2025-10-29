using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DisasterAlleviationFoundation.Tests
{
    [TestClass]
    public class SeedDataTests
    {
        private Mock<RoleManager<IdentityRole>> _mockRoleManager;
        private IServiceProvider _serviceProvider;

        [TestInitialize]
        public void Setup()
        {
            var store = new Mock<IRoleStore<IdentityRole>>();
            _mockRoleManager = new Mock<RoleManager<IdentityRole>>(
                store.Object, null, null, null, null
            );

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(_mockRoleManager.Object);
            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        [TestMethod]
        public async Task Initialize_CreatesMissingRoles()
        {
            // Arrange
            var createdRoles = new List<string>();

            _mockRoleManager
                .Setup(rm => rm.RoleExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(false);

            _mockRoleManager
                .Setup(rm => rm.CreateAsync(It.IsAny<IdentityRole>()))
                .Callback<IdentityRole>(role => createdRoles.Add(role.Name))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            await SeedData.Initialize(_serviceProvider);

            // Assert
            Assert.AreEqual(4, createdRoles.Count, "Should create all four roles");
            CollectionAssert.AreEquivalent(
                new[] { "Admin", "Volunteer", "Donor", "Coordinator" },
                createdRoles
            );
        }

        [TestMethod]
        public async Task Initialize_DoesNotCreateExistingRoles()
        {
            // Arrange
            _mockRoleManager
                .Setup(rm => rm.RoleExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(true); // all roles exist

            var createdRoles = new List<string>();

            _mockRoleManager
                .Setup(rm => rm.CreateAsync(It.IsAny<IdentityRole>()))
                .Callback<IdentityRole>(r => createdRoles.Add(r.Name))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            await SeedData.Initialize(_serviceProvider);

            // Assert
            Assert.AreEqual(0, createdRoles.Count, "No roles should be created if all exist");
        }
    }
}
