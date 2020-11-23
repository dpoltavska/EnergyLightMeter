using System;
using Android.Content;
using Android.Graphics;
using EnergyLightMeter.Android.Services;
using EnergyLightMeter.Camera;
using EnergyLightMeter.Droid.Camera2;
using Xamarin.Essentials;
using EnergyLightMeter.Droid.Camera2;
using EnergyLightMeter.Shared;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using CameraPreview = EnergyLightMeter.Camera.CameraPreview;

[assembly: ExportRenderer(typeof(CameraPreview), typeof(CameraViewServiceRenderer))]
namespace EnergyLightMeter.Droid.Camera2
{
    public class CameraViewServiceRenderer : ViewRenderer<CameraPreview, CameraDroid>
	{
		private CameraDroid _camera;
        private CameraPreview _currentElement;
        private readonly Context _context;
        private ImageProcessor _imageProcessor;

		public CameraViewServiceRenderer(Context context) : base(context)
		{
			_context = context;
            _imageProcessor = new ImageProcessor();
		}

		protected override void OnElementChanged(ElementChangedEventArgs<CameraPreview> e)
		{
			base.OnElementChanged(e);

			_camera = new CameraDroid(Context);

            SetNativeControl(_camera);
            var mainTheread = MainThread.IsMainThread;
            if (e.NewElement != null && _camera != null)
			{
                _currentElement = e.NewElement;
                _currentElement.Click += OnCameraPreviewClicked;
                _camera.SetCameraOption(_currentElement.Camera);
                _camera.Photo += OnPhoto;
            }
		}

        void OnCameraPreviewClicked(object sender, EventArgs e)
        {
            _camera.CloseCamera();
            _currentElement.Camera = _currentElement.Camera == CameraOptions.Front ? CameraOptions.Rear : CameraOptions.Front;
            _camera.SetCameraOption(_currentElement.Camera);
            _camera.OpenCamera(1080, 453);
        }

        public void TakePicture()
        {
            _camera.LockFocus();
        }

        private void OnPhoto(object sender, Models.Image imgSource)
        {
            var array = _imageProcessor.FromYuvToRgb(imgSource.Array, imgSource.Width, imgSource.Height);
            Bitmap image = BitmapFactory.DecodeByteArray(array, 0, array.Length);
            //Here you have the image byte data to do whatever you want 
            Device.BeginInvokeOnMainThread(() =>
            {            
                if (image != null)
                {
                    var color = _imageProcessor.GetDominantColor(image);
                    var waveLength = _imageProcessor.GetWaveLenght(color);

                    //must be better way to update label. Place label on camera component itself? So we have access to it from renderer.
                    ((App)Application.Current).WaveLenghtValue = waveLength;
                    ((App)Application.Current).DominantColor = new Xamarin.Forms.Color((double)color.Red / 255, (double)color.Green / 255, (double)color.Blue / 255, (double)color.Alpha / 255);
                }
            });   
        }

        protected override void Dispose(bool disposing)
		{
			_camera.Photo -= OnPhoto;

			base.Dispose(disposing);
		}
	}
}
