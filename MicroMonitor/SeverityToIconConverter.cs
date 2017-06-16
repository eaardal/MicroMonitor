using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MicroMonitor
{
    class SeverityToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var severity = (MicroLogSeverity)value;

            switch (severity)
            {
                case MicroLogSeverity.Info:
                    return "Images/icon-info.png";
                case MicroLogSeverity.Warning:
                    return "Images/icon-warning.png";
                case MicroLogSeverity.Error:
                    return "Images/icon-error.png";
                default:
                    return "Images/icon-info.png";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
