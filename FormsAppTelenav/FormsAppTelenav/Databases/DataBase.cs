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
    
    public enum DealerResponse {
        NoAuctions,
        NoAuctionsFromCompany,
        NotEnoughAuctions,
        Success,
        Unreachable
    }

    public class DataBase : IMessageHandler
    {
        
        private SQLiteAsyncConnection connection;
        private double[] currencyValues = { 1, 1.21336, 4.63716 };
        private string[] currencySymbols = { "EUR", "USD", "RON" };


        public SQLiteAsyncConnection Connection
        {
            get
            {
                if (connection == null)
                {
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
            connection.CreateTableAsync<AuctionBundleForDb>().Wait();
            connection.CreateTableAsync<PersonToAuctionBundleConnection>().Wait();
            connection.CreateTableAsync<AppSettings>().Wait();
            connection.CreateTableAsync<AuctionBundleForHistory>().Wait();
            connection.CreateTableAsync<StationaryCredit>().Wait();
            CheckSymbols();
            //App.MiddleDealer.RegisterMessage(MessageAction.AddedAuctionBundle, this);

        }

        public async Task<DealerResponse> ChangeAppTime(DateTime lastTime){
            DealerResponse response = new DealerResponse();
            List<AppSettings> settings = await GetAppSettings();
            AppSettings setting = settings[0];
            List<object> payload = new List<object>();
            payload.Add(lastTime);
            payload.Add(setting);
            response = App.MiddleDealer.OnEvent(MessageAction.ChangeAppTime, payload);
            return response;
        }

        public async Task<AuctionBundleForDb> GetAuctionBundleForSymbol(string symbol, Person person){
            List<AuctionBundleForDb> personsBundles = await GetAuctionBundlesForPerson(person);
            if (personsBundles.Count == 0){
                return null;
            }
            foreach (AuctionBundleForDb a in personsBundles){
                if (a.Symbol == symbol){
                    return a;
                }
            }
            return null;
        }

        public async Task<DealerResponse> SellAuctionBundle(AuctionBundle auctionBundle)
        {
            DealerResponse response = new DealerResponse();
            List<object> payload = new List<object>();
            payload.Add(auctionBundle);
            AuctionBundleForDb bundleForSymbol = await GetAuctionBundleForSymbol(auctionBundle.Symbol, App.User);
            if (bundleForSymbol != null)
            {
                payload.Add(bundleForSymbol);
                response = App.MiddleDealer.OnEvent(Databases.MessageAction.SellAuctionBundle, payload);
            }
            else
            {
                return DealerResponse.NoAuctionsFromCompany;
            }

            return response;

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
            catch (InvalidOperationException e)
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

        public async Task<DealerResponse> AddCredit(StationaryCredit stationaryCredit){
            await connection.InsertAsync(stationaryCredit);
            List<object> payload = new List<object>();
            payload.Add(stationaryCredit);
            return App.MiddleDealer.OnEvent(MessageAction.BuyCredit, payload); 

        }

        public async Task<int> SaveCredit(StationaryCredit stationaryCredit){
            return await connection.UpdateAsync(stationaryCredit);
        }

        public async Task<List<StationaryCredit>> GetCredits(){
            List<StationaryCredit> sCredits = new List<StationaryCredit>();
            try
            {
                
                var task = connection.Table<StationaryCredit>().ToListAsync();
                Task.WaitAll(task);
                sCredits = task.Result;
                return sCredits;
            }
            catch (InvalidOperationException e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                sCredits.Clear();
                return sCredits;
            }
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

        public async Task<int> SaveAuctionBundle(AuctionBundleForDb auctionBundle)
        {
            return await connection.UpdateAsync(auctionBundle);
        }

        public async Task<int> SaveAppSetting(AppSettings setting)
        {
            return await connection.UpdateAsync(setting);
        }

        private async void CheckSymbols()
        {
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

        public async Task<List<AuctionBundleForDb>> GetAuctionBundlesForPerson(Person person)
        {
            int pID = person.Id;
            try
            {
                return await connection.Table<AuctionBundleForDb>().Where(a => a.PersonID == pID).ToListAsync();
            }
            catch (InvalidOperationException e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return null;
            }
        }

        public async void createCurrencyTable()
        {
            await connection.CreateTableAsync<Currency>();
        }

        public async Task<int> DeletePerson(Person person)
        {
            return await connection.DeleteAsync(person);
        }

        public async Task<DealerResponse> AddAuctionBundle(AuctionBundle auctionBundle)
        {
            DealerResponse response = new DealerResponse();
            List<object> payload = new List<object>();
            payload.Add(auctionBundle);
            double numberToBuy = double.Parse(auctionBundle.Number, System.Globalization.CultureInfo.InvariantCulture);
            List<AuctionBundleForDb> list = await GetAuctionBundles();
            AuctionBundleForDb bundleForSymbol = await GetAuctionBundleForSymbol(auctionBundle.Symbol, App.User);
            if (bundleForSymbol == null){
                
                AuctionBundleForDb dbBundle = new AuctionBundleForDb();
                dbBundle.Number = double.Parse(auctionBundle.Number, System.Globalization.CultureInfo.InvariantCulture);
                dbBundle.MedianValue = auctionBundle.CloseValueAtDateBought;
                dbBundle.Symbol = auctionBundle.Symbol;
                dbBundle.Name = auctionBundle.Name;
                dbBundle.PersonID = App.User.Id;
                response = App.MiddleDealer.OnEvent(Databases.MessageAction.AddedAuctionBundle, payload);
                if (response == DealerResponse.Success)
                {
                    await connection.InsertAsync(dbBundle);
                }
            }
            else {
                payload.Add(bundleForSymbol);
                response = App.MiddleDealer.OnEvent(Databases.MessageAction.AddedAuctionBundle, payload);


            }




            return response;


        }


        public async Task<List<AuctionBundleForDb>> GetAuctionBundles()
        {
            return await connection.Table<AuctionBundleForDb>().ToListAsync();
        }

        public async Task<int> AddCurrency(Currency currency)
        {
            return await connection.InsertAsync(currency);
        }

        public async Task<Currency> GetCurrency(string symbol)
        {
            ExceptionDispatchInfo capturedException = null;
            try
            {
                return await connection.Table<Currency>().Where(row => row.Name.Equals(symbol)).FirstAsync();
            }
            catch (InvalidOperationException e)
            {
                capturedException = ExceptionDispatchInfo.Capture(e);
                if (capturedException != null)
                {
                    return null;
                }
                return null;

            }

        }

        public async Task<Currency> GetCurrency(int id)
        {
            try
            {
                return await connection.Table<Currency>().Where(row => row.Id == id).FirstAsync();
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public async Task<List<Currency>> GetCurrencies()
        {
            return await connection.Table<Currency>().ToListAsync();
        }

        public async Task<int> SaveCurrency(Currency currency)
        {
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

        public DealerResponse OnMessageReceived(MessageAction message, List<object> payload)
        {
            Person person = App.User;
            DealerResponse response = DealerResponse.Unreachable;
            switch (message)
            {
                
                case MessageAction.AddedAuctionBundle:
                    {
                       
                        person = App.User;
                       
                        var auctionBundle = payload[0] as AuctionBundle;
                        double numberToBuy = double.Parse(auctionBundle.Number, System.Globalization.CultureInfo.InvariantCulture);
                        if (payload.Count == 1)
                        {
                            double auxNumber = double.Parse(auctionBundle.Number, System.Globalization.CultureInfo.InvariantCulture);
                            double amountToPay = auctionBundle.CloseValueAtDateBought * auxNumber;
                            person.Amount -= amountToPay;
                            
                            AuctionBundleForHistory bundle = new AuctionBundleForHistory(auctionBundle.Symbol, auctionBundle.Name, auctionBundle.OpenValueAtDateBought, auctionBundle.CloseValueAtDateBought, auctionBundle.DateBought, auctionBundle.Number, AuctionAction.BOUGHT); ;
                            bundle.PersonID = person.Id;
                            App.LocalDataBase.AddAuctionBundleToHistory(bundle);
                            App.LocalDataBase.SavePerson(person);
                            response = DealerResponse.Success;
                        }
                        else
                        {
                            AuctionBundleForDb bundleForSymbol = payload[1] as AuctionBundleForDb;
                            bundleForSymbol.MedianValue = ((bundleForSymbol.MedianValue * bundleForSymbol.Number) + (numberToBuy * auctionBundle.CloseValueAtDateBought)) / (double.Parse(auctionBundle.Number, System.Globalization.CultureInfo.InvariantCulture) + bundleForSymbol.Number);
                            bundleForSymbol.Number += double.Parse(auctionBundle.Number, System.Globalization.CultureInfo.InvariantCulture);
                            AuctionBundleForHistory bundle = new AuctionBundleForHistory(auctionBundle.Symbol, auctionBundle.Name, auctionBundle.OpenValueAtDateBought, auctionBundle.CloseValueAtDateBought, auctionBundle.DateBought, auctionBundle.Number, AuctionAction.BOUGHT); ;
                            bundle.PersonID = person.Id;
                            person.Amount -= numberToBuy * auctionBundle.CloseValueAtDateBought;
                            App.LocalDataBase.SavePerson(person);
                            App.LocalDataBase.AddAuctionBundleToHistory(bundle);
                            SaveAuctionBundle(bundleForSymbol);
                            response = DealerResponse.Success;
                        }
                        break;

                    }
                case MessageAction.BuyCredit: {
                        response = DealerResponse.Success;
                        break;
                    }
                case MessageAction.ChangeAppTime : {
                        DateTime lastTime = (DateTime)payload[0];
                        AppSettings setting = (AppSettings)payload[1];
                        DateTime timeNow = DateTime.Now.ToLocalTime();
                        TimeSpan span = timeNow.Subtract(lastTime);
                        double realMinutes = span.TotalMinutes;

                        double appTimePassed = realMinutes * App.Multiplier;
                        Math.Floor(appTimePassed);
                        DateTime newTime = lastTime.AddMinutes(appTimePassed);
                        setting.LastLogin = newTime.ToString();
                        SaveAppSetting(setting);
                        int x = 0;
                        response = DealerResponse.Success;
                        break;
                    }
                case MessageAction.SellAuctionBundle:
                    {
                        AuctionBundle auctionBundle = payload[0] as AuctionBundle;
                        AuctionBundleForDb bundleForSymbol = payload[1] as AuctionBundleForDb;
                        double numberToSell = double.Parse(auctionBundle.Number, System.Globalization.CultureInfo.InvariantCulture);
                        if ((bundleForSymbol.Number - numberToSell) >= 0)
                        {
                            bundleForSymbol.Number -= numberToSell;
                            App.LocalDataBase.SaveAuctionBundle(bundleForSymbol);
                            person.Amount += numberToSell * auctionBundle.CloseValueAtDateBought;
                            AuctionBundleForHistory bundle = new AuctionBundleForHistory(auctionBundle.Symbol, auctionBundle.Name, auctionBundle.OpenValueAtDateBought, auctionBundle.CloseValueAtDateBought, auctionBundle.DateBought, auctionBundle.Number, AuctionAction.SOLD); ;
                            bundle.PersonID = person.Id;
                            App.LocalDataBase.AddAuctionBundleToHistory(bundle);
                            App.LocalDataBase.SavePerson(person);
                            App.LocalDataBase.SaveAuctionBundle(bundleForSymbol);
                            response = DealerResponse.Success;
                        }
                        else
                        {
                            return DealerResponse.NotEnoughAuctions;
                        }
                            
                        
                        break;
                    }
                     
            } 

            return response;

        }
    }
}
