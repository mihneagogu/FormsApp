﻿using FormsAppTelenav.Classes;
using FormsAppTelenav.Models;
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
            
        }

        public MainView()
        {
            InitializeComponent();
            Person person = App.User;

            //AuctionHouseCommand = new Command(() => Navigation.PushAsync(new AuctionHouseView()));
            BindingContext = binding;
            binding.MoneyStatement = person.Amount;

            MeddleWithDB();


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

        private async Task<double> CalculateMoneyToGive(Person person, ContentPage page){
            /// va trebui comparat timpul actual cu timpul ultimei plati: de adaugat lastpay la credit in baza de date, si pe baza diferentei acelea se vor face calculele
            /// de asemenea va exista un nou camp pentru stationarycredit care va zice cate luni sunt ramase din plata
            List<StationaryCredit> stationaryCredits = await App.LocalDataBase.GetCredits();
            if (stationaryCredits.Count != 0)
            {
                StationaryCredit credit = stationaryCredits[0];
                DateTime timeNow = DateTime.Now.ToLocalTime();
                TimeSpan span = timeNow.Subtract(credit.LatestPayment);
                double monthsSinceLastPayment = span.TotalMinutes;
                monthsSinceLastPayment = monthsSinceLastPayment / 1;
                double mRemaining = (double)credit.MonthsRemaining;
                monthsSinceLastPayment = Math.Floor(monthsSinceLastPayment);
                await page.DisplayAlert("", monthsSinceLastPayment + " months have passed since you last paid your credit. You now have to pay for " +
                                        (double)(credit.MonthsRemaining) + " - " + " " + monthsSinceLastPayment + " = " + ((double)credit.MonthsRemaining - monthsSinceLastPayment) + "  more months", "OK");
                if ((mRemaining - monthsSinceLastPayment) >= 0)
                {
                    double currentMoney = person.Amount;
                    currentMoney -= ((double)credit.Cost / (double)credit.Duration) * (monthsSinceLastPayment);
                    await DisplayAlert("", "You will have " + currentMoney + " money, before paying it you had " + person.Amount, "OK");
                    person.Amount = currentMoney;
                    credit.MonthsRemaining -= monthsSinceLastPayment;
                    credit.LatestPayment = DateTime.Now;
                    await App.LocalDataBase.SavePerson(person);
                    await App.LocalDataBase.SaveCredit(credit);

                }
                else
                {
                    await DisplayAlert("", "durata de cand nu ai mai platit e mai lunga decat durata creditului, de schimbat cod", "OK");
                }


                return 0;
            }
            return 0;
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


        private async void MeddleWithDB()
        {

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
                    binding.MoneyStatement = person.Amount;

                }
                binding.MoneyStatement = person.Amount;
                await CalculateMoneyToGive(person, this);
        






            }

            binding.MoneyStatement = person.Amount;
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