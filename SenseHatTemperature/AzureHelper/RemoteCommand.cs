namespace SenseHatTelemeter.AzureHelper
{
    public class RemoteCommand
    {
        public string Name { get; set; }
        public string MessageId { get; set; }
        public string CreatedTime { get; set; }
        public string UpdatedTime { get; set; }
        public object Result { get; set; }
        public object ErrorMessage { get; set; }
        public Parameters Parameters { get; set; }
    }

    public class Parameters
    {
        public bool IsOn { get; set; }
    }
}
