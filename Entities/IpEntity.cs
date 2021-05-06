using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Nti.XlsxReader.Entities
{
    public class IpEntity : INotifyPropertyChanged
    {
        private string _device;
        public string Device
        {
            get => _device;
            set
            {
                _device = value;
                OnPropertyChanged();
            }
        }

        private string _network1;
        public string Network1
        {
            get => _network1;
            set
            {
                _network1 = value;
                OnPropertyChanged();
            }
        }

        private string _network2;
        public string Network2
        {
            get => _network2;
            set
            {
                _network2 = value;
                OnPropertyChanged();
            }
        }

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

        private string _iFace1;
        public string IFace1
        {
            get => _iFace1;
            set
            {
                _iFace1 = value;
                OnPropertyChanged();
            }
        }

        private string _iFace2;
        public string IFace2
        {
            get => _iFace2;
            set
            {
                _iFace2 = value;
                OnPropertyChanged();
            }
        }

        private string _priority;
        public string Priority
        {
            get => _priority;
            set
            {
                _priority = value;
                OnPropertyChanged();
            }
        }

        private string _videoGroup;
        public string VideoGroup
        {
            get => _videoGroup;
            set
            {
                _videoGroup = value;
                OnPropertyChanged();
            }
        }

        private string _control;
        public string Control
        {
            get => _control;
            set
            {
                _control = value;
                OnPropertyChanged();
            }
        }

        private string _registrator;
        public string Registrator
        {
            get => _registrator;
            set
            {
                _registrator = value;
                OnPropertyChanged();
            }
        }

        private string _registratorTimeout;
        public string RegistartorTimeout
        {
            get => _registratorTimeout;
            set
            {
                _registratorTimeout = value;
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
