using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BlockChain
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HistoryPage : ContentPage
	{
		public HistoryPage ()
		{
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent ();
            TransactionHistory();
		}

        void TransactionHistory()
        {

        }

        async void TransactionButt(object sender, EventArgs args)
        {
            await Navigation.Popasync();

        }
    }
}