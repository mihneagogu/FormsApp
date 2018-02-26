using System;
using System.Globalization;
using FormsAppTelenav.Models;

namespace FormsAppTelenav.Classes
{
    public class AuctionActionStringConverter : Xamarin.Forms.IValueConverter
    {
        public AuctionActionStringConverter()
        {
            
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "";
            }

            if (value is AuctionAction)
            {
                AuctionAction v = (AuctionAction)value;
                return v == AuctionAction.BOUGHT ? "BOUGHT":"SOLD";
            }
            return value;        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
