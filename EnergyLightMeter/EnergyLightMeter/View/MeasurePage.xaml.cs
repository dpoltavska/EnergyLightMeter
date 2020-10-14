using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnergyLightMeter.Services;
using Plugin.Media;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EnergyLightMeter.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MeasurePage : ContentPage
    {
        private ILightProvider lightProvider;
        public MeasurePage()
        {
            InitializeComponent();

            lightProvider = DependencyService.Get<ILightProvider>();

            Label_val.Text = lightProvider.GetLightValue().ToString();
        }

        async protected override void OnAppearing()
        {
            base.OnAppearing();

            await GetCameraPermission();
        }

        public void UpdateLight(float light)
        {
            Label_val.Text = light.ToString();
        }

        //Change Label
        public void UpdateWaveLenght(string waveLenght)
        {
            Label_val.Text = waveLenght;
        }        
        
        public void UpdateDominantColor(Color color)
        {
            DominantColor.BackgroundColor = color;
        }

        async Task<bool> GetCameraPermission()
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync<CameraPermission>();
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Camera))
                    {
                        var result = await DisplayAlert("Camera access needed", "App needs Camera access enabled to work.", "ENABLE", "CANCEL");

                        if (!result)
                            return false;
                    }

                    status = await CrossPermissions.Current.RequestPermissionAsync<CameraPermission>();
                }

                if (status == PermissionStatus.Granted)
                {
                    return true;
                }
                else
                {
                    await DisplayAlert("Could not access Camera", "App needs Camera access to work. Go to Settings >> App to enable Camera access ", "GOT IT");
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}