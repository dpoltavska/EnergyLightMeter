using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using EnergyLightMeter.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(EnergyLightMeter.Droid.LightProvider))]
namespace EnergyLightMeter.Droid
{
    public class LightProvider : ILightProvider
    {
        public float GetLightValue()
        {
            return 666;
        }
    }
}