using System;
using SQLite;
namespace FormsAppTelenav.Models
{
    public class Currency
    {
        public Currency()
        {

        }
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }
        public double ExchangeRate { get; set; }

    }
}
