using System;

using Xamarin.Forms;

namespace FormsAppTelenav.Models
{
    public class Stovm : ContentPage
    {
        public Stovm()
        {
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Hello ContentPage" }
                }
            };
        }
    }
}

