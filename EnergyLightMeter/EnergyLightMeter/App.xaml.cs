using System;
using EnergyLightMeter.View;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EnergyLightMeter
{
    public partial class App : Application
    {
        private float light;
        private string waveLenght;
        private MeasurePage measurePage;

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
                measurePage.UpdateLight(value);
            }
        }

        public App()
        {
            InitializeComponent();

            Plugin.Media.CrossMedia.Current.Initialize();

            var tabbedPge = new TabbedPage();
            measurePage = new MeasurePage();
            tabbedPge.Children.Add(measurePage);
            tabbedPge.Children.Add(new FileDataPage());

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
