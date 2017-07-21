using SenseHatTelemeter.Helpers;

namespace SenseHatTelemeter.Sensors
{
    public class TemperatureAndPressureSensor : SensorBase
    {
        public static TemperatureAndPressureSensor Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new TemperatureAndPressureSensor();
                        }
                    }
                }

                return instance;
            }
        }

        private static volatile TemperatureAndPressureSensor instance;

        private const float tempOffset = 42.5F;
        private const float tempScaler = 480.0F;

        private const float pressureScaler = 4096.0F;

        public float GetTemperature()
        {
            CheckInitialization();

            // Register address list
            const byte tempLowByteRegisterAddress = 0x2B;
            const byte tempHighByteRegisterAddress = 0x2C;

            // Read low, and high bytes and convert them to 16-bit signed integer
            var temperature = RegisterHelper.GetShort(device,
                new byte[] { tempLowByteRegisterAddress, tempHighByteRegisterAddress });

            // Convert to physical units [degrees Celsius]
            return temperature / tempScaler + tempOffset;
        }

        public float GetPressure()
        {
            CheckInitialization();

            // Register address list
            const byte pressureLowByteRegisterAddress = 0x28;
            const byte pressureMiddleByteRegisterAddress = 0x29;
            const byte pressureHighByteRegisterAddress = 0x2A;

            // Read registers convert resulting values to a 32-bit signed integer
            var pressure = RegisterHelper.GetInt(device, new byte[] {
                pressureLowByteRegisterAddress,
                pressureMiddleByteRegisterAddress,
                pressureHighByteRegisterAddress });

            // Convert to physical units [hectopascals, hPa]
            return pressure / pressureScaler;
        }

        protected override void Configure()
        {
            CheckInitialization();

            const byte controlRegisterAddress = 0x20;
            var controlRegisterByteValue = TemperatureAndPressureSensorHelper.ConfigureControlByte();

            RegisterHelper.WriteByte(device, controlRegisterAddress, controlRegisterByteValue);
        }

        private TemperatureAndPressureSensor()
        {
            sensorAddress = 0x5C;

            whoAmIRegisterAddress = 0x0F;
            whoAmIDefaultValue = 0xBD;
        }
    }
}
