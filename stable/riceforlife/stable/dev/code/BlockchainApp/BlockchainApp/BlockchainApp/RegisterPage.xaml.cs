using BlockchainAPI;
using BlockchainAPI.Models;
using Newtonsoft.Json;
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

                    User user = new User();
                    user.username = userId.Text;
                    user.firstName = fName.Text;
                    user.lastName = lName.Text;
                    user.password = password.Text;

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