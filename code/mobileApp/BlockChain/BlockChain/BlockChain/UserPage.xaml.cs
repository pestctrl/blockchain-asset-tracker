using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BlockChain
{
	public partial class UserPage : ContentPage
	{
        ObservableCollection<AssetView> obc = new ObservableCollection<AssetView>();
        String owner;

        public class Property
        {
            [JsonProperty("$class")]
            public string objectType { get; set; }
            public string PropertyId { get; set; }
            public string description { get; set; }
            public string owner { get; set; }
        }

        public class AssetView
        {
            public string title { get; set; }
            public string subtitle { get; set; }
        }

		public UserPage (String traderID, String name)
		{
            owner = traderID;

			InitializeComponent ();
            welcomeMessage.Text = String.Format("Hello, {0}!", name);
            NavigationPage.SetHasNavigationBar(this, false);
            populateAssetList();
        }

        public async Task<String> getAssetsOwnedByMe()
        {
            // BAD, INSTANTIATE ELSEWHERE
            HttpClient client = new HttpClient();
            string requestURL = String.Format("http://129.213.108.205:3000/api/org.acme.biznet.Property?filter=%7B%22where%22%3A%20%7B%22owner%22%3A%20%22resource%3Aorg.acme.biznet.Trader%23{0}%22%7D%7D",
                owner);
            var results = await client.GetAsync(requestURL);

            return await results.Content.ReadAsStringAsync() ;
        }

        

        void populateAssetList()
        {
            String stuff = Task.Run(() => getAssetsOwnedByMe()).Result;
            stuff = stuff.Substring(1, stuff.Length - 2);
            foreach (String s in Regex.Split(stuff,@"(?<=\}),"))
            {
                var obj = JsonConvert.DeserializeObject<Property>(s);
                // BAD, JUST USE THE SAME OBJECT
                obc.Add(new AssetView() { title = obj.PropertyId, subtitle = obj.description });
            }

            current_asset_list.ItemsSource = obc;
        }

        void Submit_Send_Clicked(object Sender, EventArgs e)
        {
            Button b = (Button)Sender;
            string PropertyId = b.CommandParameter.ToString();
            Navigation.PushAsync(new TransferPage(PropertyId));
            for(int i = obc.Count()-1; i >=0 ; i--)
            {
                if(obc.ElementAt<AssetView>(i).title == PropertyId)
                {
                    obc.RemoveAt(i);
                }
            }
        }

        void logout(object Sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
	}
}