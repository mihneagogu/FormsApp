using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormsAppTelenav.Models
{
    public class AuctionBundle
    {
        private string symbol;
        private string name;
        private double openValueAtDateBought;
        private double closeValueAtDateBought;
        private string dateBought;
        private double number;
        public AuctionBundle(string symbol, string name, double openValueAtDateBought, double closeValueAtDateBought, string dateBought, double number)
        {
            this.symbol = symbol;
            this.name = name;
            this.openValueAtDateBought = openValueAtDateBought;
            this.closeValueAtDateBought = closeValueAtDateBought;
            this.dateBought = dateBought;
            this.number = number;
        }
    }
}
