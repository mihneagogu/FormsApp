using System;
using System.Collections.Generic;
using FormsAppTelenav.Classes;
using FormsAppTelenav.Models;
using Xamarin.Forms;

namespace FormsAppTelenav.Views
{
    public partial class AuctionHouseView : ContentPage
    {
        void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            AuctionView auctionView = new AuctionView((e.SelectedItem as PyAuction).Symbol, (e.SelectedItem as PyAuction).Name);
            Navigation.PushAsync(auctionView);
        }

        public StockSymbols sSymbols = new StockSymbols();
        public AuctionHouseView()
        {
            InitializeComponent();
            BindingContext = sSymbols;
        }
    }
}
