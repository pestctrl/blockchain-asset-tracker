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
                    //txtBarcode.Text = result.Text;
                });
            };

            await Navigation.PushAsync(scanPage);
        }
    }
}