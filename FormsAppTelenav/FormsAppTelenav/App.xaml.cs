using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using FormsAppTelenav.Databases;
using FormsAppTelenav.Models;
using Xamarin.Forms;

namespace FormsAppTelenav
{
    public partial class App : Application
    {
        public static Dealer MiddleDealer { get; set; }

        public static DataBase LocalDataBase {
            get; set;
        }


        public static List<Currency> Currencies { get { return currencies; } set { currencies = value; } }
        private static List<Currency> currencies = new List<Currency>();
        public App()
        {
            
            InitializeComponent();
            LocalDataBase = new DataBase(DependencyService.Get<ILocalFileHelper>().GetLocalFilePath("Person.db3"));
            MiddleDealer = new Dealer();


            MainPage = new NavigationPage(new FormsAppTelenav.Views.MainView());

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
