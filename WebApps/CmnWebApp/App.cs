namespace WebApps.CmnWebApp
{
    /// <summary>
    /// Class describes common web application functionality
    /// </summary>
    public abstract class App
    {
        public AppInfo AppInfo { get; set; }
        public Tab TabInstance { get; set; }
        protected Browser browserInstance;

        public App(Browser browser, AppInfo appInfo)
        {
            AppInfo = appInfo;
            browserInstance = browser;
            TabInstance = browser.GetCurrentTab();
            TabInstance.Url = appInfo.BaseUrl;
        }
    }
}
