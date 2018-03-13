using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BlockChain
{
	public partial class MainPage : ContentPage
	{
        static int loginCode = 0;


        public class Trader
        {
            [JsonProperty("$class")]
            public string objectType { get; set; }
            public string traderId { get; set; }
            public string firstName { get; set; }
            public string lastName { get; set; }
        }

        public MainPage()
		{
			InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        public async Task<String> getName(string traderID)
        {
            HttpClient client = new HttpClient();
            var requestURL = String.Format("http://129.213.108.205:3000/api/org.acme.biznet.Trader/{0}",traderID);
            var results = await client.GetAsync(requestURL);

            var resultsString = await results.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<Trader>(resultsString);
            return String.Format("{0} {1}", obj.firstName, obj.lastName);
        }

        async void LoginButton(object sender, EventArgs args)
        {
            string name = Task.Run(() => getName(login_id.Text)).Result;
            switch (loginCode)
            {
                case 0:
                    await DisplayAlert("Alert", "Login Successful\n" + login_id.Text + "\n" + login_password.Text, "OK");
                    await Navigation.PushAsync(new UserPage(login_id.Text, name));
                    break;
                case 1:
                    DisplayAlert("Alert", "Login Failed", "OK");
                    break;
                case 2:
                    DisplayAlert("Alert", "Connection Failed", "OK");
                    break;
                default:
                    DisplayAlert("Alert", "System Error", "OK");
                    break;
            }
        }

        async void RegisterButton(object sender, EventArgs args)
        {

        }
    }
}
