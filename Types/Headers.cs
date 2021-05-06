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
    }
}
