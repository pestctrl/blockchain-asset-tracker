using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BlockChain
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TransferPage : ContentPage
	{
        

        public TransferPage (String pid)
		{
			InitializeComponent ();
            propertyId.Text = pid;
		}

        // BAD, MAKE INTO LIBRARY
        public async Task<bool> sendProperty(String propertyID, String recipientID)
        {
            // BAD, INSTANTIATE ELSEWHERE
            HttpClient client = new HttpClient();

            Dictionary<String,String> parameters = new Dictionary<string, string>
            {
                { "property",propertyID },
                { "newOwner",recipientID }
            };
            // BAD, SET IN WEB CONFIG
            var results = await client.PostAsync("http://129.213.108.205:3000/api/org.acme.biznet.Trade", 
                new FormUrlEncodedContent(parameters));
            var stringResults = await results.Content.ReadAsStringAsync();
            
            return true;
        }
        async Task sendAsset()
        {
            if (await sendProperty(propertyId.Text, RecipientID.Text))
            {
                await DisplayAlert("Alert", String.Format("Property Sent to {0}", RecipientID.Text), "Confirm");
                await Navigation.PopAsync();
            }
        }
    }
}