using ClosedXML.Excel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Nti.XlsxReader.Types
{
    public static class ValueColumns
    {
        public static List<ValueColumn> GetSignalColumns() => new List<ValueColumn>
            {
                new ValueColumn(Headers.DescriptionHeader),
                new ValueColumn(Headers.IndexHeader),
                new ValueColumn(Headers.UnitsHeader),
                new ValueColumn(Headers.SetpointsTypeHeader),
                new ValueColumn(Headers.SetpointValuesHeader),
                new ValueColumn(Headers.DelayTimeHeader),
                new ValueColumn(Headers.InversionHeader),
                new ValueColumn(Headers.SystemIdHeader),
                new ValueColumn(Headers.SignalIdHeader),
                new ValueColumn(Headers.SignalTypeHeader),
                new ValueColumn(Headers.PstsHeader),
                new ValueColumn(Headers.ShmemHeader),
                new ValueColumn(Headers.UpsHeader),
            };

        public static List<ValueColumn> GetIpColumns() => new List<ValueColumn>
        {
            new ValueColumn(Headers.IpDeviceHeader),
            new ValueColumn(Headers.IpNetwork1Header),
            new ValueColumn(Headers.IpNetwork2Header),
            new ValueColumn(Headers.IpDeviceNameHeader),
            new ValueColumn(Headers.IpIFace1Header),
            new ValueColumn(Headers.IpIFace2Header),
            new ValueColumn(Headers.IpPriorityHeader),
            new ValueColumn(Headers.IpVideoGroupHeader),
            new ValueColumn(Headers.IpControlHeader),
            new ValueColumn(Headers.IpRegistartorHeader),
            new ValueColumn(Headers.IpRegistartorTimeoutHeader),
        };

        public static List<ValueColumn> GetUpsColumns() => new List<ValueColumn>
        {
            new ValueColumn(Headers.UpsIdHeader),
            new ValueColumn(Headers.UpsGroupHeader),
            new ValueColumn(Headers.UpsAlarmGroupHeader),
            new ValueColumn(Headers.UpsWindowHeader)
        };
    }

    public class ValueColumn : INotifyPropertyChanged
    {
        public ValueColumn(string header)
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
