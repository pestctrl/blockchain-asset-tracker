using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlockchainAPI;
using BlockchainAPI.Models;
using BlockchainAPI.Transactions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BlockchainApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PropertyViewPage : ContentView
	{
        BlockchainClient client;
        Property property;

        public PropertyViewPage ()
		{
			InitializeComponent ();
		}

        public PropertyViewPage(BlockchainClient client, Property property)
        {
            this.client = client;
            this.property = property;

            InitializeComponent();
            propertyImage.Source = ImageSource.FromStream(()=> property.image);
        }

        async void Back(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}