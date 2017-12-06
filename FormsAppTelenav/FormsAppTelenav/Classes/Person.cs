using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormsAppTelenav.Classes
{
    class Person
    {
        private string name;
        private double currentMoney;
        private double monthlyIncome;

        public Person(string name, double currentMoney)
        {
            this.name = name;
            this.currentMoney = currentMoney;
        }
        

        public string Name
        {
            set { name = value; }
            get { return name; }
        }

        public double CurrentMoney
        {
            set { currentMoney = value; }
            get { return currentMoney; }
        }

        public string MoneyStatement
        {
            get { return "You currently have " + currentMoney + " $"; }   
        } 
        
        public string ExpensesToPay
        {
            get { return ""; }
        }

        public double MonthlyIncome
        {
            set { monthlyIncome = value; }
            get { return monthlyIncome;  }
        }

        public string MonthlyIncomeStatement
        {
            get { return "Your monthly income is " + MonthlyIncome + " $"; }
        }
    }
}
