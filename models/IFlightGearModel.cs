using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Microsoft.Maps.MapControl.WPF;
using System.Threading;

namespace flightGear.models
{
    interface IFlightGearModel: INotifyPropertyChanged
    {

        //connection to flight gear
        void connect(string ip, int port);
        void disconnect();
        void start();


        //properties of flight gear
        double Heading { set; get; }
        double VerticalSpeed { set; get; }
        double GroundSpeed { set; get; }
        double AirSpeed { set; get; }
        double GpsAltitude { set; get; }
        double Roll { set; get; }
        double Pitch { set; get; }
        double AltimeterAltitude { set; get; }

        double Rudder { set; get; }
        double Elevator { set; get; }
        double Aileron { set; get; }
        double Throttle { set; get; }
        Location Location { get; set; }
        
        


    }
}
