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
    public partial class DepositListView : ContentPage
    {
        public DepositListView()
        {
            InitializeComponent();
        }

        private async void GetAllDepositedMoney_Clicked(object sender, EventArgs e)
        {
            await App.LocalDataBase.GetAllDepositedMoney();
        }

        // cand apasa un buton sa se ia banii din cont > MD > persoana primeste banii depusi initial + dobanda
    }
}