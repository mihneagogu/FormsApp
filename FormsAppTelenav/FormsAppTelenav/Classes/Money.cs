using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormsAppTelenav.Classes
{
    public class Money
    {
        private double absoluteValue;
        private Currency currency;

        public enum Currency { EUR, USD , RON };
        private double[] currencyValues =  {1, 1.21336, 4.63716};
        private string[] currencySymbols = { "EUR", "$", "RON" };

        public Money(double absoluteValue, Currency currency)
        {
            this.absoluteValue = absoluteValue;
            this.currency = currency;
            TransformCurrency(Currency.RON);
        }

        public Money() {
            
        }

        private void TransformCurrency(Currency desiredCurrency)
        {
            absoluteValue *= currencyValues[(int)desiredCurrency]/currencyValues[(int)currency];
           
            currency = desiredCurrency;
        }

        public double Value
        {
            get { return absoluteValue; }
        }

        public string Symbol{
            get { return currencySymbols[(int)currency]; }
        }



       


    }
}
