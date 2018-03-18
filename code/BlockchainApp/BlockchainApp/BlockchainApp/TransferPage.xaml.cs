using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using BlockchainAPI;
using Plugin.Geolocator;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BlockchainApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TransferPage : ContentPage
	{
        private BlockchainClient client;

        public TransferPage (BlockchainClient client, String pid)
		{
            this.client = client;
            getLocation();
			InitializeComponent ();
            propertyId.Text = pid;
        }

        public async void getLocation()
        {
            var locator = CrossGeolocator.Current;
            var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10));
            latitude.Text = position.Latitude.ToString();
            longitude.Text = position.Longitude.ToString();
        }

        async Task sendAsset()
        {
            bool result;
            using (UserDialogs.Instance.Loading("Sending"))
            {
                await Task.Delay(10);
                result = client.sendProperty(propertyId.Text, RecipientID.Text, latitude.Text, longitude.Text);
            }
            if (result)
            {
                await DisplayAlert("Alert", String.Format("Property Sent to {0}", RecipientID.Text), "Confirm");
                await Navigation.PopAsync();
            }
        }
    }
}