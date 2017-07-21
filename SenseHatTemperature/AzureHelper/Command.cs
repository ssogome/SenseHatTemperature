namespace SenseHatTelemeter.AzureHelper
{
    public class Command
    {
        public string Name { get; set; }

        public CommandParameter[] Parameters { get; set; }
    }

    public class CommandParameter
    {
        public string Name { get; set; }

        public string Type { get; set; }
    }
}
