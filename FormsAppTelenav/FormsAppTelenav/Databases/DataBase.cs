using FormsAppTelenav.Classes;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormsAppTelenav.Models;

namespace FormsAppTelenav.Databases
{
    public class DataBase
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
            /*createPersonTable();
            createCurrencyTable(); */
            CheckSymbols();

        }

        private async void CheckSymbols(){
            for (int i = 0; i < currencySymbols.Count(); i++)
            {
                var currency = await GetCurrency(currencySymbols[i]);
                int q = 0;
                if (currency == null){
                    currency = new Currency();
                    currency.Name = currencySymbols[i];
                    currency.ExchangeRate = currencyValues[i];
                    await AddCurrency(currency);
                    App.Currencies.Add(currency);
                    System.Diagnostics.Debug.WriteLine("Added curr " + currency.Name + " | " );
                }
                   

            }
            
        }



        public async void createCurrencyTable(){
            await connection.CreateTableAsync<Currency>();
            

        } 

        public async Task<int> AddCurrency(Currency currency)
        {
            return await connection.InsertAsync(currency);
        }

        public async Task<Currency> GetCurrency(string symbol){
            try
            {
                return await connection.Table<Currency>().Where(row => row.Name.Equals(symbol)).FirstAsync();
            }
            catch(InvalidOperationException) {
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

        public Task<List<Person>> GetPeople()
        {
            return connection.Table<Person>().ToListAsync();
        }

        public Task<int> SavePerson(Person person)
        {
            return connection.UpdateAsync(person);
        }



    }
}
