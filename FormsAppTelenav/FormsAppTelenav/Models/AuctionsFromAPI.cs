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
        private string auctionURL = "https://www.quandl.com/api/v1/datasets/WIKI/{0}.csv?sort_order=asc&trim_start=2017-12-12&trim_end=2017-12-14";

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
                char[] delimiter = { ',' };
                string[] stats = stringResponse.Split(delimiter);
                ParseCsvAndAddToList(stats, stock);
                return true;
            }
            else
            {
                return false;
            }

        }

        private void ParseCsvAndAddToList(string[] stats, ObservableCollection<Auction> stock)
        {
            int indexOfColumn = 0;
            string remember12 = "";
            string[] stockValues = new string[13];
            foreach (string s in stats)
            {
                if (s.Contains("Date") || s.Contains("Open") || s.Contains("High") || s.Contains("Low") || s.Contains("Close") || s.Contains("Volume") || s.Contains("Ex-Dividend") ||
                    s.Contains("Split Ratio") || s.Contains("Adj. Open") || s.Contains("Adj. High") || s.Contains("Adj. Low") || s.Contains("Adj. Close") || s.Contains("Adj. Volume"))
                {
                    if (s.Contains("Adj. Volume"))
                    {
                        string[] values = s.Split(new string[] { "\n" }, StringSplitOptions.None);
                        remember12 = values[1];

                    }
                }
                else
                {

                    stockValues[0] = remember12;

                    if (indexOfColumn == 11)
                    {
                        if (s.Contains("\n"))
                        {
                            int auxIndex = indexOfColumn;
                            System.Diagnostics.Debug.WriteLine("contains backslash n");
                            string[] values = s.Split(new string[] { "\n" }, StringSplitOptions.None);
                            stockValues[12] = values[0];
                            remember12 = values[1];
                        }
                    }
                    else
                    {
                        stockValues[indexOfColumn + 1] = s;
                    }


                    indexOfColumn++;
                    if (indexOfColumn == 12)
                    {
                        for (int i = 0; i <= 12; i++)
                        {
                            System.Diagnostics.Debug.WriteLine(i + ": " + stockValues[i]);
                        }
                        AddStock(stockValues, stock);
                        indexOfColumn = 0;
                    }

                }
            }
        }

        private void AddStock(string[] stockValues, ObservableCollection<Auction> stock)
        {
            
            var currentCulture = System.Globalization.CultureInfo.CurrentUICulture;
            var numberFormat = (System.Globalization.NumberFormatInfo)currentCulture.NumberFormat.Clone();
            numberFormat.NumberDecimalSeparator = ".";
            double x1 = double.Parse(stockValues[1], numberFormat);
            double x2 = double.Parse(stockValues[2], numberFormat);
            double x3 = double.Parse(stockValues[3], numberFormat);
            double x4 = double.Parse(stockValues[4], numberFormat);
            double x5 = double.Parse(stockValues[5], numberFormat);
            double x6 = double.Parse(stockValues[6], numberFormat);
            double x7 = double.Parse(stockValues[7], numberFormat);
            double x8 = double.Parse(stockValues[8], numberFormat);
            double x9 = double.Parse(stockValues[9], numberFormat);
            double x10 = double.Parse(stockValues[10], numberFormat);
            double x11 = double.Parse(stockValues[11], numberFormat);
            double x12 = double.Parse(stockValues[12], numberFormat);
            stock.Add(new Auction(stockValues[0], x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12));
        }


        
    }
}
