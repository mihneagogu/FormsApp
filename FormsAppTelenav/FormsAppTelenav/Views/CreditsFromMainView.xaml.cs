using FormsAppTelenav.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FormsAppTelenav.Classes;
using System.Collections.ObjectModel;

namespace FormsAppTelenav.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreditsFromMainView : ContentPage
    {

        protected override async void OnAppearing()
        {
            NavigationPage.SetHasNavigationBar(this, false);
        }
       
        private ObservableCollection<Income> bindingList = new ObservableCollection<Income>();
        List<StationaryCredit> credits = new List<StationaryCredit>();
        public List<StationaryCredit> Credits { set { credits = value; } get { return credits; } }
        public CreditsFromMainView(List<StationaryCredit> credits)
        {
            InitializeComponent();
            Credits = credits;
            DoBindings();
            BindingContext = this;
        }

        private async void DoBindings()
        {
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


    }
}