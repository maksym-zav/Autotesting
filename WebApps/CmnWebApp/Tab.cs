using Logs;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System.Collections.ObjectModel;
using System.Diagnostics;
using WebApps.Extensions;

namespace WebApps.CmnWebApp
{
    /// <summary>
    /// Class represents browser tab
    /// </summary>
    public class Tab
    {
        private readonly IWebDriver driver;
        private readonly string handle;
        //private readonly IElementLocator elementLocator;
        private readonly IPageObjectMemberDecorator pomDecorator;

        public Tab(IWebDriver driver)
        {
            this.driver = driver;
            this.handle = driver.CurrentWindowHandle;
            //this.elementLocator = new CustomElementLocator(driver, WaitOptions.DefaultTimeThrow);
            this.pomDecorator = new CustomPageObjectMemberDecorator(driver);
        }

        public Tab(IWebDriver driver, string handle) : this(driver)
        {
            this.handle = handle;
        }

        public string Url
        {
            get => driver.Url;
            set => driver.Url = value;
        }

        public string Title => driver.Title;

        public T InitializePage<T>() where T : Page
        {
            Log.Info($"Initialize page '{typeof(T).Name}'");
            T page = (T)Activator.CreateInstance(typeof(T), this);
            // PageFactory.InitElements(page, elementLocator, pomDecorator);            
            PageFactory.InitElements(driver, page, pomDecorator);
            if (page.IsPageExists())
            {
                Log.Info($"Page '{typeof(T).Name}' is found");
            }

            return page;
        }

        public bool IsElementExists(By by)
        {
            return this.Find(by, WaitOptions.NullTimeDoNotThrow) != null;
            //return driver.FindElements(by).Count > 0;
        }

        public Element Find(By by, WaitOptions waitOptions = null) => Find<Element>(by, waitOptions);

        public T Find<T>(By by, WaitOptions waitOptions = null) where T : Element
        {
            waitOptions ??= WaitOptions.DefaultTimeThrow;
            var result = Stopwatch.StartNew().Wait(() => driver.FindElement(by), waitOptions.Timeout);
            if (result.Success)
            {
                T wrapper = (T)Activator.CreateInstance(typeof(T), driver, result.Result, by);
                return wrapper;
            }
            else
            {
                if (waitOptions.Throw)
                {
                    Log.Error($"Element '{by}' is NOT found.", result.Exception);
                }
            }

            return null;
        }

        public ReadOnlyCollection<Element> FindAll(By by) => FindAll<Element>(by);

        public ReadOnlyCollection<T> FindAll<T>(By by) where T : Element
        {
            ReadOnlyCollection<IWebElement> elements = driver.FindElements(by);
            List<T> result = new List<T>();
            foreach (IWebElement element in elements)
            {
                result.Add((T)Activator.CreateInstance(typeof(T), driver, element, by));
            }

            return result.AsReadOnly();
        }

        public bool IsTabExists() => driver.WindowHandles.Contains(handle);
        public void Reload() => driver.Navigate().Refresh();
        public void Forward() => driver.Navigate().Forward();
        public void Back() => driver.Navigate().Back();

        public string WaitAndConfirmAlert(WaitOptions waitOptions = null, bool confirm = true, string promptText = null)
        {
            waitOptions ??= WaitOptions.DefaultTimeThrow;
            var result = Stopwatch.StartNew().Wait(() => driver.SwitchTo().Alert(), waitOptions.Timeout);
            if (result.Success)
            {
                IAlert alert = result.Result;
                Log.Info($"Alert with text '{alert.Text}' is found");
                if (promptText != null)
                {
                    alert.SendKeys(promptText);
                }

                if (confirm)
                {
                    alert.Accept();
                }
                else
                {
                    alert.Dismiss();
                }

                return alert.Text;
            }
            else
            {
                if (waitOptions.Throw)
                {
                    Log.Error("Alert isn't found", result.Exception);
                }
            }

            return null;
        }

        public object ExecuteScript(string script)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            object retValue = js.ExecuteScript(script);
            return retValue;
        }

        public void Close()
        {
            if (this.IsTabExists())
            {
                driver.Close();
            }
        }


    }
}
