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
            NavigationPage.SetHasNavigationBar(this, false);

            var userPage = new NavigationPage(new UserPage(client));
            var packagesPage = new NavigationPage(new PackagesPage(client));

            InitializeComponent();
            Children.Add(userPage);
            Children.Add(packagesPage);
        }
    }
}