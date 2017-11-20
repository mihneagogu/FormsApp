using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormsAppTelenav.Classes
{
    public class Credit
    {
        private double creditCost;
        private int creditDuration;
        private double creditInterest;

        public Credit(string x_creditCost, string x_creditDuration, string x_creditInterest)
        {
            double aux1 = Double.Parse(x_creditCost);
            int aux2 = int.Parse(x_creditDuration);
            double aux3 = Double.Parse(x_creditInterest);
            SetCreditCost(aux1);
            SetCreditDuration(aux2);
            SetCreditInterest(aux3);

        }

        public void SetCreditCost(double creditCost)
        {
            this.creditCost = creditCost;
        }

        public void SetCreditDuration(int creditDuration)
        {
            this.creditDuration = creditDuration;
        }

        public void SetCreditInterest(double creditInterest)
        {
            this.creditInterest = creditInterest;
        }

        public double GetCreditCost()
        {
            return creditCost;
        }

        public int GetCreditDuration()
        {
            return creditDuration;
        }

        public double GetCreditInterest()
        {
            return creditInterest;
        }

    }
}
