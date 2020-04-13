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
        //private bool connected;

        public MainWindow()
        {

            InitializeComponent();
            ipTextBox.Text = ConfigurationManager.AppSettings["ip"];
            portTextBox.Text = ConfigurationManager.AppSettings["port"];


            this.model = new MyDashboardModel(null);
            vm = new FlightGearViewModel(this.model);
            //SteerVM steerVM = new SteerVM(this.model);
            //MapVM mapVM = new MapVM(this.model);

            ////mapUC.DataContext = mapVM;
            //steerUC.DataContext = steerVM;
            //DataContext = vm;

            vm.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == "VM_ErrorString")
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        errorWindow.Content = vm.VM_ErrorString;
                        errorArea.Background = Brushes.Red;
                    });

                    
                    
                }
                if (e.PropertyName == "VM_Connected")
                {
                    if(vm.VM_Connected == true)
                    {
                        connectionStatus.Content = "Connected";
                        connectionStatus.Foreground = Brushes.Green;
                        elipseConnectionStatus.Fill = Brushes.Green;
                        errorWindow.Content = "";
                        errorArea.Background = Brushes.Transparent;
                        //connected = true;
                    }
                    if(vm.VM_Connected == false)
                    {
                        connectionStatus.Content = "Disconnected";
                        connectionStatus.Foreground = Brushes.Red;
                        elipseConnectionStatus.Fill = Brushes.Red;

                        //connected = false;
                    }
                }
                
            };


        }

        private void connectButton_Click(object sender, RoutedEventArgs e)
        {
            if (!vm.VM_Connected)
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
                    model.setClient(new MyTelnetClient());
                    
                    vm.connect(ip, port);
                    SteerVM steerVM = new SteerVM(this.model);
                    //MapVM mapVM = new MapVM(this.model);
                    vm = new FlightGearViewModel(this.model);
                    //mapUC.DataContext = mapVM;
                    steerUC.DataContext = steerVM;
                    DataContext = vm;




                }
               
                

            }
            else
            {
                errorWindow.Content = "Exception: you are already connected";
                errorArea.Background = Brushes.Red;
            }



        }

        private void disconnectButton_Click(object sender, RoutedEventArgs e)
        {
            vm.disconnect();
        }

    }
}
