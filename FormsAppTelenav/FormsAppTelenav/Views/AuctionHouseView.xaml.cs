using System;
using System.Collections.Generic;
using FormsAppTelenav.Classes;
using Xamarin.Forms;

namespace FormsAppTelenav.Views
{
    public partial class AuctionHouseView : ContentPage
    {
        void ToAppleStock_Clicked(object sender, System.EventArgs e)
        {
            AuctionView auctionView = new AuctionView("AAPL");
            Navigation.PushAsync(auctionView);
           
        }

        void ToMicrosoftStock_Clicked(object sender, System.EventArgs e)
        {
            AuctionView auctionView = new AuctionView("MSFT");
            Navigation.PushAsync(auctionView);
       



        }

        public AuctionHouseView()
        {
            InitializeComponent();
        }
    }
}
