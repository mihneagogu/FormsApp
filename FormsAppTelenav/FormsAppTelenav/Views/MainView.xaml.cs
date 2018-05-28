using FormsAppTelenav.Classes;
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
//using Android.Graphics;

namespace FormsAppTelenav.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainView : ContentPage 
    {
        void JobsMonkey_Tapped(object sender, System.EventArgs e)
        {
            JobsView view = new JobsView();
            Navigation.PushAsync(view);
        }
        async void CreditsMonkey_Tapped(object sender, System.EventArgs e)
        {
            List<StationaryCredit> credits = await App.LocalDataBase.GetCredits();
            CreditsFromMainView view = new CreditsFromMainView(credits);
            await Navigation.PushAsync(view);
        }

        async void HistoryMonkey_Tapped(object sender, System.EventArgs e)
        {
            List<AuctionBundleForHistory> history = await App.LocalDataBase.GetHistory();
            if (history.Count != 0)
            {
                HistoryView historyView = new HistoryView(history);
                await Navigation.PushAsync(historyView);
            }
            else
            {
                await DisplayAlert("", "You have yet to buy auctions from a company", "OK");
            }
        }
        void BankMonkey_Tapped(object sender, System.EventArgs e)
        {
            BankView bankView = new BankView();


            Navigation.PushAsync(bankView);
        }

        void AuctionHouseMonkey_Tapped(object sender, System.EventArgs e)
        {
            AuctionHouseView auctionHouseView = new AuctionHouseView();


            Navigation.PushAsync(auctionHouseView);
        }


        private MainViewBindingModel binding = new MainViewBindingModel();

         

        public string DeleteLastLetter(string str)
        {
            return str.Substring(0, str.Length - 1);
        }

        protected override async void OnAppearing()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            var x = RotateCharacter(bankMonkey, System.Threading.CancellationToken.None, 0);

            var Y = RotateCharacter(auctionHouseMonkey, System.Threading.CancellationToken.None, 4);
            var z = RotateCharacter(historyMonkey, System.Threading.CancellationToken.None, 8);
            var a = RotateCharacter(creditsMonkey, System.Threading.CancellationToken.None, 12);
            var b = RotateCharacter(jobsMonkey, System.Threading.CancellationToken.None, 17);



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

        private async Task RotateCharacter(VisualElement element, System.Threading.CancellationToken token, int sleepTime){
            await Task.Delay(sleepTime);
            while (!token.IsCancellationRequested){
                await element.RotateTo(5, 60, Easing.Linear);
                await element.RotateTo(0, 0);
                await element.RotateTo(-5, 60, Easing.Linear);
                await element.RotateTo(0, 0);
            }
        }

        private async void MeddleWithDB()
        {
            /// de trimis request la middledealer sa verifica income-urile cand porneste aplicatia



            Person person = App.User;
            List<AppSettings> settings = await App.LocalDataBase.GetAppSettings();
            
            if (settings.Count == 0)
            {
                AppSettings currentSetting = new AppSettings();
                currentSetting.CurrentPerson = person.Id;
                DateTime currentTime = DateTime.Now.ToLocalTime();
                currentSetting.FirstLogin = currentTime.ToString();
                currentSetting.LastRealLogin = currentTime.ToString();
                currentSetting.LastLogin = currentTime.ToString();
                int awaiter = await App.LocalDataBase.AddAppSetting(currentSetting);
            }
            else
            {
                AppSettings setting = settings[settings.Count - 1] as AppSettings;
                DateTime timeLastLogin = DateTime.Parse(setting.LastRealLogin);
                DealerResponse response = await App.LocalDataBase.ChangeAppTime(timeLastLogin);
                setting = (await App.LocalDataBase.GetAppSettings())[0];
              
                List<Income> incomes = await App.LocalDataBase.GetIncomes();
                if (incomes.Count > 0)
                {
                    List<object> payload = new List<object>();
                    foreach (Income i in incomes)
                    {
                        payload.Add(i);
                    }
                    await App.LocalDataBase.ManageIncomes(payload);
                }
                List<AppSettings> s = await App.LocalDataBase.GetAppSettings();
                incomes = await App.LocalDataBase.GetIncomes();

                if (response == DealerResponse.Success){
                    await DisplayAlert("", "Successfyully updated time!" + s[0].LastLogin.ToString(), "OK");
                }

                DateTime currentTime = DateTime.Now.ToLocalTime();
                double currentAmount = CalculateMoneyToEarn(Convert.ToDateTime(setting.LastRealLogin), person);
               
                if (currentAmount != person.Amount)
                {
                    person.Amount = currentAmount;
                    
                    int awaiter = await App.LocalDataBase.SavePerson(person);
                    binding.MoneyStatement = person.Amount;

                }
                binding.MoneyStatement = person.Amount;
                //await CalculateMoneyToGive(person, this);
       
            }

            binding.MoneyStatement = person.Amount;
            settings = await App.LocalDataBase.GetAppSettings();

            PersonToAuctionBundleConnection conn = new PersonToAuctionBundleConnection();
            List<PersonToAuctionBundleConnection> conns = await App.LocalDataBase.GetPersonToAuctionBundleConncetions();
            List<AuctionBundleForHistory> bundles = await App.LocalDataBase.GetHistory();
            List<StationaryCredit> credits = await App.LocalDataBase.GetCredits();
           
            Currency curr = await App.LocalDataBase.GetCurrency(person.CurrencyID);
            if (bundles.Count != 0)
            {
                //HistoryButton.IsEnabled = true;
            }
            if (credits.Count != 0)
            {
                //CreditListButton.IsEnabled = true;
            }

           
                    
            
        }

        private async void ToHistory_Clicked(object sender, EventArgs e)
        {
            List<AuctionBundleForHistory> history = await App.LocalDataBase.GetHistory();
            if (history.Count != 0) {
                HistoryView historyView = new HistoryView(history);
                await Navigation.PushAsync(historyView);
            }
            else
            {
                await DisplayAlert("", "You have yet to buy auctions from a company", "OK");
            }

            
        }

        private async void ToCreditList_Clicked(object sender, EventArgs e)
        {
            List<StationaryCredit> credits = await App.LocalDataBase.GetCredits();
            CreditsFromMainView view = new CreditsFromMainView(credits);
            await Navigation.PushAsync(view);


        }

        private void ToJobsView_Clicked(object sender, EventArgs e)
        {
            JobsView view = new JobsView();
            Navigation.PushAsync(view);
        }

        private async void MakeIncomeChart()
        {
            List<Income> incomes = await App.LocalDataBase.GetIncomes();
            AppSettings setting = (await App.LocalDataBase.GetAppSettings())[0];
            await App.LocalDataBase.ChangeAppTime(DateTime.Parse(setting.LastRealLogin));
            setting = (await App.LocalDataBase.GetAppSettings())[0];
            foreach (Income i in incomes)
            {
                double currentAppMonth = (DateTime.Parse(setting.LastLogin)).Month;
                double futureMonth = (DateTime.Parse(i.LastSupposedPayment).AddDays(i.Frequency)).Month;
                if (futureMonth == currentAppMonth)
                {
                    // se adauga la lista de entries si se creeaza dupa chartul
                }
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