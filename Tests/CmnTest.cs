using Logs;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebApps;
using WebApps.CmnWebApp;
using WebApps.DemoApp1;

namespace Tests
{
    /// <summary>
    /// Common class for all tests
    /// </summary>
    public class CmnTest
    {
        private WebDriver driver;

        [SetUp]
        public void CmnSetup()
        {
            Log.TestName = TestContext.CurrentContext.Test.MethodName;
            Log.AddDefaultLoggers();

            driver = new ChromeDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            driver.Manage().Window.Maximize();
        }

        [TearDown]
        public void CmnTearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                // When test execution unexpectedly finished because of uncaught exception send this exception to logger
                if (!Log.TestFinished)
                {
                    Log.Error(TestContext.CurrentContext.Result.Message
                        + TestContext.CurrentContext.Result.StackTrace, true);
                }
            }

            if (driver != null)
            {
                driver.Quit();
                driver.Dispose();
            }
        }

        private AppInfo DemoApp1Info = new AppInfo()
        {
            BaseUrl = "https://www.saucedemo.com/",
        };

        // Application properties

        private DemoApp demoApp;
        public DemoApp DemoApp
        {
            get
            {
                if (demoApp == null)
                {
                    Browser browser = new Browser(driver);
                    demoApp = new DemoApp(browser, DemoApp1Info);
                }

                return demoApp;
            }
        }

    }
}
