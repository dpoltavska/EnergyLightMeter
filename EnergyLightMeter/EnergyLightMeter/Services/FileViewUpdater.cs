using System;

namespace EnergyLightMeter.Services
{
    public class FileViewUpdater
    {
        public event Action UpdateFile;

        public virtual void OnFileUpdated()
        {
            UpdateFile?.Invoke();
        }
    }
}
