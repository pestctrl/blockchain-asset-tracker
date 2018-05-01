using BlockchainAPI;
using BlockchainAPI.Models;
using System;
using System.Collections.ObjectModel;
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
            var props = Task.Run(() => localClient.GetMyProperties()).Result;

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

        async void CreateAsset(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new RegisterPropertyPage(this.client));

            updateAssetList(client);
        }
    }
}