using Logs;
using WebApps;
using WebApps.DemoApp1;

namespace Tests
{
    public class TestExample1 : CmnTest
    {
        LogonInfo logonInfo = new LogonInfo()
        {
            UserName = "standard_user",
            Password = "secret_sauce"
        };

        [SetUp]
        public void Setup()
        {
            Log.TestName = "TestName1";
            Log.TestDescription = "TestDescription1";
            Log.TestProject = "TestProject1";
            Log.Designer = "user1";
        }

        [Test]
        public void Test1()
        {
            #region Step 1
            Log.Step("Navigate to application page. Leave 'Username' and 'Password' fields empty. Click Logon");

            DemoApp.Logon.ButtonLogon.Click();
            DemoApp.Logon.ErrorMessage.Verify("Epic sadface: Username is required");

            Log.EndStep("Error message displayed: 'User name is required ...'");
            #endregion

            #region Step 2
            Log.Step("Enter correct username. Click Logon");

            DemoApp.Logon.EditUsername.Type(logonInfo.UserName);
            DemoApp.Logon.ButtonLogon.Click();
            DemoApp.Logon.ErrorMessage.Verify("Epic sadface: Password is required");

            Log.EndStep("Error message displayed: 'Password is required ...'");
            #endregion

            #region Step 3
            Log.Step("Enter some incorrect password. Click Logon");

            DemoApp.Logon.EditPassword.Type("incorrect password");
            DemoApp.Logon.ButtonLogon.Click();
            DemoApp.Logon.ErrorMessage.Verify("Epic sadface: Username and password do not match any user in this service");

            Log.EndStep("Error message displayed: 'Username and password do not match ...'");
            #endregion

            #region Step 4
            Log.Step("Enter correct username and password. Click Logon");

            DemoApp.Logon.EditPassword.Clear();
            DemoApp.Logon.EditPassword.Type(logonInfo.Password, false);
            DemoApp.Logon.ButtonLogon.Click();

            DemoApp.MainPage.IsPageExists();

            Log.EndStep("Main page is displayed");
            #endregion

            Log.EndCase();
        }
    }
}
