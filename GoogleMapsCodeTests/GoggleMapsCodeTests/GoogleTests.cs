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

        private string landmarkInput = "Leaning Tower of Pisa";
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

            Assert.IsTrue(help.IsOutputCorrect(landmarkOutput));
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
            help.AddAndSendInput(landmarkInput);

            Assert.IsTrue(help.IsActionBarDisplaying());
        }


        /// <summary>
        /// 1.5 Landmark test, landmark admission information displayed
        /// </summary>
        [Test]
        public void LandmarkInput_AdmissionTest()
        {
            help.AddAndSendInput(landmarkInput);

            Assert.IsTrue(help.IsAdmissionsInfoDisplaying());
        }

        /// <summary>
        /// 1.6 Landmark test, Google reviews showing
        /// </summary>
        [Test]
        public void LandmarkInput_ReviewStarsTest()
        {
            help.AddAndSendInput(landmarkInput);

            Assert.IsTrue(help.IsGoogleReviewsActive());
        }

        /// <summary>
        /// 1.7 Landmark test, Popular times element active
        /// </summary>
        [Test]
        public void LandmarkInput_PopularTimesTest()
        {
            help.AddAndSendInput(landmarkInput);

            Assert.IsTrue(help.IsPopularTimesDisplayed());
        }

        ///<summary>
        ///2. normal existing street address, test if input is found 
        ///</summary>
        [Test]
        public void StreetNameInputTest()
        {
            help.AddAndSendInput(streetAddressInput);

            //var outputname = WebDriver.FindElement(By.CssSelector(placeNameSelector));

            Assert.IsTrue(help.IsOutputCorrect(streetAdresssOutput));
        }

        /// <summary>
        /// 2.1 Street name input, test if address is correct
        /// </summary>
        [Test]
        public void StreetNameInput_AdressTest()
        {
            help.AddAndSendInput(streetAddressInput);

            Assert.That(help.IsAddressCorrect(streetAdresssAddress));
        }

        /// <summary>
        /// 2.2 Street name input, test if image is displayed
        /// </summary>
        [Test]
        public void StreetNameInput_HeaderPhotoTest()
        {
            help.AddAndSendInput(streetAddressInput);

            Assert.That(help.IsHeaderPhotoDisplaying());
        }

        /// <summary>
        /// 2.3 Street name input, test if action bar shows
        /// </summary>
        [Test]
        public void StreetNameInput_ActionBarTest()
        {
            help.AddAndSendInput(streetAddressInput);

            Assert.That(help.IsActionBarDisplaying());
        }

        ///<summary>
        ///3. Search input is an existing area
        ///</summary>
        [Test]
        public void AreaInputTest()
        {
            help.AddAndSendInput(areaNameInput);
            
            Assert.IsTrue(help.IsOutputCorrect(areaNameOutput));
        }

        ///<summary>
        ///3.1 Area search input, header image show
        ///</summary>
        [Test]
        public void AreaInput_HeaderPhotoTest()
        {
            help.AddAndSendInput(areaNameInput);

            Assert.That(help.IsHeaderPhotoDisplaying());
        }

        ///<summary>
        ///3.2 Area search input, information shows
        ///</summary>
        [Test]
        public void AreaInput_InfoSectionTest()
        {
            help.AddAndSendInput(areaNameInput);

            Assert.That(help.IsPlaceInfoDisplaying());
        }

        ///<summary>
        ///3.3 Area search input, action bar is enabled and displays
        ///</summary>
        [Test]
        public void AreaInput_ActionBarTest()
        {
            help.AddAndSendInput(areaNameInput);

            Assert.That(help.IsActionBarDisplaying());
        }

        ///<summary>
        ///4. Search input is an existing country
        ///</summary>
        [Test]
        public void CountryInputTest()
        {
            help.AddAndSendInput(countryNameInput);

            Assert.IsTrue(help.IsOutputCorrect(countryNameInput));
        }

        /// <summary>
        /// 4.1 country search input, header image shows
        /// </summary>
        [Test]
        public void CountryInput_HeaderPhotoTest()
        {
            help.AddAndSendInput(countryNameInput);

            Assert.That(help.IsHeaderPhotoDisplaying());
        }

        /// <summary>
        /// 4.2 country search input, Quick facts show
        /// </summary>
        [Test]
        public void CountryInput_QuickfactsTest()
        {
            help.AddAndSendInput(countryNameInput);

            Assert.That(help.DoesPageTextContain("Quick facts"));
        }

        /// <summary>
        /// 4.3 country search input, action bar is enabled and displayed
        /// </summary>
        [Test]
        public void CountryInput_ActionBarTest()
        {
            help.AddAndSendInput(countryNameInput);

            Assert.That(help.IsActionBarDisplaying());
        }

        /// <summary>
        /// 5. Address input leaves room for multiple search outcomes
        /// </summary>
        [Test]
        public void MultiplePossibleOutcomesTest()
        {
            help.AddAndSendInput(mutipleOutcomesInput);

            //Count of results should be more than one, since placename occurs more than once on map
            Assert.IsTrue(help.CountOccuranceOnPage(mutipleOutcomesInput) > 1);
        }

        /// <summary>
        /// 6. searchbar input is empty string
        /// </summary>
        [Test]
        public void EmptyStringInputTest()
        {

            string areaInformationSelector = "#passive-assist > div > div.J43RCf > div > div";

            help.AddAndSendInput(string.Empty);

            try { 
            bool areaInformationEnabled = WebDriver.FindElement(By.CssSelector(areaInformationSelector)).Enabled;

            Assert.IsTrue(areaInformationEnabled);

            }
            catch
            {
                Assert.Fail();
            }
        }

        /// <summary>
        /// 7. no capitalization used
        /// </summary>
        [Test]
        public void NoCapitalLettersInputTest()
        {
            string inputString = countryNameInput.ToLower();

            help.AddAndSendInput(inputString);

            Assert.IsTrue(help.IsOutputCorrect(countryNameInput));
        }
        
        /// <summary>
        /// 8. misspelled input
        /// </summary>
        [Test]
        public void MisspelledInputTest()
        {
            help.AddAndSendInput(misspelledInput);

            Assert.IsTrue(help.IsOutputCorrect(areaNameOutput));
        }

        /// <summary>
        /// 9. Unknown place name
        /// </summary>
        [Test]
        public void UnknownPlaceInputTest()
        {
            help.AddAndSendInput(unknownPlace);

            string notFoundSelector = "#QA0Szd > div > div > div.w6VYqd > div.bJzME.tTVLSc > div > div.e07Vkf.kA9KIf > div > div > div:nth-child(2)";
            var outputContent = WebDriver.FindElement(By.CssSelector(notFoundSelector));

            //Get regex match words Google Maps, can`t find inputString in case text elements change
            Regex notFoundRx = new Regex(@"(Google Maps)[\W\w]+(can't find " + unknownPlace + ")");

            Match notFoundMatch = notFoundRx.Match(outputContent.Text);

            Assert.IsTrue(notFoundMatch.Success);
        }

        /// <summary>
        /// 10. Html and Javascript input
        /// </summary>
        [Test]
        public void HTMLInputTest()
        {
            help.AddAndSendInput(htmlInput);

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

            help.AddAndSendInput(specialCharacters);

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