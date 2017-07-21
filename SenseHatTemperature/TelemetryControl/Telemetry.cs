using SenseHatTelemeter.Helpers;
using SenseHatTelemeter.Sensors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SenseHatTelemeter.TelemetryControl
{
    public class Telemetry
    {
        //Event
        public event EventHandler<TelemetryEventArgs> DataReady = delegate { };

        //Telemetry task
        public bool IsActive { get; private set; } = false;
        private Task telemetryTask;
        private CancellationTokenSource telemetryCancellationTokenSource;

        //Sensors
        private TemperatureAndPressureSensor temperatureAndPressureSensor = TemperatureAndPressureSensor.Instance;
        private HumidityAndTemperatureSensor hunidityAndTemperatureSensor = HumidityAndTemperatureSensor.Instance;

        private TimeSpan readoutDelay;

        public void Start()
        {
            if (!IsActive)
            {
                InitializeTelemetryTask();
                telemetryTask.Start();
                IsActive = true;
            }
        }

        public void Stop()
        {
            if (IsActive)
            {
                telemetryCancellationTokenSource.Cancel();
                IsActive = false;
            }
        }

        public static async Task<Telemetry> CreateAsync(TimeSpan readoutDelay)
        {
            Check.IsNull(readoutDelay);

            var telemetry = new Telemetry(readoutDelay);
            await telemetry.InitializeSensors();

            return telemetry;
        }

        private Telemetry(TimeSpan readoutDelay)
        {
            this.readoutDelay = readoutDelay;
        }

        private async Task InitializeSensors()
        {
            await temperatureAndPressureSensor.Initialize();
            VerifyInitialization(temperatureAndPressureSensor, "Temperature and pressure sensor is unavailable");

            await hunidityAndTemperatureSensor.Initialize();
            VerifyInitialization(hunidityAndTemperatureSensor, "Humidity sensor is unavailable");
        }

        private void VerifyInitialization(SensorBase sensorBase, string exceptionMessage)
        {
            if (!sensorBase.IsInitialized)
            {
                throw new Exception(exceptionMessage);
            }
        }

        private void InitializeTelemetryTask()
        {
            telemetryCancellationTokenSource = new CancellationTokenSource();

            telemetryTask = new Task(() =>
            {
                while (!telemetryCancellationTokenSource.IsCancellationRequested)
                {
                    if (IsActive)
                    {
                        var temperature = temperatureAndPressureSensor.GetTemperature();
                        var humidity = hunidityAndTemperatureSensor.GetHumidity();

                        DataReady(this, new TelemetryEventArgs(temperature, humidity));

                        Task.Delay(readoutDelay).Wait();
                    }
                }
            }, telemetryCancellationTokenSource.Token);
        }
    }
}
