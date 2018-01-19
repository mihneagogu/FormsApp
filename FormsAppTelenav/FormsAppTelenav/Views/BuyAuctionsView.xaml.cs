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
            //AuctionBundle auctionBundle = new AuctionBundle()
            // se adauga la stockportfolio al persoanei la lista de AuctionBundle
        }
    }
}