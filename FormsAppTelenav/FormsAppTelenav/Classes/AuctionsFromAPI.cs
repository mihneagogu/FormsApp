using System;
using System.Net.Http;

namespace FormsAppTelenav.Classes
{
    public class AuctionsFromAPI
    {
        private HttpClient client = new HttpClient();
        private string auctionURL = "https://www.quandl.com/api/v1/datasets/WIKI/AAPL.csv";
        public AuctionsFromAPI()
        {
             
        }

        public async void GetAuction(){
            string symbolAuctionURL = auctionURL;
            HttpResponseMessage response = await client.GetAsync(symbolAuctionURL);
            if (response.IsSuccessStatusCode){

                string stringR = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine(stringR);
            }

        }
    }
}
