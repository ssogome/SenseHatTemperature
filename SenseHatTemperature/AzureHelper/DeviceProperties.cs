using SenseHatTelemeter.Helpers;
using Windows.ApplicationModel;

namespace SenseHatTelemeter.AzureHelper
{
    public class DeviceProperties
    {
        public string DeviceID { get; set; }

        public bool HubEnabledState { get; set; }

        public string DeviceState { get; set; }

        public string Manufacturer { get; set; }

        public string ModelNumber { get; set; }

        public string SerialNumber { get; set; }

        public string FirmwareVersion { get; set; }

        public string AvailablePowerSources { get; set; }

        public string PowerSourceVoltage { get; set; }

        public string BatteryLevel { get; set; }

        public string MemoryFree { get; set; }

        public string Platform { get; set; }

        public string Processor { get; set; }

        public string InstalledRAM { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public DeviceProperties(string deviceId)
        {
            DeviceID = deviceId;
            SetDefaultValues();
        }

        private void SetDefaultValues()
        {
            HubEnabledState = true;
            DeviceState = "normal";
            Manufacturer = Package.Current.PublisherDisplayName;
            ModelNumber = "Sense HAT #1";
            SerialNumber = "0123456789";
            FirmwareVersion = VersionHelper.GetPackageVersion();
            AvailablePowerSources = "1";
            PowerSourceVoltage = "5 V";
            BatteryLevel = "N/A";
            MemoryFree = "N/A";
            Platform = "Windows 10 IoT Core " + VersionHelper.GetWindowsVersion();
            Processor = "ARM";
            InstalledRAM = "1 GB";
            Latitude = 47.6063889;
            Longitude = -122.3308333;
        }
    }
}
