using Acr.UserDialogs;
using BlockchainAPI;
using BlockchainAPI.Models;
using BlockchainAPI.Transactions;
using System;
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

        async Task Unbox(object sender, EventArgs e)
        {
            using (UserDialogs.Instance.Loading("Unboxing"))
            {
                UnboxPackage unboxPackage = new UnboxPackage();
                unboxPackage.package = this.package.PackageId;
                unboxPackage.recipient = client.thisTrader.traderId;
                string expectedRecipient = package.recipient.Substring(35);

                if(expectedRecipient != unboxPackage.recipient)
                {
                    await DisplayAlert("Error", "You are not the recipient, and are not allowed to unbox this package", "Ok");
                }
                else
                {
                    await client.UnboxPackage(unboxPackage);
                    await DisplayAlert("Success", "The contents have been added to your package", "Ok");
                    await Navigation.PopAsync();
                }
            }
        }

        async Task EmailQRCode()
        {
            await Navigation.PushAsync(new SendEmailPage(client,package.PackageId));
        }

        async void Back(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
	}

 

}