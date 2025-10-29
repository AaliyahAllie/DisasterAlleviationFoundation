using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace DisasterAlleviationFoundation.UITests
{
    [TestClass]
    public class AdminVolunteersTests
    {
        private IWebDriver driver;

        [TestInitialize]
        public void Setup()
        {
            var options = new ChromeOptions();
            // Headless mode for CI/CD
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
        public void AdminVolunteersPage_DisplaysTableAndButtons()
        {
            // Navigate to Admin Volunteers dashboard
            driver.Navigate().GoToUrl("https://localhost:5001/Admin/Volunteers");

            // Verify heading
            var heading = driver.FindElement(By.TagName("h2"));
            Assert.AreEqual("Admin Dashboard - Volunteers", heading.Text);

            // Verify table exists
            var table = driver.FindElement(By.CssSelector("table.table-bordered"));
            Assert.IsNotNull(table);

            // Verify Assign/Edit and Delete buttons exist
            var assignButtons = driver.FindElements(By.LinkText("Assign/Edit"));
            var deleteButtons = driver.FindElements(By.CssSelector("button.btn-danger"));

            Assert.IsTrue(assignButtons.Count > 0);
            Assert.IsTrue(deleteButtons.Count > 0);
        }

        [TestMethod]
        public void AssignVolunteerPage_SubmitForm_ShowsSuccess()
        {
            // Navigate to Assign/Edit page for a test volunteer (ID = 1)
            driver.Navigate().GoToUrl("https://localhost:5001/Admin/AssignVolunteer?volunteerId=1");

            // Fill form fields
            driver.FindElement(By.Id("TaskDescription")).SendKeys("Distribute medical kits");
            driver.FindElement(By.Id("DisasterId")).SendKeys("1"); // Select first disaster
            driver.FindElement(By.Id("Location")).SendKeys("Downtown");

            // Submit form
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            // Verify redirect back to dashboard
            Assert.IsTrue(driver.Url.EndsWith("/Admin/Volunteers"));
        }
    }
}
