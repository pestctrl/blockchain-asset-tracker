using BlockchainAPI;
using BlockchainAPI.Models;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BlockchainApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RegisterPropertyPage : ContentPage
	{
        private BlockchainClient client;

        public RegisterPropertyPage(BlockchainClient blockchainclient)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            client = blockchainclient;
        }

        async void CreateProperty(object sender, EventArgs e)
	    {
            Property proeprty = new Property
            {
                PropertyId = property_id.Text,
                description = description.Text,
                owner = client.thisTrader.traderId
            };

            BlockchainClient.Result error;
            error = await client.RegisterNewProperty(proeprty);

            switch (error)
            {
                case BlockchainClient.Result.SUCCESS:
                    await DisplayAlert("Alert", "Sucessful create Asset", "Ok");
                    break;
                case BlockchainClient.Result.EXISTS:
                    await DisplayAlert("Alert", "Unsucessful create Asset: Asset id already exists", "Ok");
                    break;
                case BlockchainClient.Result.NETWORK:
                    await DisplayAlert("Alert", "Error: Network down. Please try again.", "Ok");
                    break;
            }

            await Navigation.PopAsync();
        }

        async void Back(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}