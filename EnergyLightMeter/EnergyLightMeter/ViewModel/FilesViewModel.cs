using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace EnergyLightMeter.ViewModel
{
    public class FilesViewModel : INotifyPropertyChanged
    {
        private string selectedFile;

        public List<string> ExistingFiles { get; set; }

        public string SelectedFile
        {
            get => selectedFile;
            set
            {
                selectedFile = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public FilesViewModel()
        {
            ExistingFiles = new List<string>() {"File1.txt", "File2.txt"};
            SelectedFile = ExistingFiles.First();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
