using flightGear.models;
using flightGear.viewModels;
using System.Windows;
using System.Windows.Media;

namespace flightGear
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        FlightGearViewModel vm;
        private bool connected;
        public MainWindow()
        {

            InitializeComponent();
            vm = new FlightGearViewModel(new MyDashboardModel(new MyTelnetClient()));

            DataContext = vm;
            
            
            
        }

        private void connectButton_Click(object sender, RoutedEventArgs e)
        {
            vm.connect();
            connectionStatus.Content = "Connected";
            connectionStatus.Foreground = Brushes.Green;
            elipseConnectionStatus.Fill = Brushes.Green;
            connected = true;
        }

        private void disconnectButton_Click(object sender, RoutedEventArgs e)
        {
            vm.disconnect();
            connectionStatus.Content = "Disconnected";
            connectionStatus.Foreground = Brushes.Red;
            elipseConnectionStatus.Fill = Brushes.Red;
            connected = false;
        }

        private void blinkElipse()
        {

        }
    }
}
