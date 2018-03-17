using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Plugin.Geolocator;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BlockchainApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TransferPage : ContentPage
	{

        public TransferPage (String pid)
		{
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







        // BAD, MAKE INTO LIBRARY
        public async Task<bool> sendProperty(String propertyID, String recipientID, String latitude, String longitude)
        {
            // BAD, INSTANTIATE ELSEWHERE
            HttpClient client = new HttpClient();

            Dictionary<String,String> parameters = new Dictionary<string, string>
            {
                { "property",propertyID },
                { "newOwner",recipientID },
                { "latitude", latitude},
                { "longitude", longitude}
            };
            // BAD, SET IN WEB CONFIG
            var results = await client.PostAsync("http://129.213.108.205:3000/api/org.acme.biznet.Trade", 
                new FormUrlEncodedContent(parameters));
            var stringResults = await results.Content.ReadAsStringAsync();
            
            return true;
        }
        async Task sendAsset()
        {
            if (await sendProperty(propertyId.Text, RecipientID.Text, latitude.Text, longitude.Text))
            {
                await DisplayAlert("Alert", String.Format("Property Sent to {0}", RecipientID.Text), "Confirm");
                await Navigation.PopAsync();
            }
        }
    }
}