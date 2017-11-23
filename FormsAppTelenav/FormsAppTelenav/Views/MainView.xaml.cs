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
    public partial class MainView : ContentPage
    {
        public MainView()
        {
            InitializeComponent();
            BindingContext = credit;
        }

        public Credit credit = new Credit();

        private void ShowCreditButton_Clicked(object sender, EventArgs e)
        {
            var creditView = new CreditView();
            creditView.BindingContext = credit;
            Navigation.PushAsync(creditView);
        }

        public Credit GetCredit()
        {
            return credit;
        }


    }
}