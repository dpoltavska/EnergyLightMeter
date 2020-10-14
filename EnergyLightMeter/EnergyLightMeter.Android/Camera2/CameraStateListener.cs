using Android.Hardware.Camera2;
using Android.Util;
using EnergyLightMeter.Droid.Camera2;

namespace EnergyLightMeter.Android.Camera2
{
	public class CameraStateListener : CameraDevice.StateCallback
	{
		public CameraDroid Camera;

		public override void OnOpened(CameraDevice camera)
		{
			if (Camera == null) return;

			Camera.CameraDevice = camera;
			Camera.StartPreview();
			Camera.OpeningCamera = false;
		}

		public override void OnDisconnected(CameraDevice camera)
		{
            if (Camera == null) return;

			camera.Close();
			Camera.CameraDevice = null;
			Camera.OpeningCamera = false;
		}

		public override void OnError(CameraDevice camera, CameraError error)
		{
			camera.Close();

			if (Camera == null) return;

			Camera.CameraDevice = null;
			Camera.OpeningCamera = false;
			Log.Error("OnError", error.ToString());
            System.Console.WriteLine(error);
		}
	}
}
