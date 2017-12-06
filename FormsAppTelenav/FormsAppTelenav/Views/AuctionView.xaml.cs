using FormsAppTelenav.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FormsAppTelenav.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AuctionView : ContentPage
    {
        private List<Auction> auctions = new List<Auction>();
        public AuctionView()
        {
            InitializeComponent();
            GenerateAuctions();
            BindingContext = this;
        }

        public void GenerateAuctions()
        {
            for (int i = 0; i < 50; i++) {
                string sI = i.ToString();
                double dI = Double.Parse(sI);
                auctions.Add(new Auction(105 + dI));
                System.Diagnostics.Debug.WriteLine("Number: " + i);
            }
        }
        public List<Auction> Auctions
        {
            set { auctions = value; }
            get { return auctions; }
        }
    }
}