using BlockchainAPI;
using BlockchainAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Property p = new Property();
            p.PropertyId = property_id.Text;
            p.description = description.Text;
            p.owner = client.thisTrader.traderId;
            BlockchainClient.Error error;
            error = await client.RegisterNewProperty(p);
            switch (error)
            {
                case BlockchainClient.Error.SUCCESS:
                    await DisplayAlert("Alert", "Sucessful create Asset", "Ok");
                    break;
                case BlockchainClient.Error.EXISTS:
                    await DisplayAlert("Alert", "Unsucessful create Asset: Asset id already exists", "Ok");
                    break;
                case BlockchainClient.Error.NETWORK:
                    await DisplayAlert("Alert", "Error: Network down. Please try again.", "Ok");
                    break;
            }
        }
	}
}