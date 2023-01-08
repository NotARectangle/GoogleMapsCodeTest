using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools.V106.Network;
using System.Globalization;
using System.Text.RegularExpressions;
using static System.Net.WebRequestMethods;

namespace GoogleMapsCodeTests
{
    public class Tests
    {
        protected WebDriver WebDriver { get; set; } = null;

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
        ///<summary>
        ///0. First sanity check, Page is loaded and page name appears
        /// </summary>
        public void PageNameTest()
        {
            //WebDriver.Navigate().GoToUrl(BaseUrl);
            Assert.AreEqual("Google Maps", WebDriver.Title);
        }

        ///<summary>
        ///1. Landmark address input, test if input is found
        ///</summary>
        [Test]
        public void LandmarkInputTest()
        {
            string inputString = "Tower of Pisa";

            AddAndSendInput(inputString);

            var outputname = WebDriver.FindElement(By.CssSelector(placeNameSelector));

            Assert.AreEqual("Leaning Tower of Pisa", outputname.Text);
        }

        ///<summary>
        ///1.1 Landmark tests full address is displayed
        ///</summary>
        [Test]
        public void LandmarkInput_AddressTest()
        {
            string inputString = "Tower of Pisa";

            AddAndSendInput(inputString);

            Assert.AreEqual("Piazza del Duomo, 56126 Pisa PI, Italy", GetAdressContent());
        }

        ///<summary>
        ///1.2 Landmark tests, Photo is showing
        ///</summary>
        [Test]
        public void LandmarkInput_HeaderPhotoTest()
        {
            string inputString = "Tower of Pisa";
            AddAndSendInput(inputString);

            Assert.IsTrue(IsHeaderPhotoDisplaying());
        }


        /// <summary>
        /// 1.3 Landmark tests, info section is showing
        /// </summary>
        [Test]
        public void LandmarkInput_InfoSectionTest()
        {
            string inputString = "Tower of Pisa";
            AddAndSendInput(inputString);

            Assert.IsTrue(IsPlaceInfoDisplaying());
        }

        /// <summary>
        /// 1.4 Landmark tests, Action is enabled and displayed
        /// </summary>
        [Test]
        public void LandmarkInput_ActionBarActiveTest()
        {
            string inputString = "Tower of Pisa";
            AddAndSendInput(inputString);

            Assert.IsTrue(IsActionBarDisplaying());
        }


        /// <summary>
        /// 1.5 Landmark test, landmark admission information displayed
        /// </summary>
        [Test]
        public void LandmarkInput_AdmissionTest()
        {
            string inputString = "Tower of Pisa";
            AddAndSendInput(inputString);

            Assert.IsTrue(IsAdmissionsInfoDisplaying());
        }

        /// <summary>
        /// 1.6 Landmark test, Google reviews showing
        /// </summary>
        [Test]
        public void LandmarkInput_ReviewStarsTest()
        {
            string inputString = "Tower of Pisa";
            AddAndSendInput(inputString);

            Assert.IsTrue(IsGoogleReviewsActive());
        }

        /// <summary>
        /// 1.7 Landmark test, Popular times element active
        /// </summary>
        [Test]
        public void LandmarkInput_PopularTimesTest()
        {
            string inputString = "Tower of Pisa";
            AddAndSendInput(inputString);

            Assert.IsTrue(IsPopularTimesDisplayed());
        }

        ///<summary>
        ///2. normal existing street address, test if input is found 
        ///</summary>
        [Test]
        public void StreetNameTest()
        {
            AddAndSendInput("Stresemannstrasse 41 Hamburg");

            var outputname = WebDriver.FindElement(By.CssSelector(placeNameSelector));

            Assert.AreEqual("Stresemannstraße 41", outputname.Text);
        }

        ///<summary>
        ///3. Search input is an existing area
        ///</summary>
        [Test]
        public void AreaInputTest()
        {
            AddAndSendInput("Museum Island Berlin");

            var outputname = WebDriver.FindElement(By.CssSelector(placeNameSelector));
            
            Assert.AreEqual("Museum Island", outputname.Text);
        }

        ///<summary>
        ///4. Search input is an existing country
        ///</summary>
        [Test]
        public void CountryInputTest()
        {
            AddAndSendInput("Kenya");

            var outputName = WebDriver.FindElement(By.CssSelector(placeNameSelector));

            Assert.AreEqual("Kenya", outputName.Text);
        }

        /// <summary>
        /// 5. Address input leaves room for multiple search outcomes
        /// </summary>
        [Test]
        public void MultiplePossibleOutcomesTest()
        {
            string inputString = "Washington";

            AddAndSendInput(inputString);

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

            AddAndSendInput(string.Empty);

            Assert.True(WebDriver.FindElement(By.CssSelector(areaInformationSelector)).Enabled);
        }

        /// <summary>
        /// 7. no capitalization used
        /// </summary>
        [Test]
        public void NoCapitalLettersInputTest()
        {
            string inputString = "kenya";

            AddAndSendInput(inputString);

            var outputname = WebDriver.FindElement(By.CssSelector(placeNameSelector));

            Assert.AreEqual("Kenya", outputname.Text);
        }
        
        /// <summary>
        /// 8. misspelled input
        /// </summary>
        [Test]
        public void MisspelledInputTest()
        {
            string inputString = "muuseums izland";

            AddAndSendInput(inputString);

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

            AddAndSendInput(inputString);

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

            AddAndSendInput(inputString);

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

            AddAndSendInput(inputString);

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

        private void AddAndSendInput(string inputString)
        {
            var input = WebDriver.FindElement(By.CssSelector(searchboxSelector));
            input.Clear();
            input.SendKeys(inputString);

            WebDriver.FindElement(By.CssSelector(magGlassSelector)).Click();
        }

        private string GetAdressContent()
        {
            string addressContentSelector = "#QA0Szd > div > div > div.w6VYqd > div.bJzME.tTVLSc > div > div.e07Vkf.kA9KIf > div > div > div:nth-child(11) > div:nth-child(3) > button > div.AeaXub > div.rogA2c";

            Thread.Sleep(5000);

            var content = WebDriver.FindElement(By.CssSelector(addressContentSelector));

            if (content != null)
            {
                return content.Text;
            }
            else
            {
                return string.Empty;
            }
        }

        private bool IsHeaderPhotoDisplaying()
        {
            string PhotoSelector = "#QA0Szd > div > div > div.w6VYqd > div.bJzME.tTVLSc > div > div.e07Vkf.kA9KIf > div > div > div.ZKCDEc > div.RZ66Rb.FgCUCc > button > img";

            //Needed to add more time for information to appear
            Thread.Sleep(5000);

            var content = WebDriver.FindElement(By.CssSelector(PhotoSelector));

            if (content != null) {
                return content.Displayed;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Return true if the place info section is displaying and text is contained in it
        /// </summary>
        private bool IsPlaceInfoDisplaying()
        {
            string infoTextSelector = "#QA0Szd > div > div > div.w6VYqd > div.bJzME.tTVLSc > div > div.e07Vkf.kA9KIf > div > div > div.y0K5Df > button > div > div:nth-child(1) > div.PYvSYb > span";

            var content = WebDriver.FindElement(By.CssSelector(infoTextSelector));

            if (content == null)
            {
                return false;
            }
            else if(content.Displayed == false) 
            {
                return false;
            }
            else
            {
                string infoText = content.Text;
                if (infoText != string.Empty)
                {
                    if (infoText.Length > 0) {
                        return true;
                    }
                    else
                    { 
                        return false; 
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Returns true if the actionbar is enabled and is displayed
        /// </summary>
        private bool IsActionBarDisplaying()
        {
            string actionBarSelector = "#QA0Szd > div > div > div.w6VYqd > div.bJzME.tTVLSc > div > div.e07Vkf.kA9KIf > div > div > div:nth-child(4)";
        
            var content = WebDriver.FindElement(By.CssSelector(actionBarSelector));

            if (content == null) 
            {
                return false;
            }
            else if (content.Enabled && content.Displayed) 
            { 
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns true if the admissionsInfo is enabled and is displayed
        /// </summary>
        private bool IsAdmissionsInfoDisplaying()
        {
            string admissionsInfoSelector = "#QA0Szd > div > div > div.w6VYqd > div.bJzME.tTVLSc > div > div.e07Vkf.kA9KIf > div > div > div:nth-child(8)";

            var content = WebDriver.FindElement(By.CssSelector(admissionsInfoSelector));

            if (content == null)
            {
                return false;
            }
            else if (content.Enabled && content.Displayed)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns true if the review stars are enabled and can be used to observe reviews
        /// </summary>
        private bool IsGoogleReviewsActive()
        {
            string reviewStarsSelector = "#QA0Szd > div > div > div.w6VYqd > div.bJzME.tTVLSc > div > div.e07Vkf.kA9KIf > div > div > div.TIHn2 > div.tAiQdd > div.lMbq3e > div.LBgpqf > div > div.fontBodyMedium.dmRWX > div.F7nice.mmu3tf";

            string reviewPanelHeaderSelector = "#QA0Szd > div > div > div.w6VYqd > div.bJzME.tTVLSc > div > div.e07Vkf.kA9KIf > div > div > div.BHymgf.eiJcBe > div > div > div.cO45I > div > div > span";

            var content = WebDriver.FindElement(By.CssSelector(reviewStarsSelector));

            if (content == null)
            {
                return false;
            }
            else if (content.Enabled && content.Displayed)
            {
                content.Click();

                var reviewHeader = WebDriver.FindElement(By.CssSelector(reviewPanelHeaderSelector));
                
                if (reviewHeader == null)
                {
                    return false;
                }
                else if (string.IsNullOrEmpty(reviewHeader.Text))
                {
                    return false;
                }
                else
                {
                    return true;
                }
               
            }
            else
            {
                return false;
            }
        }

        private bool IsPopularTimesDisplayed()
        {
            string popularTimesSelector = "#QA0Szd > div > div > div.w6VYqd > div.bJzME.tTVLSc > div > div.e07Vkf.kA9KIf > div > div > div.UmE4Qe";

            var content = WebDriver.FindElement(By.CssSelector(popularTimesSelector));

            if (content == null)
            {
                return false;
            }
            else if (content.Enabled && content.Displayed)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}