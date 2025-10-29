using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace DisasterAlleviationFoundation.UITests
{
    [TestClass]
    public class DisasterIndexTests : BaseUITest
    {
        [TestMethod]
        public void Index_DisasterList_DisplaysReports()
        {
            driver.Navigate().GoToUrl("https://localhost:5001/Disasters");

            // Check table exists
            var table = driver.FindElement(By.CssSelector("table.table"));
            Assert.IsNotNull(table);

            // Check at least one row exists (optional)
            var rows = table.FindElements(By.CssSelector("tbody tr"));
            Assert.IsTrue(rows.Count > 0);
        }

        [TestMethod]
        public void FilterDisasters_ByLocationAndSeverity_Works()
        {
            driver.Navigate().GoToUrl("https://localhost:5001/Disasters");

            // Fill location filter
            driver.FindElement(By.Name("location")).SendKeys("City A");

            // Fill severity filter
            var severitySelect = driver.FindElement(By.Name("severity"));
            severitySelect.FindElement(By.CssSelector("option[value='High']")).Click();

            // Click Filter
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            // Wait for page refresh
            System.Threading.Thread.Sleep(2000);

            // Check if table rows exist
            var rows = driver.FindElements(By.CssSelector("table tbody tr"));
            Assert.IsTrue(rows.Count > 0);
        }
    }
}
