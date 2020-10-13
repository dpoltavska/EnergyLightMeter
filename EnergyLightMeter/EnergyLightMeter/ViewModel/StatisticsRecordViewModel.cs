using System;

namespace EnergyLightMeter.ViewModel
{
    public class StatisticsRecordViewModel
    {
        public DateTime Date { get; set; }

        public double MeasuredIluminance { get; set; }

        public double RealIluminance { get; set; }

        public int WaveLength { get; set; }

        public byte Red { get; set; }

        public byte Green { get; set; }

        public byte Blue { get; set; }
    }
}
