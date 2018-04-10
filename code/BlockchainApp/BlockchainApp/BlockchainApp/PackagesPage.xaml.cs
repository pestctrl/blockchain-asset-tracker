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

        public PackagesPage ()
		{
			InitializeComponent ();
		}

        public PackagesPage(BlockchainClient client)
        {
            this.client = client;

            InitializeComponent();
            welcomeMessage.Text = String.Format("Hello, {0}!", client.thisTrader.fullName);
            NavigationPage.SetHasNavigationBar(this, false);
            
        }
    }
}