using FormsAppTelenav.Classes;
using System;
using SkiaSharp;
using Microcharts;
using MEntry = Microcharts.Entry;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;
using FormsAppTelenav.Models;

namespace FormsAppTelenav.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AuctionView : ContentPage
    {
        private ObservableCollection<Auction> stock = new ObservableCollection<Auction>();
        private ObservableCollection<Auction> inverseStock = new ObservableCollection<Auction>();
        private ObservableCollection<AuctionBundle> singularyStock = new ObservableCollection<AuctionBundle>();

        private List<MEntry> entries = new List<MEntry>();

		protected override void OnAppearing()
		{
            NavigationPage.SetHasNavigationBar(this, false);
            List<VisualElement> elements = new List<VisualElement>();
            elements.Add(BuyAuctionIcon);
            elements.Add(SellAuctionIcon);
            App.MoveButtons(elements);
		}

		string lastColor;

        private AuctionsFromAPI auctions = new AuctionsFromAPI();
        private string auctionName;
        private string symbol;
        public AuctionView(string symbol, string auctionName)
        {
            InitializeComponent();
            
            this.symbol = symbol;
            this.auctionName = auctionName;
            MakeAuctions();
            
            BindingContext = this;

        }

        private string GetRandomColor()
        {
            Random random = new Random();
            Color color = Color.FromRgb(random.NextDouble(), random.NextDouble(), random.NextDouble());
            int r = (int)(color.R * 255);
            int g = (int)(color.G * 255);
            int b = (int)(color.B * 255);
            string hex = "#" + r.ToString("x2") + g.ToString("x2") + b.ToString("x2");
            return hex;

        }

        public ObservableCollection<AuctionBundle> SingularyStock { set { singularyStock = value; } get { return singularyStock; } }

        public ObservableCollection<Auction> InverseStock { set { inverseStock = value; } get { return inverseStock; } }

        public ObservableCollection<Auction> Stock { 
            set { stock = value; }
            get { return stock; }
        }

        private void ToSellAuctions_Clicked(object sender, EventArgs e)
        {
            BuyAuctionsView buyAuctionsView = new BuyAuctionsView(new ToBuyAuction(symbol, auctionName, stock[0].CloseValue, stock[0].Date), AuctionAction.SOLD);
            Navigation.PushAsync(buyAuctionsView);
        }

        private void ToBuyAuctions_Clicked(object sender, EventArgs e)
        {
            BuyAuctionsView buyAuctionsView = new BuyAuctionsView(new ToBuyAuction(symbol ,auctionName, stock[0].CloseValue, stock[0].Date), AuctionAction.BOUGHT);
            Navigation.PushAsync(buyAuctionsView);
        }

        public string Profit { get; set; }
        private int counter = 0;
        private async void MakeAuctions()
        {
           
            bool gotResponse = await auctions.GetAuction(symbol, stock);
            if (!gotResponse)
            {
                await DisplayAlert("", "We can't find the stock market, sorry!", "OK");
            }
            else
            {
                for (int i = stock.Count - 1; i >= 0; i--)
                {
                    inverseStock.Add(stock[i]);
                }
                for (int i = 6; i >= 0; i--)
                {
                    Auction s = stock[i];
                        Random random = new Random();
                        MEntry mEntry = new MEntry(float.Parse(s.CloseValue.ToString()));
                        mEntry.Label = s.Date.ToString();
                        mEntry.ValueLabel = s.CloseValue.ToString();
                        string c = GetRandomColor();
                        while (c == lastColor){
                            c = GetRandomColor();
                        }
                        lastColor = c;
                        mEntry.Color = SKColor.Parse(c);
                        entries.Add(mEntry);
                        counter++;
                    
                }


                LineChart chart = new LineChart() { Entries = entries };
                chart.LineAreaAlpha = 0;
                chart.MaxValue = float.Parse(entries[0].Value.ToString(), App.DoubleCultureInfo);
                chart.MinValue = float.Parse(entries[5].Value.ToString(), App.DoubleCultureInfo);
                chart.LabelTextSize = 25;
                AuctionChart.Chart = chart;
                AuctionChart.IsEnabled = true;




                AuctionBundle bindingBundle = new AuctionBundle();
                bindingBundle.Symbol = symbol;
                bindingBundle.CloseValueAtDateBought = stock[0].CloseValue;
                singularyStock.Add(bindingBundle);
                AuctiomGrid.BindingContext = SingularyStock[0];
                double profit = (inverseStock[0].CloseValue - inverseStock[inverseStock.Count - 1].CloseValue) / inverseStock[inverseStock.Count - 1].CloseValue;
                profit /= 100;
                PriceLabel.Text = string.Format("{0:0.000000} %", profit);
                Person person = App.User;
               // BuyButton.IsEnabled = true;
                AuctionBundleForDb boughtBundle = await App.LocalDataBase.GetAuctionBundleForSymbol(symbol, person);
                if (boughtBundle != null && boughtBundle.Number != 0)
                {
                 //   SellButton.IsEnabled = true;
                }
            }

            }

        private  void BuyAuction_Tapped(object sender, EventArgs e)
        {
            BuyAuctionsView buyAuctionsView = new BuyAuctionsView(new ToBuyAuction(symbol, auctionName, stock[0].CloseValue, stock[0].Date), AuctionAction.BOUGHT);
            Navigation.PushAsync(buyAuctionsView);
        }

        private  void SellAuction_Tapped(object sender, EventArgs e)
        {
            BuyAuctionsView buyAuctionsView = new BuyAuctionsView(new ToBuyAuction(symbol, auctionName, stock[0].CloseValue, stock[0].Date), AuctionAction.SOLD);
            Navigation.PushAsync(buyAuctionsView);
        }
    }

        
    }


