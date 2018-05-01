using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using BlockchainAPI.Transactions;
using BlockchainAPI;
using System.Collections.ObjectModel;

namespace BlockchainApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HistoryPage : ContentPage
	{
        private BlockchainClient client;
        ObservableCollection<CreatePackage> transactions;
        public HistoryPage (BlockchainClient client)
		{
            this.client = client;
            transactions = new ObservableCollection<CreatePackage>();

            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent ();

            UpdateTransactionList(client);
            listView.ItemsSource = transactions;
		}

        void UpdateTransactionList(BlockchainClient localClient)
        {
            transactions.Clear();
            var Results = Task.Run(() => localClient.GetUserTransactions()).Result;

            foreach (var transaction in Results)
            {
                transactions.Add(transaction);
            }
        }

        async void Selected_Handler(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            var packages = e.SelectedItem as CreatePackage;
            await Navigation.PushAsync(new HistoryDetailPage(packages, client));
        }

        void Handle_Refreshing(object sender, EventArgs e)
        {
            UpdateTransactionList(client);

            listView.EndRefresh();
        }
    }
}