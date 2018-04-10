using BlockchainAPI;
using BlockchainAPI.Models;
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

namespace BlockchainApp
{
	public partial class UserPage : ContentPage
    {
        
        private BlockchainClient client;

        public class AssetView
        {
            public string title { get; set; }
            public string subtitle { get; set; }
        }

        public UserPage()
        {
            InitializeComponent();
        }

        public UserPage(BlockchainClient client)
        {
            this.client = client;

            InitializeComponent();
            welcomeMessage.Text = String.Format("Hello, {0}!", client.thisTrader.fullName);
            NavigationPage.SetHasNavigationBar(this, false);
            updateAssetList(client);
        }
        
        void updateAssetList(BlockchainClient localClient)
        {
            ObservableCollection<AssetView> obc = new ObservableCollection<AssetView>();
            var assets = Task.Run(() => localClient.getMyProperties()).Result;
            foreach (Property obj in assets)
            {
                obc.Add(new AssetView() { title = obj.PropertyId, subtitle = obj.description });
            }

            current_asset_list.ItemsSource = obc;
        }

        void Handle_Refreshing(object sender, EventArgs e)
        {
            updateAssetList(client);

            current_asset_list.EndRefresh();
        }

        async void Submit_Send_Clicked(object Sender, EventArgs e)
        {
            string PropertyId = ((Button)Sender).CommandParameter.ToString();

            await Navigation.PushAsync(new TransferPage(client,PropertyId));
        }

        void logout(object Sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        async void TransactionButton(object sender, EventArgs args)
        {
            List<Transaction> transactions = await client.GetUserTransactions();
            await Navigation.PushAsync(new HistoryPage(transactions));
        }

        async void CreateAsset(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new RegisterPropertyPage(this.client));
        }

        async void OnMore(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            DisplayAlert("More Context Action", mi.CommandParameter + " more context action", "OK");

            string PropertyId = ((MenuItem)sender).CommandParameter.ToString();

            
            //await Navigation.PushAsync(new TransferPage(client, PropertyId));
        }

        public void OnDelete(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            DisplayAlert("Delete Context Action", mi.CommandParameter + " delete context action", "OK");
        }
        
    }
}