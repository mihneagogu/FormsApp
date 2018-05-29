using FormsAppTelenav.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FormsAppTelenav.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class JobsView : ContentPage
    {
        private ObservableCollection<Income> jobs = new ObservableCollection<Income>();

		protected override void OnAppearing()
		{
            NavigationPage.SetHasNavigationBar(this, false);
		}

		public ObservableCollection<Income> Jobs
        {
            get { return jobs; }
            set { jobs = value; }
        }
        public JobsView()
        {
            BindingContext = this;
            InitializeComponent();
            jobListView.ItemsSource = Jobs;
            
           
            
            
            MeddleWithIncomes();
        }
        // pentru jobs doar trebuie facut un view si cu o lista, logica pentru orice income exista

        public async void MeddleWithIncomes()
        {
            AppSettings setting = (await App.LocalDataBase.GetAppSettings())[0];
            await App.LocalDataBase.ChangeAppTime(DateTime.Parse(setting.LastRealLogin));
            setting = (await App.LocalDataBase.GetAppSettings())[0];
            Income gardner = new Income("Gardner", 2000, true, IncomeCategory.Job, 30);
            gardner.Times = -1;
            gardner.LastRealPayment = setting.LastRealLogin;
            gardner.LastAppPayment = setting.LastLogin;
            gardner.ContractTime = gardner.LastRealPayment.ToString();
            
            gardner.LastRealSupposedPayment = setting.LastRealLogin;
            gardner.LastSupposedPayment = setting.LastLogin;
            gardner.LastRealSupposedPayment = setting.LastRealLogin;
            gardner.LastSupposedPayment = setting.LastLogin;
            Jobs.Add(gardner);

            Income programmer = new Income("Senior programmer", 9000, true, IncomeCategory.Job, 30);
            gardner.Times = -1;
            gardner.LastRealPayment = setting.LastRealLogin;
            gardner.LastAppPayment = setting.LastLogin;
            gardner.ContractTime = gardner.LastRealPayment.ToString();
            
            gardner.LastRealSupposedPayment = setting.LastRealLogin;
            gardner.LastSupposedPayment = setting.LastLogin;
            gardner.LastRealSupposedPayment = setting.LastRealLogin;
            gardner.LastSupposedPayment = setting.LastLogin;
            Jobs.Add(programmer); 

        }

       

        private async void QuitJobs_Clicked(object sender, EventArgs e)
        {
            List<Income> incomes = await App.LocalDataBase.GetIncomes();
            foreach(Income i in incomes)
            {
                if (i.Category == IncomeCategory.Job)
                {

                    await App.LocalDataBase.DeleteIncome(i);
                    await DisplayAlert("", "You have just quit your job as a " + i.Name , "OK");
                }
            }
        }

        private async void jobListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Income income = e.SelectedItem as Income;
            int jobcount = 0;
            List<Income> incomes = await App.LocalDataBase.GetIncomes();
            foreach(Income i in incomes)
            {
                if (i.Category == IncomeCategory.Job)
                {
                    jobcount++;
                }
            }
            if (jobcount >= 1)
            {
                if (e.SelectedItem != null)
                {
                    await DisplayAlert("", "You've already got a job, quit it before you get another one", "OK");
                }
            }
            else
            {
                await App.LocalDataBase.AddIncome(income);
                await DisplayAlert("","You are now a " + income.Name, "OK");
            }
            incomes = await App.LocalDataBase.GetIncomes();
            int x = 0;
            ((ListView)sender).SelectedItem = null;
        }

        private async void QuitJobIcon_Tapped(object sender, EventArgs e)
        {
            List<Income> incomes = await App.LocalDataBase.GetIncomes();
            foreach (Income i in incomes)
            {
                if (i.Category == IncomeCategory.Job)
                {

                    await App.LocalDataBase.DeleteIncome(i);
                    await DisplayAlert("", "You have just quit your job as a " + i.Name, "OK");
                }
            }
        }
    }
}