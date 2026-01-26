using SeleniumExtras.PageObjects;
using WebApps.CmnWebApp;
using WebApps.CmnWebApp.Elements;

namespace WebApps.DemoApp1
{
    /// <summary>
    /// Provides Logon page properties and methods
    /// </summary>
    public class LogonPage : Page
    {
        public LogonPage(Tab fromTab) : base(fromTab) { }

        // PageContainer is used to make sure that correct page is opened
        [FindsBy(How = How.ClassName, Using = "login_container")]
        public override Element PageContainer { get; set; }

        [FindsBy(How = How.XPath, Using = "//input[@id='user-name']")]
        public EditElement EditUsername { get; set; }
        
        [FindsBy(How = How.XPath, Using = "//input[@id='password']")]
        public EditElement EditPassword { get; set; }
        
        [FindsBy(How = How.XPath, Using = "//input[@id='login-button']")]
        public ButtonElement ButtonLogon { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='error-message-container error']")]
        public Element ErrorMessage { get; set; }
        
        public void PerformLogon(string userName, string password)
        {
            EditUsername.Type(userName);
            EditPassword.Type(password, false);
            ButtonLogon.Click();
        }

        public void PerformLogon(LogonInfo logonInfo) => PerformLogon(logonInfo.UserName, logonInfo.Password);
    }

}
