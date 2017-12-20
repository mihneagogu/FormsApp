using FormsAppTelenav.Classes;
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
    public partial class MainView : ContentPage
    {
        private Person person = new Person("User1", 2520.5);
        AuctionsFromAPI auctionHouse = new AuctionsFromAPI();

        public MainView()
        {
            InitializeComponent();
            BindingContext = new Person("User1", 2520.5);
            
            auctionHouse.GetAuction();
        }

        private void ToBank_Clicked(object sender, EventArgs e)
        {
            BankView bankView = new BankView();
            person.MonthlyIncome = 1000;
            bankView.BindingContext = person;
            Navigation.PushAsync(bankView);
        }

        private void ToAuctions_Clicked(object sender, EventArgs e)
        {
            AuctionView auctionView = new AuctionView();
            auctionView.BindingContext = auctionHouse;
            Navigation.PushAsync(auctionView);
        }
    }
}