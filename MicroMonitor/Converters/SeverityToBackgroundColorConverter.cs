using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace MicroMonitor.Converters
{
    class SeverityToBackgroundColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var severityHack = (string)value;
            var parts = severityHack.Split(';');
            var severity = Enum.Parse(typeof(MicroLogSeverity), parts[0]);
            var isMarkedAsRead = bool.Parse(parts[1]);

            if (isMarkedAsRead)
            {
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

            switch (severity)
            {
                case MicroLogSeverity.Info:
                    return new SolidColorBrush(Colors.DarkGray);
                case MicroLogSeverity.Warning:
                    return new SolidColorBrush(Colors.Orange);
                case MicroLogSeverity.Error:
                    return new SolidColorBrush(Colors.Red);
                default:
                    return new SolidColorBrush(Colors.DarkGray);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
