using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
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

            var outputname = WebDriver.FindElement(By.CssSelector(placeNameSelector));

            Assert.AreEqual("Kenya", outputname.Text);
        }

        /// <summary>
        /// Good Address input, multiple outcomes option
        /// </summary>
        [Test]
        public void GoodAddress5()
        {
            var input = WebDriver.FindElement(By.CssSelector(searchboxSelector));
            input.Clear();
            input.SendKeys("Narnia");

            WebDriver.FindElement(By.CssSelector(magGlassSelector)).Click();

            var resultsForPlacenameSelector = "#QA0Szd > div > div > div.w6VYqd > div.bJzME.tTVLSc > div > div.e07Vkf.kA9KIf > div > div > div.m6QErb.DxyBCb.kA9KIf.dS8AEf.ecceSd > div.m6QErb.DxyBCb.kA9KIf.dS8AEf.ecceSd";

            var content = WebDriver.FindElement(By.CssSelector(resultsForPlacenameSelector)).Text;

            //Get Text content count the number of times Place name is found in element


            Console.WriteLine(content);

            Assert.Pass();
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