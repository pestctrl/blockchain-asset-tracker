using BlockchainAPI;
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
	public partial class RegisterPage : ContentPage
	{
		public RegisterPage ()
		{
			InitializeComponent ();
		}

        async void CompletedRegister(object sender, EventArgs  e)
        {
            BlockchainClient blockChainClient = new BlockchainClient(userId.Text);

            if (blockChainClient.userExist)
            {
                await DisplayAlert("Alert", "The user Id have been taken", "Ok");
            }
            else
            {
                blockChainClient.RegisterNewTrader(userId.Text, fName.Text, lName.Text);
                await DisplayAlert("Alert", "Sucessful register with user ID" + userId.Text, "Ok");
                await Navigation.PopAsync();
            }
        }
	}
}