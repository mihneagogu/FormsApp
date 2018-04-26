using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormsAppTelenav.Classes
{

    public enum IncomeCategory
    {
        DefaultDeposit,
        DefaultInterest,
        Random
    }

    public class Income
    {
        private string name;
        private double absoluteValue;
        private bool periodical;
        private string category;
        private double frequency;

        
        // daca vine periodic: frequency = la cate luni vine
        // daca Times == -1 atunci este pe perioada nedefinita
        public Income()
        {

        }

        public Income(string name, double absoluteValue, bool periodical, IncomeCategory category, double frequency)
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
        public int Times { get; set; }
        public string LastSupposedPayment { get; set; }
        public string LastRealSupposedPayment { get; set; }
        public int TimesLeft { get; set; }
        public IncomeCategory Category { get; set; }
        public string ContractTime { get; set; }
        // frequency = la fiecare cate minute sa se faca plata
        public double Frequency { get; set; }
        public string LastRealPayment { get; set; }
        public string LastAppPayment { get; set; }
        public double OverTimeAddition { get; set; }
        public double DepositInterest { get; set; }

        


    }

}
