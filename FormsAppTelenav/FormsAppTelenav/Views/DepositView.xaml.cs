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
    public partial class DepositView : ContentPage
    {
        public DepositView()
        {
            InitializeComponent();
        }

        
        private async void MakeDefaultDeposit_Clicked(object sender, EventArgs e)
        {
            AppSettings setting = (await App.LocalDataBase.GetAppSettings())[0];
            Income income = new Income();
            income.AbsoluteValue = double.Parse(AbsoluteValueEntry.Text, App.DoubleCultureInfo);
            income.Times = -1;
            income.ContractTime = setting.LastRealLogin;
            income.Category = IncomeCategory.DefaultDeposit;
            income.LastAppPayment = setting.LastLogin;
            income.LastRealPayment = setting.LastRealLogin;
            income.Periodical = true;
            double frequency = double.Parse(FrequencyEntry.Text, App.DoubleCultureInfo);
            switch (frequency)
            {
                case 1:
                    frequency = 31;
                    break;
                case 3:
                    frequency = 92;
                    break;
                case 6:
                    frequency = 182;
                    break;
                case 12:
                    frequency = 365;
                    break;

            }
            income.OverTimeAddition = 0;
            income.Frequency = frequency;
            income.LastRealSupposedPayment = setting.LastRealLogin;
            income.LastSupposedPayment = setting.LastLogin;
            income.DepositInterest = double.Parse(InterestEntry.Text, App.DoubleCultureInfo);
            await App.LocalDataBase.AddIncome(income);
        }
    }
}