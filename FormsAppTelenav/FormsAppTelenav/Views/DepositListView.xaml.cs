using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormsAppTelenav.Classes;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FormsAppTelenav.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DepositListView : ContentPage
    {
        // absvalue, interest, overtimeaddition, freq
        private ObservableCollection<Income> bindingList = new ObservableCollection<Income>();
        public DepositListView()
        {
            InitializeComponent();
            DoBindings();
            BindingContext = this;
        }

        private async void DoBindings() {
            List<Income> incomes = await App.LocalDataBase.GetIncomes();
            if (incomes.Count != 0)
            {
                foreach (Income i in incomes)
                {
                    if (i.Times == -1)
                    {
                        bindingList.Add(i);
                    }
                }
                depositList.ItemsSource = bindingList;
            }
        }

        private async void GetAllDepositedMoney_Clicked(object sender, EventArgs e)
        {
            await App.LocalDataBase.GetAllDepositedMoney();
        }

        // cand apasa un buton sa se ia banii din cont > MD > persoana primeste banii depusi initial + dobanda
    }
}