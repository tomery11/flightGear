using flightGear.models;
using flightGear.viewModels;
using System.Windows;

namespace flightGear
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        FlightGearViewModel vm;
        public MainWindow()
        {

            InitializeComponent();
            vm = new FlightGearViewModel(new MyDashboardModel(new MyTelnetClient()));
            

            DataContext = vm;
            
        }
    }
}
