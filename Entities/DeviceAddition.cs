using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Nti.XlsxReader.Entities
{
    public class DeviceAddition : INotifyPropertyChanged
    {
        private string _deviceName;
        public string DeviceName
        {
            get => _deviceName;
            set
            {
                _deviceName = value;
                OnPropertyChanged();
            }
        }

        private string _addition;
        public string Addition
        {
            get => _addition;
            set
            {
                _addition = value;
                OnPropertyChanged();
            }
        }

        #region PropertyChanged Impllementation

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
