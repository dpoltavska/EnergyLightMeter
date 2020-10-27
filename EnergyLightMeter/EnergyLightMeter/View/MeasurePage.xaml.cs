using System;
using System.Collections.Generic;
using System.IO;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using EnergyLightMeter.Services;
using EnergyLightMeter.ViewModel;
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
        private IFileProvider fileProvider;
        private Color dominantColor = Color.Accent;

        public FilesViewModel FileNames { get; set; }

        public MeasurePage()
        {
            InitializeComponent();

            lightProvider = DependencyService.Get<ILightProvider>();
            fileProvider = DependencyService.Get<IFileProvider>();

            FileNames = new FilesViewModel();

            BindingContext = FileNames;

            LabelIluminance.Text = lightProvider.GetLightValue().ToString();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await GetCameraPermission();
        }

        public void UpdateLight(float light)
        {
            LabelIluminance.Text = light.ToString();
        }

        //Change Label
        public void UpdateWaveLenght(string waveLenght)
        {
            LabelWavelength.Text = waveLenght;
        }

        public void UpdateDominantColor(Color color)
        {
            this.dominantColor = color;

            DominantColor.Color = color;
            LabelWavelength.Text = WavelengthDetector.GetWaveLengthDiapason(color);
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

                await DisplayAlert("Could not access Camera", "App needs Camera access to work. Go to Settings >> App to enable Camera access ", "GOT IT");
                
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        async Task<bool> GetExternalStoragePermission()
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync<StoragePermission>();
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Storage))
                    {
                        var result = await DisplayAlert("Storage access needed", "App needs Sotrage access enabled to work.", "ENABLE", "CANCEL");

                        if (!result)
                            return false;
                    }

                    status = await CrossPermissions.Current.RequestPermissionAsync<StoragePermission>();
                }

                if (status == PermissionStatus.Granted)
                {
                    return true;
                }

                await DisplayAlert("Could not access Storage", "App needs Storage access to work.", "GOT IT");

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private async void CaptureMeasures(object sender, EventArgs args)
        {
            using (UserDialogs.Instance.Loading("Saving", maskType: MaskType.Black))
            {
                if (await GetExternalStoragePermission())
                {
                    var color = DominantColor.Color;

                    fileProvider.SaveRecord(this.FileNames.SelectedFile, new StatisticsRecordViewModel()
                    {
                        Date = DateTime.Now,
                        MeasuredIluminance = double.Parse(LabelIluminance.Text),
                        RealIluminance = string.IsNullOrEmpty(RealIlluminance.Text) ? null : (double?)double.Parse(RealIlluminance.Text),
                        WavelengthDiapason = LabelWavelength.Text,
                        WaveLength = string.IsNullOrEmpty(RealWavelength.Text) ? null : (int?) int.Parse(RealWavelength.Text),
                        Red = (byte)(this.dominantColor.R * 255),
                        Green = (byte)(this.dominantColor.G * 255),
                        Blue = (byte)(this.dominantColor.B * 255)
                    });
                }
            }

            UserDialogs.Instance.Toast(new ToastConfig("Saved"));
        }

        private void ChosenFile_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            FileNames.SelectedFile = FileNames.ExistingFiles[ChosenFile.SelectedIndex];
        }
    }
}