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
    public partial class CreditView : ContentPage
    {
        private Credit credit = new Credit();

        public CreditView()
        {
            InitializeComponent();
            BindingContext = credit;
            credit.BuyerMonthlyIncome = 2000;
            
        }

        private void ShowCreditButton_Clicked(object sender, EventArgs e)
        {
            var creditListView = new CreditListView();
            creditListView.BindingContext = credit;
 
            Navigation.PushAsync(creditListView);
        }

              
        
    }
}