using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using BlockchainAPI.Transactions;
using BlockchainAPI.Models;
using BlockchainAPI;

namespace BlockchainApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HistoryPage : ContentPage
	{
        private BlockchainClient client;
        public HistoryPage (BlockchainClient client, List<CreatePackage> transactions)
		{
            this.client = client;
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent ();

            listView.ItemsSource = transactions;

		}

        async void Back_User_page(object sender, EventArgs args)
        {
            await Navigation.PopAsync();
        }

        async void Selected_Handler(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            var packages = e.SelectedItem as CreatePackage;
            //await Navigation.PushAsync(new HistoryDetailPage(packages, client));
            listView.SelectedItem = null;
        }
    }
}