using System.Windows.Media;

namespace MicroMonitor.Utilities
{
    public static class ColorExtensions
    {
        private const int MinLightness = 1;
        private const int MaxLightness = 10;
        private const float MinLightnessCoef = 1f;
        private const float MaxLightnessCoef = 0.4f;

        public static Color ChangeLightness(this Color color, int lightness)
        {
            // Thanks to Nikolay Khil: https://stackoverflow.com/questions/12894605/how-to-make-specific-color-darken-or-lighten-based-on-value-in-wpf

            if (lightness < MinLightness)
                lightness = MinLightness;
            else if (lightness > MaxLightness)
                lightness = MaxLightness;

            float coef = MinLightnessCoef +
                         (
                             (lightness - MinLightness) *
                             ((MaxLightnessCoef - MinLightnessCoef) / (MaxLightness - MinLightness))
                         );

            return Color.FromArgb(color.A, (byte)(color.R * coef), (byte)(color.G * coef),
                (byte)(color.B * coef));
        }
    }
}