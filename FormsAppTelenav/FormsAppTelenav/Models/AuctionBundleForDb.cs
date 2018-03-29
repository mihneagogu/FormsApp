using System;
using SQLite;

namespace FormsAppTelenav.Models
{
    public class AuctionBundleForDb
    {
        public AuctionBundleForDb()
        {
        }

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public double Number { get; set; }
        public double MedianValue { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public int PersonID { get; set; }


    }
}
