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
    public partial class CreditsFromMainView : ContentPage
    {
        List<StationaryCredit> credits = new List<StationaryCredit>();
        public List<StationaryCredit> Credits { set { credits = value; } get { return credits; } }
        public CreditsFromMainView(List<StationaryCredit> credits)
        {
            InitializeComponent();
            Credits = credits;
            BindingContext = this;
        }


    }
}