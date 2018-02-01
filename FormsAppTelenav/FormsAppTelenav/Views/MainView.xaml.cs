using FormsAppTelenav.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Windows.Input;
using SQLite;
using FormsAppTelenav.Databases;
using FormsAppTelenav.Models;

namespace FormsAppTelenav.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainView : ContentPage
    {

        Person person = new Person("Mikeymike");
        public Person Person {
            set; get;
        }



        public MainView()
        {
            InitializeComponent();
            person.CurrencyID = 2;
            person.Amount = 5000;
            MeddleWithDB(person);


            

            //AuctionHouseCommand = new Command(() => Navigation.PushAsync(new AuctionHouseView()));
            BindingContext = this;

        }

        



        private void ToBank_Clicked(object sender, EventArgs e)
        {
            BankView bankView = new BankView();
           
            
            Navigation.PushAsync(bankView);
        }

        private void ToAuctionHouse_Clicked(object sender, EventArgs e)
        {
            AuctionHouseView auctionHouseView = new AuctionHouseView();

            
            Navigation.PushAsync(auctionHouseView);
        }

        private async void MeddleWithDB(Person person)
        {

            List<Person> ppl = await App.LocalDataBase.GetPeople();
            if (ppl.Count == 0)
            {
                int awaiter = await App.LocalDataBase.AddPerson(person);
            }
            
            List<AuctionBundle> aunctionBundles = await App.LocalDataBase.GetAuctionBundles();
            Person p = ppl[ppl.Count - 1];
            Currency curr = await App.LocalDataBase.GetCurrency(p.CurrencyID);
            await DisplayAlert("ok", ppl.Count.ToString(), "ok");
            
        }


        /* public ICommand AuctionHouseCommand
        {

            get
            {
                return new Command(() => Navigation.PushAsync(new AuctionHouseView()));
            }
        } */
    }
}