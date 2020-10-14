using System;
using EnergyLightMeter.Extensions;

namespace EnergyLightMeter.Services
{
    public static class WavelengthDetector
    {
        public static string GetWaveLengthDiapason(byte r, byte g, byte b)
        {
            var color = ColorExtension.CreateColor(r, g, b);

            var rd = r / 256.0;
            var gd = g / 256.0;
            var bd = b / 256.0;

            var M = Math.Max(Math.Max(rd, gd), bd);
            var m = Math.Min(Math.Min(rd, gd), bd);
            var C = M - m;

            double H = 0;

            if (C == 0)
            {
                return "underfined";
            }

            if (M == rd)
            {
                H = (gd - bd) % 6;
            }
            if (M == gd)
            {
                H = (bd - rd) / C + 2;
            }
            if (M == bd)
            {
                H = (rd - gd) / C + 4;
            }

            if (H > 0.3 && H < 0.8)
            {
                return "635-590 nm"; // "orange"
            }

            H = Math.Round(H, MidpointRounding.ToEven);

            switch (H)
            {
                case 0: return "700-635 nm";          //"red"
                case 1: return "590-560 nm";          //"yellow";
                case 2: return "560-520 nm";          //"green";
                case 3: return "520-490 nm";          //"cyan";
                case 4: return "490-450 nm";          //"blue";
                case 5: return "450-400 nm";          //"purple";
                case 6: return "< 400 nm";            //"red again"
                default: return "underfined";
            }
        }
    }
}
