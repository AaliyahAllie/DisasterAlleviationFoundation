using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace DisasterAlleviationFoundation.UITests
{
    [TestClass]
    public class DonationCreateTests : BaseUITest
    {
        [TestMethod]
        public void CreateDonation_ValidInput_RedirectsToIndex()
        {
            driver.Navigate().GoToUrl("https://localhost:5001/Donations/Create");

            // Select Category
            var category = driver.FindElement(By.Id("Category"));
            category.FindElement(By.CssSelector("option[value='Food']")).Click();

            // Enter Quantity
            driver.FindElement(By.Id("Quantity")).SendKeys("10");

            // Enter Description
            driver.FindElement(By.Id("Description")).SendKeys("Canned food for flood victims");

            // Submit form
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            System.Threading.Thread.Sleep(2000);

            // Assert redirect to Index
            Assert.IsTrue(driver.Url.Contains("/Donations"));
        }
    }
}
