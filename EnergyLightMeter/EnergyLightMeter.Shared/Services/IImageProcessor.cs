using EnergyLightMeter.Shared.Models;

namespace EnergyLightMeter.Shared
{
    public interface IImageProcessor
    {
        Color GetDominantColor(byte[] image);
        string GetWaveLenght(Color color);
    }
}