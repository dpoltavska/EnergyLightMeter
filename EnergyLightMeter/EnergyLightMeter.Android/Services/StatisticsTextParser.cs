using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using EnergyLightMeter.ViewModel;

namespace EnergyLightMeter.Droid.Services
{
    public static class StatisticsTextParser
    {
        public static List<StatisticsRecordViewModel> ParseToStatisticsModelList(string text)
        {
            var lines = text.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            return lines.Select(ParseStatisticsModel).ToList();
        }

        private static StatisticsRecordViewModel ParseStatisticsModel(string line)
        {
            var properties = line.Split(';');

            return new StatisticsRecordViewModel
            {
                Date = DateTime.Parse(properties[0]),
                MeasuredIluminance = Double.Parse(properties[1]),
                RealIluminance = string.IsNullOrEmpty(properties[2]) ? null : (double?) double.Parse(properties[2]),
                WavelengthDiapason = properties[3],
                WaveLength = string.IsNullOrEmpty(properties[4]) ? null : (int?) int.Parse(properties[4]),
                Red = byte.Parse(properties[5]),
                Green = byte.Parse(properties[6]),
                Blue = byte.Parse(properties[7]),
            };
        }
    }
}