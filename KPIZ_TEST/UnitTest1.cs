using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

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
                WebDriverWait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
                Driver.Navigate().GoToUrl(BaseUrl);
                Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            }

            [TearDown]
            public void TearDown()
            {
                Logout();
                Driver?.Quit();
                Driver?.Dispose();

            }

            [Test]
            public void Login()
            {
                ClickElement(By.Id("header-account-label"), "Account Menu");
                EnterText(By.Id("email"), ValidEmail, "Email");
                EnterText(By.Id("password"), ValidPassword, "Password");

                IWebElement loginButton = Driver.FindElement(By.CssSelector("button.btn.btn-primary.btn-lg.d-block"));
                Actions actions = new Actions(Driver);
                actions.MoveToElement(loginButton).Perform();
                loginButton.Click();

                WebDriverWait.Until(ExpectedConditions.UrlToBe(BaseUrl + "account-overview/"));
            }

            [Test]
            public void Logout()
            {

                ClickElement(By.Id("header-account-label"), "Account Menu");
                ClickElement(By.CssSelector(".header-dropdown-section.bg-grey-200.text-center"), "Logout Menu");

            }

            [Test]
            public void AddProductToFavorites()
            {
                Login();

                Driver.Navigate().GoToUrl(BaseUrl + ProductUrl);

                ClickElement(By.CssSelector(".btn.btn-sm.btn-link.link-2.fw-bold"), "Add to Favorites Button");

                ClickElement(By.Id("header-account-label"), "Account Menu");

                ClickElement(By.CssSelector("li a[href='favorites']"), "Favorites Link");


                var favoritesList = Driver.FindElement(By.CssSelector(".pfAccountProduct.panel"));
                Assert.IsTrue(favoritesList.Text.Contains(ProductName), "Product not found in Favorites");

                ClickElement(By.CssSelector(".btn-close.pfAccountProduct-remove"), "Remove from Favorites Button");
            }

            private void ClickElement(By locator, string description = "")
            {
                WebDriverWait.Until(ExpectedConditions.ElementToBeClickable(locator)).Click();
                if (!string.IsNullOrEmpty(description))
                {
                    Console.WriteLine($"Clicked element: {description}");
                }
            }

            private void EnterText(By locator, string text, string description = "")
            {
                WebDriverWait.Until(ExpectedConditions.ElementToBeClickable(locator)).SendKeys(text);
                if (!string.IsNullOrEmpty(description))
                {
                    Console.WriteLine($"Entered text '{text}' in {description}");
                }
            }
        }
    }
}