using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using MicroMonitor.Config;
using MicroMonitor.Infrastructure;
using MicroMonitor.Model;
using MicroMonitor.Utilities;

namespace MicroMonitor.Converters
{
    class SeverityToBackgroundColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var logEntry = value as MicroLogEntry;
            
            if (logEntry == null)
            {
                Logger.Error("LogEntry was null in converter: {0}, {1}, {2}, {3}", value, targetType, parameter, culture);

                return HexUtil.HexToSolidColorBrush(ColorConstants.InfoColor);
            }

            var isStale = AppConfiguration.LogEntryStaleEnabled() && 
                (DateTime.Now - logEntry.Timestamp).TotalMinutes > AppConfiguration.LogEntryStaleThresholdInMinutes();

            switch (logEntry.Severity)
            {
                case MicroLogSeverity.Info:
                    return isStale
                        ? HexUtil.HexToSolidColorBrush(AppConfiguration.LogEntryColorInfoStale())
                        : HexUtil.HexToSolidColorBrush(AppConfiguration.LogEntryColorInfo());

                case MicroLogSeverity.Warning:
                    return isStale
                        ? HexUtil.HexToSolidColorBrush(AppConfiguration.LogEntryColorWarningStale())
                        : HexUtil.HexToSolidColorBrush(AppConfiguration.LogEntryColorWarning());

                case MicroLogSeverity.Error:
                    return isStale
                        ? HexUtil.HexToSolidColorBrush(AppConfiguration.LogEntryColorErrorStale())
                        : HexUtil.HexToSolidColorBrush(AppConfiguration.LogEntryColorError());

                default:
                    return isStale
                        ? HexUtil.HexToSolidColorBrush(AppConfiguration.LogEntryColorInfoStale())
                        : HexUtil.HexToSolidColorBrush(AppConfiguration.LogEntryColorInfo());
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
