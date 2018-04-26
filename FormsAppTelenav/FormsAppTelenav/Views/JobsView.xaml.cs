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
    public partial class JobsView : ContentPage
    {
        public JobsView()
        {
            InitializeComponent();
            MeddleWithIncomes();
        }
        // pentru jobs doar trebuie facut un view si cu o lista, logica pentru orice income exista

        public async void MeddleWithIncomes()
        {
            List<Income> incomes = await App.LocalDataBase.GetIncomes();

        }

        private async void CreateIncome_Clicked(object sender, EventArgs e)
        {
            AppSettings setting = (await App.LocalDataBase.GetAppSettings())[0];
            Income income = new Income();
            income.Name = NameEntry.Text;
            income.Frequency = double.Parse(FrequencyEntry.Text);
            income.AbsoluteValue = double.Parse(AbsoluteValueEntry.Text);
            income.Category = IncomeCategory.Random;
            income.Periodical = true;
            income.LastRealPayment = setting.LastRealLogin;
            income.LastAppPayment = setting.LastLogin;
            income.ContractTime = income.LastRealPayment.ToString();
            income.Times = 5;
            income.LastRealSupposedPayment = setting.LastRealLogin;
            income.LastSupposedPayment = setting.LastLogin;
            income.TimesLeft = income.Times;
            income.LastRealSupposedPayment = setting.LastRealLogin;
            income.LastSupposedPayment = setting.LastLogin;
            await App.LocalDataBase.AddIncome(income);
           
            List<Income> incomes = await App.LocalDataBase.GetIncomes();
            int x = 0;

        }
    }
}