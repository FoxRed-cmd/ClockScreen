using System.Windows;

namespace ClockScreen
{
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();

            Loaded += (s, e) => { DataContext = Owner.DataContext; };
            Closing += (s, e) => { (DataContext as BaseViewModel).WriteSettings(); };
        }
    }
}
