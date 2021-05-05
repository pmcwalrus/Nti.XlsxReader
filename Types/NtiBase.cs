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
        }

        private ObservableCollection<SignalEntity> _signals;
        /// <summary>
        /// Раскладка
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

        #region PropertyChanged Impllementation

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
