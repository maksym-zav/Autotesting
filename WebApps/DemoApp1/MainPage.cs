using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using WebApps.CmnWebApp;
using WebApps.CmnWebApp.Elements;

namespace WebApps.DemoApp1
{
    /// <summary>
    /// Provides Main app page properties and methods
    /// </summary>
    public class MainPage : Page
    {
        public MainPage(Tab fromTab) : base(fromTab) { }

        // PageContainer is used to make sure that correct page is opened
        [FindsBy(How = How.XPath, Using = "//div[@id='contents_wrapper']")]
        public override Element PageContainer { get; set; }

        [FindsBy(How = How.XPath, Using = "//a[@class='shopping_cart_link']")]
        public ButtonElement ButtonCart { get; set; }

        [FindsBy(How = How.XPath, Using = "//select[@class='product_sort_container']")]
        public SelectElement Sorting { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='inventory_item']")]
        public List<Element> Inventory { get; set; }

        public List<MainPageProductItem> GetProductsItems()
        { 
            List<MainPageProductItem> products = new List<MainPageProductItem>();
            foreach (var item in TabInstance.FindAll(By.XPath("//div[@class='inventory_item']")))
            {
                products.Add(new MainPageProductItem(TabInstance, item));
            }

            return products;
        }
    }
}
