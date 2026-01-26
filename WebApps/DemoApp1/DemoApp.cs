using WebApps.CmnWebApp;

namespace WebApps.DemoApp1
{
    public class DemoApp : App
    {
        public DemoApp(Browser browser, AppInfo appInfo) : base(browser, appInfo) { }

        private LogonPage logon;
        private MainPage mainPage;

        public LogonPage Logon => logon ??= TabInstance.InitializePage<LogonPage>();
        public MainPage MainPage => mainPage ??= TabInstance.InitializePage<MainPage>();
    }
}