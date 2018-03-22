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
    public partial class BuyAuctionsView : ContentPage
    {
        private ToBuyAuction auctionToBuy;
        private AuctionAction action;
        public BuyAuctionsView(ToBuyAuction auctionToBuy, AuctionAction action)
        {
            InitializeComponent();
            this.auctionToBuy = auctionToBuy;
            this.action = action;
            BuyOrSellLabel.Text = "How many auctions do you want to " + action + "?";
            BindingContext = this;
        }


        public string CompanyNameStatement
        {
            set { }
            get { return "You chose to buy auctions from " + auctionToBuy.Name; }
        }

        public string PriceValueStatement
        {
            set { }
            get { return "The current price of an auction is " + auctionToBuy.CloseValueAtDateBought + " $"; }
        }

        private void ConfirmedPayment_Clicked(object sender, EventArgs e)
        {
            AuctionBundle auctionBundle = new AuctionBundle(auctionToBuy.Symbol, auctionToBuy.Name, auctionToBuy.CloseValueAtDateBought, auctionToBuy.CloseValueAtDateBought, auctionToBuy.Date, NumberEntry.Text);
            AddBundleToStockPortfolio(auctionBundle);

        }

        private async void AddBundleToStockPortfolio(AuctionBundle auctionBundle)
        {
            string numberToBuy = auctionBundle.Number;
            if (action == AuctionAction.BOUGHT)
            {
                Person person = App.User;
                string auctionNumber = auctionBundle.Number;
                double auxAuctionNumber = double.Parse(auctionNumber);
                if (person.Amount < (auctionBundle.CloseValueAtDateBought * auxAuctionNumber))
                {
                    await DisplayAlert("", "You do not have enough money", "OK");
                }
                else
                {
                    auctionBundle.PersonID = person.Id;
                    int response = await App.LocalDataBase.AddAuctionBundle(auctionBundle);
                    if (response == 0){
                        await DisplayAlert("", "Congratulations, you have just bought " + auctionNumber + "auctions", "OK");
                    }


                }
            }
            else
            {
                int response = await App.LocalDataBase.SellAuctionBundle(auctionBundle);
                switch(response){
                    case 0:
                        {
                            await DisplayAlert("", "Congratuations, you have just sold " + auctionBundle.Number, "OK");
                            break;
                        }
                    case 1:
                        {
                            await DisplayAlert("", "You have no auctions", "OK");
                            break; 
                        }
                    case 2:
                        {
                            await DisplayAlert("", "You have not bought auctions froms this company yet or have sold them all", "OK");
                            break;
                        }
                    case 3:
                        {
                            await DisplayAlert("", "You do not have enough auctions", "OK");
                            break;
                        }
                }
                // idee pentru alerte: sa returneze MW o un int care sa zica ce eroare apare si in functie de ce eroare este sa apara alerta?!

               
            }
        }
    }

}
