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
        private List<AuctionBundleForHistory> history;
        private MainViewBindingModel binding = new MainViewBindingModel();


        protected override async void OnAppearing()
        {
            await MeddleWithDB();
        }

        public MainView()
        {
            InitializeComponent();
            
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

        private static bool isFirst = true;
        private async Task<int> MeddleWithDB()
        {
            if (!isFirst)
                return 0;

            isFirst = false;

            Person person = App.User;
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
                    binding.MoneyStatement = "You have " + person.Amount.ToString() + " currency";
                    MoneyEntry.IsVisible = true;
                }


            }
          

            settings = await App.LocalDataBase.GetAppSettings();
            List<AuctionBundle> aunctionBundles = await App.LocalDataBase.GetAuctionBundles();
            
            PersonToAuctionBundleConnection conn = new PersonToAuctionBundleConnection();
            List<PersonToAuctionBundleConnection> conns = await App.LocalDataBase.GetPersonToAuctionBundleConncetions();
            List<AuctionBundleForHistory> bundles = await App.LocalDataBase.GetHistory();
           
            Currency curr = await App.LocalDataBase.GetCurrency(person.CurrencyID);
            if (bundles.Count != 0)
            {
                HistoryButton.IsEnabled = true;
                history = await App.LocalDataBase.GetHistory();
            }
            return 0;
                    
            
        }

        private async void ToHistory_Clicked(object sender, EventArgs e)
        {
            
            if (history.Count != 0) {
                HistoryView historyView = new HistoryView(history);
                await Navigation.PushAsync(historyView);
            }
            else
            {
                await DisplayAlert("", "You have yet to buy auctions from a company", "OK");
            }

            
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