using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleMapsCodeTests
{
    public class Helper
    {

        private WebDriver webDriver;
        private string driverpath;

        private string baseUrl { get; set; } = "https://www.google.com/maps";

        private string cookieSelector = "#yDmH0d > c-wiz > div > div > div > div.NIoIEf > div.G4njw > div.AIC7ge > div.CxJub > div.VtwTSb > form:nth-child(2)";

        private string searchboxSelector = "#searchboxinput";
        private string magGlassSelector = "#searchbox > div.pzfvzf";
        private string placeNameSelector = "#QA0Szd > div > div > div.w6VYqd > div.bJzME.tTVLSc > div > div.e07Vkf.kA9KIf > div > div > div.TIHn2 > div.tAiQdd > div.lMbq3e > div:nth-child(1) > h1 > span:nth-child(1)";

        public Helper(WebDriver driver)
        {
            webDriver = driver;

        }

        /// <summary>
        /// Adds inputString to Searchbar and presses the magnifying glass to run search
        /// </summary>
        /// <param name="inputString"></param>
        public bool AddAndSendInput(string inputString)
        {
            try
            { 
            
                var input = webDriver.FindElement(By.CssSelector(searchboxSelector));
                var magGlass = webDriver.FindElement(By.CssSelector(magGlassSelector));

                    if (input != null && magGlass != null) 
                    { 
                        input.Clear();
                        input.SendKeys(inputString);

                        magGlass.Click();

                        return true;
                    }
                    else
                    {
                        return false;
                    }

            }
            catch 
            {
                return false;
            }
        }

        public bool IsOutputCorrect(string outputString)
        {
            try { 

                var outputname = webDriver.FindElement(By.CssSelector(placeNameSelector));

                if (outputname == null || string.IsNullOrEmpty(outputname.Text))
                {
                    return false;
                }
                else if (outputname.Text.Contains(outputString))
                {
                    return true;
                }
                else
                {
                    return false;
                }

                }
            catch 
            { 
                return false; 
            }

        }

        /// <summary>
        /// Returns true if expected adress is contained within results page
        /// </summary>
        /// <param name="expectedAddress"></param>
        /// <returns></returns>
        public bool IsAddressCorrect(string expectedAddress)
        {
            //selector returns results text
            string addressContentSelector = "#QA0Szd > div > div > div.w6VYqd > div.bJzME.tTVLSc > div > div.e07Vkf.kA9KIf > div > div ";


            Thread.Sleep(5000);

            try
            {
                var content = webDriver.FindElement(By.CssSelector(addressContentSelector));

                if (content != null)
                {
                    string actualAddress = content.Text;
                    if (actualAddress.Contains(expectedAddress))
                    {
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
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Return true if header image is active and displays
        /// </summary>
        public bool IsHeaderPhotoDisplaying()
        {
            string PhotoSelector = "#QA0Szd > div > div > div.w6VYqd > div.bJzME.tTVLSc > div > div.e07Vkf.kA9KIf > div > div > div.ZKCDEc > div.RZ66Rb.FgCUCc > button > img";

            //Needed to add more time for information to appear
            Thread.Sleep(5000);

            var content = webDriver.FindElement(By.CssSelector(PhotoSelector));

            if (content != null)
            {
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
        public bool IsPlaceInfoDisplaying()
        {
            string infoTextSelector = "#QA0Szd > div > div > div.w6VYqd > div.bJzME.tTVLSc > div > div.e07Vkf.kA9KIf > div > div > div.y0K5Df";

            var content = webDriver.FindElement(By.CssSelector(infoTextSelector));

            if (content == null)
            {
                return false;
            }
            else if (content.Displayed == false)
            {
                return false;
            }
            else
            {
                string infoText = content.Text;
                if (infoText != string.Empty)
                {
                    if (infoText.Length > 0)
                    {
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
        public bool IsActionBarDisplaying()
        {
            string actionBarSelector = "#QA0Szd > div > div > div.w6VYqd > div.bJzME.tTVLSc > div > div.e07Vkf.kA9KIf > div > div > div:nth-child(4)";

            var content = webDriver.FindElement(By.CssSelector(actionBarSelector));

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
        public bool IsAdmissionsInfoDisplaying()
        {
            string admissionsInfoSelector = "#QA0Szd > div > div > div.w6VYqd > div.bJzME.tTVLSc > div > div.e07Vkf.kA9KIf > div > div > div:nth-child(8)";

            var content = webDriver.FindElement(By.CssSelector(admissionsInfoSelector));

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
        public bool IsGoogleReviewsActive()
        {
            string reviewStarsSelector = "#QA0Szd > div > div > div.w6VYqd > div.bJzME.tTVLSc > div > div.e07Vkf.kA9KIf > div > div > div.TIHn2 > div.tAiQdd > div.lMbq3e > div.LBgpqf > div > div.fontBodyMedium.dmRWX > div.F7nice.mmu3tf";

            string reviewPanelHeaderSelector = "#QA0Szd > div > div > div.w6VYqd > div.bJzME.tTVLSc > div > div.e07Vkf.kA9KIf > div > div > div.BHymgf.eiJcBe > div > div > div.cO45I > div > div > span";

            var content = webDriver.FindElement(By.CssSelector(reviewStarsSelector));

            if (content == null)
            {
                return false;
            }
            else if (content.Enabled && content.Displayed)
            {
                content.Click();

                var reviewHeader = webDriver.FindElement(By.CssSelector(reviewPanelHeaderSelector));

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

        public bool IsPopularTimesDisplayed()
        {
            string popularTimesSelector = "#QA0Szd > div > div > div.w6VYqd > div.bJzME.tTVLSc > div > div.e07Vkf.kA9KIf > div > div > div.UmE4Qe";

            var content = webDriver.FindElement(By.CssSelector(popularTimesSelector));

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
