using Logs.TestInfo;
using System.Diagnostics;

namespace Logs.Loggers
{
    /// <summary>
    /// Logs test execution data to debug window
    /// </summary>
    internal class TraceLogger : ILogger
    {
        public void Log(object sender, TestRunData data)
        {
            MessageData message = data.LastStep.LastMessage;
            switch (message?.MessageType)
            {
                case MessageCategory.Error:
                case MessageCategory.Info:
                case MessageCategory.Screenshot:
                    Trace.WriteLine($"{message.MessageType.ToString().ToUpper()}: {message.MessageText}");
                    break;
                default:
                    break;
            }
        }
    }
}
