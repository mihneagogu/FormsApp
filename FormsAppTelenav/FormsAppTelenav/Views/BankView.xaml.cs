using FormsAppTelenav.Classes;
using FormsAppTelenav.Databases;
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
	public partial class BankView : ContentPage
	{
       
        private Credit credit = new Credit();
		public BankView ()
		{
			InitializeComponent();
            


        }

        private void ToCreditView_Clicked(object sender, EventArgs e)
        {
            CreditView creditView = new CreditView();
            Navigation.PushAsync(creditView);
        }

        private async void MeddleWithDB(Person person)
        {
            
            List<Person> ppl = await App.LocalDataBase.GetPeople();

        } 
    }
}