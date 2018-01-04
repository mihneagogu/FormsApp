using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormsAppTelenav.Classes
{
    public class Auction
    {
        private string date;
        private double openValue, highValue, lowValue, closeValue, volume, exDividend, splitRatio, adjOpen, adjHigh, adjLow, adjClose, adjVolume;

        public Auction(string date, double openValue, double highValue, double lowValue, double closeValue, double volume,
            double exDividend, double splitRatio, double adjOpen, double adjHigh, double adjLow, double adjClose, double adjVolume)
        {
            this.date = date;
            this.openValue = openValue;
            this.highValue = highValue;
            this.lowValue = lowValue;
            this.closeValue = closeValue;
            this.volume = volume;
            this.exDividend = exDividend;
            this.splitRatio = splitRatio;
            this.adjOpen = adjOpen;
            this.adjHigh = adjHigh;
            this.adjLow = adjLow;
            this.adjClose = adjClose;
            this.adjVolume = adjVolume;
        }

        public string Date
        {
            set { date = value; }
            get { return date; }
        }

        public double OpenValue
        {
            set { openValue = value; }
            get { return openValue; }
        }
        
        public double CloseValue
        {
            set { closeValue = value; }
            get { return closeValue; }
        }

        
    }
}
