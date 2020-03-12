using System;
using System.Globalization;
using Xamarin.Forms;

namespace PokeAnimation.BindingConverters
{
    public class DivideByConvertParameterConverter : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var v = (double)value;
            var p = System.Convert.ToDouble(parameter);
            return v / p;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion Methods
    }
}