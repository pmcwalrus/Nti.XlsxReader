using ClosedXML.Excel;
using Nti.XlsxReader.Entities;
using Nti.XlsxReader.Types;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
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

        private string _shmemsListName = "shmems";
        public string ShmemListName
        {
            get => _shmemsListName;
            set
            {
                _shmemsListName = value;
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

        private string _vkListName = "ВК";
        public string VkListName
        {
            get => _vkListName;
            set
            {
                _vkListName = value;
                OnPropertyChanged();
            }
        }

        private string _addShmemsListName = "add_shmems";
        public string AddShmemsListName
        {
            get => _addShmemsListName;
            set
            {
                _addShmemsListName = value;
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

        public void OpenFile(string fileName, IProgress<int> progress = null)
        {
            FileName = fileName;
            DataBase = ExtractFileData(fileName, progress);            
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

        private NtiBase ExtractFileData(string fileName, IProgress<int> progress = null)
        {
            BaseParsed = false;
            var result = new NtiBase();
            var wb = new XLWorkbook(fileName);
            result.Signals = new ObservableCollection<SignalEntity>(ParseSignals(wb, progress));
            result.Ip = new ObservableCollection<IpEntity>(ParseIp(wb));
            result.Ups = new ObservableCollection<UpsEntity>(ParseUps(wb));
            result.Layout = new ObservableCollection<SignalOnDevice>(ParseLayout(wb));
            result.Vk = new ObservableCollection<VkEntity>(ParseVk(wb));
            result.Shmems = new ObservableCollection<ShmemEntity>(ParseShmems(wb));
            result.XmlTop = GetXmlDirectParts(wb, XmlTopListName);
            result.XmlBot = GetXmlDirectParts(wb, XmlBotListName);
            result.AddShmems = GetXmlDirectParts(wb, AddShmemsListName);
            result.DeviceAdds = new ObservableCollection<DeviceAddition>(GetDeviceAdditions(wb));
            BaseParsed = true;
            return result;
        }

        #region Main Signals Base Parse

        private List<SignalEntity> ParseSignals(XLWorkbook wb, IProgress<int> progress = null)
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
                int ignoreCellColor = 0;
                try
                {
                    ignoreCellColor  = ws.Cell(i, ws.FirstColumnUsed().ColumnNumber()).Style.Fill.BackgroundColor.Color.ToArgb();
                }
                catch
                {
                    ignoreCellColor = 0;
                }
                if (ignoreCellColor == Color.FromArgb(255, 193, 152, 224).ToArgb())
                {
                    continue;
                }
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
                entity.Vk = GetParamValue(ws, signalColumns, i, Headers.VkHeader);
                entity.Is420mA = GetParamValue(ws, signalColumns, i, Headers.SignalTypeTextHeader).Contains("4-20");
                entity.UpdateTreshold = GetParamValue(ws, signalColumns, i, Headers.UpdateThersholdHeader);
                entity.Script = GetParamValue(ws, signalColumns, i, Headers.ScriptHeader);
                if (entity.SetpointTypes != null)
                {
                    if (entity.SetpointValues == null
                        || entity.SetpointTypes.Count != entity.SetpointValues.Count)
                        throw new FormatException($"Сигнал {entity.Description} содержит разное количество типов и значений уставок.");
                }
                result.Add(entity);
                progress?.Report((int)(((double)i / lastRow.RowNumber()) * 100));
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
            var typeColumn = ws.FirstColumnUsed();

            var ipColumns = ParseHeader(ws, ValueColumns.GetIpColumns()); // Parse Header
            var currentType = DeviceType.Unknown;
            for (var i = headerRow.RowBelow().RowNumber(); i <= lastRow.RowNumber(); ++i)
            {
                var deviceName = GetParamValue(ws, ipColumns, i, Headers.IpDeviceNameHeader);
                if (string.IsNullOrWhiteSpace(deviceName))
                {
                    var typeValue = ws.Cell(i, typeColumn.ColumnNumber()).GetString();
                    if (typeValue.Contains("!!"))
                    {
                        var typeStr = typeValue.Replace("!!", string.Empty);
                        if (typeStr == Headers.IpTypeDeviceString)
                            currentType = DeviceType.Device;
                        else if (typeStr == Headers.IpTypeWorkstationString)
                            currentType = DeviceType.Worstation;
                        else if (typeStr == Headers.IpTypeExternalSystemString)
                            currentType = DeviceType.ExternalSystem;
                        else currentType = DeviceType.Unknown;
                    }
                    continue;
                }
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
                    VideoGroup = GetParamValue(ws, ipColumns, i, Headers.IpVideoGroupHeader),
                    DeviceType = currentType,
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

        #region Parse Injections

        private string GetXmlDirectParts(XLWorkbook wb, string sheetName)
        {
            var ws = wb.Worksheet(sheetName);
            var firstRow = ws.FirstRowUsed();
            if (firstRow == null) return string.Empty;
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

        private List<DeviceAddition> GetDeviceAdditions(XLWorkbook wb)
        {
            var result = new List<DeviceAddition>();
            var sheets = wb.Worksheets.Where(x => x.Name.StartsWith(Headers.DeviceAdditionSheetPreamble));
            foreach (var s in sheets)
            {
                result.Add(new DeviceAddition
                {
                    DeviceName = s.Name.Remove(0, Headers.DeviceAdditionSheetPreamble.Length),
                    Addition = GetXmlDirectParts(wb, s.Name)
                });
            }
            return result;
        }

        #endregion

        #region Parse Vk

        private List<VkEntity> ParseVk(XLWorkbook wb)
        {
            var result = new List<VkEntity>();
            var ws = wb.Worksheet(VkListName);
            var headerRow = ws.FirstRowUsed();
            var lastRow = ws.LastRowUsed();
            var signalColumns = ParseHeader(ws, ValueColumns.GetVkColumns()); // Parse Header
            for (var i = headerRow.RowBelow().RowNumber(); i <= lastRow.RowNumber(); ++i)
            {
                var number = GetParamValue(ws, signalColumns, i, Headers.VkNumberHeader);
                if (string.IsNullOrWhiteSpace(number))
                    continue;
                var entity = new VkEntity
                {
                    Number = number,
                    Description = GetParamValue(ws, signalColumns, i, Headers.VkDescriptionHeader),
                    Name = GetParamValue(ws, signalColumns, i, Headers.VkNameHeader)
                };
                result.Add(entity);
            }
            return result;
        }

        #endregion

        #region Parse Shmems

        private List<ShmemEntity> ParseShmems(XLWorkbook wb)
        {
            var result = new List<ShmemEntity>();
            var ws = wb.Worksheet(ShmemListName);
            var headerRow = ws.FirstRowUsed();
            var lastRow = ws.LastRowUsed();
            var shmemColumns = ParseHeader(ws, ValueColumns.GetShmemColumns()); // Parse Header
            for (var i = headerRow.RowBelow().RowNumber(); i <= lastRow.RowNumber(); ++i)
            {
                var name = GetParamValue(ws, shmemColumns, i, Headers.ShmemNameHeader);
                if (string.IsNullOrWhiteSpace(name)) continue;
                var entity = new ShmemEntity
                {
                    Name = name,
                    KeepAlive = GetParamValue(ws, shmemColumns, i, Headers.ShmemKeepaliveHeader),
                    KeepMb = GetParamValue(ws, shmemColumns, i, Headers.ShmemKeepMbHeader),
                    KeepTime = GetParamValue(ws, shmemColumns, i, Headers.ShmemKeepTimeHeader),
                    Logging = GetParamValue(ws, shmemColumns, i, Headers.ShememLoggingHeader),
                    StaleTimeout = GetParamValue(ws, shmemColumns, i, Headers.ShmemStaleTimeoutHeader),
                    Type = GetParamValue(ws, shmemColumns, i, Headers.ShmemTypeHeader)
                };
                result.Add(entity);
            }
            return result;
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
            var value = string.Empty;
            //try
            //{
            //    value = ws.Cell(row, paramColumns
            //        .First(x => x.Header == paramHeader)
            //        .Column.ColumnNumber()).GetString();
            //}
            //catch (FormatException e)
            //{
                value = ws.Cell(row, paramColumns
                    .First(x => x.Header == paramHeader)
                    .Column.ColumnNumber()).CachedValue?.ToString(); ;
            //}
            return value;
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
