using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormsAppTelenav.Models
{
    public class ToBuyAuction
    {
        private string symbol;
        private string companyName;
        private double closeValueAtDateBought;
        private string date;

        public ToBuyAuction(string symbol, string companyName, double closeValueAtDateBought, string date)
        {
            this.symbol = symbol;
            this.companyName = companyName;
            this.closeValueAtDateBought = closeValueAtDateBought;
            this.date = date;
        }

        public string Symbol
        {
            set { }
            get { return symbol; }
        }
        public string Name
        {
            set { companyName = value; }
            get { return companyName; }
        }

        public double CloseValueAtDateBought
        {
            set { closeValueAtDateBought = value; }
            get { return closeValueAtDateBought; }
        }

        public string Date
        {
            set { date = value; } 
            get { return date; }
        }
    }
}
