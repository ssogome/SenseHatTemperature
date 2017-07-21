using System;
using System.Linq;
using Windows.Devices.I2c;

namespace SenseHatTelemeter.Helpers
{
    public class RegisterHelper
    {
        public static byte ReadByte(I2cDevice device, byte address)
        {
            Check.IsNull(device);

            // Write buffer contains the register address
            var writeBuffer = new byte[] { address };

            // Read buffer is a single-element byte array
            var readBuffer = new byte[1];

            device.WriteRead(writeBuffer, readBuffer);

            return readBuffer.First();
        }

        public static void WriteByte(I2cDevice device, byte address, byte value)
        {
            Check.IsNull(device);

            var writeBuffer = new byte[] { address, value };

            device.Write(writeBuffer);
        }

        public static short GetShort(I2cDevice device, byte[] addressList)
        {
            const int length = 2;

            Check.IsLengthEqualTo(addressList.Length, length);

            var bytes = GetBytes(device, addressList, length);

            return BitConverter.ToInt16(bytes.ToArray(), 0);
        }

        public static int GetInt(I2cDevice device, byte[] addressList)
        {
            const int minLength = 3;
            const int maxLength = 4;

            Check.IsLengthInValidRange(addressList.Length, minLength, maxLength);

            var bytes = GetBytes(device, addressList, maxLength);

            return BitConverter.ToInt32(bytes.ToArray(), 0);
        }

        private static byte[] GetBytes(I2cDevice device, byte[] addressList, int totalLength)
        {
            var bytes = new byte[totalLength];

            for (int i = 0; i < addressList.Length; i++)
            {
                bytes[i] = ReadByte(device, addressList[i]);
            }

            if (!BitConverter.IsLittleEndian)
            {
                bytes = bytes.Reverse().ToArray();
            }

            return bytes;
        }
    }
}
