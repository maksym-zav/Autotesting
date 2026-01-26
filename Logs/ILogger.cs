using Logs.TestInfo;

namespace Logs
{
    /// <summary>
    /// Describes obligatory methods for each logger
    /// </summary>
    internal interface ILogger
    {
        void Log(object sender, TestRunData data);
    }
}
