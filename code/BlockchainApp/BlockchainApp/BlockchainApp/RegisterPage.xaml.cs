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
	public partial class RegisterPage : ContentPage
	{
        BlockchainClient client;
		public RegisterPage (BlockchainClient client)
		{
            this.client = client;
			InitializeComponent ();
		}

        async void CompletedRegister(object sender, EventArgs  e)
        {
            if (await client.userExists(userId.Text))
            {
                await DisplayAlert("Alert", "The user Id have been taken", "Ok");
            }
            else
            {
                if (userId.Text != "" && fName.Text != "" && lName.Text != "" 
                    && userId.Text != null && fName.Text != null && lName.Text != null)
                {
                    Trader t = new Trader();
                    t.traderId = userId.Text;
                    t.firstName = fName.Text;
                    t.lastName = lName.Text;
                    await client.RegisterNewTrader(t);
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