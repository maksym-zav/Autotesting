using OpenQA.Selenium;

namespace WebApps.CmnWebApp.Elements
{
    public class ButtonElement : Element
    {
        public ButtonElement(IWebDriver driver, IWebElement element, By by) : base(driver, element, by) { }
    }
}
