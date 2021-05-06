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
                var description = GetSignalParam(ws, signalColumns, i, SignalEntity.DescriptionHeader);
                if (string.IsNullOrWhiteSpace(description))
                    continue;
                var entity = new SignalEntity { Description = description };
                entity.Index = GetSignalParam(ws, signalColumns, i, SignalEntity.IndexHeader);
                entity.DelayTimeString = GetSignalParam(ws, signalColumns, i, SignalEntity.DelayTimeHeader);
                entity.Inversion = !string.IsNullOrWhiteSpace(GetSignalParam(ws, signalColumns, i, SignalEntity.InversionHeader));
                entity.Psts = GetSignalParam(ws, signalColumns, i, SignalEntity.PstsHeader);
                entity.SetpoinsValueString = GetSignalParam(ws, signalColumns, i, SignalEntity.SetpointValuesHeader);
                entity.SetpointTypesString = GetSignalParam(ws, signalColumns, i, SignalEntity.SetpointsTypeHeader);
                entity.Shmem = GetSignalParam(ws, signalColumns, i, SignalEntity.ShmemHeader);
                entity.SignalId = GetSignalParam(ws, signalColumns, i, SignalEntity.SignalIdHeader);
                entity.SystemId = GetSignalParam(ws, signalColumns, i, SignalEntity.SystemIdHeader);
                entity.Units = GetSignalParam(ws, signalColumns, i, SignalEntity.UnitsHeader);
                entity.Ups = GetSignalParam(ws, signalColumns, i, SignalEntity.UpsHeader);
                entity.TypeString = GetSignalParam(ws, signalColumns, i, SignalEntity.SignalTypeHeader);
                result.Add(entity);
            }
            return result;
        }
        private static string GetSignalParam(IXLWorksheet ws, List<SignalColumn> signalColumns, int row, string paramHeader)
        {
            return ws.Cell(row, signalColumns
                .First(x => x.Header == paramHeader)
                .Column.ColumnNumber()).GetString();
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
