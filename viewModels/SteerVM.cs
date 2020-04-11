using flightGear.models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flightGear.viewModels
{
    class SteerVM : INotifyPropertyChanged
    {

        IFlightGearModel model;
        private double rudder;
        private double elevator;
        private double throttle;
        private double aileron;

        public event PropertyChangedEventHandler PropertyChanged;

        public SteerVM(IFlightGearModel model)
        {
            this.model = model;

            this.model.PropertyChanged +=
             delegate (Object sender, PropertyChangedEventArgs e) { NotifyPropertyChanged("VM_" + e.PropertyName); };
        }


        public double VM_Rudder
        {
            get { return rudder; }
            set
            {
                if (this.rudder != value)
                {
                    this.rudder = Math.Round(value, 2);
                    Console.WriteLine("rudder = " + value);
                    model.set_steer("rudder", value);
                    NotifyPropertyChanged("VM_" + "Rudder");

                }

            }
        }

        private void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        public double VM_Elevator
        {
            get { return elevator; }
            set
            {
                if (this.elevator != value)
                {
                    this.elevator = Math.Round(value, 2);
                    Console.WriteLine("elevator = " + value);
                    model.set_steer("elevator", value);
                    NotifyPropertyChanged("VM_" + "Elevator");

                }

            }
        }
        public double VM_Aileron
        {
            get { return aileron; }
            set
            {
                if (this.aileron != value)
                {
                    this.aileron = Math.Round(value, 2);
                    Console.WriteLine("aileron = " + value);
                    model.set_steer("aileron", value);

                }

            }
        }
        public double VM_Throttle
        {
            get { return throttle; }
            set
            {
                if (this.throttle != value)
                {
                    this.throttle = Math.Round(value, 2);
                    Console.WriteLine("throttle = " + value);
                    model.set_steer("throttle", value);

                }

            }
        }

    }
}
