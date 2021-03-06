﻿using System;
using System.Globalization;

namespace FormsAppTelenav.Classes
{
    public class DoubleStringConverter : Xamarin.Forms.IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null){
                return "";
            }
            if (value is double) {
                return value.ToString();
            }
            return value;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.Equals("")){
                return null;
            }
            double number;
            bool canConvert = Double.TryParse(value.ToString(), out number);
            if (canConvert)
            {
                if (value is string)
                {
                    return Double.Parse(value.ToString());
                }
            }
            return value;
        }
    }
}
