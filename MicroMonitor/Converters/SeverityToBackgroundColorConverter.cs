using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using MicroMonitor.Model;
using MicroMonitor.Utilities;

namespace MicroMonitor.Converters
{
    class SeverityToBackgroundColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var severity = (MicroLogSeverity) Enum.Parse(typeof(MicroLogSeverity), value.ToString());
            var severityParam = parameter as string;

            if (severityParam != null && severityParam == "Light")
            {
                switch (severity)
                {
                    case MicroLogSeverity.Info:
                        return HexUtil.HexToSolidColorBrush(ColorConstants.InfoColorLight);
                    case MicroLogSeverity.Warning:
                        return HexUtil.HexToSolidColorBrush(ColorConstants.WarningColorLight);
                    case MicroLogSeverity.Error:
                        return HexUtil.HexToSolidColorBrush(ColorConstants.ErrorColorLight);
                    default:
                        return HexUtil.HexToSolidColorBrush(ColorConstants.InfoColorLight);
                }
            }

            switch (severity)
            {
                case MicroLogSeverity.Info:
                    return HexUtil.HexToSolidColorBrush(ColorConstants.InfoColor);
                case MicroLogSeverity.Warning:
                    return HexUtil.HexToSolidColorBrush(ColorConstants.WarningColor);
                case MicroLogSeverity.Error:
                    return HexUtil.HexToSolidColorBrush(ColorConstants.ErrorColor);
                default:
                    return HexUtil.HexToSolidColorBrush(ColorConstants.InfoColor);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
