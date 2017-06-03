using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Webplayer.Modules.Spotify
{
    /// <summary>
    /// Bool to Visibility enum
    /// </summary>
    class VisibilityConverterd : IValueConverter
    {
        public static string Inverted = "Inverted";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(parameter == Inverted)
                return (bool)value ? Visibility.Visible : Visibility.Hidden;

            return (bool)value ? Visibility.Hidden: Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
