using SenseHatTelemeter.Helpers;
using System;
using Windows.UI.Xaml.Data;

namespace SenseHatTelemeter.Converters
{
    public class TemperatureToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string result = Constants.UnavailableValue;

            try
            {
                float temperature = System.Convert.ToSingle(value);

                char degChar = (char)176;

                result = string.Format("{0,4:F2} {1}C", temperature, degChar);
            }
            catch (Exception) { }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
