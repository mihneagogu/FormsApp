using FormsAppTelenav.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace FormsAppTelenav.Classes
{
    public class Person
    {
       // private Money currentMoney;
        /*private static Money currentMoney = new Money(2000, Money.Currency.EUR);
        private List<Income> incomes;
        private List<Expense> expenses = new List<Expense>();
        private List<AuctionBundle> stockPortfolio; */

        public Person(string name)
        {
            Name = name;
          
        }

        public Person(){
            
        }
        
        [PrimaryKey, AutoIncrement]
        public int Id
        {
            get;
            set;
        }
        public string Name
        {
            get; set;
        }

        public string StockIDs
        {
            get; set;
        }

        public double Amount {
            get; set;
        }

        public int CurrencyID { get; set; }

        /*public Money CurrentMoney
        {
            set { currentMoney = value; }
            get { return currentMoney; }
        }
         
        public string MoneyStatement
        {
            get { return "You currently have " + currentMoney.Value + " " + currentMoney.Symbol; }   
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
            get { return stockPortfolio; }
        } */

       
    }
}
