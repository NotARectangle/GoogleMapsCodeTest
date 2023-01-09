using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleMapsCodeTests
{
    public class WinEdgeTests : GoogleTests
    {
           
        private string driverpath = @"..\res\msedgedriver.exe";

       // Helper help = null;

        private string BaseUrl { get; set; } = "https://www.google.com/maps";

        private string cookieSelector = "#yDmH0d > c-wiz > div > div > div > div.NIoIEf > div.G4njw > div.AIC7ge > div.CxJub > div.VtwTSb > form:nth-child(2)";

        
        [SetUp]
        public override void Setup()
        {
            WebDriver = GetWebDriver();
            WebDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(100);

            WebDriver.Navigate().GoToUrl(BaseUrl);
            WebDriver.FindElement(By.CssSelector(cookieSelector)).Click();

            help = new Helper(WebDriver);
        }

        //Return webdriver instead of Edgedriver to be flexible if you want a none chrome driver
        private WebDriver GetWebDriver()
        {
            EdgeOptions options = new EdgeOptions();

            options.AddArgument("--lang=en-ca");

            return new EdgeDriver(driverpath, options);
        }


    }
}
