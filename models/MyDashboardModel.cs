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

        public double Heading { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double VerticalSpeed { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double GroundSpeed { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double AirSpeed { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double GpsAltitude { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double Roll { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double Pitch { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double AltimeterAltitude { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public event PropertyChangedEventHandler PropertyChanged;

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
