using BlockchainAPI;
using BlockchainAPI.Models;
using BlockchainAPI.Transactions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BlockchainApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserPage : ContentPage
    {
        
        private BlockchainClient client;
        ObservableCollection<Property> properties;
        
        public UserPage()
        {
            InitializeComponent();
        }

        public UserPage(BlockchainClient client)
        {
            this.client = client;
            properties = new ObservableCollection<Property>();

            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            updateAssetList(client);
            current_asset_list.ItemsSource = properties;
        }
        
        void updateAssetList(BlockchainClient localClient)
        {
            properties.Clear();
            var props = Task.Run(() => localClient.getMyProperties()).Result;
            foreach (Property obj in props)
            {
                properties.Add(obj);
            }
        }

        void Handle_Refreshing(object sender, EventArgs e)
        {
            updateAssetList(client);

            current_asset_list.EndRefresh();
        }

        void logout(object Sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new MainPage(client.GetBlockChainService()))
            {
                BarBackgroundColor = Color.FromRgb(5, 5, 5)
            };
        }

        async void TransactionButton(object sender, EventArgs args)
        {
            List<CreatePackage> transactions = await client.GetUserTransactions();
            await Navigation.PushAsync(new HistoryPage(transactions));
        }

        async void CreateAsset(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new RegisterPropertyPage(this.client));
            updateAssetList(client);
        }

        void GetAllToggled()
        {
          
        }

        //async void OnMore(object sender, EventArgs e)
        //{
        //    var mi = ((MenuItem)sender);
        //    DisplayAlert("More Context Action", mi.CommandParameter + " more context action", "OK");

        //    string PropertyId = ((MenuItem)sender).CommandParameter.ToString();

        //    Package p = new Package();
        //    //client.createPackage(p);
        //    //await Navigation.PushAsync(new TransferPage(client, PropertyId));
        //}

        //public void OnDelete(object sender, EventArgs e)
        //{
        //    var mi = ((MenuItem)sender);
        //    DisplayAlert("Delete Context Action", mi.CommandParameter + " delete context action", "OK");
        //}
        
    }
}