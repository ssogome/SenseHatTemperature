using Microsoft.Azure.Devices.Client;
using SenseHatTelemeter.AzureHelper;
using System;
using SenseHatTelemeter.TelemetryControl;
using SenseHatTelemeter.ViewModel;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Diagnostics;
using Windows.UI.Core;
using SenseHatTelemeter.Helpers;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SenseHatTelemeter
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        private const string deviceId = "The Id you gave your device in azureiotsuite.com";
        private const string hostname = "<Your ioT hub Id >.azure-device.net";
        private const string deviceKey = "your device key from aureiotsuite.com";

        private DeviceClient deviceClient;

        private const int secReadoutDelay = 5;
        private TelemetryViewModel telemetryViewModel = new TelemetryViewModel();
        private Telemetry telemetry;

        public MainPage()
        {
            this.InitializeComponent();
        }

        private async Task InitializeDeviceClient()
        {
            var authentication = new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey);
            deviceClient = DeviceClient.Create(hostname, authentication);

            await deviceClient.OpenAsync();
        }

        private async void ButtonConnect_Click(object sender, RoutedEventArgs e)
        {
            if (!telemetryViewModel.IsConnected)
            {
                try
                {
                    // Connect to the cloud
                    await InitializeDeviceClient();

                    // Setup telemetry
                    telemetry = await Telemetry.CreateAsync(TimeSpan.FromSeconds(secReadoutDelay));
                    telemetry.DataReady += Telemetry_DataReady;

                    // Start message listener
                    BeginRemoteCommandHandling();

                    telemetryViewModel.IsConnected = true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }

        private void Telemetry_DataReady(object sender, TelemetryEventArgs e)
        {
            DisplaySensorReadings(e);

            var telemetryData = new TelemetryData()
            {
                DeviceId = deviceId,
                Temperature = e.Temperature,
                Humidity = e.Humidity
            };

            var telemetryMessage = MessageHelper.Serialize(telemetryData);
            deviceClient.SendEventAsync(telemetryMessage);
        }

        private async void DisplaySensorReadings(TelemetryEventArgs telemetryEventArgs)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                telemetryViewModel.Temperature = telemetryEventArgs.Temperature;
                telemetryViewModel.Humidity = telemetryEventArgs.Humidity;
            });
        }

        private async void ButtonSendDeviceInfo_Click(object sender, RoutedEventArgs e)
        {
            var deviceInfo = new DeviceInfo()
            {
                IsSimulatedDevice = false,
                ObjectType = "DeviceInfo",
                Version = "1.1",
                DeviceProperties = new DeviceProperties(deviceId),

                // Configure commands
                Commands = new Command[]
                {
                    CommandHelper.CreateUpdateTelemetryStatusCommand()
                }
            };

            var deviceInfoMessage = MessageHelper.Serialize(deviceInfo);

            try
            {
                await deviceClient.SendEventAsync(deviceInfoMessage);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void ButtonStartTelemetry_Click(object sender, RoutedEventArgs e)
        {
            telemetry.Start();
            telemetryViewModel.IsTelemetryActive = telemetry.IsActive;
        }

        private void ButtonStopTelemetry_Click(object sender, RoutedEventArgs e)
        {
            telemetry.Stop();
            telemetryViewModel.IsTelemetryActive = telemetry.IsActive;
        }

        private void BeginRemoteCommandHandling()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    var message = await deviceClient.ReceiveAsync();

                    if (message != null)
                    {
                        await HandleMessage(message);
                    }
                }
            });
        }

        private async Task HandleMessage(Message message)
        {
            try
            {
                // Deserialize message to remote command
                var remoteCommand = MessageHelper.Deserialize(message);

                // Parse command
                await ParseCommand(remoteCommand);

                // Send confirmation to the cloud
                await deviceClient.CompleteAsync(message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

                // Reject message, if it was not parsed correctly
                await deviceClient.RejectAsync(message);
            }
        }

        private async Task ParseCommand(RemoteCommand remoteCommand)
        {
            // Verify remote command name
            if (string.Compare(remoteCommand.Name, CommandHelper.UpdateTelemetryStatusCommandName) == 0)
            {
                // Update telemetry status depending on the IsOn parameter value
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    if (remoteCommand.Parameters.IsOn)
                    {
                        ButtonStartTelemetry_Click(this, null);
                    }
                    else
                    {
                        ButtonStopTelemetry_Click(this, null);
                    }
                });
            }
        }
    }
}
