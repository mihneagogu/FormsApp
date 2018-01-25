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

        Person person = new Person("Mikeymike");
        public Person Person {
            set; get;
        }

        DataBase db = new Databases.DataBase();
        private DataBase LocalDB { get; set; }

        public MainView()
        {
            InitializeComponent();
            
            db.createDatabase(DependencyService.Get<ILocalFileHelper>().GetLocalFilePath("Person.db3"));
            MeddleWithDB(person);

            //db.AddPerson(person);
           
            //GetPeople();
            

            //AuctionHouseCommand = new Command(() => Navigation.PushAsync(new AuctionHouseView()));
            BindingContext = this;

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

        private async void MeddleWithDB(Person person)
        {
           // int x = await db.AddPerson(person);
            List<Person> ppl = await db.GetPeople();
            int q = 0;
            for (int i = 0; i < ppl.Count; i++)
            {
                System.Diagnostics.Debug.WriteLine(ppl[i].Id+ " " + ppl[i].StockPortfolio + " " + ppl[i].Name + " | ");
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