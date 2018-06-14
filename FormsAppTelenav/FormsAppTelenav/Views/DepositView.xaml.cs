using FormsAppTelenav.Classes;
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
    public partial class DepositView : ContentPage
    {
        public DepositView()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            List<VisualElement> elements = new List<VisualElement>();
            elements.Add(MakeDepositIcon);
          //  elements.Add(DepositListIcon);
            App.MoveButtons(elements);
        }

        private async void MakeDefaultDeposit_Clicked(object sender, EventArgs e)
        {
            AppSettings setting = (await App.LocalDataBase.GetAppSettings())[0];
            Income income = new Income();
            income.AbsoluteValue = double.Parse(AbsoluteValueEntry.Text, App.DoubleCultureInfo);
            income.Times = -1;
            income.ContractTime = setting.LastRealLogin;
            income.Category = IncomeCategory.DefaultDeposit;
            income.LastAppPayment = setting.LastLogin;
            income.LastRealPayment = setting.LastRealLogin;
            income.Periodical = true;
            double frequency = double.Parse(FrequencyEntry.Text, App.DoubleCultureInfo);
            switch (frequency)
            {
                case 1:
                    frequency = 31;
                    break;
                case 3:
                    frequency = 92;
                    break;
                case 6:
                    frequency = 182;
                    break;
                case 12:
                    frequency = 365;
                    break;

            }
            income.OverTimeAddition = 0;
            income.Frequency = frequency;
            income.LastRealSupposedPayment = setting.LastRealLogin;
            income.LastSupposedPayment = setting.LastLogin;
            income.DepositInterest = double.Parse(InterestEntry.Text, App.DoubleCultureInfo);
            await App.LocalDataBase.AddIncome(income);
        }

        private  void ToDepositListView_Clicked(object sender, EventArgs e)
        {
            DepositListView view = new DepositListView();
            Navigation.PushAsync(view);
        }

        private async void MakeDeposit_Tapped(object sender, EventArgs e)
        {
            // de verificat daca datele introduse sunt bune
            if (!(AbsoluteValueEntry.Text == null || FrequencyEntry.Text == null || InterestEntry.Text == null))
            {
                string frequencyText = FrequencyEntry.Text;
                string interestText = InterestEntry.Text;
                string absoluteValueText = AbsoluteValueEntry.Text;
                double f;
                double a;
                double i;
                bool freqIsOk = double.TryParse(frequencyText, out f);
                bool interestIsOk = double.TryParse(interestText, out i);
                bool absoluteValueIsOk = double.TryParse(absoluteValueText, out a);
                if (freqIsOk && interestIsOk && absoluteValueIsOk)
                {

                    AppSettings setting = (await App.LocalDataBase.GetAppSettings())[0];
                    Income income = new Income();
                    income.AbsoluteValue = double.Parse(AbsoluteValueEntry.Text, App.DoubleCultureInfo);
                    income.Times = -1;
                    income.ContractTime = setting.LastRealLogin;
                    income.Category = IncomeCategory.DefaultDeposit;
                    income.LastAppPayment = setting.LastLogin;
                    income.LastRealPayment = setting.LastRealLogin;
                    income.Periodical = true;
                    double frequency = double.Parse(FrequencyEntry.Text, App.DoubleCultureInfo);
                    switch (frequency)
                    {
                        case 1:
                            frequency = 31;
                            break;
                        case 3:
                            frequency = 92;
                            break;
                        case 6:
                            frequency = 182;
                            break;
                        case 12:
                            frequency = 365;
                            break;

                    }
                    income.OverTimeAddition = 0;
                    income.Frequency = frequency;
                    income.LastRealSupposedPayment = setting.LastRealLogin;
                    income.LastSupposedPayment = setting.LastLogin;
                    income.DepositInterest = double.Parse(InterestEntry.Text, App.DoubleCultureInfo);
                    await App.LocalDataBase.AddIncome(income);
                    await DisplayAlert("", "Thank you for depositing", "OK");
                }
                else
                {
                    await DisplayAlert("", "Please introduce the proper values", "OK");
                }
            }
            else
            {
                await DisplayAlert("", "Please fill in all the fields", "OK");
            }
        }

        private void DepositList_Tapped(object sender, EventArgs e)
        {
           // DepositListView view = new DepositListView();
           // Navigation.PushAsync(view);
        }
    }
}