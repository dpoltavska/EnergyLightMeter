using Android.Content;
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
        private IImageProcessor _imageProcessor;

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

            if (e.NewElement != null && _camera != null)
			{
                _currentElement = e.NewElement;
                _camera.SetCameraOption(_currentElement.Camera);
                _camera.Photo += OnPhoto;
            }
		}

        public void TakePicture()
        {
            _camera.LockFocus();
        }

        private void OnPhoto(object sender, byte[] imgSource)
		{
           //Here you have the image byte data to do whatever you want 
            Device.BeginInvokeOnMainThread(() =>
            {
                var color = _imageProcessor.GetDominantColor(imgSource);
                var waveLength = _imageProcessor.GetWaveLenght(color);

                //must be better way to update label. Place label on camera component itself? So we have access to it from renderer.
                ((App)Application.Current).WaveLenghtValue = waveLength;
            });   
        }

        protected override void Dispose(bool disposing)
		{
			_camera.Photo -= OnPhoto;

			base.Dispose(disposing);
		}
	}
}
