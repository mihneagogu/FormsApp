using System;
using System.Collections.Generic;
using FormsAppTelenav.Classes;
using FormsAppTelenav.Models;
using Xamarin.Forms;

namespace FormsAppTelenav.Views
{
    public partial class AuctionHouseView : ContentPage
    {

        protected override Boolean OnBackButtonPressed(){
            bool b = false;
            if (!b)
            {
                this.DisplayAlert("", "Pressed phone back button", "OK");
                Navigation.PushAsync(new MainView());
                return true;
            }
            return base.OnBackButtonPressed();

        }

        void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            AuctionView auctionView = new AuctionView((e.SelectedItem as PyAuction).Symbol, (e.SelectedItem as PyAuction).Name);
            Navigation.PushAsync(auctionView);
        }

        public StockSymbols sSymbols = new StockSymbols();
        public AuctionHouseView()
        {
            InitializeComponent();
            //NavigationPage.SetHasBackButton(this, false);
            BindingContext = sSymbols;
        }
    }
}
