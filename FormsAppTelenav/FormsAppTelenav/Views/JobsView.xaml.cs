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

        public async void MeddleWithIncomes()
        {
            List<Income> incomes = await App.LocalDataBase.GetIncomes();

        }

        private async void CreateIncome_Clicked(object sender, EventArgs e)
        {
            Income income = new Income();
            income.Name = NameEntry.Text;
            income.Frequency = double.Parse(FrequencyEntry.Text);
            income.AbsoluteValue = double.Parse(AbsoluteValueEntry.Text);
            income.Category = "Sasd";
            income.Periodical = true;
            income.LastRealPayment = DateTime.Now.ToString();
            income.Times = 5;
            income.TimesLeft = income.Times;
            await App.LocalDataBase.AddIncome(income);
            List<Income> incomes = await App.LocalDataBase.GetIncomes();
            int x = 0;

        }
    }
}