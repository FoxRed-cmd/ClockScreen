using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace ClockScreen
{
    internal class BaseViewModel : INotifyPropertyChanged
    {
        public int HourAngle
        {
            get => _hourAngle;
            set
            {
                _hourAngle = value;
                NotifyPropertyChanged(nameof(HourAngle));
            }
        }
        public int MinuteAngle
        {
            get => _minuteAngle;
            set
            {
                _minuteAngle = value;
                NotifyPropertyChanged(nameof(MinuteAngle));
            }
        }
        public int SecondAngle
        {
            get => _secondAngle;
            set
            {
                _secondAngle = value;
                NotifyPropertyChanged(nameof(SecondAngle));
            }
        }
        public string TextTime
        {
            get => _textTime;
            set
            {
                _textTime = value;
                NotifyPropertyChanged(nameof(TextTime));
            }
        }

        public double Top
        {
            get => _top;
            set
            {
                if (_top != value)
                {
                    _top = value;
                    NotifyPropertyChanged(nameof(Top));
                }
            }
        }
        public double Left
        {
            get => _left;
            set
            {
                if (_left != value)
                {
                    _left = value;
                    NotifyPropertyChanged(nameof(Left));
                }
            }
        }
        public bool IsStartup
        {
            get => _isStatrup;
            set
            {
                if (_isStatrup != value)
                {
                    _isStatrup = value;
                    NotifyPropertyChanged(nameof(Left));
                }
            }
        }

        private int _hourAngle;
        private int _minuteAngle;
        private int _secondAngle;
        private string _textTime;

        private double _top;
        private double _left;
        private bool _isStatrup;

        private MyTimer _timer;
        private RegistryKey _wrKey;


        public BaseViewModel()
        {
            _textTime = string.Empty;
            _timer = new MyTimer() { Interval = 200 };
            _timer.Elapsed += Timer_Elapsed;
            _timer.Start();
        }

        private void Timer_Elapsed(object? sender, EventArgs e)
        {
            HourAngle = (30 * DateTime.Now.Hour - 90) + (DateTime.Now.Minute / 2);
            MinuteAngle = 6 * DateTime.Now.Minute - 90;
            SecondAngle = 6 * DateTime.Now.Second - 90;
            TextTime = DateTime.Now.ToString("HH:mm:ss");
        }

        public void WriteSettings()
        {
            using (_wrKey = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run"))
            {
                if (_wrKey != null)
                {
                    if (IsStartup && _wrKey.GetValue("ClockScreen") == null)
                        _wrKey.SetValue("ClockScreen", Assembly.GetExecutingAssembly().Location.Replace(".dll", ".exe"));
                    else if (IsStartup == false && _wrKey.GetValue("ClockScreen") != null)
                        _wrKey.DeleteValue("ClockScreen");
                }
            }

            using (_wrKey = Registry.CurrentUser.CreateSubKey("SOFTWARE\\ClockScreen"))
            {
                _wrKey.SetValue(nameof(Top), Top.ToString(), RegistryValueKind.String);
                _wrKey.SetValue(nameof(Left), Left.ToString(), RegistryValueKind.String);
            }
        }

        public void ReadSettings()
        {
            using (_wrKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run"))
            {
                if (_wrKey != null)
                    IsStartup = _wrKey.GetValue("ClockScreen") == null ? false : true;
            }

            using (_wrKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\ClockScreen"))
            {
                if (_wrKey != null)
                {
                    Top = Convert.ToDouble(_wrKey.GetValue(nameof(Top)).ToString());
                    Left = Convert.ToDouble(_wrKey.GetValue(nameof(Left)).ToString());
                }
                else
                {
                    WriteSettings();
                }
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
