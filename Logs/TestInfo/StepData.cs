namespace Logs.TestInfo
{
    public class StepData
    {
        public string Description { get; set; }
        public string Expected { get; set; }
        public Status StepStatus { get; set; }
        public int Number { get; set; }
        public List<MessageData> Messages { get; set; } = new List<MessageData>();
        public MessageData LastMessage => Messages.LastOrDefault();

        public void AddMessage(MessageCategory type, string text)
        {
            Messages.Add(new MessageData()
            {
                MessageType = type,
                MessageText = text
            });
        }
    }
}