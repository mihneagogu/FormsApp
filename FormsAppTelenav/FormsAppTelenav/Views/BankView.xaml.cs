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

        protected override async void OnAppearing()
        {
            var x = RotateCharacter(DepositIcon, System.Threading.CancellationToken.None, 0);

            var Y = RotateCharacter(CreditIcon, System.Threading.CancellationToken.None, 4);
        }

        private async Task RotateCharacter(VisualElement element, System.Threading.CancellationToken token, int sleepTime)
        {
            await Task.Delay(sleepTime);
            while (!token.IsCancellationRequested)
            {
                await element.RotateTo(5, 60, Easing.Linear);
                await element.RotateTo(0, 0);
                await element.RotateTo(-5, 60, Easing.Linear);
                await element.RotateTo(0, 0);
            }
        }

        private void ToCreditView_Clicked(object sender, EventArgs e)
        {
            CreditView creditView = new CreditView();
            Navigation.PushAsync(creditView);
        }

        private void ToDepositView_Clicked(object sender, EventArgs e)
        {
            DepositView depositView = new DepositView();
            Navigation.PushAsync(depositView);
        }

        private void CreditIcon_Tapped(object sender, EventArgs e)
        {
            CreditView creditView = new CreditView();
            Navigation.PushAsync(creditView);
        }

        private void DepositIcon_Tapped(object sender, EventArgs e)
        {
            DepositView depositView = new DepositView();
            Navigation.PushAsync(depositView);
        }
    }
}