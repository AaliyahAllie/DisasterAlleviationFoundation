using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace DisasterAlleviationFoundation.UITests
{
    [TestClass]
    public class VolunteersPageTests
    {
        private IWebDriver driver;

        [TestInitialize]
        public void Setup()
        {
            var options = new ChromeOptions();
            // Headless mode for CI/CD (Azure Pipelines)
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
        public void VolunteersListPage_DisplaysTableAndRegisterButton()
        {
            // Navigate to Volunteers list page
            driver.Navigate().GoToUrl("https://localhost:5001/Volunteers");

            // Verify heading
            var heading = driver.FindElement(By.TagName("h2"));
            Assert.AreEqual("Registered Volunteers", heading.Text);

            // Verify Register button
            var registerButton = driver.FindElement(By.LinkText("Register as Volunteer"));
            Assert.IsNotNull(registerButton);

            // Verify table exists
            var table = driver.FindElement(By.CssSelector("table.table-striped"));
            Assert.IsNotNull(table);

            // Optional: Verify table headers
            var headers = table.FindElements(By.TagName("th"));
            Assert.IsTrue(headers[0].Text.Contains("Name"));
            Assert.IsTrue(headers[1].Text.Contains("Age"));
            Assert.IsTrue(headers[2].Text.Contains("Skills"));
            Assert.IsTrue(headers[3].Text.Contains("Availability"));
        }

        [TestMethod]
        public void VolunteerRegistrationPage_SubmitForm_ShowsSuccess()
        {
            // Navigate to Register page
            driver.Navigate().GoToUrl("https://localhost:5001/Volunteers/Register");

            // Fill in form
            driver.FindElement(By.Id("Name")).SendKeys("Test Volunteer");
            driver.FindElement(By.Id("Age")).SendKeys("25");
            driver.FindElement(By.Id("Skills")).SendKeys("First Aid, Coordination");
            driver.FindElement(By.Id("Availability")).SendKeys("Weekends");

            // Submit form
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            // After submission, check if redirected to Volunteers list
            Assert.IsTrue(driver.Url.EndsWith("/Volunteers"));

            // Optional: Verify the new volunteer appears in the table
            var table = driver.FindElement(By.CssSelector("table.table-striped"));
            Assert.IsTrue(table.Text.Contains("Test Volunteer"));
        }
    }
}
