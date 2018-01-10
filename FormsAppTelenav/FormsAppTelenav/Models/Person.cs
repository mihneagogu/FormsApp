using FormsAppTelenav.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormsAppTelenav.Classes
{
    public class Person
    {
        private string name;
       // private Money currentMoney;
        private static Money currentMoney = new Money(2000, "USD");
        private List<Income> incomes;
        private List<Expense> expenses = new List<Expense>();
        private List<AuctionBundle> stockPortfolio;
        public Person(string name)
        {
            this.name = name;
          
        }
        

        public string Name
        {
            set { name = value; }
            get { return name; }
        }

        public Money CurrentMoney
        {
            set { currentMoney = value; }
            get { return currentMoney; }
        }

        public string MoneyStatement
        {
            get { return "You currently have " + currentMoney.AmountInCurrency + " " + currentMoney.CurrencySymbol; }   
        } 
        
        public List<Expense> ExpensesToPay
        {
            get { return expenses; }
        }

        public List<Income> MonthlyIncome
        {
            set { incomes = value; }
            get { return incomes;  }
        }

        public List<AuctionBundle> StockPortfolio
        {
            set { this.stockPortfolio = value; }
            get { return StockPortfolio; }
        }

       
    }
}
