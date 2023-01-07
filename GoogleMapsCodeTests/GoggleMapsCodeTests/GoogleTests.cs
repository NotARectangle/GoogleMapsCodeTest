﻿using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Text.RegularExpressions;
using static System.Net.WebRequestMethods;

namespace GoogleMapsCodeTests
{
    public class Tests
    {
        private WebDriver WebDriver { get; set; } = null;

        private string Driverpath = @"..\res\chromedriver.exe";

        private string BaseUrl { get; set; } = "https://www.google.com/maps";

        private string cookieSelector = "#yDmH0d > c-wiz > div > div > div > div.NIoIEf > div.G4njw > div.AIC7ge > div.CxJub > div.VtwTSb > form:nth-child(2)";

        private string searchboxSelector = "#searchboxinput";
        private string magGlassSelector = "#searchbox > div.pzfvzf";
        private string placeNameSelector = "#QA0Szd > div > div > div.w6VYqd > div.bJzME.tTVLSc > div > div.e07Vkf.kA9KIf > div > div > div.TIHn2 > div.tAiQdd > div.lMbq3e > div:nth-child(1) > h1 > span:nth-child(1)";

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

        ///<summary>
        ///Good adress input, attraction
        ///</summary>
        [Test]
        public void GoodAddress1()
        {
            var input = WebDriver.FindElement(By.CssSelector(searchboxSelector));
            input.Clear();
            input.SendKeys("National Museum Scotland");

            WebDriver.FindElement(By.CssSelector(magGlassSelector)).Click();


            var outputname = WebDriver.FindElement(By.CssSelector(placeNameSelector));

            Assert.AreEqual("National Museum of Scotland", outputname.Text);
        }

        ///<summary>
        ///Testing full address is displayed
        ///</summary>
        [Test]
        public void GoodAddress1_1()
        {
        }

        ///<summary>
        ///Good Address Input, normal existing street
        ///</summary>
        [Test]
        public void GoodAddress2()
        {
            var input = WebDriver.FindElement(By.CssSelector(searchboxSelector));
            input.Clear();
            input.SendKeys("Stresemannstrasse 41 Hamburg");

            WebDriver.FindElement(By.CssSelector(magGlassSelector)).Click();

            var outputname = WebDriver.FindElement(By.CssSelector(placeNameSelector));

            Assert.AreEqual("Stresemannstraße 41", outputname.Text);
        }

        ///<summary>
        ///Good Address Input, Part of a Town
        ///</summary>
        [Test]
        public void GoodAddress3()
        {
            var input = WebDriver.FindElement(By.CssSelector(searchboxSelector));
            input.Clear();
            input.SendKeys("Museum Island Berlin");

            WebDriver.FindElement(By.CssSelector(magGlassSelector)).Click();


            var outputname = WebDriver.FindElement(By.CssSelector(placeNameSelector));
            
            Assert.AreEqual("Museum Island", outputname.Text);
        }

        ///<summary>
        ///Good Address Input, Country Input
        ///</summary>
        [Test]
        public void GoodAddress4()
        {
            var input = WebDriver.FindElement(By.CssSelector(searchboxSelector));
            input.Clear();
            input.SendKeys("Kenya");

            WebDriver.FindElement(By.CssSelector(magGlassSelector)).Click();

            var outputName = WebDriver.FindElement(By.CssSelector(placeNameSelector));

            Assert.AreEqual("Kenya", outputName.Text);
        }

        /// <summary>
        /// Good Address input, multiple outcomes option
        /// </summary>
        [Test]
        public void GoodAddress5()
        {
            string inputString = "Washington";

            var input = WebDriver.FindElement(By.CssSelector(searchboxSelector));
            input.Clear();
            input.SendKeys(inputString);

            WebDriver.FindElement(By.CssSelector(magGlassSelector)).Click();

            //Get results element
            var resultsForPlacenameSelector = "#QA0Szd > div > div > div.w6VYqd > div.bJzME.tTVLSc > div > div.e07Vkf.kA9KIf > div > div > div.m6QErb.DxyBCb.kA9KIf.dS8AEf.ecceSd > div.m6QErb.DxyBCb.kA9KIf.dS8AEf.ecceSd";

            var content = WebDriver.FindElement(By.CssSelector(resultsForPlacenameSelector)).Text;

            //Get Text content count the number of times Place name is found in element
            Regex goalRx = new Regex(inputString);

            MatchCollection matches = goalRx.Matches(content);

            //Count of results should be more than one, since placename occurs more than once on map
            Assert.IsTrue(matches.Count > 1);
        }

        /// <summary>
        /// 6. searchbar input is empty string
        /// </summary>
        [Test]
        public void BadAdress1()
        {
            string areaInformationSelector = "#passive-assist > div > div.J43RCf > div > div";

            var input = WebDriver.FindElement(By.CssSelector(searchboxSelector));
            input.Clear();

            WebDriver.FindElement(By.CssSelector(magGlassSelector)).Click();

            Assert.True(WebDriver.FindElement(By.CssSelector(areaInformationSelector)).Enabled);
        }

        /// <summary>
        /// 7. no capitalization used
        /// </summary>
        [Test]
        public void BadAdress2()
        {
            string inputString = "kenya";

            var input = WebDriver.FindElement(By.CssSelector(searchboxSelector));
            input.Clear();
            input.SendKeys(inputString);

            WebDriver.FindElement(By.CssSelector(magGlassSelector)).Click();

            var outputname = WebDriver.FindElement(By.CssSelector(placeNameSelector));

            Assert.AreEqual("Kenya", outputname.Text);
        }
        
        /// <summary>
        /// 8. misspelled input
        /// </summary>
        [Test]
        public void BadAdress3()
        {
            string inputString = "muuseums izland";

            var input = WebDriver.FindElement(By.CssSelector(searchboxSelector));
            input.Clear();
            input.SendKeys(inputString);

            WebDriver.FindElement(By.CssSelector(magGlassSelector)).Click();

            var outputname = WebDriver.FindElement(By.CssSelector(placeNameSelector));

            Assert.AreEqual("Museum Island", outputname.Text);
        }

        /// <summary>
        /// 9. Unknown place name
        /// </summary>
        [Test]
        public void BadAdress4()
        {
            string inputString = "albionssb";

            var input = WebDriver.FindElement(By.CssSelector(searchboxSelector));
            input.Clear();
            input.SendKeys(inputString);

            WebDriver.FindElement(By.CssSelector(magGlassSelector)).Click();

            string notFoundSelector = "#QA0Szd > div > div > div.w6VYqd > div.bJzME.tTVLSc > div > div.e07Vkf.kA9KIf > div > div > div:nth-child(2)";
            var outputContent = WebDriver.FindElement(By.CssSelector(notFoundSelector));

            //Get regex match words Google Maps, can`t find inputString in case text elements change
            Regex notFoundRx = new Regex(@"(Google Maps)[\W\w]+(can't find " + inputString + ")");

            Match notFoundMatch = notFoundRx.Match(outputContent.Text);

            Assert.IsTrue(notFoundMatch.Success);
        }

        /// <summary>
        /// 10. Html and Javascript input
        /// </summary>
        [Test]
        public void BadAdress5()
        {
            string inputString = "<script> alert(\"Alert! Alert!\");</script>";

            var input = WebDriver.FindElement(By.CssSelector(searchboxSelector));
            input.Clear();
            input.SendKeys(inputString);

            WebDriver.FindElement(By.CssSelector(magGlassSelector)).Click();

            string notFoundSelector = "#QA0Szd > div > div > div.w6VYqd > div.bJzME.tTVLSc > div > div.e07Vkf.kA9KIf > div > div > div:nth-child(2)";
            var outputContent = WebDriver.FindElement(By.CssSelector(notFoundSelector));

            //Get regex match words Google Maps, can`t find inputString in case text elements change
            Regex notFoundRx = new Regex(@"(Google Maps).+(can't find )");
            var outputText = outputContent.Text;

            Match notFoundMatch = notFoundRx.Match(outputContent.Text);

            Assert.IsTrue(notFoundMatch.Success);
        }

        /// <summary>
        /// 11. Special character input
        /// </summary>
        [Test]
        public void BadAdress6()
        {
            string inputString = "%&5%$%";

            var input = WebDriver.FindElement(By.CssSelector(searchboxSelector));
            input.Clear();
            input.SendKeys(inputString);

            WebDriver.FindElement(By.CssSelector(magGlassSelector)).Click();

            string notFoundSelector = "#QA0Szd > div > div > div.w6VYqd > div.bJzME.tTVLSc > div > div.e07Vkf.kA9KIf > div > div > div:nth-child(2)";
            var outputContent = WebDriver.FindElement(By.CssSelector(notFoundSelector));

            //Get regex match words Google Maps, can`t find inputString in case text elements change
            Regex notFoundRx = new Regex(@"(Google Maps)[\W\w]+(can't find %&5%\$%)");
            var outputText = outputContent.Text;

            Match notFoundMatch = notFoundRx.Match(outputContent.Text);

            Assert.IsTrue(notFoundMatch.Success);
        }

        //Return webdriver instead of chromedriver to be flexible if you want a none chrome driver
        private WebDriver GetChromeDriver()
        {
            ChromeOptions options = new ChromeOptions();

            options.AddArgument("--lang=en-ca");

            return new ChromeDriver(Driverpath, options, TimeSpan.FromSeconds(300));
        }
    }
}