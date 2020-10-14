using System;
using Xamarin.Forms;

namespace EnergyLightMeter.Camera
{
    //Which camera to use? Specified in the component definitinon in markup of page.
    public enum CameraOptions
    {
        Rear,
        Front
    }

    public class CameraPreview : Xamarin.Forms.View
    {
        Command cameraClick;

        public static readonly BindableProperty CameraProperty = BindableProperty.Create(
            propertyName: "Camera",
            returnType: typeof(CameraOptions),
            declaringType: typeof(CameraPreview),
            defaultValue: CameraOptions.Rear);

        public CameraOptions Camera {
            get => (CameraOptions)GetValue(CameraProperty);
            set => SetValue(CameraProperty, value);
        }

        public Command CameraClick {
            get => cameraClick;
            set => cameraClick = value;
        }

        public void PictureTaken()
        {
            PictureFinished?.Invoke();
        }

        public event Action PictureFinished;
    }
}
