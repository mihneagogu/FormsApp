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
        private double creditCost;
        private int creditDuration;
        private List<CreditForCustomRow> payments = new List<CreditForCustomRow>();
        private double creditInterest;
        public event PropertyChangedEventHandler PropertyChanged;
        public Credit() {
            
        }

        protected void RecomputePayments()
        {
            payments.Clear();
            for (int i = 1; i <= creditDuration; i++)
            {
                double cost;
                int currentMonth, monthsRemaining;
                string auxDuration = creditDuration.ToString();
                double DAuxDuration = Double.Parse(auxDuration);
                cost = creditCost / DAuxDuration;
                cost += ((creditInterest / 100) * creditCost) / DAuxDuration;
                currentMonth = i;
                monthsRemaining = creditDuration - i;
                payments.Add(new CreditForCustomRow(cost, currentMonth, monthsRemaining));
            }
        }


        public List<CreditForCustomRow> Payments{
            set { payments = value;  }
            get { return payments; }
        }

        public double Cost
        {
            set {
                if (creditCost != value)
                {
                    creditCost = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Cost"));
                    }

                    RecomputePayments();
                }
                 
            }
            get { return creditCost; }
        }

        public int Duration {

            set {
                if (creditDuration != value) {
                    creditDuration = value;
                    if (PropertyChanged != null){
                        PropertyChanged(this, new PropertyChangedEventArgs("Duration"));
                    }
                    RecomputePayments();
                }
            }
            get { return creditDuration;  }
        } 

        public double Interest {
            set {
                if (creditInterest != value) {
                    creditInterest = value;
                    if (PropertyChanged != null){
                        PropertyChanged(this, new PropertyChangedEventArgs("Interest"));
                    }
                    RecomputePayments();
                }
            }
            get { return creditInterest;  }
        }


    }
}
