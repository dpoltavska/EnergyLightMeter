
using System.Collections.Generic;
using System.IO;
using Android.OS;
using EnergyLightMeter.Services;
using EnergyLightMeter.ViewModel;
using Xamarin.Forms;

[assembly: Dependency(typeof(EnergyLightMeter.Droid.Services.FileProvider))]
namespace EnergyLightMeter.Droid.Services
{
    public class FileProvider : IFileProvider
    {
        public void SaveRecord(string fileName, StatisticsRecordViewModel record)
        {
            var path = Environment.GetExternalStoragePublicDirectory(Environment.DirectoryDownloads).AbsolutePath;

            var filePath = Path.Combine(path, fileName);

            using (var streamWriter = new StreamWriter(filePath, true))
            {
                streamWriter.WriteLine(record.ToString());
            }
        }

        public List<StatisticsRecordViewModel> GetRecords(string fileName)
        {
            try
            {
                var path = Environment.GetExternalStoragePublicDirectory(Environment.DirectoryDownloads).AbsolutePath;

                var filePath = Path.Combine(path, fileName);

                using (var streamReader = new StreamReader(filePath))
                {
                    var data = streamReader.ReadToEnd();

                    return StatisticsTextParser.ParseToStatisticsModelList(data);
                }
            }
            catch
            {
                return new List<StatisticsRecordViewModel>();
            }
        }
    }
}