using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormsAppTelenav.Models
{
    public class PersonToAuctionBundleConnection
    {

        public PersonToAuctionBundleConnection()
        {

        }
        
        public int PersonID { get; set; }
        public int AuctionBundleID { get; set; }

    }
}
