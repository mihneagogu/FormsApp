using FormsAppTelenav.Classes;
using FormsAppTelenav.Models;
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
    public partial class CreditView : ContentPage
    {
        private Credit credit = new Credit();

        public CreditView()
        {
            InitializeComponent();
            BindingContext = credit;
            credit.BuyerMonthlyIncome = 2000;
            
        }

        private async void ShowCreditButton_Clicked(object sender, EventArgs e)
        {
            var creditListView = new CreditListView();
            creditListView.BindingContext = credit;
            Models.StationaryCredit c1 = new Models.StationaryCredit();
            c1.Interest = 10;
            c1.Cost = 2000 + DateTime.Now.Second;
            await App.LocalDataBase.AddCredit(c1);
            //  ^ cod provizoriu de test
            List<StationaryCredit> stationaryCredits = await App.LocalDataBase.GetCredits();
            await Navigation.PushAsync(creditListView);
        }

              
        
    }
}