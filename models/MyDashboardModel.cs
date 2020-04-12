using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Maps.MapControl.WPF;
using System.IO;

namespace flightGear.models
{
    class MyDashboardModel : IFlightGearModel
    {

        ITelnetClient telnetClient;
        private static Mutex mutex = new Mutex();
        volatile Boolean stop;
        private double heading;
        private double verticalSpeed;
        private double groundSpeed;
        private double airSpeed;
        private double gpsAltitude;
        private double roll;
        private double pitch;
        private double altimeterAltitude;

        private Location location;

        private string errorString;

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
                NotifyPropertyChanged("Pitch");
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


        public Location Location {
            get { return location; }
            set
            {
                location = value;
                NotifyPropertyChanged("Location");
            }
        }
        public string ErrorString
        {
            get { return errorString; }
            set
            {
                errorString = value;
                NotifyPropertyChanged("ErrorString");
            }
        }

        public double Rudder { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double Elevator { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double Aileron { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double Throttle { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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

        public ITelnetClient GetTelnetClient()
        {
            return this.telnetClient;
        }


        public void connect(string ip, int port)
        {
            telnetClient.connect(ip, port);
            start();

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
                    try
                    {
                        // this part will update the board with all the clocks
                        //heading update
                        mutex.WaitOne();
                        telnetClient.write("get /instrumentation/heading-indicator/indicated-heading-deg\n");
                        Heading = Math.Round(Double.Parse(telnetClient.read()), 2);
                        mutex.ReleaseMutex();



                        //VerticalSpeed update
                        mutex.WaitOne();
                        telnetClient.write("get /instrumentation/gps/indicated-vertical-speed\n");
                        VerticalSpeed = Math.Round(Double.Parse(telnetClient.read()), 2);
                        mutex.ReleaseMutex();

                        //GroundSpeed update
                        mutex.WaitOne();
                        telnetClient.write("get /instrumentation/gps/indicated-ground-speed-kt\n");
                        GroundSpeed = Math.Round(Double.Parse(telnetClient.read()), 2);
                        mutex.ReleaseMutex();

                        //AirSpeed update
                        mutex.WaitOne();
                        telnetClient.write("get /instrumentation/airspeed-indicator/indicated-speed-kt\n");
                        AirSpeed = Math.Round(Double.Parse(telnetClient.read()), 2);
                        mutex.ReleaseMutex();

                        //GpsAltitude update
                        mutex.WaitOne();
                        telnetClient.write("get /instrumentation/gps/indicated-altitude-ft\n");
                        GpsAltitude = Math.Round(Double.Parse(telnetClient.read()), 2);
                        mutex.ReleaseMutex();

                        //Roll update
                        mutex.WaitOne();
                        telnetClient.write("get /instrumentation/attitude-indicator/internal-roll-deg\n");
                        Roll = Math.Round(Double.Parse(telnetClient.read()), 2);
                        mutex.ReleaseMutex();

                        //Pitch update
                        mutex.WaitOne();
                        telnetClient.write("get /instrumentation/attitude-indicator/internal-pitch-deg\n");
                        Pitch = Math.Round(Double.Parse(telnetClient.read()), 2);
                        mutex.ReleaseMutex();

                        //AltimeterAltitude update
                        mutex.WaitOne();
                        telnetClient.write("get /instrumentation/altimeter/indicated-altitude-ft\n");
                        AltimeterAltitude = Math.Round(Double.Parse(telnetClient.read()), 2);
                        mutex.ReleaseMutex();

                        // get longtitude
                        mutex.WaitOne();
                        telnetClient.write("get /position/longitude-deg\n");
                        double longtitude = double.Parse(this.telnetClient.read());
                        mutex.ReleaseMutex();

                        // get latitude
                        mutex.WaitOne();
                        telnetClient.write("get /position/latitude-deg\n");
                        string ans = this.telnetClient.read();
                        double latitude = double.Parse(ans);
                        Location = new Location(latitude, longtitude);
                        mutex.ReleaseMutex();


                        Thread.Sleep(250);
                    }
                    catch(IOException e)
                    {
                        ErrorString = "Exception: you are already connected to the server";
                        //throw new IOException();
                        
                    }
                    catch(TimeoutException e)
                    {

                        ErrorString = "Exception: timeout";
                        //throw new TimeoutException();
                    }

                }
            }).Start();
        }

        public void get_location()
        {
            // get longtitude
            mutex.WaitOne();
            telnetClient.write("get /position/longitude-deg\n");
            double longtitude = double.Parse(this.telnetClient.read());
            mutex.ReleaseMutex();
            
            // get latitude
            mutex.WaitOne();
            telnetClient.write("get /position/latitude-deg\n");
            double latitude = double.Parse(this.telnetClient.read());
            mutex.ReleaseMutex();
            
            Location = new Location(latitude, longtitude);
        }

        public void set_steer(string name, double value)
        {
            
            string to_send = null;
            if (name.Equals("throttle"))
            {
                to_send = "set /controls/engines/current-engine/throttle " + value + "\n";
            }
            else if (name.Equals("aileron"))
            {
                to_send = "set /controls/flight/aileron " + value + "\n";
            }
            else if (name.Equals("elevator"))
            {
                to_send = "set /controls/flight/elevator " + value + "\n";
            }
            else if (name.Equals("rudder"))
            {
                to_send = "set /controls/flight/rudder " + value + "\n";
            }
            Console.WriteLine(to_send);
            mutex.WaitOne();
            telnetClient.write(to_send);
            telnetClient.read();
            mutex.ReleaseMutex();
            

        }



    }
}
