using Logs;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System.Collections.ObjectModel;
using System.Diagnostics;
using WebApps.Extensions;

namespace WebApps.CmnWebApp
{
    /// <summary>
    /// Common wrapper for Selenium IWebElement
    /// </summary>
    public class Element
    {
        protected IWebElement wrappedElement;
        protected IWebDriver driver;
        protected By by;

        public bool IsEnabled => wrappedElement.Enabled;
        public bool IsVisible => wrappedElement.Displayed;
        public virtual string Text => wrappedElement.Text;

        public Element(IWebDriver driver, IWebElement element, By by)
        {
            this.by = by;
            this.driver = driver;
            this.wrappedElement = element;
        }

        public bool IsElementExists() => driver.FindElements(by).Count() > 0;
        public string GetAttribute(string attributeName) => wrappedElement.GetAttribute(attributeName);
        public string GetProperty(string propertyName) => wrappedElement.GetDomProperty(propertyName);
        public bool WaitForProperty(string propertyName, string expectedValue, WaitOptions waitOptions = null)
        {
            waitOptions ??= WaitOptions.DefaultTimeThrow;
            var result = Stopwatch.StartNew().Wait(() => String.Equals(GetProperty(propertyName), expectedValue,
                StringComparison.OrdinalIgnoreCase), waitOptions.Timeout);
            if (waitOptions.Throw && !result.Success)
            {
                Log.Error($"Property '{propertyName}' value of element '{by}' is '{GetProperty(propertyName)}', but expected '{expectedValue}'");
            }

            return result.Success;
        }

        /// <summary>
        /// Wait until the element is visible and enabled
        /// </summary>
        //private bool WaitForElementInteractable(WaitOptions waitOptions = null)
        //{
        //    waitOptions ??= WaitOptions.DefaultTimeThrow;
        //    bool result = this.WaitForProperty("hidden", "false", new WaitOptions(waitOptions.Timeout, false))
        //        && this.WaitForProperty("disabled", "false", new WaitOptions(waitOptions.Timeout, false));
        //    if (!result && waitOptions.Throw)
        //    {
        //        throw new TimeoutException($"Element '{by}' is not interactable");
        //    }

        //    return result;
        //}

        public void Click()
        {
            wrappedElement.Click();
        }

        public void DblClick()
        {
            Actions action = new Actions(driver);
            action.MoveToElement(wrappedElement).DoubleClick().Perform();
        }

        public Element Find(By by, WaitOptions waitOptions = null) => Find<Element>(by, waitOptions);

        public T Find<T>(By by, WaitOptions waitOptions = null) where T : Element
        {
            waitOptions ??= WaitOptions.DefaultTimeThrow;
            var result = Stopwatch.StartNew().Wait(() => wrappedElement.FindElement(by), waitOptions.Timeout);
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
            List<T> result = new List<T>();
            ReadOnlyCollection<IWebElement> foundElements = wrappedElement.FindElements(by);
            foreach (IWebElement element in foundElements)
            {
                result.Add((T)Activator.CreateInstance(typeof(T), driver, element, by));
            }

            return result.AsReadOnly();
        }

        /// <summary>
        /// Execute JS script on current element. Use 'arguments[0]' for access
        /// </summary>
        public object ExecuteScript(string script)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            object retValue = js.ExecuteScript(script, wrappedElement);
            return retValue;
        }

        public virtual bool Verify(string expectedValue, bool throwError = true)
        {
            string actualText = this.Text;
            bool result = actualText == expectedValue;
            if (!result && throwError)
            {
                Log.Error($"Value is '{actualText}', but should be '{expectedValue}'");
            }

            return result;
        }

        public void Highlight()
        {
            this.ExecuteScript("arguments[0].style.backgroundColor='#FDFF47'");
        }

    }
}
