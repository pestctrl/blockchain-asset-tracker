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
	public partial class PackageDetailPage : ContentPage
	{
        BlockchainClient client;
        Package package;
		public PackageDetailPage (Package package, BlockchainClient client)
		{
            this.client = client;
            this.package = package;
			InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = package;
            recipient.Text = "Recipient: " + package.recipient.Substring(35);
            contents.Text = "Properties: " + GetProperties(package);
		}

        string GetProperties(Package package)
        {
            string properties = "";
            for (int i = 0; i < package.contents.Count; i++)
            {
                string property =  package.contents[i].Substring(37);
                property = property.Replace("%20", " ");
                if (i == package.contents.Count - 1)
                    properties = properties + property + ".";
                else
                    properties = properties + property + ", ";
            }

            return properties;
        }

        void Unbox(object sender, EventArgs e)
        {
           
        }

        async void Back(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
	}

 

}