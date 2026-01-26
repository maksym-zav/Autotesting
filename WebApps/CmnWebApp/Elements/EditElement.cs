using OpenQA.Selenium;

namespace WebApps.CmnWebApp.Elements
{
    /// <summary>
    /// Wrapper for editable elements
    /// </summary>
    public class EditElement : Element
    {
        public EditElement(IWebDriver driver, IWebElement element, By by) : base(driver, element, by) { }

        public override string Text => base.GetProperty("value");

        public void Clear()
        {
            wrappedElement.Clear();
        }

        public void Type(string text, bool verify = true)
        {
            wrappedElement.SendKeys(text);
            if (verify)
            {
                Verify(text);
            }
        }
    }
}
