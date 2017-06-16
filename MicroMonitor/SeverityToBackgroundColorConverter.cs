using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace MicroMonitor
{
    class SeverityToBackgroundColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var severity = (MicroLogSeverity)value;
            switch (severity)
            {
                case MicroLogSeverity.Info:
                    return new SolidColorBrush(Colors.LightGray);
                case MicroLogSeverity.Warning:
                    return new SolidColorBrush(Colors.LightSalmon);
                case MicroLogSeverity.Error:
                    return new SolidColorBrush(Colors.LightCoral);
                default:
                    return new SolidColorBrush(Colors.LightGray);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
