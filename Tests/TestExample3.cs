using Logs;

namespace Tests
{
    public class TestExample3 : CmnTest
    {
        [SetUp]
        public void Setup()
        {
            Log.TestName = "TestName2";
            Log.TestDescription = "Some description ...";
            Log.TestProject = "Demo1";
            Log.Designer = "user1";
        }

        [Test]
        public void Test3()
        {
            #region Step 1
            Log.Step("Description 1");
            
            //throw new Exception();
            Log.Info("Info");
            //Log.Error("stheth");
            
            Log.Info("Info");

            Log.EndStep("Expected 1");
            #endregion

            #region Step 2
            Log.Step("Description 2");
            
            Log.Error("stheth2");
            Log.Error("stheth3");
            
            Log.EndStep("Expected 2");
            #endregion

            #region Step 3
            Log.Step("Description 3");

            Log.EndStep("Expected 3");
            #endregion

            Log.EndCase();
        }
    }
}
