using System;
using System.Globalization;

namespace FormsAppTelenav.Classes
{
    public class MoneyStringConverter : Xamarin.Forms.IValueConverter
    {

        public MoneyStringConverter(){
            
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null){
                return "";
            }
            if ((value is double)){
                double aux = (double) value;

                    string v = "You have " + aux.ToString() + " currency";
                    return v;
               
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
