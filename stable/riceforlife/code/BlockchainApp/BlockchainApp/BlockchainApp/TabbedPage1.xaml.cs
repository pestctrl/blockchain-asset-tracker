using BlockchainAPI;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BlockchainApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabbedPage1 : TabbedPage
    {
        public TabbedPage1 ()
        {
            InitializeComponent();
        }

        public TabbedPage1(BlockchainClient client)
        {
            InitializeComponent();
            this.Title = String.Format("Hello, {0}!", client.thisTrader.fullName);
            NavigationPage.SetHasNavigationBar(this, false);
          

            var userPage = new NavigationPage(new UserPage(client));
            var packagesPage = new NavigationPage(new PackagesPage(client));
            var historyPage = new NavigationPage(new HistoryPage(client));

            userPage.Title = "My Properties";
            packagesPage.Title = "My Packages";
            historyPage.Title = "My History";


            InitializeComponent();
            Children.Add(userPage);
            Children.Add(packagesPage);
            Children.Add(historyPage);
        }
    }
}