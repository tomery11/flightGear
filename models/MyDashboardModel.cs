using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace flightGear.models
{
    class MyDashboardModel : IFlightGearModel
    {

        ITelnetClient telnetClient;
        volatile Boolean stop;
        private double heading;
        private double verticalSpeed;
        private double groundSpeed;
        private double airSpeed;
        private double gpsAltitude;
        private double roll;
        private double pitch;
        private double altimeterAltitude;

        public double Heading
        {
            get { return heading; }
            set
            {
                heading = value;
                NotifyPropertyChanged("Heading");
            }
        }
        public double VerticalSpeed {
            get { return verticalSpeed; }
            set
            {
                verticalSpeed = value;
                NotifyPropertyChanged("VerticalSpeed");
            }
        }
        public double GroundSpeed {
            get { return groundSpeed; }
            set
            {
                groundSpeed = value;
                NotifyPropertyChanged("GroundSpeed");
            }
        }
        public double AirSpeed {
            get { return airSpeed; }
            set
            {
                airSpeed = value;
                NotifyPropertyChanged("AirSpeed");
            }
        }
        public double GpsAltitude {
            get { return gpsAltitude; }
            set
            {
                gpsAltitude = value;
                NotifyPropertyChanged("GpsAltitude");
            }
        }
        public double Roll {
            get { return roll; }
            set
            {
                roll = value;
                NotifyPropertyChanged("Roll");
            }
        }
        public double Pitch {
            get { return pitch; }
            set
            {
                pitch = value;
                NotifyPropertyChanged("Pitche");
            }
        }
        public double AltimeterAltitude {
            get { return altimeterAltitude; }
            set
            {
                altimeterAltitude = value;
                NotifyPropertyChanged("AltimeterAltitude");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
        public MyDashboardModel(ITelnetClient telnetClient)
        {
            this.telnetClient = telnetClient;
            stop = false;
        }

       


        public void connect(string ip, int port)
        {
            telnetClient.connect(ip, port);
            
        }

        public void disconnect()
        {
            telnetClient.disconnect();
            stop = true;
        }

        public void start()
        {
            new Thread(delegate ()
            {
                while (!stop)
                {
                    // this part will update the board with all the clocks
                    //heading update
                    telnetClient.write("/instrumentation/heading-indicator/indicated-heading-deg");
                    Heading = Double.Parse(telnetClient.read());
                    //VerticalSpeed update
                    telnetClient.write("/instrumentation/gps/indicated-vertical-speed");
                    VerticalSpeed = Double.Parse(telnetClient.read());
                    //GroundSpeed update
                    telnetClient.write("/instrumentation/gps/indicated-ground-speed-kt");
                    GroundSpeed = Double.Parse(telnetClient.read());
                    //AirSpeed update
                    telnetClient.write("/instrumentation/airspeed-indicator/indicated-speed-kt");
                    AirSpeed = Double.Parse(telnetClient.read());
                    //GpsAltitude update
                    telnetClient.write("/instrumentation/gps/indicated-altitude-ft");
                    GpsAltitude = Double.Parse(telnetClient.read());
                    //Roll update
                    telnetClient.write("/instrumentation/attitude-indicator/internal-roll-deg");
                    Roll = Double.Parse(telnetClient.read());
                    //Pitch update
                    telnetClient.write("/instrumentation/attitude-indicator/internal-pitch-deg");
                    Pitch = Double.Parse(telnetClient.read());
                    //AltimeterAltitude update
                    telnetClient.write("/instrumentation/altimeter/indicated-altitude-ft");
                    AltimeterAltitude = Double.Parse(telnetClient.read());

                    Thread.Sleep(250);
                }
            }).Start();
        }
    }
}
