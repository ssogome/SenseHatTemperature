using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SenseHatTelemeter.Helpers;

namespace SenseHatTelemeter.Sensors
{
    public class HumidityAndTemperatureSensor: SensorBase
    {
        public static HumidityAndTemperatureSensor Instance
        {
            get
            {
                if(instance == null)
                {
                    lock (syncRoot)
                    {
                        if(syncRoot == null)
                        {
                            instance = new HumidityAndTemperatureSensor();
                        }
                    }
                }
                return instance;
            }
        }

        private static volatile HumidityAndTemperatureSensor instance;

        private float humidityScalerH0;
        private float humidityScalerH1;
        private float humidityScalerT0;
        private float humidityScalerT1;

        public float GetHumidity()
        {
            CheckInitialization();

            const byte humidityLowByteRegisterAddress = 0x28;
            const byte humidityHighByteRegisterAddress = 0x29;

            var rawHumidity = RegisterHelper.GetShort(device,
                new byte[] { humidityLowByteRegisterAddress, humidityHighByteRegisterAddress });

            return ConvertToRelativeHumidity(rawHumidity);
        }

        protected override void Configure()
        {
            CheckInitialization();

            const byte controlRegisterAddress = 0x20;
            var controlRegisterByteValue = HumiditySensorHelper.ConfigureControlByte();

            RegisterHelper.WriteByte(device, controlRegisterAddress, controlRegisterByteValue);

            GetHumidityScalers();
        }

        private HumidityAndTemperatureSensor()
        {
            sensorAddress = 0x5F;

            whoAmIRegisterAddress = 0x0F;
            whoAmIDefaultValue = 0xBC;
        }

        private void GetHumidityScalers()
        {
            CheckInitialization();

            const byte h0RegisterAddress = 0x30;
            const byte h1RegisterAddress = 0x31;

            const byte t0LowByteRegisterAddress = 0x36;
            const byte t0HighByteRegisterAddress = 0x37;

            const byte t1LowByteRegisterAddress = 0x3A;
            const byte t1HighByteRegisterAddress = 0x3B;

            const float hScaler = 2.0f;

            humidityScalerH0 = RegisterHelper.ReadByte(device, h0RegisterAddress) / hScaler;
            humidityScalerH1 = RegisterHelper.ReadByte(device, h1RegisterAddress) / hScaler;

            humidityScalerT0 = RegisterHelper.GetShort(device,
                new byte[] { t0LowByteRegisterAddress, t0HighByteRegisterAddress });
            humidityScalerT1 = RegisterHelper.GetShort(device,
                new byte[] { t1LowByteRegisterAddress, t1HighByteRegisterAddress });
        }

        private float ConvertToRelativeHumidity(short rawHumidity)
        {
            var slope = (humidityScalerH1 - humidityScalerH0) / (humidityScalerT1 - humidityScalerT0);

            return slope * (rawHumidity - humidityScalerT0) + humidityScalerH0;
        }

    }
}
