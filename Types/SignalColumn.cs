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
                new SignalColumn(SignalEntity.DescriptionHeader),
                new SignalColumn(SignalEntity.IndexHeader),
                new SignalColumn(SignalEntity.UnitsHeader),
                new SignalColumn(SignalEntity.SetpointsTypeHeader),
                new SignalColumn(SignalEntity.SetpointValuesHeader),
                new SignalColumn(SignalEntity.DelayTimeHeader),
                new SignalColumn(SignalEntity.InversionHeader),
                new SignalColumn(SignalEntity.SystemIdHeader),
                new SignalColumn(SignalEntity.SignalIdHeader),
                new SignalColumn(SignalEntity.SignalTypeHeader),
                new SignalColumn(SignalEntity.PstsHeader),
                new SignalColumn(SignalEntity.ShmemHeader),
                new SignalColumn(SignalEntity.UpsHeader),
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
