using SenseHatTelemeter.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.I2c;

namespace SenseHatTelemeter.Sensors
{
    public class SensorBase
    {
        public bool IsInitialized { get; protected set; }
        protected I2cDevice device;

        protected byte sensorAddress = 0x00;
        protected byte whoAmIRegisterAddress = 0x00;
        protected byte whoAmIDefaultValue = 0xFF;

        protected static object syncRoot = new object();

        public async Task<bool> Initialize()
        {
            device = await I2cHelper.GetI2cDevice(sensorAddress);
            if(device != null)
            {
                IsInitialized = WhoAmI(whoAmIRegisterAddress, whoAmIDefaultValue);

                if (IsInitialized)
                {
                    Configure();
                }
            }
            return IsInitialized;

        }

        protected void CheckInitialization()
        {
            if (!IsInitialized)
            {
                throw new Exception("Device is not initialized");
            }
        }

        protected bool WhoAmI(byte registerAddress, byte expectedValue)
        {
            byte whoami = RegisterHelper.ReadByte(device, registerAddress);

            return whoami == expectedValue;
        }

        protected virtual void Configure()
        {

        }
    }
}
