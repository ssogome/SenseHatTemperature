namespace SenseHatTelemeter.AzureHelper
{
    public class DeviceInfo
    {
        public bool IsSimulatedDevice { get; set; }

        public string Version { get; set; }

        public string ObjectType { get; set; }

        public DeviceProperties DeviceProperties { get; set; }

        public Command[] Commands { get; set; }
    }
}
