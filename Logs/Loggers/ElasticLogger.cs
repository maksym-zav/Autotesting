using Logs.TestInfo;

namespace Logs.Loggers
{
    /// <summary>
    /// Logs test execution data to elastic db.
    /// Stores information about run e.g. status, tester, d&t for statistics purposes
    /// </summary>
    internal class ElasticLogger : ILogger
    {
        public void Log(object sender, TestRunData data)
        {
            if (data.EventCategory == MessageCategory.EndCase)
            {
                //throw new NotImplementedException();
            }
        }
    }
}
