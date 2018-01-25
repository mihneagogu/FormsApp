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
        Databases.DataBase db = new Databases.DataBase();
        private ToBuyAuction auctionToBuy;
        public BuyAuctionsView(ToBuyAuction auctionToBuy)
        {
            InitializeComponent();
            db.createDatabase(DependencyService.Get<ILocalFileHelper>().GetLocalFilePath("Person.db3"));
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
            int x = 0;
        }

        private async void AddBundleToStockPortfolio(AuctionBundle auctionBundle)
        {
            List<Person> ppl = await db.GetPeople();
            ppl[ppl.Count - 1].StockPortfolio += auctionBundle.Symbol + "|" + auctionBundle.Name + "|" + auctionBundle.OpenValueAtDateBought + "|" 
                 + "|" + auctionBundle.CloseValueAtDateBought + "|" + auctionBundle.DateBought + "|" + auctionBundle.Number + "\n";
            int awaiter = await db.SavePerson(ppl[ppl.Count - 1] as Person);
            ppl = await db.GetPeople();
            int q = 0;


        }
    }
}