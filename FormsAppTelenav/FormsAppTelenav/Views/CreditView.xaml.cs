using FormsAppTelenav.Classes;
using FormsAppTelenav.Databases;
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
            if (credit.Interest != null && credit.Duration != null && credit.Cost != null){
                if (credit.IsAffordable()){
                    
                    var creditListView = new CreditListView();
                    creditListView.BindingContext = credit;
                    StationaryCredit stationaryCredit = new StationaryCredit();
                    stationaryCredit.Interest = (double)credit.Interest;
                    stationaryCredit.Duration = (double)credit.Duration;
                    stationaryCredit.Cost = (double)credit.Cost;
                    stationaryCredit.DateBought = DateTime.Now;
                    DealerResponse response = await App.LocalDataBase.AddCredit(stationaryCredit);
                    List<StationaryCredit> credits = await App.LocalDataBase.GetCredits();
                    if (response == DealerResponse.Success){
                        await DisplayAlert("", "Operation Successful", "OK");
                        await Navigation.PushAsync(creditListView);
                    }
                    else {
                        await DisplayAlert("", "Something went wrong, we're sorry", "OK");
                    }


                }
                else {
                    await DisplayAlert("", "You cannot afford the credit", "OK");
                }
            }
            else {
                await DisplayAlert("", "Please fill in all the fields", "OK");
            }



            /*await App.LocalDataBase.AddCredit(c1);
          
            List<StationaryCredit> stationaryCredits = await App.LocalDataBase.GetCredits(); */
        }

              
        
    }
}