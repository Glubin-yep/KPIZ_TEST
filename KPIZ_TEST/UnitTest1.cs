using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using OpenQA.Selenium.Interactions;
using static System.Collections.Specialized.BitVector32;
using static System.Net.Mime.MediaTypeNames;

namespace KPIZ_TEST
{
    public class Tests
    {
        [TestFixture]
        public class PureFormulasTestSuite
        {
            private ChromeDriver Driver;
            private WebDriverWait WebDriverWait;

            private const string BaseUrl = "https://www.pureformulas.com/";
            private const string ProductUrl = "product/pure-mineral-complete-by-pureformulas/1000046496";
            private const string ProductName = "Pure";
            private const string ValidEmail = "mohetij290@lucvu.com";
            private const string ValidPassword = "ei*ZBP32Tut&k%X";

            [SetUp]
            public void SetUp()
            {
                Driver = new ChromeDriver();
                WebDriverWait = new WebDriverWait(Driver, TimeSpan.FromSeconds(5));
                Driver.Navigate().GoToUrl(BaseUrl);
                Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            }

            [TearDown]
            public void TearDown()
            {
                if (Driver != null)
                {
                    Driver.Quit();
                    Driver.Dispose();
                }
            }

            [Test]
            public  void Login()
            {
                Driver.FindElement(By.Id("header-account-label")).Click();
                Driver.FindElement(By.Id("email")).SendKeys(ValidEmail);
                Driver.FindElement(By.Id("password")).SendKeys(ValidPassword);

                IWebElement loginButton = Driver.FindElement(By.CssSelector("button.btn.btn-primary.btn-lg.d-block"));
                Actions actions = new Actions(Driver);
                actions.MoveToElement(loginButton).Perform();
                loginButton.Click();

                WebDriverWait.Until(ExpectedConditions.UrlToBe(BaseUrl + "account-overview/"));

                
                Console.WriteLine("Login completed successfully");
            }

            [Test]
            public void Logout()
            {
                Login();

                IWebElement accountButton = Driver.FindElement(By.Id("header-account-label"));
                Actions actions = new Actions(Driver);
                actions.MoveToElement(accountButton).Perform();

                Driver.FindElement(By.CssSelector(".header-dropdown-section.bg-grey-200.text-center")).Click();
                Console.Write(accountButton.Text);
            }

            [Test]
            public void AddProductToFavorites()
            {
                Login();

                Driver.Navigate().GoToUrl(BaseUrl + ProductUrl);

                WebDriverWait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(".btn.btn-sm.btn-link.link-2.fw-bold")));
                Driver.FindElement(By.CssSelector(".btn.btn-sm.btn-link.link-2.fw-bold")).Click();

                WebDriverWait.Until(ExpectedConditions.ElementToBeClickable(By.Id("header-account-label")));
                Driver.FindElement(By.Id("header-account-label")).Click();

                WebDriverWait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("li a[href='favorites']")));
                Driver.FindElement(By.CssSelector("li a[href='favorites']")).Click();

                var favoritesList = Driver.FindElement(By.CssSelector(".pfAccountProduct.panel"));
                Assert.IsTrue(favoritesList.Text.Contains(ProductName));
                Driver.FindElement(By.CssSelector(".btn-close.pfAccountProduct-remove")).Click();
            }

        }
    }
}