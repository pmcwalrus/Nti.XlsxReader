using ClosedXML.Excel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Nti.XlsxReader.Types
{
    public static class SignalColumns
    {
        public static List<SignalColumn> GetSignalColumns() => new List<SignalColumn>
            {
                new SignalColumn(Headers.DescriptionHeader),
                new SignalColumn(Headers.IndexHeader),
                new SignalColumn(Headers.UnitsHeader),
                new SignalColumn(Headers.SetpointsTypeHeader),
                new SignalColumn(Headers.SetpointValuesHeader),
                new SignalColumn(Headers.DelayTimeHeader),
                new SignalColumn(Headers.InversionHeader),
                new SignalColumn(Headers.SystemIdHeader),
                new SignalColumn(Headers.SignalIdHeader),
                new SignalColumn(Headers.SignalTypeHeader),
                new SignalColumn(Headers.PstsHeader),
                new SignalColumn(Headers.ShmemHeader),
                new SignalColumn(Headers.UpsHeader),
            };
    }

    public class SignalColumn : INotifyPropertyChanged
    {
        public SignalColumn(string header)
        {
            Header = header;
        }

        private IXLColumn _column;
        public IXLColumn Column
        {
            get => _column;
            set 
            {
                _column = value;
                OnPropertyChanged();
            }
        }

        private string _header;
        public string Header
        {
            get => _header;
            private set
            {
                _header = value;
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
