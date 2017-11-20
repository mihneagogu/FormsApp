using FormsAppTelenav.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FormsAppTelenav.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainView : ContentPage
    {
        public MainView()
        {
            InitializeComponent();
        }

        private static Credit credit;

        private void ShowCreditButton_Clicked(object sender, EventArgs e)
        {
            string cost = CostEntry.Text;
            string duration = DurationEntry.Text;
            string interest = InterestEntry.Text;

            int i;
            double d;
            bool result1 = double.TryParse(CostEntry.Text, out d);
            bool result2 = int.TryParse(DurationEntry.Text, out i);
            bool result3 = double.TryParse(InterestEntry.Text, out d);
            if (result1 && result2 && result3)
            {
                credit = new Credit(cost, duration, interest);
                Navigation.PushAsync(new CreditView());
            }
            else
            {
                DisplayAlert("Wrong credintials", "", "OK");
            }
           
        }

        public static Credit GetCredit()
        {
            return credit;
        }

        public static List<CreditForCustomRow> GetCreditForCustomRow()
        {
            List<CreditForCustomRow> credits = new List<CreditForCustomRow>();
            for (int i = 1; i <= credit.GetCreditDuration(); i++)
            {
                double cost;
                int currentMonth, monthsRemaining;
                string auxDuration = credit.GetCreditDuration().ToString();
                double DAuxDuration = Double.Parse(auxDuration);
                cost = credit.GetCreditCost() / DAuxDuration;
                cost += ((credit.GetCreditInterest() / 100) * credit.GetCreditCost()) / DAuxDuration;
                currentMonth = i;
                monthsRemaining = credit.GetCreditDuration() - i;
                credits.Add(new CreditForCustomRow(cost, currentMonth, monthsRemaining));
            }
            return credits;
        }
    }
}