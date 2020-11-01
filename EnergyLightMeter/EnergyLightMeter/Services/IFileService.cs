using System.Collections.Generic;
using EnergyLightMeter.ViewModel;

namespace EnergyLightMeter.Services
{
    public interface IFileService
    {
        void SaveRecord(string fileName, StatisticsRecordViewModel record);

        List<StatisticsRecordViewModel> GetRecords(string fileName);
    }
}
