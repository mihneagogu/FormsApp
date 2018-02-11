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
        private string moneyStatement = "";
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
            CalculateMoneyToEarn();


            

            //AuctionHouseCommand = new Command(() => Navigation.PushAsync(new AuctionHouseView()));
            BindingContext = this;

        }

        

        private void CalculateMoneyToEarn()
        {
            DateTime constantTime = new DateTime(2018, 2, 10, 9, 1, 1, DateTimeKind.Local);
            DateTime timeNow = DateTime.Now.ToLocalTime();
            TimeSpan span = timeNow.Subtract(constantTime);
            double minutes = span.TotalMinutes;
            double amount = 0;
            amount += 1 * Math.Floor(minutes / 10);
            double currentAmount = person.Amount + amount;
            MoneyStatement = "You have currently " + currentAmount + "$";
            
            
            
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
            
            PersonToAuctionBundleConnection conn = new PersonToAuctionBundleConnection();
            List<PersonToAuctionBundleConnection> conns = await App.LocalDataBase.GetPersonToAuctionBundleConncetions();
            Person p = ppl[ppl.Count - 1];
            Currency curr = await App.LocalDataBase.GetCurrency(p.CurrencyID);
            int q = 0;
            await DisplayAlert("ok", ppl.Count.ToString(), "ok");
            
        }

        private void ToHistory_Clicked(object sender, EventArgs e)
        {
            HistoryView historyView = new HistoryView();
            Navigation.PushAsync(historyView);
        }

        public string MoneyStatement
        {
            set; get;
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