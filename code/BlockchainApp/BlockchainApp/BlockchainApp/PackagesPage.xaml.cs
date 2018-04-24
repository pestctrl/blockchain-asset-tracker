using BlockchainAPI;
using BlockchainAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BlockchainApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PackagesPage : ContentPage
	{

        private BlockchainClient client;

        public class PackageView
        {
            public string title { get; set; }
            public string subtitle { get; set; }
        }

        public PackagesPage(BlockchainClient client)
        {
            this.client = client;

            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        public async Task CreatePackage()
        {
            var results = await client.getMyProperties();
            await Navigation.PushAsync(new CreatePackagePage(client, results));
        }
    }
}