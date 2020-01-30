using System;
using System.Globalization;
using Xamarin.Forms;

namespace ULFG.Forms.Behaviors.Events.ArgsConverters
{
    /// <summary>
    /// <see cref="IValueConverter"/> para la conversión de los parametros <see cref="ToggledEventArgs"/>
    /// </summary>
    public class ToggledArgsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var eventArgs = value as ToggledEventArgs;
            return eventArgs.Value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
