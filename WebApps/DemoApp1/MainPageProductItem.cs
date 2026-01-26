using OpenQA.Selenium;
using WebApps.CmnWebApp;

namespace WebApps.DemoApp1
{
    public class MainPageProductItem : Page
    {
        public MainPageProductItem(Tab fromTab, Element pageContainer) : base(fromTab, pageContainer) { }

        public string Name => PageContainer.Find(By.XPath(".//div[@class='inventory_item_name ']")).Text;
        public string Description => PageContainer.Find(By.XPath(".//div[@class='inventory_item_desc']")).Text;
        public string Price => PageContainer.Find(By.XPath(".//div[@class='inventory_item_price']")).Text;
        public void AddToCart()
        {
            PageContainer.Find(By.XPath(".//button[@class='btn_inventory']")).Click();
        }
    }
}
