using System;
using System.Diagnostics;
using Android.Media;
using Java.Nio;

namespace EnergyLightMeter.Droid.Camera2
{
    public class ImageAvailableListener : Java.Lang.Object, ImageReader.IOnImageAvailableListener
    {
        public event EventHandler<Models.Image> Photo;
        private Stopwatch _stopwatch;

        public ImageAvailableListener()
        {
            _stopwatch = new Stopwatch();
            _stopwatch.Start();
        }

        public void OnImageAvailable(ImageReader reader)
        {
            Image image = null;

            try
            {
                image = reader.AcquireLatestImage();
                if (image != null && _stopwatch.ElapsedMilliseconds > 150)
                {
                    _stopwatch.Restart();
                    var bitmapArray = YUV_420_888toNV21(image);
                    var result = new Models.Image
                    {
                        Array = bitmapArray,
                        Height = image.Height,
                        Width = image.Width
                    };

                    Photo?.Invoke(this, result);
                }
            }
            catch (Exception ex)
            {
                // ignored
            }
            finally
            {
                image?.Close();
            }
        }

        private static byte[] YUV_420_888toNV21(Image image)
        {
            int width = image.Width;
            int height = image.Height;
            int ySize = width * height;
            int uvSize = width * height / 4;

            byte[] nv21 = new byte[ySize + uvSize * 2];

            ByteBuffer yBuffer = image.GetPlanes()[0].Buffer; // Y
            ByteBuffer uBuffer = image.GetPlanes()[1].Buffer; // U
            ByteBuffer vBuffer = image.GetPlanes()[2].Buffer; // V

            int rowStride = image.GetPlanes()[0].RowStride;
            if (image.GetPlanes()[0].PixelStride != 1)
            {
                return null;
            }

            int pos = 0;

            if (rowStride == width)
            {
                // likely
                yBuffer.Get(nv21, 0, ySize);
                pos += ySize;
            }
            else
            {
                int yBufferPos = -rowStride; // not an actual Position
                for (; pos < ySize; pos += width)
                {
                    yBufferPos += rowStride;
                    yBuffer.Position(yBufferPos);
                    yBuffer.Get(nv21, pos, width);
                }
            }

            rowStride = image.GetPlanes()[2].RowStride;
            int pixelStride = image.GetPlanes()[2].PixelStride;

            if (rowStride != image.GetPlanes()[1].RowStride)
            {
                return null;
            }

            if (pixelStride != image.GetPlanes()[1].PixelStride)
            {
                return null;
            }

            if (pixelStride == 2 && rowStride == width && uBuffer.Get(0) == vBuffer.Get(1))
            {
                // maybe V an U planes overlap as per NV21, which means vBuffer[1] is alias of uBuffer[0]
                sbyte savePixel = vBuffer.Get(1);
                try
                {
                    vBuffer.Put(1, savePixel);
                    if (uBuffer.Get(0) == (byte)~savePixel)
                    {
                        vBuffer.Put(1, savePixel);
                        vBuffer.Position(0);
                        uBuffer.Position(0);
                        vBuffer.Get(nv21, ySize, 1);
                        uBuffer.Get(nv21, ySize + 1, uBuffer.Remaining());

                        return nv21; // shortcut
                    }
                }
                catch (ReadOnlyBufferException ex)
                {
                    // unfortunately, we cannot check if vBuffer and uBuffer overlap
                }

                // unfortunately, the check failed. We must save U and V pixel by pixel
                vBuffer.Put(1, savePixel);
            }

            // other optimizations could check if (pixelStride == 1) or (pixelStride == 2), 
            // but performance gain would be less significant
            byte[] vBufferArray = new byte[vBuffer.Remaining()];
            byte[] uBufferArray = new byte[uBuffer.Remaining()];
            vBuffer.Get(vBufferArray);
            uBuffer.Get(uBufferArray);
            for (int row = 0; row < height / 2; row++)
            {
                for (int col = 0; col < width / 2; col++)
                {
                    int vuPos = col * pixelStride + row * rowStride;
                    nv21[pos++] = vBufferArray[vuPos];
                    nv21[pos++] = uBufferArray[vuPos];
                }
            }

            return nv21;
        }
    }
}
