using System.Windows.Media;

namespace MicroMonitor
{
    class HexUtil
    {
        public static SolidColorBrush HexToSolidColorBrush(string hex)
        {
            return (SolidColorBrush)new BrushConverter().ConvertFrom(hex);
        }
    }
}
