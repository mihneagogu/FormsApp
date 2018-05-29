using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using FormsAppTelenav.Databases;
using FormsAppTelenav.Models;
using Xamarin.Forms;
using FormsAppTelenav.Classes;
using System.Threading.Tasks;

namespace FormsAppTelenav
{
    public partial class App : Application
    {
        public static FormsAppTelenav.Databases.Dealer MiddleDealer { get; set; }

        public static DataBase LocalDataBase
        {
            get; set;
        }


        public static uint GeneralMovementDuration { get { return 60; } }

        /// Divider = cate minute din realitate inseamna 1 luna a aplicatiei
        public static System.Globalization.CultureInfo DoubleCultureInfo { get { return System.Globalization.CultureInfo.InvariantCulture; } }
        public const double Divider = 2;
        // Multiplier = 1 minut din realitate este egal cu 100 de minute ale aplicatiei
        public const double Multiplier = 100;

       
        public static void MoveButtons(List<VisualElement> elements){
            
            int span = (int)(GeneralMovementDuration / elements.Count);
            for (int i = 0; i < elements.Count; i++){
                var x = RotateCharacter(elements[i], System.Threading.CancellationToken.None, 0 + (i*span));
            }
        }

        private static async Task RotateCharacter(VisualElement element, System.Threading.CancellationToken token, int sleepTime)
        {
            await Task.Delay(sleepTime);
            while (!token.IsCancellationRequested)
            {
                await element.RotateTo(5, GeneralMovementDuration, Easing.Linear);
                await element.RotateTo(0, 0);
                await element.RotateTo(-5, GeneralMovementDuration, Easing.Linear);
                await element.RotateTo(0, 0);

            }
        }


        // Daca se schimba person, sa se trimite la middleware mesaj ca sa schime si User din App.xaml.cs ?
        public static Person User 
        { 
            get 
            {
                if (currentUser == null)
                    currentUser = GetPerson();

                return currentUser;
            } 
        }

        private static Person currentUser = null;
        

        public static List<Currency> Currencies { get { return currencies; } set { currencies = value; } }
        private static List<Currency> currencies = new List<Currency>();

        public App()
        {
            
            InitializeComponent();
            LocalDataBase = new DataBase(DependencyService.Get<ILocalFileHelper>().GetLocalFilePath("Person.db3"));
            MiddleDealer = new Dealer();
            MiddleDealer.RegisterMessage(MessageAction.AddedAuctionBundle, LocalDataBase);
            MiddleDealer.RegisterMessage(MessageAction.SellAuctionBundle, LocalDataBase);
            MiddleDealer.RegisterMessage(MessageAction.BuyCredit, LocalDataBase);
            MiddleDealer.RegisterMessage(MessageAction.ManageIncomes, LocalDataBase);
            MiddleDealer.RegisterMessage(MessageAction.GetAllDepositedMoney, LocalDataBase);

            MainPage = new NavigationPage(new FormsAppTelenav.Views.MainView());
        }

        public static Person GetPerson()
        {
            List<Person> ppl = new List<Person>();
            LocalDataBase.GetPeople(ref ppl);
            if (ppl.Count == 0)
            {
                Person person = new Person("Mike");
                person.Amount = 5000;
                person.CurrencyID = 2;

                LocalDataBase.AddPerson(person);
                return person;
            }
            else
                return ppl[ppl.Count - 1] as Person;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
