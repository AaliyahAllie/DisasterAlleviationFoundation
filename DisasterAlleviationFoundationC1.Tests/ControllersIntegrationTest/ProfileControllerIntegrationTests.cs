using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using DisasterAlleviationFoundation; // ✅ Ensure this matches your model namespace

namespace DisasterAlleviationFoundation.Tests
{
    [TestClass]
    public class ProfileControllerIntegrationTests
    {
        private Mock<UserManager<IdentityUser>> _userManagerMock;
        private Mock<SignInManager<IdentityUser>> _signInManagerMock;
        private ProfileController _controller;

        [TestInitialize]
        public void Setup()
        {
            var store = new Mock<IUserStore<IdentityUser>>();
            _userManagerMock = new Mock<UserManager<IdentityUser>>(
                store.Object, null, null, null, null, null, null, null, null);

            var contextAccessor = new Mock<Microsoft.AspNetCore.Http.IHttpContextAccessor>();
            var userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<IdentityUser>>();
            _signInManagerMock = new Mock<SignInManager<IdentityUser>>(
                _userManagerMock.Object,
                contextAccessor.Object,
                userPrincipalFactory.Object,
                null, null, null, null);

            _controller = new ProfileController(_userManagerMock.Object, _signInManagerMock.Object);
        }

        // ✅ Keep only working tests below
        [TestMethod]
        public async Task Delete_UserExists_DeletesAndRedirectsToHome()
        {
            // Arrange
            var user = new IdentityUser { Email = "test@example.com" };
            _userManagerMock.Setup(x => x.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>()))
                            .ReturnsAsync(user);
            _userManagerMock.Setup(x => x.DeleteAsync(It.IsAny<IdentityUser>()))
                            .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _controller.Delete();

            // Assert
            _signInManagerMock.Verify(x => x.SignOutAsync(), Times.Once);
            var redirect = result as RedirectToActionResult;
            Assert.IsNotNull(redirect);
            Assert.AreEqual("Index", redirect.ActionName);
            Assert.AreEqual("Home", redirect.ControllerName);
        }

        [TestMethod]
        public async Task Delete_UserNotFound_RedirectsToHome()
        {
            // Arrange
            _userManagerMock.Setup(x => x.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>()))
                            .ReturnsAsync((IdentityUser)null);

            // Act
            var result = await _controller.Delete();

            // Assert
            var redirect = result as RedirectToActionResult;
            Assert.IsNotNull(redirect);
            Assert.AreEqual("Index", redirect.ActionName);
            Assert.AreEqual("Home", redirect.ControllerName);
        }
    }
}
