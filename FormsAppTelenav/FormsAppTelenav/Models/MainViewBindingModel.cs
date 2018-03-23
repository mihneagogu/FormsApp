using FormsAppTelenav.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormsAppTelenav.Models
{
    public class MainViewBindingModel : ChangeNotify
    {

        private string KEY_MONEY_STATEMENT_CHANGED = "MoneyStatement";
        private string builder = "You have x {0} money";
        private string moneyStatement ="";

        public MainViewBindingModel(string statement)
        {
            MoneyStatement = statement;
        }

        public MainViewBindingModel()
        {

        }

        public string MoneyStatement
        {
            set
            {
                
                OnPropertyChanged(KEY_MONEY_STATEMENT_CHANGED, ref moneyStatement, value);
            }
            get { return moneyStatement; }
        }
    }
}
