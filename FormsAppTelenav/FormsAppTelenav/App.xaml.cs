using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Xamarin.Forms;

namespace FormsAppTelenav
{
    public partial class App : Application
    {
        public int xApp = 3;
        public App()
        {
            

            InitializeComponent();

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
