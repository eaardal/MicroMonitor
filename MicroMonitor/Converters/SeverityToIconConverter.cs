using System;
using System.Globalization;
using System.Windows.Data;
using MicroMonitor.Model;

namespace MicroMonitor.Converters
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
