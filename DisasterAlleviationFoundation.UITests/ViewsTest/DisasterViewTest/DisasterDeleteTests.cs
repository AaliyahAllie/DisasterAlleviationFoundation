using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace DisasterAlleviationFoundation.UITests
{
    [TestClass]
    public class DisasterDeleteTests : BaseUITest
    {
        [TestMethod]
        public void DeleteDisaster_ExistingDisaster_SuccessfulRedirect()
        {
            driver.Navigate().GoToUrl("https://localhost:5001/Disasters/Delete/1"); // Replace 1 with an existing DisasterId

            // Click Delete button
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            // Wait for redirect
            System.Threading.Thread.Sleep(2000);

            // Assert redirected to index page
            Assert.IsTrue(driver.Url.Contains("/Disasters"));
        }
    }
}
