using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using EnergyLightMeter.Extensions;
using EnergyLightMeter.Services;
using EnergyLightMeter.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EnergyLightMeter.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FileDataPage : ContentPage
    {
        private IFileProvider fileProvider;

        public FilesViewModel FileNames { get; set; }

        public FileDataPage()
        {
            InitializeComponent();

            fileProvider = DependencyService.Get<IFileProvider>();

            FileNames = new FilesViewModel();

            BindingContext = FileNames;

            InitializaGrid();
        }

        public void InitializaGrid()
        {
            ScrollView sc = new ScrollView();
            Grid innerG = new Grid();

            innerG.ColumnDefinitions = new ColumnDefinitionCollection();

            for (int i = 0; i < 6; i++)
            {
                innerG.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            }

            var bgColor = Color.Silver;
            var rowsColor = Color.WhiteSmoke;
            int padding = 10;

            var style = new Style(typeof(Label))
            {
                Setters =
                {
                    //new Setter
                    //{
                    //    Property = Label.PaddingProperty,
                    //    Value = 10
                    //},
                    //new Setter
                    //{
                    //    Property = Label.BackgroundColorProperty,
                    //    Value = bgColor
                    //},
                    new Setter
                    {
                        Property =Label.VerticalTextAlignmentProperty,
                        Value = TextAlignment.Center
                    },
                    new Setter
                    {
                        Property = Label.HorizontalTextAlignmentProperty,
                        Value = TextAlignment.Center
                    }
                }
            };

            innerG.Children.Add(new Label() { Text = "Time of capture", Style = style }, 0, 0);
            innerG.Children.Add(new Label() { Text = "Lx(m)", Style = style },  1, 0);
            innerG.Children.Add(new Label() { Text = "Lx(r)", Style = style }, 2, 0);
            innerG.Children.Add(new Label() { Text = "Diapason", Style = style }, 3, 0);
            innerG.Children.Add(new Label() { Text = "Wavelength", Style = style }, 4, 0);

            var stack = new StackLayout();
            stack.Children.Add(new Label() { Text = "Dominant Color", Style = style });

            innerG.Children.Add(stack, 5, 0);

            innerG.RowDefinitions = new RowDefinitionCollection();

            var statistics = fileProvider.GetRecords(this.FileNames.SelectedFile);

            for (int i = 0; i < statistics.Count; i++)
            {
                innerG.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

                var record = statistics.ElementAt(i);

                innerG.Children.Add(new Label() { Text = record.Date.ToString(), BackgroundColor = rowsColor }, 0, i+1);
                innerG.Children.Add(new Label() { Text = record.MeasuredIluminance.ToString(), BackgroundColor = rowsColor }, 1, i + 1);
                innerG.Children.Add(new Label() { Text = record.RealIluminance?.ToString() ?? "-", HorizontalTextAlignment = TextAlignment.Center, BackgroundColor = rowsColor }, 2, i + 1);
                innerG.Children.Add(new Label() { Text = record.WavelengthDiapason, BackgroundColor = rowsColor }, 3, i + 1);
                innerG.Children.Add(new Label() { Text = record.WaveLength?.ToString() ?? "-" , HorizontalTextAlignment = TextAlignment.Center, BackgroundColor = rowsColor }, 4, i + 1);
                innerG.Children.Add(
                    new BoxView()
                    {
                        Color = ColorExtension.CreateColor(record.Red, record.Green, record.Blue),
                        HorizontalOptions = LayoutOptions.Fill,
                        VerticalOptions = LayoutOptions.Fill,
                        BackgroundColor = rowsColor
                    }, 5, i+1);
            }

            sc.Content = innerG; //content in scrollview
            StackLayoutGrid.Children.Clear(); //header grid view in stackLayout
            StackLayoutGrid.Children.Add(sc); // scrollview in stackLayout
        }

        private void FilePageChosenFile_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            using (UserDialogs.Instance.Loading("Opening", maskType: MaskType.Black))
            {
                FileNames.SelectedFile = FileNames.ExistingFiles[FilePageChosenFile.SelectedIndex];

                InitializaGrid();
            }
        }
    }
}