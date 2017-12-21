using FormsAppTelenav.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;

namespace FormsAppTelenav.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AuctionView : ContentPage
    {
        private ObservableCollection<Auction> stock = new ObservableCollection<Auction>();
        private AuctionsFromAPI auctions = new AuctionsFromAPI();
        public AuctionView(string symbol)
        {
            InitializeComponent();
            auctions.GetAuction(symbol, Stock);
            BindingContext = this;
            
        }

        public ObservableCollection<Auction> Stock { 
            set { stock = value; }
            get { return stock; }
        }

        
    }


}