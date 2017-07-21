using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenseHatTelemeter.TelemetryControl
{
    public class TelemetryEventArgs
    {
        public float Temperature { get; private set; }
        public float Humidity { get; private set; }

        public TelemetryEventArgs(float temperature, float humidity)
        {
            Temperature = temperature;
            Humidity = humidity;
        }

        public override string ToString()
        {
            return $"Temperature: {Temperature:F2}, Humidity: {Humidity:F2}";
        }
    }
}
