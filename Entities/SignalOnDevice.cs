using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Nti.XlsxReader.Entities
{
    public class SignalOnDevice : INotifyPropertyChanged
    {

        private string _index;
        public string Index
        {
            get => _index;
            set
            {
                _index = value;
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

        private string _numberStr;
        public string NumberStr
        {
            get => _numberStr;
            set
            {
                _numberStr = value;
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
