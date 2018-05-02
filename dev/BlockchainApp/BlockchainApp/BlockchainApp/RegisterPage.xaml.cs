using BlockchainAPI;
using BlockchainAPI.Models;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BlockchainApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RegisterPage : ContentPage
	{
        BlockchainClient client;
		public RegisterPage (BlockchainClient client)
		{
            this.client = client;
			InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        async void CompletedRegister(object sender, EventArgs  e)
        {
            if (await client.UserExists(userId.Text))
            {
                await DisplayAlert("Alert", "The user Id have been taken", "Ok");
            }
            else
            {
                if (userId.Text != "" && fName.Text != "" && lName.Text != "" 
                    && userId.Text != null && fName.Text != null && lName.Text != null)
                {
                    User user = new User
                    {
                        username = userId.Text,
                        firstName = fName.Text,
                        lastName = lName.Text,
                        password = password.Text,
                        TraderType = traderType.Text
                    };

                    await client.RegisterNewTrader(user);
                    await DisplayAlert("Alert", "Sucessful register with user ID" + userId.Text, "Ok");
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Alert", "Make sure you fill all the box", "Ok");
                }
                
            }
        }
    }
}