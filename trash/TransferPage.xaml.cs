using System;
using System.Threading.Tasks;
using Acr.UserDialogs;
using BlockchainAPI;
using BlockchainAPI.Transactions;
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
            
			InitializeComponent();
            propertyId.Text = pid;

            NavigationPage.SetHasNavigationBar(this, false);
        }

        public async Task<TransferPage> CreateTransferPage(BlockchainClient client, String pid)
        {
            TransferPage package = new TransferPage(client,pid);
            await package.getLocation();
            return package;
        }

        public async Task getLocation()
        {
            var locator = CrossGeolocator.Current;
            var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10));
            latitude.Text = position.Latitude.ToString();
            longitude.Text = position.Longitude.ToString();
        }

        async Task sendAsset()
        {
            BlockchainClient.Result error;
            using (UserDialogs.Instance.Loading("Sending"))
            {
                Transaction transaction = new Transaction
                {
                    property = propertyId.Text,
                    origOwner = client.thisTrader.traderId,
                    newOwner = RecipientID.Text,
                    latitude = Double.Parse(latitude.Text),
                    longitude = Double.Parse(longitude.Text)
                };

                error = await client.SendProperty(transaction);
            }
            switch (error)
            {
                case BlockchainClient.Result.SUCCESS:
                    await DisplayAlert("Alert", String.Format("Property Sent to {0}", RecipientID.Text), "Confirm");
                    await Navigation.PopAsync();
                    break;
                case BlockchainClient.Result.EXISTS:
                    await DisplayAlert("Alert", String.Format("Error: User doesn't exist"), "Confirm");
                    await Navigation.PopAsync();
                    break;
                case BlockchainClient.Result.NETWORK:
                    await DisplayAlert("Alert", String.Format("Netowrk error: Please try again."), "Confirm");
                    await Navigation.PopAsync();
                    break;
            }
        }
    }
}