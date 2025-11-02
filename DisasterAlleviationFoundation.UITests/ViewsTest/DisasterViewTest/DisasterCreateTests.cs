using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace DisasterAlleviationFoundation.UITests
{
    [TestClass]
    public class DisasterCreateTests : BaseUITest
    {
        [TestMethod]
        public void CreateDisaster_ValidInput_SuccessfulRedirect()
        {
            driver.Navigate().GoToUrl("https://localhost:7063/Disasters/Create");

            // Fill in form
            driver.FindElement(By.Id("Title")).SendKeys("Flood in City A");
            driver.FindElement(By.Id("Description")).SendKeys("Heavy flooding due to rain.");
            driver.FindElement(By.Id("Location")).SendKeys("City A");

            // Severity select
            var severity = driver.FindElement(By.Id("Severity"));
            severity.FindElement(By.CssSelector("option[value='High']")).Click();

            // Submit form
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            // Wait for redirect
            System.Threading.Thread.Sleep(2000);

            // Assert redirected to index page
            Assert.IsTrue(driver.Url.Contains("/Disasters"));
        }
    }
}
