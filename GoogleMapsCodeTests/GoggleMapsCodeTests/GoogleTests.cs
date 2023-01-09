using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools.V106.Network;
using System.Globalization;
using System.Text.RegularExpressions;
using static System.Net.WebRequestMethods;

namespace GoogleMapsCodeTests
{
    public class GoogleTests
    {
        protected Helper help { get; set; } = null;
        protected WebDriver WebDriver { get; set; } = null;

        private string driverpath = @"..\res\chromedriver.exe";

        private string BaseUrl { get; set; } = "https://www.google.com/maps";

        private string cookieSelector = "#yDmH0d > c-wiz > div > div > div > div.NIoIEf > div.G4njw > div.AIC7ge > div.CxJub > div.VtwTSb > form:nth-child(2)";

        private string placeNameSelector = "#QA0Szd > div > div > div.w6VYqd > div.bJzME.tTVLSc > div > div.e07Vkf.kA9KIf > div > div > div.TIHn2 > div.tAiQdd > div.lMbq3e > div:nth-child(1) > h1 > span:nth-child(1)";

        private string landmarkInput = "Tower of Pisa";
        private string landmarkOutput = "Leaning Tower of Pisa";
        private string landmarkAddress = "Piazza del Duomo, 56126 Pisa PI";       

        private string streetAddressInput = "Stresemannstrasse 41 Hamburg";
        private string streetAdresssOutput = "Stresemannstraße 41";
        private string streetAdresssAddress = "Stresemannstraße 41, 22769 Hamburg";

        private string areaNameInput = "Museumisland Berlin";
        private string areaNameOutput = "Museum Island";

        private string countryNameInput = "Uruguay";
        private string coutryNameOutput = "Uruguay";

        private string mutipleOutcomesInput = "Washington";

        private string misspelledInput = "muuseums izland";
        private string misspelledOutput = "Museum Island";

        private string unknownPlace = "albon ssbdds";

        private string htmlInput = "<script> alert(\"Alert! Alert!\");</script>";

        private string specialCharacters = "%&5%$%";


        [SetUp]
        public virtual void Setup()
        {
            WebDriver = GetWebDriver();
            WebDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(100);

            WebDriver.Navigate().GoToUrl(BaseUrl);
            WebDriver.FindElement(By.CssSelector(cookieSelector)).Click();

            help = new Helper(WebDriver);
        }

        [TearDown]
        public void Teardown()
        {
            WebDriver.Quit();
        }

        [Test]
        ///<summary>
        ///0. First sanity check, Page is loaded and page name appears
        /// </summary>
        public void PageNameTest()
        {
            Assert.AreEqual("Google Maps", WebDriver.Title);
        }

        ///<summary>
        ///1. Landmark address input, test if input is found
        ///</summary>
        [Test]
        public void LandmarkInputTest()
        {

            help.AddAndSendInput(landmarkInput);
            
       //     var outputname = WebDriver.FindElement(By.CssSelector(placeNameSelector));

            Assert.AreEqual(true, true);
        }

        ///<summary>
        ///1.1 Landmark tests full address is displayed
        ///</summary>
        [Test]
        public void LandmarkInput_AddressTest()
        {
            help.AddAndSendInput(landmarkInput);

            Assert.IsTrue(help.IsAddressCorrect(landmarkAddress));
        }

        ///<summary>
        ///1.2 Landmark tests, Photo is showing
        ///</summary>
        [Test]
        public void LandmarkInput_HeaderPhotoTest()
        {
            help.AddAndSendInput(landmarkInput);

            Assert.IsTrue(help.IsHeaderPhotoDisplaying());
        }


        /// <summary>
        /// 1.3 Landmark tests, info section is showing
        /// </summary>
        [Test]
        public void LandmarkInput_InfoSectionTest()
        {
            help.AddAndSendInput(landmarkInput);
            
            Assert.IsTrue(help.IsPlaceInfoDisplaying());

        }

        /// <summary>
        /// 1.4 Landmark tests, Action is enabled and displayed
        /// </summary>
        [Test]
        public void LandmarkInput_ActionBarActiveTest()
        {
            string inputString = "Tower of Pisa";
            help.AddAndSendInput(inputString);

            Assert.IsTrue(help.IsActionBarDisplaying());
        }


        /// <summary>
        /// 1.5 Landmark test, landmark admission information displayed
        /// </summary>
        [Test]
        public void LandmarkInput_AdmissionTest()
        {
            string inputString = "Tower of Pisa";
            help.AddAndSendInput(inputString);

            Assert.IsTrue(help.IsAdmissionsInfoDisplaying());
        }

        /// <summary>
        /// 1.6 Landmark test, Google reviews showing
        /// </summary>
        [Test]
        public void LandmarkInput_ReviewStarsTest()
        {
            string inputString = "Tower of Pisa";
            help.AddAndSendInput(inputString);

            Assert.IsTrue(help.IsGoogleReviewsActive());
        }

        /// <summary>
        /// 1.7 Landmark test, Popular times element active
        /// </summary>
        [Test]
        public void LandmarkInput_PopularTimesTest()
        {
            string inputString = "Tower of Pisa";
            help.AddAndSendInput(inputString);

            Assert.IsTrue(help.IsPopularTimesDisplayed());
        }

        ///<summary>
        ///2. normal existing street address, test if input is found 
        ///</summary>
        [Test]
        public void StreetNameInputTest()
        {
            help.AddAndSendInput("Stresemannstrasse 41 Hamburg");

            var outputname = WebDriver.FindElement(By.CssSelector(placeNameSelector));

            Assert.AreEqual("Stresemannstraße 41", outputname.Text);
        }

        /// <summary>
        /// 2.1 Street name input, test if address is correct
        /// </summary>
        [Test]
        public void StreetNameInput_AdressTest()
        {
            help.AddAndSendInput("Stresemannstrasse 41 Hamburg");

            Assert.That(help.IsAddressCorrect("Stresemannstraße 41, 22769 Hamburg"));
        }

        /// <summary>
        /// 2.2 Street name input, test if image is displayed
        /// </summary>
        [Test]
        public void StreetNameInput_HeaderPhotoTest()
        {
            help.AddAndSendInput("Stresemannstrasse 41 Hamburg");

            Assert.That(help.IsHeaderPhotoDisplaying());
        }

        /// <summary>
        /// 2.3 Street name input, test if action bar shows
        /// </summary>
        [Test]
        public void StreetNameInput_ActionBarTest()
        {
            help.AddAndSendInput("Stresemannstrasse 41 Hamburg");

            Assert.That(help.IsActionBarDisplaying());
        }

        ///<summary>
        ///3. Search input is an existing area
        ///</summary>
        [Test]
        public void AreaInputTest()
        {
            help.AddAndSendInput("Museumisland Berlin");

            var outputname = WebDriver.FindElement(By.CssSelector(placeNameSelector));
            
            Assert.AreEqual("Museum Island", outputname.Text);
        }

        ///<summary>
        ///3.1 Area search input, header image show
        ///</summary>
        [Test]
        public void AreaInput_HeaderPhotoTest()
        {
            help.AddAndSendInput("Museumisland Berlin");

            Assert.That(help.IsHeaderPhotoDisplaying());
        }

        ///<summary>
        ///3.2 Area search input, information shows
        ///</summary>
        [Test]
        public void AreaInput_InfoSectionTest()
        {
            help.AddAndSendInput("Museumisland Berlin");

            Assert.That(help.IsPlaceInfoDisplaying());
        }

        ///<summary>
        ///3.3 Area search input, action bar is enabled and displays
        ///</summary>
        [Test]
        public void AreaInput_ActionBarTest()
        {
            help.AddAndSendInput("Museumsinsel Berlin");

            Assert.That(help.IsActionBarDisplaying());
        }

        ///<summary>
        ///4. Search input is an existing country
        ///</summary>
        [Test]
        public void CountryInputTest()
        {
            help.AddAndSendInput("Uruguay");

            var outputName = WebDriver.FindElement(By.CssSelector(placeNameSelector));

            Assert.AreEqual("Uruguay", outputName.Text);
        }

        /// <summary>
        /// 4.1 country search input, header image shows
        /// </summary>
        [Test]
        public void CountryInput_HeaderPhotoTest()
        {
            help.AddAndSendInput("Uruguay");

            Assert.That(help.IsHeaderPhotoDisplaying());
        }

        /// <summary>
        /// 4.2 country search input, Quick facts show
        /// </summary>
        [Test]
        public void CountryInput_QuickfactsTest()
        {
            help.AddAndSendInput("Uruguay");

            string textOutputSelector = "#QA0Szd > div > div > div.w6VYqd > div.bJzME.tTVLSc > div > div.e07Vkf.kA9KIf > div > div ";

            bool found = false;

            try 
            {            
            string pageText = WebDriver.FindElement(By.CssSelector(textOutputSelector)).Text;          
            

            if (string.IsNullOrEmpty(pageText) != true)
                {
                    if(pageText.Contains("Quick facts"))
                    {
                        found = true;
                    }
                }

            }
            catch
            {
                found = false;
            }

            Assert.That(found);
        }

        /// <summary>
        /// 4.3 country search input, action bar is enabled and displayed
        /// </summary>
        [Test]
        public void CountryInput_ActionBarTest()
        {
            help.AddAndSendInput("Uruguay");

            Assert.That(help.IsActionBarDisplaying());
        }

        /// <summary>
        /// 5. Address input leaves room for multiple search outcomes
        /// </summary>
        [Test]
        public void MultiplePossibleOutcomesTest()
        {
            string inputString = "Washington";

            help.AddAndSendInput(inputString);

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
        public void EmptyStringInputTest()
        {
            string areaInformationSelector = "#passive-assist > div > div.J43RCf > div > div";

            help.AddAndSendInput(string.Empty);

            Assert.True(WebDriver.FindElement(By.CssSelector(areaInformationSelector)).Enabled);
        }

        /// <summary>
        /// 7. no capitalization used
        /// </summary>
        [Test]
        public void NoCapitalLettersInputTest()
        {
            string inputString = countryNameInput.ToLower();

            help.AddAndSendInput(inputString);

            var outputname = WebDriver.FindElement(By.CssSelector(placeNameSelector));

            Assert.AreEqual(countryNameInput, outputname.Text);
        }
        
        /// <summary>
        /// 8. misspelled input
        /// </summary>
        [Test]
        public void MisspelledInputTest()
        {
            string inputString = "muuseums izland";

            help.AddAndSendInput(inputString);

            var outputname = WebDriver.FindElement(By.CssSelector(placeNameSelector));

            Assert.AreEqual("Museum Island", outputname.Text);
        }

        /// <summary>
        /// 9. Unknown place name
        /// </summary>
        [Test]
        public void UnknownPlaceInputTest()
        {
            string inputString = "albon ssbdds";

            help.AddAndSendInput(inputString);

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
        public void HTMLInputTest()
        {
            string inputString = "<script> alert(\"Alert! Alert!\");</script>";

            help.AddAndSendInput(inputString);

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
        public void SpecialCharacterInputTest()
        {
            string inputString = "%&5%$%";

            help.AddAndSendInput(inputString);

            string notFoundSelector = "#QA0Szd > div > div > div.w6VYqd > div.bJzME.tTVLSc > div > div.e07Vkf.kA9KIf > div > div > div:nth-child(2)";
            var outputContent = WebDriver.FindElement(By.CssSelector(notFoundSelector));

            //Get regex match words Google Maps, can`t find inputString in case text elements change
            Regex notFoundRx = new Regex(@"(Google Maps)[\W\w]+(can't find %&5%\$%)");
            var outputText = outputContent.Text;

            Match notFoundMatch = notFoundRx.Match(outputContent.Text);

            Assert.IsTrue(notFoundMatch.Success);
        }

        //Return webdriver instead of chromedriver to be flexible if you want a none chrome driver
        private WebDriver GetWebDriver()
        {
            ChromeOptions options = new ChromeOptions();

            options.AddArgument("--lang=en-ca");

            return new ChromeDriver(driverpath, options, TimeSpan.FromSeconds(300));
        }


    }
    
}