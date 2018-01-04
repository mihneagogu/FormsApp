using FormsAppTelenav.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Windows.Input;

namespace FormsAppTelenav.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainView : ContentPage
    {
        private Person person = new Person("User1", (App.Current as FormsAppTelenav.App).xApp);

        public Person Person {
            set; get;
        }

        public MainView()
        {
            InitializeComponent();
            Person = person;
            AuctionHouseCommand = new Command(() => Navigation.PushAsync(new AuctionHouseView()));
            BindingContext = this;

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
            AuctionHouseView auctionHouseView = new AuctionHouseView();
            Navigation.PushAsync(auctionHouseView);
        }

        public ICommand AuctionHouseCommand{
            set; get;
        }
    }
}