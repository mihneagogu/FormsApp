using FormsAppTelenav.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FormsAppTelenav.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HistoryView : ContentPage
    {
        private ObservableCollection<AuctionBundleForHistory> history = new ObservableCollection<AuctionBundleForHistory>();

        protected override async void OnAppearing()
        {
            NavigationPage.SetHasNavigationBar(this, false);
        }

        public HistoryView(List<AuctionBundleForHistory> bundles)
        {
            foreach(AuctionBundleForHistory b in bundles)
            {
                history.Add(b);
                
            }
            BindingContext = this;
            InitializeComponent();
        }

        public ObservableCollection<AuctionBundleForHistory> Bundles
        {
            set { history = value; }
            get { return history; }
        }
    }
}