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
    public partial class TabbedPage1 : TabbedPage
    {
        private BlockchainClient client;

        public TabbedPage1 ()
        {
            InitializeComponent();
        }

        public TabbedPage1(BlockchainClient client)
        {
            InitializeComponent();
            this.Title = String.Format("Hello, {0}!", client.thisTrader.fullName);
            //NavigationPage.SetHasNavigationBar(this, false);

            var userPage = new NavigationPage(new UserPage(client));
            var packagesPage = new NavigationPage(new PackagesPage(client));

            userPage.Title = "My Properties";
            packagesPage.Title = "My Packages";

            InitializeComponent();
            Children.Add(userPage);
            Children.Add(packagesPage);
        }
    }
}