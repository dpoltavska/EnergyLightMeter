using Xamarin.Forms;

namespace EnergyLightMeter.Extensions
{
    public static class ColorExtension
    {
        public static Color CreateColor(byte r, byte g, byte b) 
            => new Color(r/256.0, g/256.0, b/256.0);
    }
}
