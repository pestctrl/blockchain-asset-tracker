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
	public partial class WelcomePage : ContentPage
	{
        private BlockchainClient client;
        ObservableCollection<Property> properties;

        public WelcomePage (BlockchainClient client)
		{
            if (client == null)
                throw new ArgumentException();

            BindingContext = client.thisTrader;
            this.client = client;
            properties = new ObservableCollection<Property>();
            InitializeComponent ();
            updateAssetList(client);
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

        void logout(object Sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new MainPage(client.GetBlockChainService()))
            {
                BarBackgroundColor = Color.FromRgb(5, 5, 5)
            };
        }

        async void TransactionButton(object sender, EventArgs args)
        {
            List<Transaction> transactions = await client.GetUserTransactions();
            await Navigation.PushAsync(new HistoryPage(transactions));
        }

        async void CreateAsset(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new RegisterPropertyPage(this.client));
            updateAssetList(client);
        }

        async void Asset(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new TabbedPage1(this.client));
        }
    }
}