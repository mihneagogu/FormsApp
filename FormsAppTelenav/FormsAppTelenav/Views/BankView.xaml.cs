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
        Databases.DataBase db = new Databases.DataBase();
        private Credit credit = new Credit();
		public BankView ()
		{
			InitializeComponent();
            db.createDatabase(DependencyService.Get<ILocalFileHelper>().GetLocalFilePath("Person.db3"));
            MeddleWithDB(person);

        }

        private void ToCreditView_Clicked(object sender, EventArgs e)
        {
            CreditView creditView = new CreditView();
            Navigation.PushAsync(creditView);
        }

        private async void MeddleWithDB(Person person)
        {
            int awaiter = await db.AddPerson(person);
            List<Person> ppl = await db.GetPeople();
            
            ppl[ppl.Count - 1].Name = "mikey cel mai tare";
            Person pers = ppl[ppl.Count - 1];
            awaiter = await db.SavePerson(pers);
            ppl = await db.GetPeople();
            await DisplayAlert("ok", ppl[ppl.Count-1].Name + " " + ppl[ppl.Count - 2].Name, "ok");
            int q = 0;

        } 
    }
}