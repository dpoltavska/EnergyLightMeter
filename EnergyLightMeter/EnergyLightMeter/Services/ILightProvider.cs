using System;
using System.Collections.Generic;
using System.Text;

namespace EnergyLightMeter.Services
{
    public interface ILightProvider
    {
        float GetLightValue();
    }
}
