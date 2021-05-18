namespace Nti.XlsxReader.Types
{
    public static class Headers
    {
        #region Main Signal List Headers

        public static string DescriptionHeader { get; set; } = "Наименование параметра";
        public static string IndexHeader { get; set; } = "Индекс параметра";
        public static string UnitsHeader { get; set; } = "Единицы измерения";
        public static string SetpointsTypeHeader { get; set; } = "Тип уставки";
        public static string SetpointValuesHeader { get; set; } = "Значение";
        public static string DelayTimeHeader { get; set; } = "Время задержки,с";
        public static string InversionHeader { get; set; } = "Инверсия";
        public static string SystemIdHeader { get; set; } = "System ID";
        public static string SignalIdHeader { get; set; } = "Signal ID (new)";
        public static string SignalTypeHeader { get; set; } = "Type";
        public static string PstsHeader { get; set; } = "ПСТС";
        public static string ShmemHeader { get; set; } = "shmem";
        public static string UpsHeader { get; set; } = "УПС";
        public static string SignalTypeTextHeader { get; set; } = "Тип сигнала";
        public static string VkHeader { get; set; } = "ВК";

        #endregion

        #region IP Headers

        public static string IpDeviceHeader { get; set; } = "Устройство";
        public static string IpNetwork1Header { get; set; } = "Сеть 1";
        public static string IpNetwork2Header { get; set; } = "Сеть 2";
        public static string IpDeviceNameHeader { get; set; } = "device_name";
        public static string IpIFace1Header { get; set; } = "iface1";
        public static string IpIFace2Header { get; set; } = "iface2";
        public static string IpPriorityHeader { get; set; } = "Приоритет";
        public static string IpVideoGroupHeader { get; set; } = "Группа видеокадров";
        public static string IpControlHeader { get; set; } = "Управление при";
        public static string IpRegistartorHeader { get; set; } = "registrator";
        public static string IpRegistartorTimeoutHeader { get; set; } = "registrator_timeout";

        public static string IpTypeWorkstationString { get; set; } = "Рабочие станции";
        public static string IpTypeDeviceString { get; set; } = "Приборы сопряжения";
        public static string IpTypeExternalSystemString { get; set; } = "Чужие системы ";
        

        #endregion

        #region Ups Headers

        public static string UpsIdHeader { get; set; } = "id";
        public static string UpsGroupHeader { get; set; } = "Группа УПС";
        public static string UpsAlarmGroupHeader { get; set; } = "№ Табло";
        public static string UpsWindowHeader { get; set; } = "Окно СК";

        #endregion

        public static string DeviceAdditionSheetPreamble { get; set; } = "dev_";

        #region Layout Header

        public static string LayoutStartSeparator { get; set; } = "Start_address";

        #endregion

        #region Vk Headers

        public static string VkNumberHeader { get; set; } = "№";
        public static string VkDescriptionHeader { get; set; } = "Наименование";        
        public static string VkNameHeader { get; set; } = "name";

        #endregion 
    }
}
