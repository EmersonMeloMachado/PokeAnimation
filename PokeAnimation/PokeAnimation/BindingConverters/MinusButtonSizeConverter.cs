using System;
using System.Globalization;
using Xamarin.Forms;

namespace PokeAnimation.BindingConverters
{
    public class MinusButtonSizeConverter : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value - (double)App.Current.Resources["buttonSize"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion Methods
    }
}