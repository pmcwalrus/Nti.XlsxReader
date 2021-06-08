using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Nti.XlsxReader.Entities
{
    public class ShmemEntity : INotifyPropertyChanged
    {
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        private string _type;
        public string Type
        {
            get => _type;
            set
            {
                _type = value;
                OnPropertyChanged();
            }
        }

        private string _staleTimeout;
        public string StaleTimeout
        {
            get => _staleTimeout;
            set
            {
                _staleTimeout = value;
                OnPropertyChanged();
            }
        }

        private string _keepAlive;
        public string KeepAlive
        {
            get => _keepAlive;
            set
            {
                _keepAlive = value;
                OnPropertyChanged();
            }
        }

        private string _logging;
        public string Logging
        {
            get => _logging;
            set
            {
                _logging = value;
                OnPropertyChanged();
            }
        }

        private string _keepTime;
        public string KeepTime
        {
            get => _keepTime;
            set
            {
                _keepTime = value;
                OnPropertyChanged();
            }
        }

        private string _keepMb;
        public string KeepMb
        {
            get => _keepMb;
            set
            {
                _keepMb = value;
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
