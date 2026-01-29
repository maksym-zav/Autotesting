using Logs.Loggers;
using Logs.TestInfo;

namespace Logs
{
    /// <summary>
    /// Class provides functionality to log test execution information
    /// </summary>
    public sealed class Log
    {
        private static Log instance = null;
        private event EventHandler<TestRunData> logEvent;
        private readonly TestRunData testData;
        private int nextStepNum = 1;

        public static string TestName
        {
            get => Instance().testData.TestName;
            set => Instance().testData.TestName = value;
        }
        public static string TestDescription
        {
            get => Instance().testData.TestDescription;
            set => Instance().testData.TestDescription = value;
        }

        public static string TestProject
        {
            get => Instance().testData.TestProject;
            set => Instance().testData.TestProject = value;
        }

        public static string Designer
        {
            get => Instance().testData.Designer;
            set => Instance().testData.Designer = value;
        }

        public static bool TestFinished { get; private set; } = false;

        private static Log Instance()
        {
            if (instance == null)
            {
                instance = new Log();
            }

            return instance;
        }

        private Log()
        {
            testData = new TestRunData()
            {
                TestDir = Environment.CurrentDirectory,
                StartDateTime = DateTime.Now,
                FinishDateTime = null,
                TestStatus = Status.NotCompleted,
                Tester = Environment.UserName
            };
        }

        public static void AddDefaultLoggers()
        {
            AddLogger(new TraceLogger());
            AddLogger(new ScreenshotLogger());
            AddLogger(new HtmlLogger());
            AddLogger(new ElasticLogger());
        }

        private static void AddLogger(ILogger logger) => Instance().logEvent += logger.Log;

        /// <summary>
        /// Log new step with description
        /// </summary>
        /// <param name="stepDescription"></param>
        public static void Step(string stepDescription)
        {
            Instance().testData.Steps.Add(new StepData() 
            { 
                Description = stepDescription,
                StepStatus = Status.NotCompleted,
                Number = Instance().nextStepNum++
            });

            Instance().testData.EventCategory = MessageCategory.Step;
            Instance().logEvent?.Invoke(instance, Instance().testData);
        }

        /// <summary>
        /// Finish current step with expected result
        /// </summary>
        /// <param name="expectedResult"></param>
        public static void EndStep(string expectedResult)
        {
            Instance().testData.LastStep.Expected = expectedResult;
            if (Instance().testData.LastStep.StepStatus == Status.NotCompleted)
            {
                Instance().testData.LastStep.StepStatus = Status.Passed;
            }

            Instance().testData.EventCategory = MessageCategory.EndStep;
            Instance().logEvent?.Invoke(instance, Instance().testData);
        }

        /// <summary>
        /// Log info message
        /// </summary>
        /// <param name="message"></param>
        public static void Info(string message)
        {
            Instance().testData.LastStep.AddMessage(MessageCategory.Info, message);
            Instance().testData.EventCategory = MessageCategory.Info;
            Instance().logEvent?.Invoke(instance, Instance().testData);
        }

        /// <summary>
        /// Log error message
        /// </summary>
        public static void Error(string message, bool stop = false)
        {
            Instance().testData.LastStep.StepStatus = Status.Failed;
            Instance().testData.LastStep.AddMessage(MessageCategory.Error, message);
            Instance().testData.EventCategory = MessageCategory.Error;
            Instance().logEvent?.Invoke(instance, Instance().testData);
            ScreenShot();

            // Finish and Fail current run if 'stop' flag set
            if (stop)
            {
                EndCase();
            }
        }

        public static void Error(string message, Exception exception)
        {
            Error(message, false);
            Exception(exception);
        }

        public static void Exception(Exception exception)
        {
            if (exception != null)
            {
                Instance().testData.LastStep.AddMessage(MessageCategory.Exception, exception.ToString());
                Instance().testData.EventCategory = MessageCategory.Exception;
                Instance().logEvent?.Invoke(instance, Instance().testData);
                if (exception.InnerException != null)
                {
                    Exception(exception.InnerException);
                }
            }
        }

        /// <summary>
        /// Finish test execution
        /// </summary>
        public static void EndCase()
        {
            // Determine test status
            if (Instance().testData.Steps.Any(x => x.StepStatus == Status.Failed))
            {
                Instance().testData.TestStatus = Status.Failed;
            }
            else if (Instance().testData.Steps.All(x => x.StepStatus == Status.Passed))
            {
                Instance().testData.TestStatus = Status.Passed;
            }

            Instance().testData.FinishDateTime = DateTime.Now;
            TestFinished = true;

            Instance().testData.EventCategory = MessageCategory.EndCase;
            Instance().logEvent?.Invoke(instance, Instance().testData);

            if (Instance().testData.TestStatus == Status.Failed)
            {
                // Get all failed messages logged while test execution in Log.Error() method
                var errors = Instance().testData.Steps
                    .SelectMany(n => n.Messages)
                    .Where(x => x.MessageType == MessageCategory.Error)
                    .Select(x => x.MessageText).ToList();

                // Fail current run
                throw new Exception(string.Join(", ", errors));
            }
        }

        /// <summary>
        /// Make screenshot
        /// </summary>
        public static void ScreenShot()
        {
            string fileName = $"{Instance().testData.TestName}_{DateTime.Now:ddMMyyyy_hhmmssff}.png";
            string filePath = Path.Combine(Instance().testData.TestDir, fileName);

            Instance().testData.LastStep.AddMessage(MessageCategory.Screenshot, filePath);
            Instance().testData.EventCategory = MessageCategory.Screenshot;
            Instance().logEvent?.Invoke(instance, Instance().testData);
        }
    }
}
