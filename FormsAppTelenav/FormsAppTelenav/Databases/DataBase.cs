﻿using FormsAppTelenav.Classes;
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
        Unreachable,
        NothingToDo
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
            connection.CreateTableAsync<Income>().Wait();
            CheckSymbols();
            //App.MiddleDealer.RegisterMessage(MessageAction.AddedAuctionBundle, this);

        }

        public async Task<DealerResponse> ManageIncomes(List<object> payload)
        {
            if (payload.Count != 0)
            {
                
                AppSettings setting = (await GetAppSettings())[0];
                await ChangeAppTime(DateTime.Parse(setting.LastRealLogin));
                setting = (await GetAppSettings())[0];
                payload.Add(setting);
                return App.MiddleDealer.OnEvent(MessageAction.ManageIncomes, payload);
            }
            return DealerResponse.NothingToDo;

        }

        public async Task<DealerResponse> GetAllDepositedMoney()
        {
            List<Income> incomes = await GetIncomes();
            List<object> payload = new List<object>();
            foreach (Income i in incomes)
            {
                if (i.Times == -1)
                {
                    payload.Add(i);
                }
            }
            return App.MiddleDealer.OnEvent(MessageAction.GetAllDepositedMoney, payload);
        }

        public async Task<int> DeleteIncome(Income income)
        {
            return await connection.DeleteAsync(income);
        }

        public async Task<DealerResponse> ChangeAppTime(DateTime lastTime){
            DealerResponse response = new DealerResponse();
            List<AppSettings> s = await GetAppSettings();
            AppSettings setting = s[0];
            DateTime currentTime = DateTime.Now;
            TimeSpan span = currentTime.Subtract(lastTime);
            double minutespan = span.TotalMinutes;
            minutespan *= App.Multiplier;
            DateTime t = DateTime.Parse(setting.LastLogin).AddMinutes(minutespan);
            setting.LastRealLogin = DateTime.Now.ToString();
            setting.LastLogin = t.ToString();
            await SaveAppSetting(setting);
            response = DealerResponse.Success;
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

        public async Task<List<Income>> GetIncomes()
        {
            try
            {
                return await connection.Table<Income>().ToListAsync();
            }
            catch (InvalidOperationException e)
            {
                
                return null;
            }
        }

        public async Task<int> AddIncome(Income income)
        {
            return await connection.InsertAsync(income);
        }

        public async Task<int> SaveIncome(Income income)
        {
            return await connection.UpdateAsync(income);
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

        public async Task<string> GetAppTime(DateTime lastTime)
        {
            await ChangeAppTime(lastTime);
            List<AppSettings> settings = await GetAppSettings();
            return settings[0].LastLogin;

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
                case MessageAction.GetAllDepositedMoney:
                    {
                        person = App.User;
                        List<Income> incomes = new List<Income>();
                        foreach (object o in payload)
                        {
                            incomes.Add((Income)o);
                        }
                        foreach (Income i in incomes)
                        {
                            person.Amount += i.AbsoluteValue + i.OverTimeAddition;
                            DeleteIncome(i);
                        }

                        return DealerResponse.Success;
                        break;
                    }
                case MessageAction.ManageIncomes:
                    {
                        List<Income> incomes = new List<Income>();
                        for (int i = 0; i < payload.Count - 1; i++)
                        {
                            incomes.Add((Income)payload[i]);
                        }
                        AppSettings setting = payload[payload.Count - 1] as AppSettings;
                        foreach(Income i in incomes)
                        {
                            if (i.Times != -1)
                            {
                                if (i.TimesLeft > 0 && i.LastRealPayment != null)
                                {
                                    DateTime lastRealLogin = DateTime.Parse(setting.LastRealLogin);
                                    TimeSpan span = lastRealLogin.Subtract(DateTime.Parse(i.LastRealSupposedPayment));
                                    double appMinutes = span.TotalMinutes * App.Multiplier;

                                    if (appMinutes > i.Frequency)
                                    {
                                        // se sustrage/se adauga la persoana cat trebuie sa plateasca
                                        

                                        double timesToSubtract = appMinutes / i.Frequency;
                                        timesToSubtract = Math.Floor(timesToSubtract);
                                        i.TimesLeft -= int.Parse(Math.Floor(timesToSubtract).ToString());
                                        i.LastSupposedPayment = (DateTime.Parse(i.LastSupposedPayment).AddMinutes(timesToSubtract * i.Frequency)).ToString();
                                        i.LastRealSupposedPayment = (DateTime.Parse(i.LastRealSupposedPayment).AddMinutes((timesToSubtract * i.Frequency) / App.Multiplier)).ToString();
                                        i.LastAppPayment = setting.LastLogin;
                                        i.LastRealPayment = setting.LastRealLogin;
                                        SaveIncome(i);
                                        person.Amount += (i.AbsoluteValue * timesToSubtract);
                                        SavePerson(person);
                                    }

                                }
                            }
                            else
                            {
                                switch (i.Category)
                                {
                                    case IncomeCategory.DefaultDeposit:
                                        {
                                            DateTime lastRealLogin = DateTime.Parse(setting.LastRealLogin);
                                            TimeSpan span = lastRealLogin.Subtract(DateTime.Parse(i.LastRealSupposedPayment));
                                            double appMinutes = span.TotalMinutes * App.Multiplier;
                                            if (appMinutes > i.Frequency)
                                            {
                                                double timesToSubtract = appMinutes / i.Frequency;
                                                timesToSubtract = Math.Floor(timesToSubtract);
                                                i.OverTimeAddition += Math.Floor(timesToSubtract) * ((i.DepositInterest / 100) * i.AbsoluteValue);
                                                i.LastSupposedPayment = (DateTime.Parse(i.LastSupposedPayment).AddMinutes(timesToSubtract * i.Frequency)).ToString();
                                                i.LastRealSupposedPayment = (DateTime.Parse(i.LastRealSupposedPayment).AddMinutes((timesToSubtract * i.Frequency)/App.Multiplier)).ToString();
                                                i.LastAppPayment = setting.LastLogin;
                                                i.LastRealPayment = setting.LastRealLogin;
                                                SaveIncome(i);
                                            }
                                            break;
                                        }
                                    case IncomeCategory.Job:
                                        {
                                            DateTime lastRealLogin = DateTime.Parse(setting.LastRealLogin);
                                            TimeSpan span = lastRealLogin.Subtract(DateTime.Parse(i.LastRealSupposedPayment));
                                            double appMinutes = span.TotalMinutes * App.Multiplier;
                                            if (appMinutes > i.Frequency)
                                            {
                                                double timesToSubtract = appMinutes / i.Frequency;
                                                timesToSubtract = Math.Floor(timesToSubtract);
                                                // se adauga la person banii
                                                i.LastSupposedPayment = (DateTime.Parse(i.LastSupposedPayment).AddMinutes(timesToSubtract * i.Frequency)).ToString();
                                                i.LastRealSupposedPayment = (DateTime.Parse(i.LastRealSupposedPayment).AddMinutes((timesToSubtract * i.Frequency) / App.Multiplier)).ToString();
                                                i.LastAppPayment = setting.LastLogin;
                                                i.LastRealPayment = setting.LastRealLogin;
                                                person.Amount += i.AbsoluteValue * timesToSubtract;
                                                SavePerson(person);
                                                SaveIncome(i);
                                            }
                                            break;
                                        }
                                }
                            }
                        }

                        return DealerResponse.Success;
                        break;
                    }
                case MessageAction.BuyCredit: {
                        StationaryCredit credit = payload[0] as StationaryCredit;
                        Income income = new Income();
                        income.Category = IncomeCategory.Credit;
                        income.AbsoluteValue = (-1)*(((credit.Cost * (100 + credit.Interest)) / 100)/credit.Duration);
                        income.Periodical = true;
                        income.Frequency = 30;
                        // contracttime, lastpayment, lastrealpayment, lastsupppayment, lastrealsupppayment
                        income.Times = (int)credit.Duration;
                        income.TimesLeft = (int)(income.Times);
                        income.ContractTime = credit.DateBought.ToString();
                        income.LastRealPayment = credit.DateBought.ToString();
                        income.LastAppPayment = credit.AppDateBought.ToString();
                        income.LastRealSupposedPayment = credit.DateBought.ToString();
                        income.LastSupposedPayment = credit.AppDateBought.ToString();
                        AddIncome(income);
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
