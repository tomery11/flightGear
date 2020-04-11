using flightGear.models;
using flightGear.viewModels;
using System.Windows;
using System.Windows.Media;
using System;
using flightGear.views;

namespace flightGear
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        FlightGearViewModel vm;
        private IFlightGearModel model;
        private bool connected;
        public MainWindow()
        {

            InitializeComponent();
            this.model = new MyDashboardModel(new MyTelnetClient());
            vm = new FlightGearViewModel(this.model);
            SteerVM steerVM = new SteerVM(this.model);
            MapVM mapVM = new MapVM(this.model);

            //mapUC.DataContext = mapVM;
            steerUC.DataContext = steerVM;
            DataContext = vm;
            
            
            
        }

        private void connectButton_Click(object sender, RoutedEventArgs e)
        {
            string ip = ipTextBox.Text;
            int port = Int32.Parse(portTextBox.Text);
            vm.connect(ip, port);
            connectionStatus.Content = "Connected";
            connectionStatus.Foreground = Brushes.Green;
            elipseConnectionStatus.Fill = Brushes.Green;
            connected = true;

            //views.steer.DataContext = new SteerVM(this.model);
        }

        private void disconnectButton_Click(object sender, RoutedEventArgs e)
        {
            vm.disconnect();
            connectionStatus.Content = "Disconnected";
            connectionStatus.Foreground = Brushes.Red;
            elipseConnectionStatus.Fill = Brushes.Red;
            connected = false;
        }

    }
}
