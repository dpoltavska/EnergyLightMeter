using Android.Graphics;
using Java.Util;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Android.Media;
using Java.Nio;
using Color = EnergyLightMeter.Shared.Models.Color;

namespace EnergyLightMeter.Android.Services
{

    public class ImageProcessor : IImageProcessor
    {
        public byte[] FromYuvToRgb(byte[] image, int width, int height)
        {
            return NV21toJPEG(image, width, height);
        }

        public Color GetDominantColor(Bitmap image)
        {
            Dictionary<int, int> pixels = new Dictionary<int, int>();

            for (int i = 0; i < image.Height; i++)
            {
                for (int j = 0; j < image.Width; j++)
                {
                    var currentPixel = image.GetPixel(j, i);
                    if (!pixels.ContainsKey(currentPixel)) {
                        pixels.Add(currentPixel, 1);
                    }
                    else
                    {
                        pixels[currentPixel] = pixels[currentPixel]++;
                    }
                }
            }
            var pixel = pixels.OrderBy(i => i.Value).First().Key;

            return new Color((pixel & 0xff0000) >> 16, (pixel & 0x00ff00) >> 8, (pixel & 0x0000ff) >> 0, 255);
        }

        public string GetWaveLenght(Color color)
        {
            var red = color.Red;
            var green = color.Green;
            var blue = color.Blue;

            return string.Empty;
        }

        private static byte[] NV21toJPEG(byte[] nv21, int width, int height)
        {
            using (var output = new MemoryStream())
            {
                YuvImage yuv = new YuvImage(nv21, ImageFormatType.Nv21, width, height, null);
                yuv.CompressToJpeg(new Rect(0, 0, width, height), 100, output);
                return output.ToArray();
            }
        }
    }
}
