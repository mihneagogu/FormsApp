using FormsAppTelenav.Classes;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormsAppTelenav.Databases
{
    public class DataBase
    {
        
        private SQLiteAsyncConnection connection;

        public DataBase()
        {
            
        }

        

        public async void createDatabase(string path)
        {
            connection = new SQLiteAsyncConnection(path);
            await connection.CreateTableAsync<Person>();


        }

        public async Task<int> AddPerson(Person person)
        {
            await connection.InsertAsync(person);
            return 0;
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
