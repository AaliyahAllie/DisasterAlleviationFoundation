using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;

namespace DisasterAlleviationFoundation.UITests
{
    [TestClass]
    public class VolunteersPageTests
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [TestInitialize]
        public void Setup()
        {
            var options = new ChromeOptions();

            // Headless mode for CI/CD (Azure Pipelines)
            if (Environment.GetEnvironmentVariable("AZURE_PIPELINES") == "true")
            {
                options.AddArgument("--headless");
                options.AddArgument("--disable-gpu");
            }

            driver = new ChromeDriver(options);
            driver.Manage().Window.Maximize();

            // Explicit wait for dynamic content
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [TestCleanup]
        public void TearDown()
        {
            driver.Quit();
        }

        [TestMethod]
        public void VolunteersListPage_DisplaysTableAndRegisterButton()
        {
            driver.Navigate().GoToUrl("https://localhost:7063/Volunteers");

            // Wait for heading
            var heading = wait.Until(d => d.FindElement(By.TagName("h2")));
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
            driver.Navigate().GoToUrl("https://localhost:7063/Volunteers/Register");

            // Fill in form
            driver.FindElement(By.Id("Name")).SendKeys("Test Volunteer");
            driver.FindElement(By.Id("Age")).SendKeys("25");
            driver.FindElement(By.Id("Skills")).SendKeys("First Aid, Coordination");
            driver.FindElement(By.Id("Availability")).SendKeys("Weekends");

            // Submit form
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            // Wait until redirected to Volunteers list page
            wait.Until(d => d.Url.EndsWith("/Volunteers") || d.Url.EndsWith("/Volunteers/"));
            Assert.IsTrue(driver.Url.EndsWith("/Volunteers") || driver.Url.EndsWith("/Volunteers/"));

            // Wait for the table to include the new volunteer
            wait.Until(d =>
            {
                var table = d.FindElement(By.CssSelector("table.table-striped"));
                return table.Text.Contains("Test Volunteer");
            });

            var tableAfterSubmit = driver.FindElement(By.CssSelector("table.table-striped"));
            Assert.IsTrue(tableAfterSubmit.Text.Contains("Test Volunteer"));
        }
    }
}
