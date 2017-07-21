using System.Collections;

namespace SenseHatTelemeter.Helpers
{
    public static class TemperatureAndPressureSensorHelper
    {
        private const int bduIndex = 2;
        private const int odrBeginIndex = 4;
        private const int odrEndIndex = 6;
        private const int pdIndex = 7;

        public static byte ConfigureControlByte(BarometerOutputDataRate outputDataRate = BarometerOutputDataRate.Hz_25,
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

        private static void SetOdr(BarometerOutputDataRate outputDataRate, BitArray bitArray)
        {
            bool[] odrBitValues;

            switch (outputDataRate)
            {
                case BarometerOutputDataRate.OneShot:
                    odrBitValues = new bool[] { false, false, false };
                    break;

                case BarometerOutputDataRate.Hz_1:
                    odrBitValues = new bool[] { true, false, false };
                    break;

                case BarometerOutputDataRate.Hz_7:
                    odrBitValues = new bool[] { false, true, false };
                    break;

                case BarometerOutputDataRate.Hz_12_5:
                    odrBitValues = new bool[] { true, true, false };
                    break;

                case BarometerOutputDataRate.Hz_25:
                default:
                    odrBitValues = new bool[] { false, false, true };
                    break;
            }

            ConversionHelper.SetBitArrayValues(bitArray, odrBitValues, odrBeginIndex, odrEndIndex);
        }
    }

    public enum BarometerOutputDataRate
    {
        OneShot, Hz_1, Hz_7, Hz_12_5, Hz_25
    }
}
