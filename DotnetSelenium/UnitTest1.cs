using NUnit.Framework.Internal;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools.V127.DeviceAccess;
using OpenQA.Selenium.Support.UI;
using System.Buffers.Text;
using OpenQA.Selenium.Interactions;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using System.Linq;


namespace DotnetSelenium
{
    [TestFixture]
    public class Tests
    {
        private IWebDriver driver;
        

        [SetUp]
        public void Setup()
        {
            var options = new ChromeOptions();
            options.AddArgument("--incognito");
            driver = new ChromeDriver(options);            
            driver.Manage().Window.Maximize();
            driver.Url = "https://epam.com/";
        }


        [TestCase("Careers", "C#", "All Locations")]
        public void ValidateUserCanSearchBasedOnCriteria(String option, String searchValue, String optionValue)
        {

            IWebElement webElement = driver.FindElement(By.LinkText(option));
            webElement.Click();

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);

            IWebElement webElement8 = driver.FindElement(By.Id("onetrust-accept-btn-handler"));
            webElement8.Click();

            IWebElement webElement1 = driver.FindElement(By.Id("new_form_job_search-keyword"));
            webElement1.SendKeys(searchValue);

            IWebElement LocationDropDownElement = driver.FindElement(By.XPath("//span[@class='select2-selection__rendered']"));
            LocationDropDownElement.Click();


            IWebElement LocationValue = LocationDropDownElement.FindElement(By.XPath($"//ul[@class='select2-results__options open os-host os-theme-dark os-host-overflow os-host-overflow-y os-host-resize-disabled os-host-scrollbar-horizontal-hidden os-host-transition']//ul[contains(.,'{optionValue}')]"));
            LocationValue.Click();

            //Check Box
            IWebElement webElement2 = driver.FindElement(By.XPath("//label[contains(.,'Remote')]"));
            webElement2.Click();

            //Find Button
            IWebElement webElement3 = driver.FindElement(By.XPath("//button[@type='submit']"));
            webElement3.Click();

            var resultListlWait = new WebDriverWait(driver, TimeSpan.FromSeconds(2))
            {
                PollingInterval = TimeSpan.FromSeconds(0.25),
                Message = "Result List has not been loaded"
            };

            var resultList = resultListlWait.Until(driver => driver.FindElement(By.XPath("//ul[@class='search-result__list']")));


            //CLick on View and Apply button
            IList<IWebElement> elements = resultList.FindElements(By.ClassName("search-result__item"));
            IWebElement last = elements.Last();
            var lastResult = last.FindElement(By.XPath(".//a[contains(.,'View and apply')]"));
            lastResult.Click();

            //Validate that the programming language mentioned in the step above is on a page
            IList<IWebElement> list = driver.FindElements(By.XPath($"//*[contains(text(),'{searchValue}')]"));
            Assert.That(list.Count > 0, $"Text not found: {searchValue}");


        }

        [TestCase("BLOCKCHAIN")]
        public void ValidateGlobalSearch(String searchString)
        {
            //cookies
            IWebElement webElement8 = driver.FindElement(By.Id("onetrust-accept-btn-handler"));
            webElement8.Click();

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);

            IWebElement webElement = driver.FindElement(By.CssSelector(".search-icon"));
            webElement.Click();

            var formSearchWait = new WebDriverWait(driver, TimeSpan.FromSeconds(2))
            {
                PollingInterval = TimeSpan.FromSeconds(0.25),
                Message = "Result List has not been loaded"
            };

            var formSearch = formSearchWait.Until(driver => driver.FindElement(By.XPath("//input[@id='new_form_search']")));
            formSearch.SendKeys(searchString);

            IWebElement webElementFindButton = driver.FindElement(By.XPath("//span[@class='bth-text-layer']"));
            webElementFindButton.Click();

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
            
            var searchResult = driver.FindElement(By.XPath("//div[@class='search-results__items']"));
            var searchResultList = searchResult.FindElements(By.ClassName("search-results__title-link"));

            bool linksContainString = !searchResultList.Any(link => link.Text.ToLower().Contains(searchString));
            Assert.That(linksContainString, Is.True, $"All links do not contain: '{searchString}'");

        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }
    }

 }