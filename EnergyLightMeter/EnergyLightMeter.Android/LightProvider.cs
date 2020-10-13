using EnergyLightMeter.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(EnergyLightMeter.Droid.LightProvider))]
namespace EnergyLightMeter.Droid
{
    public class LightProvider : ILightProvider
    {
        public float GetLightValue()
        {
            return 666;
        }
    }
}