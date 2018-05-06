using System;
using SQLite;

namespace FormsAppTelenav.Models
{
    public class StationaryCredit
    {

        /// Class fot the credit that you buy when you press "Buy Credit", is a stationary version of the original Credit.cs class that does not involve bindings
        public StationaryCredit()
        {
            
            
        }
        [PrimaryKey, AutoIncrement]
        public int Id { set; get; }
        public double Cost { set; get; }
        public double Interest { set; get; }
        public double Duration { set; get; }
        public DateTime DateBought { set; get; }
        public DateTime AppDateBought { get; set; }
        public DateTime LatestPayment { set; get; }
        public double MonthsRemaining { set; get; }
    }
}
