﻿using ClosedXML.Excel;
using Nti.XlsxReader.Entities;
using Nti.XlsxReader.Types;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

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

        private string _layoutListName = "Раскладка";
        public string LayoutListName
        {
            get => _layoutListName;
            set
            {
                _layoutListName = value;
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

        private string _xmlTopListName = "xml_top";
        public string XmlTopListName
        {
            get => _xmlTopListName;
            set
            {
                _xmlTopListName = value;
                OnPropertyChanged();
            }
        }

        private string _xmlBotListName = "xml_bot";
        public string XmlBotListName
        {
            get => _xmlBotListName;
            set
            {
                _xmlBotListName = value;
                OnPropertyChanged();
            }
        }

        #endregion

        private bool _baseParsed;
        public bool BaseParsed 
        {
            get => _baseParsed;
            private set
            {
                _baseParsed = value;
                OnPropertyChanged();
            }
        }

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
            BaseParsed = false;
            var result = new NtiBase();
            var wb = new XLWorkbook(fileName);
            result.Signals = new ObservableCollection<SignalEntity>(ParseSignals(wb));
            result.Ip = new ObservableCollection<IpEntity>(ParseIp(wb));
            result.Ups = new ObservableCollection<UpsEntity>(ParseUps(wb));
            result.Layout = new ObservableCollection<SignalOnDevice>(ParseLayout(wb));
            result.XmlTop = GetXmlDirectParts(wb, XmlTopListName);
            result.XmlBot = GetXmlDirectParts(wb, XmlBotListName);
            BaseParsed = true;
            return result;
        }

        #region Main Signals Base Parse

        private List<SignalEntity> ParseSignals(XLWorkbook wb)
        {
            var result = new List<SignalEntity>();
            var ws = wb.Worksheet(SignalListName);
            var headerRow = ws.FirstRowUsed();
            var lastRow = ws.LastRowUsed();

            var signalColumns = ParseHeader(ws, ValueColumns.GetSignalColumns()); // Parse Header

            for (var i = headerRow.RowBelow().RowNumber(); i <= lastRow.RowNumber(); ++i)
            {
                var description = GetParamValue(ws, signalColumns, i, Headers.DescriptionHeader);
                if (string.IsNullOrWhiteSpace(description))
                    continue;
                var entity = new SignalEntity { Description = description };
                entity.Index = GetParamValue(ws, signalColumns, i, Headers.IndexHeader);
                entity.DelayTimeString = GetParamValue(ws, signalColumns, i, Headers.DelayTimeHeader);
                entity.Inversion = !string.IsNullOrWhiteSpace(GetParamValue(ws, signalColumns, i, Headers.InversionHeader));
                entity.Psts = GetParamValue(ws, signalColumns, i, Headers.PstsHeader);
                entity.SetpoinsValueString = GetParamValue(ws, signalColumns, i, Headers.SetpointValuesHeader);
                entity.SetpointTypesString = GetParamValue(ws, signalColumns, i, Headers.SetpointsTypeHeader);
                entity.Shmem = GetParamValue(ws, signalColumns, i, Headers.ShmemHeader);
                entity.SignalId = GetParamValue(ws, signalColumns, i, Headers.SignalIdHeader);
                entity.SystemId = GetParamValue(ws, signalColumns, i, Headers.SystemIdHeader);
                entity.Units = GetParamValue(ws, signalColumns, i, Headers.UnitsHeader);
                entity.Ups = GetParamValue(ws, signalColumns, i, Headers.UpsHeader);
                entity.TypeString = GetParamValue(ws, signalColumns, i, Headers.SignalTypeHeader);
                result.Add(entity);
            }
            return result;
        }

        #endregion

        #region IP data parse

        private List<IpEntity> ParseIp(XLWorkbook wb)
        {
            var result = new List<IpEntity>();
            var ws = wb.Worksheet(IpListName);
            var headerRow = ws.FirstRowUsed();
            var lastRow = ws.LastRowUsed();

            var ipColumns = ParseHeader(ws, ValueColumns.GetIpColumns()); // Parse Header

            for (var i = headerRow.RowBelow().RowNumber(); i <= lastRow.RowNumber(); ++i)
            {
                var deviceName = GetParamValue(ws, ipColumns, i, Headers.IpDeviceNameHeader);
                if (string.IsNullOrWhiteSpace(deviceName))
                    continue;
                var entity = new IpEntity
                {
                    DeviceName = deviceName,
                    Device = GetParamValue(ws, ipColumns, i, Headers.IpDeviceHeader),
                    Control = GetParamValue(ws, ipColumns, i, Headers.IpControlHeader),
                    IFace1 = GetParamValue(ws, ipColumns, i, Headers.IpIFace1Header),
                    IFace2 = GetParamValue(ws, ipColumns, i, Headers.IpIFace2Header),
                    Network1 = GetParamValue(ws, ipColumns, i, Headers.IpNetwork1Header),
                    Network2 = GetParamValue(ws, ipColumns, i, Headers.IpNetwork2Header),
                    Priority = GetParamValue(ws, ipColumns, i, Headers.IpPriorityHeader),
                    RegistartorTimeout = GetParamValue(ws, ipColumns, i, Headers.IpRegistartorTimeoutHeader),
                    Registrator = GetParamValue(ws, ipColumns, i, Headers.IpRegistartorHeader),
                    VideoGroup = GetParamValue(ws, ipColumns, i, Headers.IpVideoGroupHeader)
                };
                result.Add(entity);
            }
            return result;
        }

        #endregion

        #region Parse UPS

        public List<UpsEntity> ParseUps(XLWorkbook wb)
        {
            var result = new List<UpsEntity>();
            var ws = wb.Worksheet(UpsListName);
            var headerRow = ws.FirstRowUsed();
            var lastRow = ws.LastRowUsed();

            var upsColumns = ParseHeader(ws, ValueColumns.GetUpsColumns()); // Parse Header

            for (var i = headerRow.RowBelow().RowNumber(); i <= lastRow.RowNumber(); ++i)
            {
                var id = GetParamValue(ws, upsColumns, i, Headers.UpsIdHeader);
                if (string.IsNullOrWhiteSpace(id))
                    continue;
                var entity = new UpsEntity
                {
                    Id = id,
                    AlarmGroup = GetParamValue(ws, upsColumns, i, Headers.UpsAlarmGroupHeader),
                    Group = GetParamValue(ws, upsColumns, i, Headers.UpsGroupHeader),
                    Window = GetParamValue(ws, upsColumns, i, Headers.UpsWindowHeader),
                };
                result.Add(entity);
            }
            return result;
        }

        #endregion

        #region Parse Layout

        private List<SignalOnDevice> ParseLayout(XLWorkbook wb)
        {
            var result = new List<SignalOnDevice>();
            var ws = wb.Worksheet(LayoutListName);
            var mainColumn = ws.FirstColumnUsed();

            var separatorRows = GetLayoutSepratorRows(ws, mainColumn);
            for (var i = 0; i < separatorRows.Count; i++)
            {
                IXLRow lastRow = i < separatorRows.Count - 1
                    ? separatorRows[i + 1].RowAbove(2)
                    : ws.LastRowUsed();
                var signals = ParseLayoutDevice(ws, mainColumn, separatorRows[i], lastRow);
                result.AddRange(signals);
            }
            return result;
        }

        private List<SignalOnDevice> ParseLayoutDevice(IXLWorksheet ws, 
            IXLColumn firstColumn, IXLRow separatorRow, IXLRow lastRow)
        {
            var result = new List<SignalOnDevice>();
            var deviceIndexRow = separatorRow.RowAbove();
            var deviceIndex = ws.Cell(deviceIndexRow.RowNumber(), firstColumn.ColumnNumber()).GetString();
            for (var columnOffset = 1; columnOffset <= 8; columnOffset++)
            {
                var deviceType = ws.Cell(deviceIndexRow.RowNumber(), firstColumn.ColumnNumber() + columnOffset).GetString();
                var startAddressStr = ws.Cell(separatorRow.RowNumber(), firstColumn.ColumnNumber() + columnOffset).GetString();
                var parseRes = int.TryParse(startAddressStr, out var startAddress);
                for (var row = separatorRow.RowBelow().RowNumber(); row <= lastRow.RowNumber(); ++row)
                {
                    var signalName = ws.Cell(row, firstColumn.ColumnNumber() + columnOffset).GetString();
                    if (string.IsNullOrWhiteSpace(signalName)) continue;
                    if (!parseRes) 
                        throw new FormatException($"Invalid start address format: {startAddressStr}");
                    var signal = new SignalOnDevice
                    {
                        SignalName = signalName,
                        DeviceIndex = deviceIndex,
                        Type = deviceType,
                        NumberStr = (startAddress + row - separatorRow.RowBelow().RowNumber()).ToString(),
                    };
                    result.Add(signal);
                }
            }
            return result;
        }

        private List<IXLRow> GetLayoutSepratorRows(IXLWorksheet ws, IXLColumn column)
        {
            var result = new List<IXLRow>();
            var lastRow = ws.LastRowUsed();
            for (var i = 1; i <= lastRow.RowNumber(); i++)
            {
                var cellValue = ws.Cell(i, column.ColumnNumber()).GetString();
                if (cellValue == Headers.LayoutStartSeparator)
                    result.Add(ws.Row(i));
            }
            return result;
        }

        #endregion

        #region Parse Top and Bottom

        private string GetXmlDirectParts(XLWorkbook wb, string sheetName)
        {
            var ws = wb.Worksheet(sheetName);
            var firstRow = ws.FirstRowUsed();
            var lastRow = ws.LastRowUsed();
            var firstColumn = ws.FirstColumnUsed();
            var lastColumn = ws.LastColumnUsed();
            var result = new StringBuilder();
            for (var row = firstRow.RowNumber(); row <= lastRow.RowNumber(); row++)
            {
                for (var col = firstColumn.ColumnNumber(); col <= lastColumn.ColumnNumber(); col++)
                {
                    var cellValue = ws.Cell(row, col).GetString();
                    result.Append(cellValue);
                    result.Append("\t");
                }
                result.Append("\r\n");
            }
            return result.ToString();
        }

        #endregion

        private List<ValueColumn> ParseHeader(IXLWorksheet ws, List<ValueColumn> valueColumns)
        {
            var lastColumn = ws.LastColumnUsed();
            var headerRow = ws.FirstRowUsed();
            for (var i = 1; i <= lastColumn.ColumnNumber(); ++i) // Parse header
            {
                foreach (var col in valueColumns)
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
                    if (cellValue.Equals(header, StringComparison.OrdinalIgnoreCase))
                        col.Column = ws.Column(i);
                }
            }
            var notExisted = valueColumns.FirstOrDefault(x => x.Column == null);
            if (notExisted != null)
                throw new Exception($"Can't find column with '{notExisted.Header}' header");
            return valueColumns;
        }

        private static string GetParamValue(IXLWorksheet ws, List<ValueColumn> paramColumns, int row, string paramHeader)
        {
            return ws.Cell(row, paramColumns
                .First(x => x.Header == paramHeader)
                .Column.ColumnNumber()).GetString();
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
