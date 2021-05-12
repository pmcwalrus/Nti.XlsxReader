using Nti.XlsxReader.Entities;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Nti.XlsxReader.Types
{
    public class NtiBase : INotifyPropertyChanged
    {
   
        public NtiBase()
        {
            Signals = new ObservableCollection<SignalEntity>();
            Ip = new ObservableCollection<IpEntity>();
            Ups = new ObservableCollection<UpsEntity>();
            Layout = new ObservableCollection<SignalOnDevice>();
            DeviceAdds = new ObservableCollection<DeviceAddition>();
        }

        private ObservableCollection<SignalEntity> _signals;
        /// <summary>
        /// Main signals worksheet data
        /// </summary>
        public ObservableCollection<SignalEntity> Signals
        {
            get => _signals;
            set
            {
                _signals = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<IpEntity> _ip;
        /// <summary>
        /// IP worksheet data
        /// </summary>
        public ObservableCollection<IpEntity> Ip
        {
            get => _ip;
            set
            {
                _ip = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<UpsEntity> _ups;
        public ObservableCollection<UpsEntity> Ups
        {
            get => _ups;
            set
            {
                _ups = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<SignalOnDevice> _layout;
        public ObservableCollection<SignalOnDevice> Layout
        {
            get => _layout;
            set
            {
                _layout = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<DeviceAddition> _deviceAdds;
        public ObservableCollection<DeviceAddition> DeviceAdds
        {
            get => _deviceAdds;
            set
            {
                _deviceAdds = value;
                OnPropertyChanged();
            }
        }

        private string _xmlTop;
        public string XmlTop
        {
            get => _xmlTop;
            set
            {
                _xmlTop = value;
                OnPropertyChanged();
            }
        }

        private string _xmlBot;
        public string XmlBot
        {
            get => _xmlBot;
            set
            {
                _xmlBot = value;
                OnPropertyChanged();
            }
        }

        private string _addShmems;
        public string AddShmems
        {
            get => _addShmems;
            set
            {
                _addShmems = value;
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
