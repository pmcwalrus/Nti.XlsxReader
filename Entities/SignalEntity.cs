using Nti.XlsxReader.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Nti.XlsxReader.Entities
{
    public class SignalEntity : INotifyPropertyChanged
    {        
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

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

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

        private bool _is420mA;
        public bool Is420mA
        {
            get => _is420mA;
            set
            {
                _is420mA = value;
                OnPropertyChanged();
            }
        }

        private string _vk;
        public string Vk
        {
            get => _vk;
            set
            {
                _vk = value;
                OnPropertyChanged();
            }
        }

        private string _updateThershold;
        public string UpdateTreshold
        {
            get => _updateThershold;
            set
            {
                _updateThershold = value;
                OnPropertyChanged();
            }
        }

        private string _script;
        public string Script
        {
            get => _script;
            set
            {
                _script = value;
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
