using System;
using System.Collections.Generic;
using System.Text;
using EnergyLightMeter.ViewModel;

namespace EnergyLightMeter.Services
{
    public interface IFileProvider
    {
        void SaveRecord(string fileName, StatisticsRecordViewModel record);

        List<StatisticsRecordViewModel> GetRecords(string fileName);
    }
}
