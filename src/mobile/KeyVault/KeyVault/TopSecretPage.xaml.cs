using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace KeyVault
{
    public partial class TopSecretPage : ContentPage
    {
        public TopSecretPage()
        {
            InitializeComponent();

            BindingContext = new TopSecretPageViewModel();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            var vm = BindingContext as TopSecretPageViewModel;

            await vm?.GetAllSecrets();
        }
    }
}
