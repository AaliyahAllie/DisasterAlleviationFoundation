using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace DisasterAlleviationFoundation.UITests
{
    [TestClass]
    public class BaseUITest
    {
        protected IWebDriver driver;

        [TestInitialize]
        public void Setup()
        {
            var options = new ChromeOptions();

            // Use headless mode for CI/CD (Azure)
            if (Environment.GetEnvironmentVariable("AZURE_PIPELINES") == "true")
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
    }
}
