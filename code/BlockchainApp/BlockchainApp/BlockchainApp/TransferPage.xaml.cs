﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using BlockchainAPI;
using BlockchainAPI.Models;
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
                Transaction tr = new Transaction();
                tr.property = propertyId.Text;
                tr.origOwner = client.thisTrader.traderId;
                tr.newOwner = RecipientID.Text;
                tr.latitude = latitude.Text;
                tr.longitude = longitude.Text;
                result = await client.sendProperty(tr);
            }
            if (result)
            {
                await DisplayAlert("Alert", String.Format("Property Sent to {0}", RecipientID.Text), "Confirm");
                await Navigation.PopAsync();
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
    }
}