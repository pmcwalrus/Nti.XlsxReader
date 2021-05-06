using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Nti.XlsxReader.Types
{
    public class SignalEntity : INotifyPropertyChanged
    {
        public const string DescriptionHeader = "Наименование параметра";
        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        public const string IndexHeader = "Индекс параметра";
        private string _index;
        public string Index
        {
            get => _index;
            set
            {
                _index = value;
                OnPropertyChanged();
            }
        }

        public const string UnitsHeader = "Единицы измерения";
        private string _units;
        public string Units
        {
            get => _units;
            set
            {
                _units = value;
                OnPropertyChanged();
            }
        }

        public const string SetpointsTypeHeader= "Тип уставки";
        private string _setpointTypesString;
        public string SetpointTypesString
        {
            get => _setpointTypesString;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    SetpointTypes = null;
                else
                {
                    SetpointTypes = new List<SetpointTypes>();
                    var sp = value
                        .Replace(" ", string.Empty)
                        .Split(',', StringSplitOptions.RemoveEmptyEntries);
                    foreach (var s in sp)
                    {
                        switch(s)
                        {
                            case "LL":
                                SetpointTypes.Add(Types.SetpointTypes.LL);
                                break;
                            case "L":
                                SetpointTypes.Add(Types.SetpointTypes.L);
                                break;
                            case "HH":
                                SetpointTypes.Add(Types.SetpointTypes.HH);
                                break;
                            case "H":
                                SetpointTypes.Add(Types.SetpointTypes.H);
                                break;
                            default:
                                throw new ArgumentException($"Unknown setpoint value: {s}");
                        }
                    }
                }
                _setpointTypesString = value;
            }
        }
        private List<SetpointTypes> _setpointTypes;
        public List<SetpointTypes> SetpointTypes
        {
            get => _setpointTypes;
            private set
            {
                _setpointTypes = value;
                OnPropertyChanged();
            }
        }

        public const string SetpointValuesHeader = "Значение";
        private string _setpointsValueString;
        public string SetpoinsValueString
        {
            get => _setpointsValueString;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    SetpointValues = null;
                else
                {
                    SetpointValues = new List<string>();
                    SetpointValues.AddRange(value
                        .Replace(" ", string.Empty)
                        .Split(',', StringSplitOptions.RemoveEmptyEntries));
                }
                _setpointsValueString = value;
                OnPropertyChanged();
            }
        }
        private List<string> _setpointValues;
        public List<string> SetpointValues
        {
            get => _setpointValues;
            private set
            {
                _setpointValues = value;
                OnPropertyChanged();
            }
        }

        public const string DelayTimeHeader = "Время задержки,с";
        private string _delayTimeString;
        public string DelayTimeString
        {
            get => _delayTimeString;
            set
            {
                _delayTimeString = value;
                OnPropertyChanged();
            }
        }

        public const string InversionHeader = "Инверсия";
        private bool _inversion;
        public bool Inversion
        {
            get => _inversion;
            set
            {
                _inversion = value;
                OnPropertyChanged();
            }
        }

        public const string SystemIdHeader = "System ID";
        private string _systemId;
        public string SystemId
        {
            get => _systemId;
            set
            {
                _systemId = value;
                OnPropertyChanged();
            }
        }

        public const string SignalIdHeader = "Signal ID (new)";
        private string _signalId;
        public string SignalId
        {
            get => _signalId;
            set
            {
                _signalId = value;
                OnPropertyChanged();
            }
        }

        public const string SignalTypeHeader = "Type";
        private string _typeString;
        public string TypeString
        {
            get => _typeString;
            set
            {
                _typeString = value;
                switch(value.ToLower().Replace(" ", string.Empty))
                {
                    case "bool":
                        Type = SignalTypes.Bool;
                        break;
                    case "int":
                        Type = SignalTypes.Int;
                        break;
                    case "float":
                        Type = SignalTypes.Float;
                        break;
                    case "alarm":
                        Type = SignalTypes.Alarm;
                        break;
                    case "critical_alarm":
                        Type = SignalTypes.CritcalAlarm;
                        break;
                    default:
                        Type = SignalTypes.Unknownn;
                        break;
                }
                OnPropertyChanged();
            }
        }
        private SignalTypes _type = SignalTypes.Unknownn;
        public SignalTypes Type
        {
            get => _type;
            set
            {
                _type = value;
                OnPropertyChanged();
            }
        }

        public const string PstsHeader = "ПСТС";
        private string _psts;
        public string Psts
        {
            get => _psts;
            set
            {
                _psts = value;
                OnPropertyChanged();
            }
        }

        public const string ShmemHeader = "shmem";
        private string _shmem;
        public string Shmem
        {
            get => _shmem;
            set
            {
                _shmem = value;
                OnPropertyChanged();
            }
        }

        public const string UpsHeader = "УПС";
        private string _ups;
        public string Ups
        {
            get => _ups;
            set
            {
                _ups = value;
                OnPropertyChanged();
            }
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
