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
        
        private MainViewBindingModel binding = new MainViewBindingModel();
        Person person = new Person("Mikeymike");
        public Person Person {
            set; get;
        }

        protected override async void OnAppearing()
        {
            await MeddleWithDB(person);
        }

        public MainView()
        {
            InitializeComponent();
            person.CurrencyID = 2;
            person.Amount = 5000;
            
            //AuctionHouseCommand = new Command(() => Navigation.PushAsync(new AuctionHouseView()));
            BindingContext = binding;

        }

        

        private double CalculateMoneyToEarn(DateTime constantTime, Person person)
        {
            DateTime timeNow = DateTime.Now.ToLocalTime();
            TimeSpan span = timeNow.Subtract(constantTime);
            double minutes = span.TotalMinutes;
            double amount = 0;
            amount += 10 * Math.Floor(minutes);
            double currentAmount = person.Amount + amount;
            string builder = "You have " + currentAmount + " currency";
            binding.MoneyStatement = builder;
            return currentAmount;
            
            
            
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

        private async Task<int> MeddleWithDB(Person person)
        {

            List<Person> ppl = await App.LocalDataBase.GetPeople();
            if (ppl.Count == 0)
            {
                int awaiter = await App.LocalDataBase.AddPerson(person);
            }
            ppl = await App.LocalDataBase.GetPeople();
            person = ppl[ppl.Count - 1] as Person;
            binding.MoneyStatement = "You have " + person.Amount + " currency";
            List<AppSettings> settings = await App.LocalDataBase.GetAppSettings();
            if (settings.Count == 0)
            {
                AppSettings currentSetting = new AppSettings();
                currentSetting.CurrentPerson = person.Id;
                DateTime currentTime = DateTime.Now.ToLocalTime();
                currentSetting.LastLogin = currentTime.ToString();
                int awaiter = await App.LocalDataBase.AddAppSetting(currentSetting);
            }
            else
            {
                AppSettings setting = settings[settings.Count - 1] as AppSettings;
                DateTime currentTime = DateTime.Now.ToLocalTime();
                double currentAmount = CalculateMoneyToEarn(Convert.ToDateTime(setting.LastLogin), person);
                if (currentAmount != person.Amount)
                {
                    setting.LastLogin = currentTime.ToString();
                    int awaiter = await App.LocalDataBase.SaveAppSetting(setting);
                    person.Amount = currentAmount;
                    
                    awaiter = await App.LocalDataBase.SavePerson(person);
                    string builder = "You have  " + currentAmount + " currency";
                }

            }
           
            ppl = await App.LocalDataBase.GetPeople();
            person = ppl[ppl.Count - 1] as Person;
            binding.MoneyStatement = "You have " + person.Amount + " currency";
            settings = await App.LocalDataBase.GetAppSettings();
            List<AuctionBundle> aunctionBundles = await App.LocalDataBase.GetAuctionBundles();
            
            PersonToAuctionBundleConnection conn = new PersonToAuctionBundleConnection();
            List<PersonToAuctionBundleConnection> conns = await App.LocalDataBase.GetPersonToAuctionBundleConncetions();
            Person p = ppl[ppl.Count - 1];
            Currency curr = await App.LocalDataBase.GetCurrency(p.CurrencyID);
            int q = 0;
            return 0;
                    
            
        }

        private void ToHistory_Clicked(object sender, EventArgs e)
        {
            HistoryView historyView = new HistoryView();
            Navigation.PushAsync(historyView);
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