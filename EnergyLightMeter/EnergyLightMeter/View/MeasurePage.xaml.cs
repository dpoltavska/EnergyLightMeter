using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnergyLightMeter.Services;
using Plugin.Media;
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

        public void UpdateLight(float light)
        {
            Label_val.Text = light.ToString();
        }
    }
}