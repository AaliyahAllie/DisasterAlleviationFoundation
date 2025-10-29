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
            driver.Navigate().GoToUrl("https://localhost:5001/Donations");

            // Check table exists
            var table = driver.FindElement(By.CssSelector("table.table"));
            Assert.IsNotNull(table);

            // Check at least one row exists
            var rows = table.FindElements(By.CssSelector("tbody tr"));
            Assert.IsTrue(rows.Count > 0);
        }

        [TestMethod]
        public void UpdateDonationStatus_ChangeStatus_Success()
        {
            driver.Navigate().GoToUrl("https://localhost:5001/Donations");

            // Find first donation row
            var firstRow = driver.FindElement(By.CssSelector("table tbody tr"));

            // Select status dropdown
            var statusSelect = firstRow.FindElement(By.CssSelector("select[name='status']"));
            statusSelect.FindElement(By.CssSelector("option[value='Distributed']")).Click();

            // Click Update button
            firstRow.FindElement(By.CssSelector("button[type='submit']")).Click();

            System.Threading.Thread.Sleep(2000);

            // Optional: Verify the status text changed (simple check)
            var updatedStatus = firstRow.FindElement(By.CssSelector("td:nth-child(4)")).Text;
            Assert.AreEqual("Distributed", updatedStatus);
        }
    }
}
