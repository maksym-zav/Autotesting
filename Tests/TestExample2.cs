using Logs;
using WebApps;
using WebApps.DemoApp1;

namespace Tests
{
    public class TestExample2 : CmnTest
    {
        LogonInfo logonInfo = new LogonInfo()
        {
            UserName = "standard_user",
            Password = "secret_sauce"
        };

        [SetUp]
        public void Setup()
        {
            Log.TestName = "TestName2";
            Log.TestDescription = "TestDescription2";
            Log.TestProject = "TestProject2";
            Log.Designer = "user1";
        }

        [Test]
        public void Test2()
        {
            #region Step 1
            Log.Step("Navigate to application page. Enter correct login and password. Click Logon");

            DemoApp.Logon.PerformLogon(logonInfo);
            DemoApp.MainPage.IsPageExists();

            Log.EndStep("Main page is displayed");
            #endregion

            #region Step 2
            Log.Step("Look at sorting type and product items order");

            DemoApp.MainPage.Sorting.VerifyState("Name (A to Z)", true);
            List<string> actualItemsOrder = DemoApp.MainPage.GetProductsItems().Select(n => n.Name).ToList();
            List<string> correctItemsOrder = new List<string>(actualItemsOrder);
            correctItemsOrder.Sort();
            if (!actualItemsOrder.SequenceEqual(correctItemsOrder))
            {
                Log.Error($"Sorting is incorrect '{string.Join(",", actualItemsOrder)}', should be '{string.Join(",", correctItemsOrder)}'");
            }

            Log.EndStep("'Name (A to Z)' value is selected by default. Products are displayed in ascending order and sorted by name");
            #endregion

            #region Step 3
            Log.Step("Select 'Name (Z to A)' value and look at products items order");

            DemoApp.MainPage.Sorting.SetState("Name (Z to A)", true);
            actualItemsOrder = DemoApp.MainPage.GetProductsItems().Select(n => n.Name).ToList();
            correctItemsOrder.Reverse();
            if (!actualItemsOrder.SequenceEqual(correctItemsOrder))
            {
                Log.Error($"Sorting is incorrect '{string.Join(",", actualItemsOrder)}', should be '{string.Join(",", correctItemsOrder)}'");
            }

            Log.EndStep("'Name (Z to A)' value is selected. Products are displayed in descending order and sorted by name");
            #endregion

            Log.EndCase();
        }
    }
}
