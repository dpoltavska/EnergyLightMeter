using EnergyLightMeter.Services;
using EnergyLightMeter.View;
using Xamarin.Forms;

namespace EnergyLightMeter
{
    public partial class App : Application
    {
        private Color color;
        private float light;
        private string waveLenght;
        private MeasurePage measurePage;
        private FileViewUpdater fileViewUpdater = new FileViewUpdater();

        public float LightValue
        {
            get => light;
            set
            {
                light = value;
                measurePage.UpdateLight(value);
            }
        }

        public string WaveLenghtValue
        {
            get => waveLenght;
            set
            {
                waveLenght = value;
                measurePage.UpdateWaveLenght(value);
            }
        }
        public Color DominantColor
        {
            get => color;
            set
            {
                color = value;
                measurePage.UpdateDominantColor(value);
            }
        }

        public App()
        {
            InitializeComponent();

            Plugin.Media.CrossMedia.Current.Initialize();

            var tabbedPge = new TabbedPage();

            measurePage = new MeasurePage();
            measurePage.FileViewUpdater = this.fileViewUpdater;

            var fileDataPage = new FileDataPage();
            this.fileViewUpdater.UpdateFile += fileDataPage.InitializeGrid;

            tabbedPge.Children.Add(measurePage);
            tabbedPge.Children.Add(fileDataPage);

            MainPage = tabbedPge;
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
