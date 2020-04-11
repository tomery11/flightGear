using flightGear.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flightGear.viewModels
{
    class SteerVM
    {

        IFlightGearModel model;
        private double rudder;
        private double elevator;
        private double throttle;
        private double aileron;
        public SteerVM(IFlightGearModel model)
        {
            this.model = model;
        }


        public double Rudder
        {
            get { return rudder; }
            set
            {
                if (this.rudder != value)
                {
                    this.rudder = Math.Round(value, 2);
                    Console.WriteLine("rudder = " + value);
                    model.set_steer("rudder", value);

                }

            }
        }
        public double Elevator
        {
            get { return elevator; }
            set
            {
                if (this.elevator != value)
                {
                    this.elevator = Math.Round(value, 2);
                    Console.WriteLine("elevator = " + value);
                    model.set_steer("elevator", value);

                }

            }
        }
        public double Aileron
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
        public double Throttle
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
