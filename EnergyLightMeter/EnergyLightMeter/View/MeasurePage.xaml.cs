using System;
using System.Threading.Tasks;
using Acr.UserDialogs;
using EnergyLightMeter.Camera;
using EnergyLightMeter.Services;
using EnergyLightMeter.ViewModel;
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
        private IFileService fileService;
        private Color dominantColor = Color.Accent;
        private bool isCaptureContinuous = false;
        private bool isWritingToTheFile = false;
        private int periodOfWriting;

        public FilesViewModel FileNames { get; set; }

        public FileViewUpdater FileViewUpdater { get; set; }

        public MeasurePage()
        {
            InitializeComponent();

            lightProvider = DependencyService.Get<ILightProvider>();
            fileService = DependencyService.Get<IFileService>();

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
                    this.SaveRecord();
                }
            }

            this.FileViewUpdater.OnFileUpdated();

            //UserDialogs.Instance.Toast(new ToastConfig("Saved"));
            UserDialogs.Instance.Toast("Saved", TimeSpan.FromMilliseconds(400));
        }

        private async Task WriteToFileUntilStopped()
        {
            if (await GetExternalStoragePermission())
            {
                while (this.isWritingToTheFile)
                {
                    this.SaveRecord();
                    UserDialogs.Instance.Toast("Saved", TimeSpan.FromMilliseconds(400));

                    await Task.Delay(this.periodOfWriting);
                }
            }

            this.FileViewUpdater.OnFileUpdated();
        }

        private void ChosenFile_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            FileNames.SelectedFile = FileNames.ExistingFiles[ChosenFile.SelectedIndex];
        }

        private void CaptureMethod_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            this.isCaptureContinuous = CaptureMethod.SelectedIndex == 1;

            UpdateContinuousCaptureControls();
        }

        private void SaveRecord()
        {
            fileService.SaveRecord(this.FileNames.SelectedFile, new StatisticsRecordViewModel()
            {
                Date = DateTime.Now,
                MeasuredIluminance = double.Parse(LabelIluminance.Text),
                RealIluminance = string.IsNullOrEmpty(RealIlluminance.Text) ? null : (double?)double.Parse(RealIlluminance.Text),
                WavelengthDiapason = LabelWavelength.Text,
                WaveLength = string.IsNullOrEmpty(RealWavelength.Text) ? null : (int?)int.Parse(RealWavelength.Text),
                Red = (byte)(this.dominantColor.R * 255),
                Green = (byte)(this.dominantColor.G * 255),
                Blue = (byte)(this.dominantColor.B * 255)
            });
        }

        private void UpdateContinuousCaptureControls()
        {
            ContinuousButtons.IsVisible = this.isCaptureContinuous;
            CapturePeriodEntry.IsVisible = this.isCaptureContinuous;
            MilliSecLabel.IsVisible = this.isCaptureContinuous;
            ButtonCamera.IsVisible = !this.isCaptureContinuous;
            StartWritingButton.IsEnabled = !this.isWritingToTheFile;
            StopWritingButton.IsEnabled = this.isWritingToTheFile;
            CapturePeriodEntry.IsEnabled = !this.isWritingToTheFile;
            ChosenFile.IsEnabled = !this.isWritingToTheFile;
            CaptureMethod.IsEnabled = !this.isWritingToTheFile;
        }

        private async void StartWritingButton_OnClicked(object sender, EventArgs e)
        {
            this.periodOfWriting = int.Parse(CapturePeriodEntry.Text);
            this.isWritingToTheFile = true;
            
            UpdateContinuousCaptureControls();

            await Task.Delay(2000);

            await this.WriteToFileUntilStopped();
        }

        private void StopWritingButton_OnClicked(object sender, EventArgs e)
        {
            this.isWritingToTheFile = false;

            UpdateContinuousCaptureControls();
        }

        private void SwitchCamerasButton_OnClicked(object sender, EventArgs e)
        {
            CameraPreview.OnClick();
        }
    }
}