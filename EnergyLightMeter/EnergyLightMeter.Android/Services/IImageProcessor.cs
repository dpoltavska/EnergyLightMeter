using Android.Graphics;
using Color = EnergyLightMeter.Shared.Models.Color;

namespace EnergyLightMeter.Android.Services
{
    public interface IImageProcessor
    {
        Color GetDominantColor(Bitmap image);
        string GetWaveLenght(Color color);
    }
}