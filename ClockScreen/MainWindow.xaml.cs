using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace ClockScreen
{
    public partial class MainWindow : Window
    {
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr window, int index, int value);

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr window, int index);

        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_TOOLWINDOW = 0x00000080;

        public static void HideFromAltTab(IntPtr Handle)
        {
            SetWindowLong(Handle, GWL_EXSTYLE, GetWindowLong(Handle, GWL_EXSTYLE) | WS_EX_TOOLWINDOW);
        }

        private SettingsWindow _settings;
        private BaseViewModel _vm;

        public MainWindow()
        {
            InitializeComponent();
            _vm = DataContext as BaseViewModel;
            Closing += (s, e) =>
            {
                _vm.WriteSettings();
            };
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            HideFromAltTab(new WindowInteropHelper(this).Handle);
            _vm.ReadSettings();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
            else if (e.MiddleButton == MouseButtonState.Pressed)
                Close();
        }

        private void FontAwesome_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!App.Current.Windows.OfType<SettingsWindow>().Any())
                new SettingsWindow() { Owner = this }.Show();
        }

        private void Border_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released)
            {
                _vm.WriteSettings();
            }
        }
    }
}
