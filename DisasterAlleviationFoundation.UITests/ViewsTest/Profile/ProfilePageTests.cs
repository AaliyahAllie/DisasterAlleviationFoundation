using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace DisasterAlleviationFoundation.UITests
{
    [TestClass]
    public class ProfilePageTests
    {
        private IWebDriver driver;

        [TestInitialize]
        public void Setup()
        {
            var options = new ChromeOptions();
            // Enable headless for CI/CD
            if (System.Environment.GetEnvironmentVariable("AZURE_PIPELINES") == "true")
            {
                options.AddArgument("--headless");
                options.AddArgument("--disable-gpu");
            }

            driver = new ChromeDriver(options);
            driver.Manage().Window.Maximize();
        }

        [TestCleanup]
        public void TearDown()
        {
            driver.Quit();
        }

        [TestMethod]
        public void ProfilePage_LoadsAndAllowsUpdate()
        {
            // Navigate to Manage Profile page
            driver.Navigate().GoToUrl("https://localhost:5001/Profile/Index"); // Adjust URL

            // Verify fields exist
            var usernameField = driver.FindElement(By.Id("UserName"));
            var emailField = driver.FindElement(By.Id("Email"));
            var passwordField = driver.FindElement(By.Id("Password"));
            Assert.IsNotNull(usernameField);
            Assert.IsNotNull(emailField);
            Assert.IsNotNull(passwordField);

            // Fill out form
            usernameField.Clear();
            usernameField.SendKeys("TestUserUpdated");

            emailField.Clear();
            emailField.SendKeys("testupdated@example.com");

            passwordField.Clear();
            passwordField.SendKeys("NewPassword123!");

            // Submit form
            var submitButton = driver.FindElement(By.CssSelector("button.btn-primary"));
            submitButton.Click();

            // Optionally verify success message appears
            var statusMessage = driver.FindElement(By.CssSelector(".alert-success"));
            Assert.IsTrue(statusMessage.Text.Contains("success") || statusMessage.Displayed);
        }

        [TestMethod]
        public void ProfilePage_DeleteButton_Present()
        {
            driver.Navigate().GoToUrl("https://localhost:5001/Profile/Index");

            // Verify Delete button exists
            var deleteButton = driver.FindElement(By.CssSelector("form[action='/Profile/Delete'] button.btn-danger"));
            Assert.IsNotNull(deleteButton);

            // Note: avoid actually clicking Delete in test environment
        }
    }
}
