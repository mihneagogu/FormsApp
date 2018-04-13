using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;

namespace FormsAppTelenav.Classes
{
    public class AuctionsFromAPI
    {
        private HttpClient client = new HttpClient();
        private string auctionURL = "https://www.alphavantage.co/query?function=TIME_SERIES_DAILY_ADJUSTED&symbol={0}&apikey=AAUAQ28HQ7C9Y6T4&datatype=csv";

        private ObservableCollection<Auction> stock = new ObservableCollection<Auction>();
        public AuctionsFromAPI()
        {

        }

        public async Task<bool> GetAuction(string symbol, ObservableCollection<Auction> stock)
        {
            string symbolAuctionURL = String.Format(auctionURL, symbol);
            HttpResponseMessage response = await client.GetAsync(symbolAuctionURL);
            if (response.IsSuccessStatusCode)
            {

                string stringResponse = await response.Content.ReadAsStringAsync();

                string[] firstSplitStats = stringResponse.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                char[] delimiter = { ',' };
                for (int i = 1; i < firstSplitStats.Length; i++){
                    string[] stats = firstSplitStats[i].Split(delimiter);
                    AddStock(stats, stock);
                }
               
                int x = 0;
                return true;
            }
            else
            {
                return false;
            }

        }



        private void AddStock(string[] stockValues, ObservableCollection<Auction> stock)
        {
            
            var currentCulture = System.Globalization.CultureInfo.CurrentUICulture;
            var numberFormat = (System.Globalization.NumberFormatInfo)currentCulture.NumberFormat.Clone();
            numberFormat.NumberDecimalSeparator = ".";
            double x1 = double.Parse(stockValues[1], numberFormat);
            string date = stockValues[0];
            double open = double.Parse(stockValues[1], numberFormat);
            double close = double.Parse(stockValues[4], numberFormat);
            Auction a = new Auction();
            a.OpenValue = open;
            a.Date = date;
            a.CloseValue = close;
            stock.Add(a);

        }


        
    }
}
