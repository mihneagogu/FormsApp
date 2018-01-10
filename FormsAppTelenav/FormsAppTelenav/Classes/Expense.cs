using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormsAppTelenav.Classes
{
    public class Expense
    {
        private string name;
        private double absoluteCost;
        private string category;
        private string description;

        

        public Expense(string name, double absoluteCost, string category, string description)
        {
            this.name = name;
            this.absoluteCost = absoluteCost;
            this.category = category;
            this.description = description;
            
        }
    }
}
