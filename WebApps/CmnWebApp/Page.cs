using Logs;

namespace WebApps.CmnWebApp
{
    /// <summary>
    /// Class represents application page or group of elements 
    /// </summary>
    public class Page
    {
        // PageContainer is used to make sure that correct page is opened
        public virtual Element PageContainer { get; set; }

        public Tab TabInstance { get; set; }

        public Page(Tab tabInstance)
        {
            this.TabInstance = tabInstance;
        }

        public Page(Tab tabInstance, Element pageContainer)
        {
            this.PageContainer = pageContainer;
            this.TabInstance = tabInstance;
        }

        public bool IsPageExists(bool throwError = true)
        {
            bool exits = this.PageContainer != null && this.PageContainer.IsElementExists();
            if (!exits && throwError)
            {
                Log.Error($"Page '{this.GetType().Name}' doesn't exist");
            }

            return exits;
        }

    }
}
