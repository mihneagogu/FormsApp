using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using Xamarin.Forms;
using System.Text;
using System.Threading.Tasks;


namespace FormsAppTelenav.Classes
{
    public class Credit : ChangeNotify
    {
        private Nullable<Double> doubleCreditCost = null;
        private Nullable<Double> doubleCreditDuration;
        private double buyerMonthlyIncome;
        private List<CreditForCustomRow> payments = new List<CreditForCustomRow>();
        private Nullable<Double> doubleCreditInterest;
        private string affordableCredit = "OK";

        private string KEY_COST_CHANGED = "Cost";
        private string KEY_DURATION_CHANGED = "Duration";
        private string KEY_INTEREST_CHANGED = "Interest";
        private string KEY_AFFORDABLE_CREDIT_CHANGED = "AffordableCredit";



        public Credit()
        {

        }

        protected void RecomputePayments()
        {
            payments.Clear();
            for (int i = 1; i <= doubleCreditDuration; i++)
            {
                double cost;
                int currentMonth, monthsRemaining;
                string auxDuration = doubleCreditDuration.Value.ToString();
                double DAuxDuration = Double.Parse(auxDuration);
                cost = doubleCreditCost.Value / DAuxDuration;
                cost += ((doubleCreditInterest.Value / 100) * doubleCreditCost.Value) / DAuxDuration;
                currentMonth = i;
                string auxMonthsRemaining = (doubleCreditDuration.Value - i).ToString();
                monthsRemaining = int.Parse(auxMonthsRemaining);
                payments.Add(new CreditForCustomRow(cost, currentMonth, monthsRemaining));
            }
        }


        public List<CreditForCustomRow> Payments
        {
            set { payments = value; }
            get { return payments; }
        }

        public Nullable<Double> Cost
        {
            set
            {

                OnPropertyChanged(KEY_COST_CHANGED, ref doubleCreditCost, value);
                if (value != null && doubleCreditInterest != null && doubleCreditDuration != null)
                {
                    CheckAffordability();
                    RecomputePayments();
                }


            }

            get { return doubleCreditCost; }
        }

        public Nullable<Double> Duration
        {

            set
            {

                OnPropertyChanged(KEY_DURATION_CHANGED, ref doubleCreditDuration, value);
                if (value != null && doubleCreditInterest != null && doubleCreditCost != null)
                {
                    CheckAffordability();
                    RecomputePayments();
                }


            }
            get { return doubleCreditDuration; }
        }

        public Nullable<Double> Interest
        {
            set
            {

                OnPropertyChanged(KEY_INTEREST_CHANGED, ref doubleCreditInterest, value);
                if (value != null && doubleCreditCost != null && doubleCreditDuration != null)
                {
                    CheckAffordability();
                    RecomputePayments();
                }

            }
            get { return doubleCreditInterest; }
        }

        public double BuyerMonthlyIncome
        {
            set { buyerMonthlyIncome = value; }
            get { return buyerMonthlyIncome; }

        }

        private bool IsAffordable()
        {

            double totalInterest = (doubleCreditInterest.Value / 100) * doubleCreditCost.Value;
            string auxDuration = doubleCreditDuration.Value.ToString();
            double DAuxDuration = Double.Parse(auxDuration);
            bool aff = true;
            if (((doubleCreditCost + totalInterest) / DAuxDuration) > ((buyerMonthlyIncome * 15) / 100))
            {
                aff = false;
                return aff;
            }
            return aff;


        }
        private void CheckAffordability()
        {
            if (IsAffordable())
            {
                AffordableCredit = "You can afford the credit";
            }
            else
            {
                double auxCost;
                string auxDuration = doubleCreditDuration.Value.ToString();
                double DAuxDuration = Double.Parse(auxDuration);
                auxCost = doubleCreditCost.Value / DAuxDuration;
                if (doubleCreditInterest != null)
                {
                    auxCost += ((doubleCreditInterest.Value / 100) * doubleCreditCost.Value) / DAuxDuration;
                    double percentage = (auxCost / BuyerMonthlyIncome) * 100;
                    double auxPercentage = Math.Round(percentage, 3);
                    double idealNrOfMonths = DAuxDuration * percentage / 15;
                    double auxIdealNrOfMonths = Math.Ceiling(idealNrOfMonths);
                    string builder = "You cannot afford it because the credit costs " + auxPercentage + "% of your monthly wage. ";
                    builder += "You could afford it if the duration were " + auxIdealNrOfMonths + " months";

                    AffordableCredit = builder;
                }
                else
                {
                    AffordableCredit = "Please fill in all the fields";
                }

            }

        }

        public string AffordableCredit
        {
            set
            {

                OnPropertyChanged(KEY_AFFORDABLE_CREDIT_CHANGED, ref affordableCredit, value);

            }

            get
            {
                return affordableCredit;
            }
        }


    }
}
