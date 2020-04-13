using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Maps.MapControl.WPF;
using System.IO;
using System.Net.Sockets;

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

        private string errorStirng;
        private bool connected;


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

                if(value.Longitude > 90)
                {
                    value.Longitude = 90;
                    location = value;
                    ErrorString = "It's the end of the world";
                    NotifyPropertyChanged("Location");

                }
                else if (value.Longitude < -90)
                {
                    value.Longitude = -90;
                    location = value;
                    ErrorString = "It's the end of the world";
                    NotifyPropertyChanged("Location");
                }
                else if (value.Latitude > 180)
                {
                    value.Latitude = 180;
                    location = value;
                    ErrorString = "It's the end of the world";
                    NotifyPropertyChanged("Location");
                }
                else if (value.Latitude < -180)
                {
                    value.Latitude = -180;
                    location = value;
                    ErrorString = "It's the end of the world";
                    NotifyPropertyChanged("Location");
                }
                else
                {

                    location = value;
                    NotifyPropertyChanged("Location");
                }


                location = value;
                NotifyPropertyChanged("Location");
            }
        }
        public string ErrorString
        {
            get { return errorStirng; }
            set
            {
                errorStirng = value;
                NotifyPropertyChanged("ErrorString");
            }
        }

        public bool Connected
        {
            get { return connected; }
            set
            {
                connected = value;
                NotifyPropertyChanged("Connected");
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

            Heading = 0;
            VerticalSpeed = 0;
            GroundSpeed = 0;
            AirSpeed = 0;
            GpsAltitude = 0;
            Roll = 0;
            Pitch = 0;
            AltimeterAltitude = 0;
            double longtitude = 32.0055;
            double latitude = 34.0000;
            Location = new Location(latitude, longtitude);


            try
            {
                telnetClient.connect(ip, port);
                Connected = true;
                start();
            }
            
            catch (SocketException er)
            {
                Console.WriteLine("SocketException: {0}", er);

                ErrorString = "Exception: no server is running";
            }
            

        }

        public void disconnect()
        {
            try
            {
                telnetClient.disconnect();
                
                Connected = false;
                stop = true;
            }
            
             catch (NullReferenceException er)
            {
                Console.WriteLine("ArgumentNullException: {0}", er);

                ErrorString = "Exception: you tried to disconnect while you haven't connected server yet";
                
            }
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
                    }catch(TimeoutException e)
                    {

                        ErrorString = "You are having a timeout exception";

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
            try
            {
                Console.WriteLine(to_send);
                mutex.WaitOne();
                telnetClient.write(to_send);
                telnetClient.read();
                mutex.ReleaseMutex();
            }
            catch (TimeoutException e)
            {

                ErrorString = "You are having a timeout exception";

            }
            catch (IOException e)
            {
                string error = e.ToString();
                
                if (error.Contains("NetWork.Strem"))
                {
                    ErrorString = "You cannot move steer before connecting to server";
                }
                
            }
            catch(NullReferenceException e)
            {
                ErrorString = "Beware! you aren't connected to the server yet";
            }

            


            //catch(ObjectDisposedException e)
            //{
            //    ErrorString = "Exception: object";
            //}


        }

        public void setClient(ITelnetClient client)
        {
            telnetClient = client;
            stop = false;
        }
    }
}
