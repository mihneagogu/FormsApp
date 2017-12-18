using System;
using System.Net.Http;

namespace FormsAppTelenav.Classes
{
    public class AuctionsFromAPI
    {
        private HttpClient client = new HttpClient();
        private string auctionURL = "https://www.quandl.com/api/v1/datasets/WIKI/AAPL.csv?sort_order=asc&trim_start=2017-12-12&trim_end=2017-12-14";
        public AuctionsFromAPI()
        {

        }

        public async void GetAuction()
        {
            string symbolAuctionURL = auctionURL;
            HttpResponseMessage response = await client.GetAsync(symbolAuctionURL);
            if (response.IsSuccessStatusCode)
            {

                string stringResponse = await response.Content.ReadAsStringAsync();
                char[] delimiter = { ',' };
                string[] stats = stringResponse.Split(delimiter);
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
                            ///System.Diagnostics.Debug.WriteLine("First: " + values[1] + " 12");
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
                                    ///System.Diagnostics.Debug.WriteLine(values[0] + " " + auxIndex);
                                    ///System.Diagnostics.Debug.WriteLine(values[1] + " " + ++auxIndex);
                                    remember12 = values[1];
                                }
                            }
                            else
                            {
                                stockValues[indexOfColumn + 1] = s;
                                ///System.Diagnostics.Debug.WriteLine(s + " " + indexOfColumn);
                            }


                            indexOfColumn++;
                            if (indexOfColumn == 12)
                            {
                                for (int i = 0; i <= 12; i++)
                                {
                                    System.Diagnostics.Debug.WriteLine(i + ": " + stockValues[i]);
                                }
                                indexOfColumn = 0;
                            }
                        
                    }
                    ///System.Diagnostics.Debug.WriteLine(stringResponse);
                }

            }
        }
    }
}
