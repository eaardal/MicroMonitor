using System.Windows.Media;

namespace MicroMonitor.Utilities
{
    class HexUtil
    {
        public static SolidColorBrush HexToSolidColorBrush(string hex)
        {
            return (SolidColorBrush)new BrushConverter().ConvertFrom(hex);
        }
    }
}
