using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using static System.Net.WebRequestMethods;

namespace TestProject1
{
    public class Tests
    {
        private WebDriver WebDriver { get; set; } = null;

        private string Driverpath = @"C:\Users\milen\source\repos\GoogleMapsCodeTest\res\chromedriver.exe";//make url less direct

        private string BaseUrl { get; set; } = "https://www.google.de/maps";

        private string cookieSelector = "#yDmH0d > c-wiz > div > div > div > div.NIoIEf > div.G4njw > div.AIC7ge > div.CxJub > div.VtwTSb > form:nth-child(2)";

        [SetUp]
        public void Setup()
        {
            WebDriver = GetChromeDriver();
            WebDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(160);

            WebDriver.Navigate().GoToUrl(BaseUrl);
            WebDriver.FindElement(By.CssSelector(cookieSelector)).Click();
        }

        [TearDown]
        public void Teardown()
        {
            WebDriver.Quit();
        }

        [Test]
        public void PageNameTest()
        {
            //WebDriver.Navigate().GoToUrl(BaseUrl);
            Assert.AreEqual("Google Maps", WebDriver.Title);
        }

        [Test]
        public void Test2()
        {
            Assert.Pass();
        }

        //Return webdriver instead of chromedriver to be flexible if you want a none chrome driver
        private WebDriver GetChromeDriver()
        {
            var options = new ChromeOptions();

            return new ChromeDriver(Driverpath, options, TimeSpan.FromSeconds(300));
        }
    }
}