using flightGear.models;
using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flightGear.viewModels
{
    class MapVM : INotifyPropertyChanged
    {
        IFlightGearModel model;
        

        public event PropertyChangedEventHandler PropertyChanged;

        public MapVM(IFlightGearModel model)
        {
            this.model = model;
        }


        public Location VM_Location
        {
            get { return model.Location; }
        }

        private void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }


    }
   
}
