using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormsAppTelenav.Classes
{
    public class PyAuction
    {
        string symbol;
        string name;
        string lastSale;
        string marketCap;
        string ipoYear;
        string sector;
        string industry;
        string summaryQuote;
        
        PyAuction(string symbol, string name, string lastSale, string marketCap, string ipoYear, string sector, string industry, string summaryQuote)
        {
            this.symbol = symbol;
            this.name = name;
            this.lastSale = lastSale;
            this.marketCap = marketCap;
            this.ipoYear = ipoYear;
            this.sector = sector;
            this.industry = industry;
            this.summaryQuote = summaryQuote;
        }
    }
}
