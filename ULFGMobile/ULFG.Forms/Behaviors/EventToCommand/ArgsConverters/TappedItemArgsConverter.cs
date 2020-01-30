using System;
using System.Globalization;
using Xamarin.Forms;

namespace ULFG.Forms.Behaviors.Events.ArgsConverters
{
    /// <summary>
    /// <see cref="IValueConverter"/> para la conversión de los parametros <see cref="ItemTappedEventArgs"/>
    /// </summary>
    public class ItemTappedEventArgsConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var eventArgs = value as ItemTappedEventArgs;
            return eventArgs.Item;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
