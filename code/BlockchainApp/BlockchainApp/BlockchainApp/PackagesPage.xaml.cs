using BlockchainAPI;
using BlockchainAPI.Models;
using BlockchainAPI.Transactions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing.Net.Mobile.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Acr.UserDialogs;
using Plugin.Geolocator;

namespace BlockchainApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PackagesPage : ContentPage
	{

        private BlockchainClient client;
        ObservableCollection<Package> packagesOwn;

        public class PackageView
        {
            public string title { get; set; }
            public string subtitle { get; set; }
        }



        public PackagesPage(BlockchainClient client)
        {
            this.client = client;
            packagesOwn = new ObservableCollection<Package>();

            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            UpdatePackageList(client);
            listView.ItemsSource = packagesOwn;

        }

        void UpdatePackageList(BlockchainClient localClient)
        {
            packagesOwn.Clear();
            var packages = Task.Run(() => localClient.GetPackage()).Result;
            foreach (var package in packages)
            {
                packagesOwn.Add(package);
            }
        }

        void Handle_Refreshing(object sender, EventArgs e)
        {
            UpdatePackageList(client);

            listView.EndRefresh();
        }

        public async Task CreatePackage()
        {
            var results = await client.getMyProperties();
            await Navigation.PushAsync(new CreatePackagePage(client, results));
        }

        async void Selected_Handler(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            var packages = e.SelectedItem as Package;
            await Navigation.PushAsync(new PackageDetailPage(packages, client));
            listView.SelectedItem = null;
        }

        private async void ScanCode_Send()
        {
            ZXingScannerPage scanPage = new ZXingScannerPage();
            scanPage.OnScanResult += (result) => {
                // Stop scanning
                scanPage.IsScanning = false;

                // Pops the page, returns to TransferPage, and displays result of the scanned code
                Device.BeginInvokeOnMainThread(async () => {
                    await Navigation.PopAsync();
                    using (UserDialogs.Instance.Loading("Updating"))
                    {
                        NewTransfer t = new NewTransfer();
                        t.package = result.Text;
                        t.handler = client.thisTrader.traderId;
                        t.ingress = false;
                        var locator = CrossGeolocator.Current;
                        var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10));
                        t.latitude = position.Latitude;
                        t.longitude = position.Longitude;
                        await client.AddNewTransfer(t);
                    }
                });
            };

            await Navigation.PushAsync(scanPage);
        }

        private async void ScanCode_Receive()
        {
            ZXingScannerPage scanPage = new ZXingScannerPage();
            scanPage.OnScanResult += (result) => {
                // Stop scanning
                scanPage.IsScanning = false;

                // Pops the page, returns to TransferPage, and displays result of the scanned code
                Device.BeginInvokeOnMainThread(async () => {
                    await Navigation.PopAsync();
                    using (UserDialogs.Instance.Loading("Updating"))
                    {
                        NewTransfer t = new NewTransfer();
                        t.package = result.Text;
                        t.handler = client.thisTrader.traderId;
                        t.ingress = true;
                        var locator = CrossGeolocator.Current;
                        var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10));
                        t.latitude = position.Latitude;
                        t.longitude = position.Longitude;
                        await client.AddNewTransfer(t);
                    }
                });
            };

            await Navigation.PushAsync(scanPage);
        }
    }
}