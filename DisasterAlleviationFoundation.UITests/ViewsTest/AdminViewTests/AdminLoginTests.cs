using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace DisasterAlleviationFoundation.UITests
{
    [TestClass]
    public class AdminLoginTests : BaseUITest
    {
        [TestMethod]
        public void AdminLogin_ValidCredentials_RedirectsToDashboard()
        {
            // 1. Navigate to Admin Login page
            driver.Navigate().GoToUrl("https://localhost:5001/Admin/Login");

            // 2. Fill in username and password
            driver.FindElement(By.Id("username")).SendKeys("adminuser");
            driver.FindElement(By.Id("password")).SendKeys("Admin123!");

            // 3. Click Login button
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            // 4. Wait for redirect (simple wait)
            System.Threading.Thread.Sleep(2000);

            // 5. Verify redirect to Admin Dashboard (update path if different)
            Assert.IsTrue(driver.Url.Contains("/Admin/Dashboard"));
        }
    }
}
