﻿using FormsAppTelenav.Classes;
using FormsAppTelenav.Databases;
using FormsAppTelenav.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FormsAppTelenav.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreditView : ContentPage
    {
        private Credit credit = new Credit();
        protected override async void OnAppearing()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            List<VisualElement> elements = new List<VisualElement>();
            elements.Add(BuyIcon);
            App.MoveButtons(elements);
        }
        public CreditView()
        {
            InitializeComponent();
            BindingContext = credit;
            credit.BuyerMonthlyIncome = 2000;
            
        }


        private async void BuyIcon_Tapped(object sender, EventArgs e)
        {
            if (credit.Interest != null && credit.Duration != null && credit.Cost != null)
            {
                if (credit.IsAffordable())
                {
                    AppSettings setting = (await App.LocalDataBase.GetAppSettings())[0];
                    await App.LocalDataBase.ChangeAppTime(DateTime.Parse(setting.LastRealLogin));
                    setting = (await App.LocalDataBase.GetAppSettings())[0];
                    var creditListView = new CreditListView();
                    creditListView.BindingContext = credit;
                    StationaryCredit stationaryCredit = new StationaryCredit();
                    stationaryCredit.Interest = (double)credit.Interest;
                    stationaryCredit.Duration = (double)credit.Duration;
                    stationaryCredit.Cost = (double)credit.Cost;
                    stationaryCredit.DateBought = DateTime.Parse(setting.LastRealLogin);
                    stationaryCredit.MonthsRemaining = (double)credit.Duration;
                    stationaryCredit.LatestPayment = DateTime.Parse(setting.LastLogin);
                    stationaryCredit.AppDateBought = DateTime.Parse(setting.LastLogin);
                    DealerResponse response = await App.LocalDataBase.AddCredit(stationaryCredit);
                    List<StationaryCredit> credits = await App.LocalDataBase.GetCredits();
                    if (response == DealerResponse.Success)
                    {
                        //await DisplayAlert("", "Operation Successful", "OK");
                        await Navigation.PushAsync(new MainView());
                    }
                    else
                    {
                        await DisplayAlert("", "Something went wrong, we're sorry", "OK");
                    }


                }
                else
                {
                    await DisplayAlert("", "You cannot afford the credit", "OK");
                }
            }
            else
            {
                await DisplayAlert("", "Please fill in all the fields", "OK");
            }
        }
    }
}