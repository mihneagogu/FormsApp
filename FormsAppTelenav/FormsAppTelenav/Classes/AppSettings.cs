using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormsAppTelenav.Classes
{
    public class AppSettings
    {
        public AppSettings()
        {

        }
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string FirstLogin { get; set; }
        public string LastLogin { get; set; }
        public int CurrentPerson { get; set; }
    }
}
