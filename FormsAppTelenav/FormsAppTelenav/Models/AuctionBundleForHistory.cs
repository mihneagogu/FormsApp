using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormsAppTelenav.Models
{
    public class AuctionBundleForHistory
    {
        public AuctionBundleForHistory(string symbol, string name, double openValueAtDateBought, double closeValueAtDateBought, string dateBought, string number, string type)
        {
            Symbol = symbol;
            Name = name;
            OpenValueAtDateBought = openValueAtDateBought;
            CloseValueAtDateBought = closeValueAtDateBought;
            DateBought = dateBought;
            Number = number;
            Type = type;

        }

        public AuctionBundleForHistory()
        {

        }

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }


        public int PersonID { get; set; }

        public string Symbol { get; set; }
        public string Name { get; set; }
        public double OpenValueAtDateBought { get; set; }
        public double CloseValueAtDateBought { get; set; }
        public string DateBought { get; set; }
        public string Number { get; set; }
        public string Type { get; set; }
    }
}
