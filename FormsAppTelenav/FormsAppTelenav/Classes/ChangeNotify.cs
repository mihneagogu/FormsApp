using System;
using System.ComponentModel;

namespace FormsAppTelenav.Classes
{
    public class ChangeNotify : System.ComponentModel.INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ChangeNotify()
        {

        }


        public bool OnPropertyChanged(string key, ref double orgValue, double newValue)
        {

            if ((orgValue == newValue) && (newValue!= 0))
            {
                return false;
            }
            orgValue = newValue;
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(key));
            }
            return true;
        }

        public bool OnPropertyChanged(string key, ref Nullable<Double> orgValue, Nullable<Double> newValue)
        {
            
            if (orgValue == newValue){
                return false;
            }
            orgValue = newValue;
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(key));
            }
            return true;
        }

        public bool OnPropertyChanged(string key, ref string orgValue, string newValue)
        {

            if (orgValue.Equals(newValue))
            {
                return false;
            }
            orgValue = newValue;
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(key));
            }
            return true;
        }

        

    }
}
