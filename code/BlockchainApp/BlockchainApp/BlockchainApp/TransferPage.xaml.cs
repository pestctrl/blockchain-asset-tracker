using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using BlockchainAPI;
using BlockchainAPI.Transactions;
using BlockchainAPI.Models;
using BlockchainAPI.Transactions;
using Plugin.Geolocator;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

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
            //SetPageQRImage();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        public async Task<TransferPage> CreateTransferPage(BlockchainClient client, String pid)
        {
            TransferPage p = new TransferPage(client,pid);
            await p.getLocation();
            return p;
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
            BlockchainClient.Error error;
            using (UserDialogs.Instance.Loading("Sending"))
            {
                Transaction tr = new Transaction();
                tr.property = propertyId.Text;
                tr.origOwner = client.thisTrader.traderId;
                tr.newOwner = RecipientID.Text;
                tr.latitude = Double.Parse(latitude.Text);
                tr.longitude = Double.Parse(longitude.Text);
                
                error = await client.sendProperty(tr);
            }
            switch (error)
            {
                case BlockchainClient.Error.SUCCESS:
                    await DisplayAlert("Alert", String.Format("Property Sent to {0}", RecipientID.Text), "Confirm");
                    await Navigation.PopAsync();
                    break;
                case BlockchainClient.Error.EXISTS:
                    await DisplayAlert("Alert", String.Format("Error: User doesn't exist"), "Confirm");
                    await Navigation.PopAsync();
                    break;
                case BlockchainClient.Error.NETWORK:
                    await DisplayAlert("Alert", String.Format("Netowrk error: Please try again."), "Confirm");
                    await Navigation.PopAsync();
                    break;
            }
        }

        private async void ScanCode()
        {
            ZXingScannerPage scanPage = new ZXingScannerPage();
            scanPage.OnScanResult += (result) => {
                // Stop scanning
                scanPage.IsScanning = false;

                // Pops the page, returns to TransferPage, and displays result of the scanned code
                Device.BeginInvokeOnMainThread(() => {
                    Navigation.PopAsync();
                    DisplayAlert("Scanned Code", result.Text, "OK");
                    txtBarcode.Text = result.Text;
                });
            };

            await Navigation.PushAsync(scanPage);
        }

        void SetPageQRImage()
        {
            //qrImage.Source = ImageSource.FromStream(() => { return DependencyService.Get<QRServices>().GenerateQRImage(propertyId.Text); });
            //DependencyService.Get<QRServices>().SaveQRImage(DependencyService.Get<QRServices>().GenerateQRImage(propertyId.Text), propertyId.Text);
        }
    }
}