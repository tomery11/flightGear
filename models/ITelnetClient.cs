using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flightGear.models
{
    interface ITelnetClient : INotifyPropertyChanged
    {

        void connect(string ip, int port);
        void write(string command);
        string read();
        void disconnect();
    }
}
