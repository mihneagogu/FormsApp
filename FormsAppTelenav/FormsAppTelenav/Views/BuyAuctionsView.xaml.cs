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
        public BuyAuctionsView(ToBuyAuction auctionToBuy)
        {
            InitializeComponent();
            this.auctionToBuy = auctionToBuy;
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
            List<Person> ppl = await App.LocalDataBase.GetPeople();
            /* ppl[ppl.Count - 1].StockPortfolio += auctionBundle.Symbol + "|" + auctionBundle.Name + "|" + auctionBundle.OpenValueAtDateBought + "|" 
                 + "|" + auctionBundle.CloseValueAtDateBought + "|" + auctionBundle.DateBought + "|" + auctionBundle.Number + "\n";
            int awaiter = await App.LocalDataBase.SavePerson(ppl[ppl.Count - 1] as Person); */
            auctionBundle.PersonID = ppl.Count;
            int awaiter = await App.LocalDataBase.AddAuctionBundle(auctionBundle);
            List<AuctionBundle> aBundles = await App.LocalDataBase.GetAuctionBundles();
            Person person = ppl[ppl.Count - 1] as Person;
            person.StockIDs += aBundles[aBundles.Count - 1].Id.ToString() + "|";
            awaiter = await App.LocalDataBase.SavePerson(person);
            ppl = await App.LocalDataBase.GetPeople();
            int q = 0;

        }
    }
}