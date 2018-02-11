using FormsAppTelenav.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;
using FormsAppTelenav.Models;

namespace FormsAppTelenav.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AuctionView : ContentPage
    {
        private ObservableCollection<Auction> stock = new ObservableCollection<Auction>();
        private AuctionsFromAPI auctions = new AuctionsFromAPI();
        private string auctionName;
        private string KEY_BUY = "buy";
        private string KEY_SELL = "sell";
        private string symbol;
        public AuctionView(string symbol, string auctionName)
        {
            InitializeComponent();
            this.symbol = symbol;
            this.auctionName = auctionName;
            MakeAuctions();
            BindingContext = this;
            
        }

        public ObservableCollection<Auction> Stock { 
            set { stock = value; }
            get { return stock; }
        }

        private void ToBuyAuctions_Clicked(object sender, EventArgs e)
        {
            BuyAuctionsView buyAuctionsView = new BuyAuctionsView(new ToBuyAuction(symbol ,auctionName, stock[0].CloseValue, stock[0].Date), KEY_BUY);
            Navigation.PushAsync(buyAuctionsView);
        }

        private async void MakeAuctions()
        {
            bool gotResponse = await auctions.GetAuction(symbol, stock);
            if (!gotResponse)
            {
                await DisplayAlert("", "We can't find the stock market, sorry!", "OK");
            }
            else
            {
                SellButton.IsEnabled = true;
                BuyButton.IsEnabled = true;
            }
        }

        private void ToSellAuctions_Clicked(object sender, EventArgs e)
        {
            BuyAuctionsView buyAuctionsView = new BuyAuctionsView(new ToBuyAuction(symbol, auctionName, stock[0].CloseValue, stock[0].Date), KEY_SELL);
            Navigation.PushAsync(buyAuctionsView);
        }
    }


}