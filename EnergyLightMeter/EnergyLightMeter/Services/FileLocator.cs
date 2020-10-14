using System;
using System.Collections.Generic;
using System.Text;
using EnergyLightMeter.ViewModel;

namespace EnergyLightMeter.Services
{
    public static class FileLocator
    {
        public static FilesViewModel FilesForStatistics { get; set; } = new FilesViewModel();
    }
}
