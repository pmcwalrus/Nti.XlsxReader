using ClosedXML.Excel;
using Nti.XlsxReader.Types;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Nti.XlsxReader
{
    public class XlsxReader : INotifyPropertyChanged
    {
        #region Sheets Names

        private string _signalListName = "Перечень";
        public string SignalListName
        {
            get => _signalListName;
            set
            {
                _signalListName = value;
                OnPropertyChanged();
            }
        }

        private string _signalOnDeviceListName = "Раскладка";
        public string SignalOnDeviceListName
        {
            get => _signalOnDeviceListName;
            set
            {
                _signalOnDeviceListName = value;
                OnPropertyChanged();
            }
        }

        private string _upsListName = "УПС";
        public string UpsListName
        {
            get => _upsListName;
            set
            {
                _upsListName = value;
                OnPropertyChanged();
            }
        }

        private string _ipListName = "IP";
        public string IpListName
        {
            get => _ipListName;
            set
            {
                _ipListName = value;
                OnPropertyChanged();
            }
        }

        #endregion

        private NtiBase _dataBase;
        public NtiBase DataBase 
        {
            get => _dataBase;
            private set
            {
                if (_dataBase == value) return;
                _dataBase = value;
                OnPropertyChanged();
            }
        }

        public void OpenFile(string fileName)
        {
            FileName = fileName;
            DataBase = ExtractFileData(fileName);
        }

        private string _fileName;
        public string FileName
        {
            get => _fileName;
            private set
            {
                _fileName = value;
                OnPropertyChanged();
            }
        }

        private NtiBase ExtractFileData(string fileName)
        {
            var result = new NtiBase();
            var wb = new XLWorkbook(fileName);
            result.Signals = new ObservableCollection<SignalEntity>(ParseSignals(wb));
            return result;
        }

        private List<SignalEntity> ParseSignals(XLWorkbook wb)
        {
            var result = new List<SignalEntity>();
            var ws = wb.Worksheet(SignalListName);
            var headerRow = ws.FirstRowUsed();
            var lastRow = ws.LastRowUsed();
            var lastColumn = ws.LastColumnUsed();

            var signalColumns = ParseSignalListHeader(ws); // Parse Header

            for (var i = headerRow.RowBelow().RowNumber(); i <= lastRow.RowNumber(); ++i)
            {
                var cellContent = ws.Cell(i, signalColumns.First(x =>
                x.Header == SignalEntity.DescriptionHeader)
                    .Column.ColumnNumber()).GetString();
                if (string.IsNullOrWhiteSpace(cellContent))
                    continue;
                result.Add(new SignalEntity { Description = cellContent });
            }
            return result;
        }

        private List<SignalColumn> ParseSignalListHeader(IXLWorksheet ws)
        {
            var lastColumn = ws.LastColumnUsed();
            var headerRow = ws.FirstRowUsed();
            var signalColumns = SignalColumns.GetSignalColumns();
            for (var i = 1; i <= lastColumn.ColumnNumber(); ++i) // Parse header
            {
                foreach (var col in signalColumns)
                {
                    var cellValue = ws.Cell(headerRow.RowNumber(), i)
                        .GetString()
                        .Replace("\r", string.Empty)
                        .Replace("\n", string.Empty)
                        .Replace(" ", string.Empty);
                    var header = col.Header
                        .Replace("\r", string.Empty)
                        .Replace("\n", string.Empty)
                        .Replace(" ", string.Empty);
                    if (cellValue.Contains(header, StringComparison.OrdinalIgnoreCase))
                        col.Column = ws.Column(i);
                }
            }
            var notExisted = signalColumns.FirstOrDefault(x => x.Column == null);
            if (notExisted != null)
                throw new Exception($"Can't find column with '{notExisted.Header}' header");
            return signalColumns;
        }

        #region PropertyChanged Impllementation

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

    }
}
