 using BlockchainAPI;
using BlockchainAPI.Models;
using BlockchainAPI.Transactions;
using System;
using System.Collections.ObjectModel;
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
            var packages = Task.Run(() => localClient.GetMyPackages()).Result;

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
                scanPage.IsScanning = false;

                Device.BeginInvokeOnMainThread(async () => {
                    await Navigation.PopAsync();
                    using (UserDialogs.Instance.Loading("Updating"))
                    {
                        Package package = await client.GetPackageInformation(result.Text);
                        string handler = package.handler.Substring(35);
                        if(handler != client.thisTrader.traderId)
                        {
                            await DisplayAlert("Not Owner", "You are trying to send a package that does not belong to you", "Ok");
                        }
                        else
                        {
                            var locator = CrossGeolocator.Current;
                            var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10));

                            NewTransfer transfer = new NewTransfer
                            {
                                package = result.Text,
                                handler = client.thisTrader.traderId,
                                ingress = false,
                                latitude = position.Latitude,
                                longitude = position.Longitude
                            };

                            await client.AddNewTransfer(transfer);
                            await DisplayAlert("Success", "The package has been relinquished", "Confirm");
                        }
                    }
                });
            };

            await Navigation.PushAsync(scanPage);
        }

        private async void ScanCode_Receive()
        {
            ZXingScannerPage scanPage = new ZXingScannerPage();
            scanPage.OnScanResult += (result) => {
                scanPage.IsScanning = false;

                Device.BeginInvokeOnMainThread(async () => {
                    await Navigation.PopAsync();
                    using (UserDialogs.Instance.Loading("Updating"))
                    {
                        Package package = await client.GetPackageInformation(result.Text);
                        if(package.handler.Substring(35) != "TRADERNULL")
                        {
                            await DisplayAlert("Error", "You are trying to receive a that has already been received", "Ok");
                        }
                        else
                        {
                            var locator = CrossGeolocator.Current;
                            var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10));

                            NewTransfer transfer = new NewTransfer
                            {
                                package = result.Text,
                                handler = client.thisTrader.traderId,
                                ingress = true,
                                latitude = position.Latitude,
                                longitude = position.Longitude
                            };

                            await client.AddNewTransfer(transfer);
                            await DisplayAlert("Success", "The package has been received", "Confirm");
                        }
                    }
                });
            };

            await Navigation.PushAsync(scanPage);
        }
    }
}