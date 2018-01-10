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
        private string currency;
        /// absolute currency = ron
        private double USD_VALUE = 0.26;
        private double EUR_VALUE = 0.22;
        private double RON_VALUE = 1;
        private double auxAmount;
        public Money(double absoluteValue, string currency)
        {
            this.absoluteValue = absoluteValue;
            this.currency = currency;
            TransformCurrency(absoluteValue, currency);
        }

        private void TransformCurrency(double absoluteValue, string currency)
        {
            if (currency.Equals("RON")) {
                auxAmount = absoluteValue;
            }
            if (currency.Equals("USD")){
                auxAmount = absoluteValue * USD_VALUE;
            }
            if (currency.Equals("EUR"))
            {
                auxAmount = absoluteValue * EUR_VALUE;
            }
        }

        public double AmountInCurrency
        {
            set { this.auxAmount = value; }
            get { return auxAmount; }
        }

        public string CurrencySymbol
        {
            set { }
            get
            {
                switch (currency)
                {
                    case "RON":
                        return "RON";
                    case "USD":
                        return "$";
                    case "EUR":
                        return "€";
                }
                return "def";
            }
        }


    }
}
