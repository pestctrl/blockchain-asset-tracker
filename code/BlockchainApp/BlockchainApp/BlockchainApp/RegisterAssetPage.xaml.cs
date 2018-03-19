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
	public partial class RegisterAssetPage : ContentPage
	{
        private BlockchainClient client;

        public RegisterAssetPage(BlockchainClient blockchainclient)
        {
            InitializeComponent();
            client = blockchainclient;
        }

        async void CreateAsset(object sender, EventArgs e)
	    {
	       client.RegisterNewAsset(asset_id.Text, description.Text, client.getUserID());
	        await DisplayAlert("Alert", "Sucessful create Asset", "Ok");
        }
	}
}