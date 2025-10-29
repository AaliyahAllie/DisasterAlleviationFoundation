using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace DisasterAlleviationFoundation.UITests
{
    [TestClass]
    public class HomePageTests : BaseUITest
    {
        [TestMethod]
        public void HomePage_StatsAndAssignments_LoadCorrectly()
        {
            driver.Navigate().GoToUrl("https://localhost:7063/Home/Index");

            // Check total volunteers card
            var volunteersCard = driver.FindElement(By.XPath("//h5[text()='Total Volunteers']"));
            Assert.IsNotNull(volunteersCard);

            // Check total incidents card
            var incidentsCard = driver.FindElement(By.XPath("//h5[text()='Total Incidents']"));
            Assert.IsNotNull(incidentsCard);

            // Check total donations card
            var donationsCard = driver.FindElement(By.XPath("//h5[text()='Total Donations']"));
            Assert.IsNotNull(donationsCard);

            // Check Volunteer Assignments table or empty text
            var assignmentsTable = driver.FindElements(By.CssSelector("table"));
            var emptyText = driver.FindElements(By.CssSelector("p.text-muted"));

            Assert.IsTrue(assignmentsTable.Count > 0 || emptyText.Count > 0);
        }
    }
}
