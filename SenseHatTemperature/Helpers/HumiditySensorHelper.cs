using System.Collections;

namespace SenseHatTelemeter.Helpers
{
    public class HumiditySensorHelper
    {
        private const int bduIndex = 2;
        private const int odrBeginIndex = 0;
        private const int odrEndIndex = 1;
        private const int pdIndex = 7;

        public static byte ConfigureControlByte(HumidityOutputDataRate outputDataRate = HumidityOutputDataRate.Hz_12_5,
            bool safeBlockUpdate = true, bool isOn = true)
        {
            var bitArray = new BitArray(Constants.ByteBitLength);

            // BDU
            bitArray.Set(bduIndex, safeBlockUpdate);

            // ODR
            SetOdr(outputDataRate, bitArray);

            // Power down bit
            bitArray.Set(pdIndex, isOn);

            return ConversionHelper.GetByteValueFromBitArray(bitArray);
        }

        private static void SetOdr(HumidityOutputDataRate outputDataRate, BitArray bitArray)
        {
            bool[] odrBitValues;

            switch (outputDataRate)
            {
                case HumidityOutputDataRate.OneShot:
                    odrBitValues = new bool[] { false, false };
                    break;

                case HumidityOutputDataRate.Hz_1:
                    odrBitValues = new bool[] { true, false };
                    break;

                case HumidityOutputDataRate.Hz_7:
                    odrBitValues = new bool[] { false, true };
                    break;

                case HumidityOutputDataRate.Hz_12_5:
                default:
                    odrBitValues = new bool[] { true, true };
                    break;
            }

            for (int i = odrBeginIndex, j = 0; i <= odrEndIndex; i++, j++)
            {
                bitArray[i] = odrBitValues[j];
            }
        }
    }

    public enum HumidityOutputDataRate
    {
        OneShot, Hz_1, Hz_7, Hz_12_5
    }
}
