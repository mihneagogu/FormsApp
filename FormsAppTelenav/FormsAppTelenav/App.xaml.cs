﻿using System;
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
        private static Person person;
        public static DataBase LocalDataBase {
            get; set;
        }
        // Daca se schimba person, sa se trimite la middleware mesaj ca sa schime si User din App.xaml.cs ?
        public static Person User { get { return person; } set { person = value; } }

        

        public static List<Currency> Currencies { get { return currencies; } set { currencies = value; } }
        private static List<Currency> currencies = new List<Currency>();
        public App()
        {
            
            InitializeComponent();
            LocalDataBase = new DataBase(DependencyService.Get<ILocalFileHelper>().GetLocalFilePath("Person.db3"));
            MiddleDealer = new Dealer();



            MainPage = new NavigationPage(new FormsAppTelenav.Views.MainView());

        }

        public static async void GetPerson()
        {
            List<Person> ppl = await LocalDataBase.GetPeople();
            Person person = ppl[ppl.Count - 1] as Person;
            
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
