using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace DisasterAlleviationFoundation.UITests
{
    [TestClass]
    public class DonationIndexTests : BaseUITest
    {
        [TestMethod]
        public void Index_DonationList_DisplaysDonations()
        {
            driver.Navigate().GoToUrl("https://localhost:7063/Donations");

            // Check table exists
            var table = driver.FindElement(By.CssSelector("table.table"));
            Assert.IsNotNull(table);

            // Check at least one row exists
            var rows = table.FindElements(By.CssSelector("tbody tr"));
            Assert.IsTrue(rows.Count > 0);
        }

        
    }
}
