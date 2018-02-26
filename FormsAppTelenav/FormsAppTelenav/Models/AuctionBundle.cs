using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormsAppTelenav.Models
{
    public class AuctionBundle
    {
     
        public AuctionBundle Copy(){
            return new AuctionBundle(Symbol, Name, OpenValueAtDateBought, CloseValueAtDateBought, DateBought, Number);
        }
        public AuctionBundle(string symbol, string name, double openValueAtDateBought, double closeValueAtDateBought, string dateBought, string number)
        {
            Symbol = symbol;
            Name = name;
            OpenValueAtDateBought = openValueAtDateBought;
            CloseValueAtDateBought = closeValueAtDateBought;
            DateBought = dateBought;
            Number = number;
        }

        public AuctionBundle()
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


    }
}
