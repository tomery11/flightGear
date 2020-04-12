using flightGear.models;
using flightGear.viewModels;
using System.Windows;
using System.Windows.Media;
using System;
using flightGear.views;
using System.Configuration;
using System.Net.Sockets;

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
            ipTextBox.Text = ConfigurationManager.AppSettings["ip"];
            portTextBox.Text = ConfigurationManager.AppSettings["port"];



        }

        private void connectButton_Click(object sender, RoutedEventArgs e)
        {
            string ip = ipTextBox.Text;
            int port = Int32.Parse(portTextBox.Text);
            if ((ip != "localhost" && ip != "127.0.0.1") || port != 5402)
            {
                errorWindow.Content = "Invalid Port or IP ! please re-insert correct credentials";
                errorArea.Background = Brushes.Red;
            }
            else
            {
                this.model = new MyDashboardModel(new MyTelnetClient());
                vm = new FlightGearViewModel(this.model);
                SteerVM steerVM = new SteerVM(this.model);
                MapVM mapVM = new MapVM(this.model);

                //mapUC.DataContext = mapVM;
                steerUC.DataContext = steerVM;
                DataContext = vm;
                try
                {
                    vm.connect(ip, port);
                    connectionStatus.Content = "Connected";
                    connectionStatus.Foreground = Brushes.Green;
                    elipseConnectionStatus.Fill = Brushes.Green;
                    errorWindow.Content = "";
                    errorArea.Background = Brushes.Transparent;
                    connected = true;
                }
                catch (ArgumentNullException er)
                {
                    Console.WriteLine("ArgumentNullException: {0}", er);
                    
                    errorWindow.Content = "Exception: " + er;
                    errorArea.Background = Brushes.Red;
                }
                catch (SocketException er)
                {
                    Console.WriteLine("SocketException: {0}", er);
                    
                    errorWindow.Content = "Exception: no server is running";
                    errorArea.Background = Brushes.Red;
                }

            }
            

            
        }

        private void disconnectButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                vm.disconnect();
                connectionStatus.Content = "Disconnected";
                connectionStatus.Foreground = Brushes.Red;
                elipseConnectionStatus.Fill = Brushes.Red;
                connected = false;
            }
            catch (NullReferenceException er)
            {
                Console.WriteLine("ArgumentNullException: {0}", er);

                errorWindow.Content = "Exception: you tried to disconnect while you haven't connected server yet";
                errorArea.Background = Brushes.Red;
            }

        }

    }
}
