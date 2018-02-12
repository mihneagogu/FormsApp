using FormsAppTelenav.Classes;
using FormsAppTelenav.Databases;
using FormsAppTelenav.Models;
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
    public partial class BuyAuctionsView : ContentPage
    {
        private ToBuyAuction auctionToBuy;
        private string key;
        public BuyAuctionsView(ToBuyAuction auctionToBuy, string key)
        {
            InitializeComponent();
            this.auctionToBuy = auctionToBuy;
            this.key = key;
            BuyOrSellLabel.Text = "How many auctions do you want to " + key + "?";          
            BindingContext = this;
        }

        public string CompanyNameStatement
        {
            set { }
            get { return "You chose to buy auctions from " + auctionToBuy.Name; }
        }

        public string PriceValueStatement
        {
            set { }
            get { return "The current price of an auction is " + auctionToBuy.CloseValueAtDateBought + " $"; }
        }

        private void ConfirmedPayment_Clicked(object sender, EventArgs e)
        {
            AuctionBundle auctionBundle = new AuctionBundle(auctionToBuy.Symbol, auctionToBuy.Name, auctionToBuy.CloseValueAtDateBought, auctionToBuy.CloseValueAtDateBought, auctionToBuy.Date, NumberEntry.Text);
            AddBundleToStockPortfolio(auctionBundle);

        }

        private async void AddBundleToStockPortfolio(AuctionBundle auctionBundle)
        {
            List<Person> ppl = await App.LocalDataBase.GetPeople();
            if (key.Equals("buy"))
            {
                Person person = ppl[ppl.Count - 1] as Person;
                string auctionNumber = auctionBundle.Number;
                double auxAuctionNumber = double.Parse(auctionNumber);
                if (person.Amount < (auctionBundle.CloseValueAtDateBought * auxAuctionNumber))
                {
                    await DisplayAlert("", "You do not have enough money", "OK");
                }
                else
                {
                    auctionBundle.PersonID = ppl.Count;
                    int awaiter = await App.LocalDataBase.AddAuctionBundle(auctionBundle);
                    List<AuctionBundle> aBundles = await App.LocalDataBase.GetAuctionBundles();
                    person = ppl[ppl.Count - 1] as Person;
                    PersonToAuctionBundleConnection conn = new PersonToAuctionBundleConnection();
                    conn.PersonID = person.Id;
                    conn.AuctionBundleID = aBundles[aBundles.Count() - 1].Id;
                    awaiter = await App.LocalDataBase.AddPersonToAuctionBundleConnection(conn);
                    await DisplayAlert("", "Congratulations, you have just bought " + auctionBundle.Number + " auctions", "OK");
                    ppl = await App.LocalDataBase.GetPeople();
                    int q = 0;
                    
                }
            }
            else
            {
                ///trebuie verificat daca persoana are actiuni de la firma respectiva, adica daca exista un ID de actiuni cu simbolul respectiv si cu ID-ul persoanei respective
                Person person = ppl[ppl.Count - 1] as Person;
                string symbol = auctionBundle.Symbol;
                List<AuctionBundle> personsAuctionBundles = await App.LocalDataBase.GetAuctionBundlesForPerson(person);
                if (personsAuctionBundles.Count == 0)
                {
                    await DisplayAlert("", "You have no auctions", "OK");
                }
                else
                {
                    List<AuctionBundle> auctionsBundlesForCurrentSymbol = new List<AuctionBundle>();
                    foreach (AuctionBundle a in personsAuctionBundles)
                    {
                        if (a.Symbol == auctionBundle.Symbol /*&& a.Number != "0"*/)
                        {
                            auctionsBundlesForCurrentSymbol.Add(a);
                        }
                    }
                    if (auctionsBundlesForCurrentSymbol.Count() == 0)
                    {
                        await DisplayAlert("", "You have not bought auctions from " + auctionBundle.Name, "OK");
                    }
                    else
                    {
                        double medianValue;
                        double totalCost = 0;
                        double totalNumber = 0;
                        foreach(AuctionBundle a in auctionsBundlesForCurrentSymbol)
                        {
                            double auxNumber = double.Parse(a.Number, System.Globalization.CultureInfo.InvariantCulture);
                            totalCost += auxNumber * a.OpenValueAtDateBought;
                            totalNumber += auxNumber;
                        }
                        medianValue = totalCost / totalNumber;
                        double profit = (auctionBundle.OpenValueAtDateBought - medianValue) * double.Parse(auctionBundle.Number, System.Globalization.CultureInfo.InvariantCulture);
                        ProfitLabel.Text = "You gain " + profit + " from transactioning " + auctionBundle.Number + " auctions from " + auctionBundle.Name;
                        double auctionNumber = double.Parse(auctionBundle.Number);
                        if (auctionNumber > totalNumber)
                        {
                            await DisplayAlert("", "You do not have this many auctions in your portfolio", "OK");
                        }
                        else
                        {
                            AuctionBundle temporaryBundle = auctionBundle;
                            foreach(AuctionBundle a in auctionsBundlesForCurrentSymbol)
                            {
                                double auxPersonNumber = double.Parse(a.Number, System.Globalization.CultureInfo.InvariantCulture);
                                double auxToBuyNumber = double.Parse(temporaryBundle.Number, System.Globalization.CultureInfo.InvariantCulture);
                                if ((auxPersonNumber - auxToBuyNumber) < 0)
                                {
                                    a.Number = "0";
                                    auxToBuyNumber -= auxPersonNumber;
                                    temporaryBundle.Number = auxToBuyNumber.ToString();
                                }
                                else
                                {
                                    a.Number = (auxPersonNumber - auxToBuyNumber).ToString();
                                    temporaryBundle.Number = "0";
                                    
                                }
                            }
                            ProfitLabel.IsVisible = true;
                        }
                    }
                }
            }

        }
    }
}