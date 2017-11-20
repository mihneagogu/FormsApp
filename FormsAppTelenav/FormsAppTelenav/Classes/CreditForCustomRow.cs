using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormsAppTelenav.Classes
{
    public class CreditForCustomRow
    {
        private double cost;
        private int month;
        private int monthsRemaining;
        public CreditForCustomRow(double cost, int month, int monthsRemaining)
        {
            this.month = month;
            this.cost = cost;
            this.monthsRemaining = monthsRemaining;
        }

        public double GetCost()
        {
            return cost;
        }

        public double Cost
        {
            get { return cost; }
        }

        public double Month
        {
            get { return month; }
        }

        public double MonthsRemaining
        {
            get { return monthsRemaining; }
        }
        public int GetMonth()
        {
            return month;
        }

        public int GetMonthsRemaining()
        {
            return monthsRemaining;
        }
    }
}
