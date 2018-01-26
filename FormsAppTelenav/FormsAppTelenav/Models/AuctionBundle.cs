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
        private string number;
        public AuctionBundle(string symbol, string name, double openValueAtDateBought, double closeValueAtDateBought, string dateBought, string number)
        {
            Symbol = symbol;
            Name = name;
            OpenValueAtDateBought = openValueAtDateBought;
            CloseValueAtDateBought = closeValueAtDateBought;
            DateBought = dateBought;
            Number = number;
        }

        public int PersonID { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public double OpenValueAtDateBought { get; set; }
        public double CloseValueAtDateBought { get; set; }
        public string DateBought { get; set; }
        public string Number { get; set; }


    }
}
