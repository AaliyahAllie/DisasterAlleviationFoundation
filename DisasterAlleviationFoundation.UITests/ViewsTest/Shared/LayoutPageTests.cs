using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace DisasterAlleviationFoundation.UITests
{
    [TestClass]
    public class LayoutPageTests
    {
        private IWebDriver driver;

        [TestInitialize]
        public void Setup()
        {
            var options = new ChromeOptions();

            // Headless for CI/CD
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
        public void LayoutPage_NavigationAndFooterElementsExist()
        {
            // Navigate to home page
            driver.Navigate().GoToUrl("https://localhost:5001/"); // adjust your URL

            // Verify Navbar exists
            var navbar = driver.FindElement(By.CssSelector("nav.navbar"));
            Assert.IsNotNull(navbar);

            // Verify main navigation links
            Assert.IsNotNull(driver.FindElement(By.LinkText("Home")));
            Assert.IsNotNull(driver.FindElement(By.LinkText("About")));
            Assert.IsNotNull(driver.FindElement(By.LinkText("Disasters")));
            Assert.IsNotNull(driver.FindElement(By.LinkText("Donations")));
            Assert.IsNotNull(driver.FindElement(By.LinkText("Volunteers")));

            // Verify Admin Login and Account links (when signed out)
            Assert.IsNotNull(driver.FindElement(By.LinkText("Admin Login")));
            Assert.IsNotNull(driver.FindElement(By.LinkText("Login")));
            Assert.IsNotNull(driver.FindElement(By.LinkText("Register")));

            // Verify Footer
            var footer = driver.FindElement(By.CssSelector("footer.footer"));
            Assert.IsNotNull(footer);

            // Check Quick Links in footer
            Assert.IsNotNull(footer.FindElement(By.LinkText("Home")));
            Assert.IsNotNull(footer.FindElement(By.LinkText("About")));
            Assert.IsNotNull(footer.FindElement(By.LinkText("Disasters")));
        }

       
    }
}
