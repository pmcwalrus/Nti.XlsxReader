namespace Nti.XlsxReader.Types
{
    public static class Headers
    {
        #region Main Signal List Headers

        public static string DescriptionHeader {get;} = "Наименование параметра";
        public static string IndexHeader { get; } = "Индекс параметра";
        public static string UnitsHeader { get; } = "Единицы измерения";
        public static string SetpointsTypeHeader { get; } = "Тип уставки";
        public static string SetpointValuesHeader { get; } = "Значение";
        public static string DelayTimeHeader { get; } = "Время задержки,с";
        public static string InversionHeader { get; } = "Инверсия";
        public static string SystemIdHeader { get; } = "System ID";
        public static string SignalIdHeader { get; } = "Signal ID (new)";
        public static string SignalTypeHeader { get; } = "Type";
        public static string PstsHeader { get; } = "ПСТС";
        public static string ShmemHeader { get; } = "shmem";
        public static string UpsHeader { get; } = "УПС";

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
        #endregion
    }
}
