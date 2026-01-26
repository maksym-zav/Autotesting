using Logs;
using OpenQA.Selenium;

namespace WebApps.CmnWebApp.Elements
{
    /// <summary>
    /// Wraps Selenium SelectElement
    /// </summary>
    public class SelectElement : Element
    {
        protected OpenQA.Selenium.Support.UI.SelectElement select;

        public SelectElement(IWebDriver driver, IWebElement element, By by) : base(driver, element, by)
        {
            select = new OpenQA.Selenium.Support.UI.SelectElement(element);
        }

        public bool IsMultiple => select.IsMultiple;

        public List<string> GetOptions(bool onlySelected = false)
        {
            IList<IWebElement> options = onlySelected ? select.AllSelectedOptions : select.Options;
            return options.Select(n => n.Text).ToList();
        }

        public string GetOptionTextByIndex(int index)
        {
            string optionText = null;
            List<string> options = this.GetOptions(false);
            if (index >= 0 && index < options.Count)
            {
                optionText = options[index];
            }

            return optionText;
        }

        public void SetState(string optionText, bool state, bool verify = true)
        {
            if (state)
            {
                select.SelectByText(optionText);
            }
            else
            {
                select.DeselectByText(optionText);
            }

            if (verify)
            {
                this.VerifyState(optionText, state, true);
            }
        }

        public bool VerifyState(string optionText, bool state, bool throwError = true)
        {
            bool isSelected = this.GetOptions(true).Contains(optionText);
            if (isSelected != state && throwError)
            {
                Log.Error($"Incorrect option '{optionText}' state '{isSelected}', should be '{state}'");
            }

            return isSelected == state;
        }

        public bool VerifyState(List<string> optionsText, bool state, bool throwError = true)
        {
            bool correctState = true;
            foreach (string optionText in optionsText)
            {
                correctState &= VerifyState(optionText, state, throwError);
            }

            return correctState;
        }
    }
}
