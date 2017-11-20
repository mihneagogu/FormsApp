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
    public partial class CreditView : ContentPage
    {
        public CreditView()
        {
            // sssss
            InitializeComponent();
            creditListView.ItemsSource = MainView.GetCreditForCustomRow();

        }


    }
}