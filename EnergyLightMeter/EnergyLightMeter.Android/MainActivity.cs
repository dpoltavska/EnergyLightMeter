using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Hardware;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using EnergyLightMeter.View;
using Xamarin.Forms;

namespace EnergyLightMeter.Droid
{
    [Activity(Label = "EnergyLightMeter", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, ISensorEventListener
    {
        private SensorManager sensorManager;
        private float lightSensorValue;
        private App app;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            app = new App();
            LoadApplication(app);

            sensorManager = (SensorManager) GetSystemService(Context.SensorService);

            Sensor lightSensor = sensorManager.GetDefaultSensor(SensorType.Light);

            sensorManager.RegisterListener(this, lightSensor, global::Android.Hardware.SensorDelay.Game);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] global::Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public void OnAccuracyChanged(Sensor sensor, SensorStatus accuracy)
        {
            // throw new NotImplementedException();
        }

        public void OnSensorChanged(SensorEvent s)
        {
            s.Sensor = sensorManager.GetDefaultSensor(SensorType.Light);
            lightSensorValue = s.Values[0];

            app.LightValue = lightSensorValue;
        }
    }
}