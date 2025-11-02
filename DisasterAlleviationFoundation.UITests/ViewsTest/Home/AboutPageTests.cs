using System.Net.Http;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace DisasterAlleviationFoundation.UITests
{
    [TestClass]
    public class AboutPageTests : BaseUITest
    {
        private const string BaseUrl = "https://localhost:7063";

        [TestMethod]
        public void AboutPage_ContentIsDisplayed()
        {
            WaitForServerToBeReady(BaseUrl);

            driver.Navigate().GoToUrl($"{BaseUrl}/Home/About");

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

        private void WaitForServerToBeReady(string url, int timeoutSeconds = 10)
        {
            var httpClient = new HttpClient();
            var timeout = DateTime.Now.AddSeconds(timeoutSeconds);
            while (DateTime.Now < timeout)
            {
                try
                {
                    var response = httpClient.GetAsync(url).Result;
                    if (response.IsSuccessStatusCode) return;
                }
                catch
                {
                    // ignore connection errors
                }
                Thread.Sleep(500);
            }
            throw new Exception($"Server not responding at {url}");
        }
    }
}
