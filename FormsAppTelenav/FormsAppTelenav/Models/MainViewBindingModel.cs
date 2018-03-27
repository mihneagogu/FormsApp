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
        private double moneyStatement;

        public MainViewBindingModel(double statement)
        {
            MoneyStatement = statement;
        }

        public MainViewBindingModel()
        {

        }

        public double MoneyStatement
        {
            set
            {
                
                    OnPropertyChanged(KEY_MONEY_STATEMENT_CHANGED, ref moneyStatement, value);
            
            }
            get { return moneyStatement; }
        }
    }
}
