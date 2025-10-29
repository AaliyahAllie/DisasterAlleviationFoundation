using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace DisasterAlleviationFoundation.UITests
{
    [TestClass]
    public class AboutPageTests : BaseUITest
    {
        [TestMethod]
        public void AboutPage_ContentIsDisplayed()
        {
            driver.Navigate().GoToUrl("https://localhost:5001/Home/About");

            // Check hero section
            var hero = driver.FindElement(By.CssSelector(".bg-light"));
            Assert.IsTrue(hero.Text.Contains("About Disaster Alleviation"));

            // Check Mission section
            var mission = driver.FindElement(By.XPath("//h3[text()='Mission']"));
            Assert.IsNotNull(mission);

            // Check Features list
            var features = driver.FindElements(By.CssSelector("ul li"));
            Assert.IsTrue(features.Count >= 5);
        }
    }
}
