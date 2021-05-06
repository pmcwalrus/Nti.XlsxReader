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

        #region PropertyChanged Impllementation

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
