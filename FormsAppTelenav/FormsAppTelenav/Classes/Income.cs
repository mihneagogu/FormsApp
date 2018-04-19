using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormsAppTelenav.Classes
{
    public class Income
    {
        private string name;
        private double absoluteValue;
        private bool periodical;
        private string category;
        private double frequency;
        // daca vine periodic: frequency = la cate luni vine

        public Income()
        {

        }

        public Income(string name, double absoluteValue, bool periodical, string category, double frequency)
        {
            Name = name;
            AbsoluteValue = absoluteValue;
            Periodical = periodical;
            Category = category;
            Frequency = frequency;
        }

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public double AbsoluteValue { get; set; }
        public bool Periodical { get; set; }
        public string Category { get; set; }
        public double Frequency { get; set; }


    }

}
