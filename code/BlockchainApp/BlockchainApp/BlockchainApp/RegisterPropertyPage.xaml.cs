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
            client = blockchainclient;
        }

        async void CreateProperty(object sender, EventArgs e)
	    {
            Property p = new Property();
            p.PropertyId = property_id.Text;
            p.description = description.Text;
            p.owner = client.thisTrader.traderId;
	        await client.RegisterNewProperty(p);
	        await DisplayAlert("Alert", "Sucessful create Asset", "Ok");
        }
	}
}