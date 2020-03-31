using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

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


    }
}
