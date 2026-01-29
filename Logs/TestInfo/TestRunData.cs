namespace Logs.TestInfo
{
    public class TestRunData
    {
        public MessageCategory EventCategory { get; set; }
        public string TestName { get; set; }
        public string TestDescription { get; set; }
        public string TestProject { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime? FinishDateTime { get; set; }
        public Status TestStatus { get; set; }
        public string TestDir { get; set; }
        public List<StepData> Steps { get; private set; } = new List<StepData>();
        public StepData LastStep => Steps.LastOrDefault();
        public string Tester { get; set; }
        public string Designer { get; set; }
    }
}
