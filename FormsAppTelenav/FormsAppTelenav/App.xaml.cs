using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using FormsAppTelenav.Databases;
using Xamarin.Forms;

namespace FormsAppTelenav
{
    public partial class App : Application
    {
        private static PersonDataBase database;
        public App()
        {
            

            InitializeComponent();

            MainPage = new NavigationPage(new FormsAppTelenav.Views.MainView());

        }

        public static PersonDataBase DataBase
        {
            get
            {
                if (database == null)
                {
                    database = new PersonDataBase(DependencyService.Get<ILocalFileHelper>().GetLocalFilePath("Employee.db3"));
                }
                return database;
            }
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
