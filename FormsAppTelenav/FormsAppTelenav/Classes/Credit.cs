using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using Xamarin.Forms;
using System.Text;
using System.Threading.Tasks;

namespace FormsAppTelenav.Classes
{
    public class Credit : INotifyPropertyChanged
    {
        private Nullable<Double> doubleCreditCost = null;
        private int intCreditDuration;
        private double buyerMonthlyIncome;
        private List<CreditForCustomRow> payments = new List<CreditForCustomRow>();
        private Nullable<Double> doubleCreditInterest; 
        private string affordableCredit = "OK";
        public event PropertyChangedEventHandler PropertyChanged;
        
        public Credit() {
            
        }

        protected void RecomputePayments()
        {
            payments.Clear();
            for (int i = 1; i <= intCreditDuration; i++)
            {
                double cost;
                int currentMonth, monthsRemaining;
                string auxDuration = intCreditDuration.ToString();
                double DAuxDuration = Double.Parse(auxDuration);
                cost = doubleCreditCost.Value / DAuxDuration;
                cost += ((doubleCreditInterest.Value / 100) * doubleCreditCost.Value) / DAuxDuration;
                currentMonth = i;
                monthsRemaining = Int32.Parse((intCreditDuration - i).ToString());
                payments.Add(new CreditForCustomRow(cost, currentMonth, monthsRemaining));
            }
        }


        public List<CreditForCustomRow> Payments{
            set { payments = value;  }
            get { return payments; }
        }

        public Nullable<Double> Cost
        {
            set {
                if (doubleCreditCost != value)
                {
                    doubleCreditCost = value;
                    if (PropertyChanged != null)
                    {

                        PropertyChanged(this, new PropertyChangedEventArgs("Cost"));
                    }
                    if (value != null)
                    {
                        CheckAffordability();
                        RecomputePayments();
                    }
                }
                 
            }
            get { return doubleCreditCost;  }
        }

        public int Duration {

            set {
                if (intCreditDuration != value) {
                    
                    intCreditDuration = value;
                    if (PropertyChanged != null){
                        PropertyChanged(this, new PropertyChangedEventArgs("Duration"));
                    }
                    
                   
                        CheckAffordability();
                        RecomputePayments();
                   
                }
            }
            get { return intCreditDuration;  }
        } 

        public Nullable<Double> Interest {
            set {
                if (doubleCreditInterest != value) {
                  
                    doubleCreditInterest = value;
                    if (PropertyChanged != null){
                        PropertyChanged(this, new PropertyChangedEventArgs("Interest"));
                    }
                    if (value != null)
                    {
                        CheckAffordability();
                        RecomputePayments();
                    }
                }
            }
            get { return doubleCreditInterest;  }
        }

        public double BuyerMonthlyIncome
        {
            set { buyerMonthlyIncome = value; }
            get { return buyerMonthlyIncome; }
            
        }

        private bool IsAffordable()
        {
            string auxDuration = intCreditDuration.ToString();
            double DAuxDuration = Double.Parse(auxDuration);
            if (doubleCreditCost/DAuxDuration > (buyerMonthlyIncome*15)/100)
            {
                return false;
            } 
            else {
                return true;
            }
        }
        private void CheckAffordability()
        {
            if (IsAffordable())
            {
                AffordableCredit = "You can afford the credit";
            }
            else
            {
                AffordableCredit = "The credit is too expensive for you";
            }
            
        }
        public string AffordableCredit
        {
            set {
                if (affordableCredit != value)
                {
                    affordableCredit = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("AffordableCredit"));
                    }
                }
            }
            get {
                return affordableCredit;
           }
        }
    }
}
