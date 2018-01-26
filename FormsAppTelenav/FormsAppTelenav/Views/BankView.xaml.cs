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
        Person person = new Person("Mikey");
        private Credit credit = new Credit();
		public BankView ()
		{
			InitializeComponent();
            MeddleWithDB(person);


        }

        private void ToCreditView_Clicked(object sender, EventArgs e)
        {
            CreditView creditView = new CreditView();
            Navigation.PushAsync(creditView);
        }

        private async void MeddleWithDB(Person person)
        {
            int awaiter = await App.LocalDataBase.AddPerson(person);
            List<Person> ppl = await App.LocalDataBase.GetPeople();

            ppl[ppl.Count - 1].Name = "mikey cel mai tare";
            Person pers = ppl[ppl.Count - 1];
            awaiter = await App.LocalDataBase.SavePerson(pers);
            ppl = await App.LocalDataBase.GetPeople();
            await DisplayAlert("ok", ppl[ppl.Count-1].Name, "ok");

        } 
    }
}