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

        public Income(string name, double absoluteValue, bool periodical, string category, double frequency)
        {
            this.name = name;
            this.absoluteValue = absoluteValue;
            this.periodical = periodical;
            this.category = category;
            this.frequency = frequency;
        }

    }

}
