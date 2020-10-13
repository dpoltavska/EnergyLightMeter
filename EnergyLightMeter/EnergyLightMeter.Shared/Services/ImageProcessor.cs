using EnergyLightMeter.Shared.Models;

namespace EnergyLightMeter.Shared
{
    public class ImageProcessor : IImageProcessor
    {
        public Color GetDominantColor(byte[] image)
        {
            return new Color(0, 0, 0);
        }

        public string GetWaveLenght(Color color)
        {
            var red = color.Red;
            var green = color.Green;
            var blue = color.Blue;

            return string.Empty;
        }
    }
}
