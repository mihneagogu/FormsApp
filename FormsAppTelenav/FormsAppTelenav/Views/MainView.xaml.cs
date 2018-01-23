using FormsAppTelenav.Classes;
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

namespace FormsAppTelenav.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainView : ContentPage
    {
        
        
        
        public Person Person {
            set; get;
        }

        public MainView()
        {
            InitializeComponent();
            createDatabase(DependencyService.Get<ILocalFileHelper>().GetLocalFilePath("Person.db3"));
            //AuctionHouseCommand = new Command(() => Navigation.PushAsync(new AuctionHouseView()));
            BindingContext = this;

        }

        private void ToBank_Clicked(object sender, EventArgs e)
        {
            BankView bankView = new BankView();
           
            //bankView.BindingContext = person;
            Navigation.PushAsync(bankView);
        }

        private void ToAuctionHouse_Clicked(object sender, EventArgs e)
        {
            AuctionHouseView auctionHouseView = new AuctionHouseView();

            
            Navigation.PushAsync(auctionHouseView);
        }

        private SQLiteAsyncConnection Database { get; set; }

        private async void createDatabase(string path)
        {
            var connection = new SQLiteAsyncConnection(path);
            await connection.CreateTableAsync<Person>();
            await connection.InsertAsync(new Person("Mihnea"));
            Database = connection;
            List<Person> ppl = await GetPeople();
            //await DisplayAlert("ok", ppl.Count.ToString(), "ok");
            for (int i = 0; i < ppl.Count; i++)
            {
                int x = ppl[i].Id;
            }
        }

        public Task<List<Person>> GetPeople()
        {
            return Database.Table<Person>().ToListAsync();
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