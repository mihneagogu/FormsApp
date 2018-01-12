using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FormsAppTelenav.Classes;
using SQLite;

namespace FormsAppTelenav.Databases
{
    public class PersonDataBase
    {
        
            readonly SQLiteAsyncConnection database;
            public PersonDataBase(string dbPath)
            {
                database = new SQLiteAsyncConnection(dbPath);
                database.CreateTableAsync<Person>();
            }

            public Task<List<Person>> GetPerson()
            {
                return database.Table<Person>().ToListAsync();
            }

            public Task<int> CreatePerson(Person person)
            {
                    return database.InsertAsync(person);

            } 

            /*public Task<Auction> GetAuctionAsync(string symbol)
            {
                return database.Table<Person>().Where(i => i.Name == symbol).FirstOrDefaultAsync();
            } */

            
            

    }

}
