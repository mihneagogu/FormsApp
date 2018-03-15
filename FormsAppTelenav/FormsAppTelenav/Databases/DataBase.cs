using FormsAppTelenav.Classes;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormsAppTelenav.Models;
using System.Runtime.ExceptionServices;

namespace FormsAppTelenav.Databases 
{
    public class DataBase : IMessageHandler
    {
        private SQLiteAsyncConnection connection;
        private double[] currencyValues = { 1, 1.21336, 4.63716 };
        private string[] currencySymbols = { "EUR", "USD", "RON" };

        public SQLiteAsyncConnection Connection { get {
                if (connection == null){
                    connection = new SQLiteAsyncConnection(Path); 
                }
                return connection;
            }
             
            set { connection = value; }
            
        }



        private string Path { get; set; }

        public DataBase(string path)
        {
            connection = new SQLiteAsyncConnection(path);
            Path = path;
            connection.CreateTableAsync<Person>().Wait();
            connection.CreateTableAsync<Currency>().Wait();
            connection.CreateTableAsync<AuctionBundle>().Wait();
            connection.CreateTableAsync<PersonToAuctionBundleConnection>().Wait();
            connection.CreateTableAsync<AppSettings>().Wait();
            connection.CreateTableAsync<AuctionBundleForHistory>().Wait();
            CheckSymbols();
            //App.MiddleDealer.RegisterMessage(MessageAction.AddedAuctionBundle, this);

        }

        public async Task<int> AddAuctionBundleToHistory(AuctionBundleForHistory auctionBundleForHistory)
        {
            return await connection.InsertAsync(auctionBundleForHistory);
        }

        public async Task<List<AuctionBundleForHistory>> GetHistory()
        {
            try
            {
                return await connection.Table<AuctionBundleForHistory>().ToListAsync();
            }
            catch(InvalidOperationException e)
            {
                return null;
            }
        }

        public async Task<List<PersonToAuctionBundleConnection>> GetPersonToAuctionBundleConncetions()
        {
            return await connection.Table<PersonToAuctionBundleConnection>().ToListAsync();
        }

        public async Task<int> AddPersonToAuctionBundleConnection(PersonToAuctionBundleConnection con)
        {
            return await connection.InsertAsync(con);
        }

        public async Task<int> AddAppSetting(AppSettings setting)
        {
            return await connection.InsertAsync(setting);
        }

        public Task<List<AppSettings>> GetAppSettings()
        {
            try
            {
                return connection.Table<AppSettings>().ToListAsync();
            }
            catch (InvalidOperationException e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<int> SaveAuctionBundle(AuctionBundle auctionBundle)
        {
            return await connection.UpdateAsync(auctionBundle);
        }

        public async Task<int> SaveAppSetting(AppSettings setting)
        {
            return await connection.UpdateAsync(setting);
        }

        private async void CheckSymbols(){
            List<Currency> currencies = await GetCurrencies();
            if (currencies.Count == 0)
            {
                for (int i = 0; i < currencySymbols.Count(); i++)
                {
                    Currency currency = new Currency();
                    currency.Name = currencySymbols[i];
                    currency.ExchangeRate = currencyValues[i];
                    await AddCurrency(currency);
                    App.Currencies.Add(currency);
                    System.Diagnostics.Debug.WriteLine("Added currency: " + currency.Name);
                }
            }
            else
            {
               if (currencySymbols.Count() != currencies.Count())
               {
                    for (int i = currencies.Count() - 1; i < currencySymbols.Count(); i++)
                    {
                        Currency currency = new Currency();
                        currency.Name = currencySymbols[i];
                        currency.ExchangeRate = currencyValues[i];
                        await AddCurrency(currency);
                        App.Currencies.Add(currency);
                        System.Diagnostics.Debug.WriteLine("Added currency: " + currency.Name);
                    }
               }
            }
            
        }

        public async Task<List<AuctionBundle>> GetAuctionBundlesForPerson(Person person)
        {
            int pID = person.Id;
            try
            {
                return await connection.Table<AuctionBundle>().Where(a => a.PersonID == pID).ToListAsync();
            }
            catch (InvalidOperationException e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return null;
            }
        }

        public async void createCurrencyTable(){
            await connection.CreateTableAsync<Currency>();
        } 

        public async Task<int> DeletePerson(Person person)
        {
            return await connection.DeleteAsync(person);
        }

        public async Task<int> AddAuctionBundle(AuctionBundle auctionBundle)
        {
            await connection.InsertAsync(auctionBundle);
            App.MiddleDealer.OnEvent(Databases.MessageAction.AddedAuctionBundle, auctionBundle);
            return 0;

        }

        public async Task<List<AuctionBundle>> GetAuctionBundles()
        {
            return await connection.Table<AuctionBundle>().ToListAsync();
        }

        public async Task<int> AddCurrency(Currency currency)
        {
            return await connection.InsertAsync(currency);
        }

        public async Task<Currency> GetCurrency(string symbol){
            ExceptionDispatchInfo capturedException = null;
            try
            {
                return await connection.Table<Currency>().Where(row => row.Name.Equals(symbol)).FirstAsync();
            }
            catch(InvalidOperationException e) {
                capturedException = ExceptionDispatchInfo.Capture(e);
                if (capturedException != null)
                {
                    return null;
                }
                return null;
                
            }
            
        }

        public async Task<Currency> GetCurrency(int id){
            try
            {
                return await connection.Table<Currency>().Where(row => row.Id == id).FirstAsync();
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public async Task<List<Currency>> GetCurrencies(){
            return await connection.Table<Currency>().ToListAsync();
        }

        public async Task<int> SaveCurrency(Currency currency){
            return await connection.UpdateAsync(currency);
        }

        public async void createPersonTable()
        {
            await connection.CreateTableAsync<Person>();
            

        }

        public async Task<int> AddPerson(Person person)
        {
            return await connection.InsertAsync(person);
        }

        public void GetPeople(ref List<Person> ppl)
        {
            
            try
            {
                var task = connection.Table<Person>().ToListAsync();
                Task.WaitAll(task);
                ppl = task.Result;
            }
            catch (InvalidOperationException e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                ppl.Clear();
            }
        }

        public Task<int> SavePerson(Person person)
        {
            return connection.UpdateAsync(person);
        }

        public void OnMessageReceived(MessageAction message, object payload)
        {
            switch (message)
            {
                case MessageAction.AddedAuctionBundle:
                    {
                        PersonToAuctionBundleConnection conn = new PersonToAuctionBundleConnection();
                        Person person = App.User;
                        conn.PersonID = person.Id;
                        var auctionBundle = payload as AuctionBundle;
                        conn.AuctionBundleID = auctionBundle.Id;
                        App.LocalDataBase.AddPersonToAuctionBundleConnection(conn);
                       /// await DisplayAlert("", "Congratulations, you have just bought " + auctionBundle.Number + " auctions", "OK");
                        double auxNumber = double.Parse(auctionBundle.Number, System.Globalization.CultureInfo.InvariantCulture);
                        double amountToPay = auctionBundle.CloseValueAtDateBought * auxNumber;
                        person.Amount -= amountToPay;

                        App.LocalDataBase.SavePerson(person);
                        AuctionBundleForHistory bundle = new AuctionBundleForHistory(auctionBundle.Symbol, auctionBundle.Name, auctionBundle.OpenValueAtDateBought, auctionBundle.CloseValueAtDateBought, auctionBundle.DateBought, auctionBundle.Number, AuctionAction.BOUGHT); ;
                        bundle.PersonID = person.Id;
                        App.LocalDataBase.AddAuctionBundleToHistory(bundle);
                        break;
                    }
            }
        }
    }
}
