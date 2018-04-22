using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using BlockchainAPI.Transactions;
using BlockchainAPI.Models;

namespace BlockchainApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HistoryPage : ContentPage
	{
		public HistoryPage (List<Transaction> transactions)
		{

            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent ();

            listView.ItemsSource = transactions;

		}

        async void Back_User_page(object sender, EventArgs args)
        {
            await Navigation.PopAsync();
        }
    }
}