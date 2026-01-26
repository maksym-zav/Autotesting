using Logs;
using OpenQA.Selenium;
using System.Diagnostics;
using WebApps.Extensions;

namespace WebApps.CmnWebApp
{
    /// <summary>
    /// Class represents browser functionality
    /// </summary>
    public class Browser
    {
        private readonly IWebDriver driver;

        public Browser(IWebDriver driver)
        {
            this.driver = driver;
        }

        public Tab GetCurrentTab()
        {
            return new Tab(driver);
        }

        public Tab OpenNewTab(string url = "")
        {
            driver.SwitchTo().NewWindow(WindowType.Tab);
            driver.Url = url;
            Tab newPage = new Tab(driver);
            return newPage;
        }

        public int GetTabsCount() => driver.WindowHandles.Count;
        public bool IsTabExists(string tabHandle) => driver.WindowHandles.Contains(tabHandle);

        public Tab WaitForNewTab(Action actionBeforeNewOpenTab, WaitOptions waitOptions = null)
        {
            waitOptions ??= WaitOptions.DefaultTimeThrow;
            string[] tabHandles = driver.WindowHandles.ToArray();
            int numOfTabs = this.GetTabsCount();
            actionBeforeNewOpenTab();
            var result = Stopwatch.StartNew().Wait(() => this.GetTabsCount() > numOfTabs, waitOptions.Timeout);
            if (result.Success)
            {
                string newTabHandle = driver.WindowHandles.Except(tabHandles).First();
                Log.Info($"New tab '{newTabHandle}' is opened");
                return new Tab(driver, newTabHandle);
            }
            else
            {
                if (waitOptions.Throw)
                {
                    Log.Error($"New tab isn't opened", result.Exception);
                }
            }

            return null;
        }

        public void Close()
        {
            driver.Quit();
            driver.Dispose();
        }
    }
}
