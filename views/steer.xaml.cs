using flightGear.viewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace flightGear.views
{
    /// <summary>
    /// Interaction logic for steer.xaml
    /// </summary>
    public partial class steer : UserControl
    {
        private double rudder;
        private double elevator;
        public double Rudder { get { return rudder; } set { rudder = value;  } }
        public double Elevator { get { return elevator; } set { elevator = value; } }

        private SteerVM steerVM;
        
        public steer()
        {
            InitializeComponent();

            this.Loaded += (s, e) =>
            {
                steerVM = (SteerVM)DataContext;


            };

        }

        private void Throttle_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            (sender as Slider).Value = Math.Round(e.NewValue, 2);
        }

        private void Aileron_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            (sender as Slider).Value = Math.Round(e.NewValue, 2);
        }
    }
}
