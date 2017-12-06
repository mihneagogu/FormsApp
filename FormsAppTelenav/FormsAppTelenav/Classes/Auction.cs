using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormsAppTelenav.Classes
{
    public class Auction
    {
        private double auctionValue;
        public Auction(double auctionValue)
        {
            this.auctionValue = auctionValue;
        }

        public double AuctionValue
        {
            set { auctionValue = value; }
            get { return auctionValue; }
        }
    }
}
