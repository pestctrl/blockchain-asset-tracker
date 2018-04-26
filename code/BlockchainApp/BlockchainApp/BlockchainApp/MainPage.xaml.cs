using Acr.UserDialogs;
using BlockchainAPI;
using BlockchainAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BlockchainApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        static int loginCode = 0;
        BlockchainClient client;

        public MainPage(IBlockchainService bcservice)
        {
            client = new BlockchainClient(bcservice);
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        async void LoginButton(object sender, EventArgs args)
        {
            User user = new User();

            user.username = login_id.Text;
            user.password = login_password.Text;



            bool loginSuccess = true;
            using (UserDialogs.Instance.Loading("Loading"))
            {
                loginSuccess = await client.login(user);
            }

  
            if (loginSuccess)
                loginCode = 0;
            else
                loginCode = 1;

            switch (loginCode)
            {
                case 0:
                    await DisplayAlert("Alert", "Login Successful\n" + login_id.Text + "\n" + login_password.Text, "OK");
                    await Navigation.PushAsync(new TabbedPage1(client));
                    break;
                case 1:
                    await DisplayAlert("Alert", "Login Failed", "OK");
                    break;
                case 2:
                    await DisplayAlert("Alert", "Connection Failed", "OK");
                    break;
                default:
                    await DisplayAlert("Alert", "System Error", "OK");
                    break;
            }
        }

        async void RegisterButton(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new RegisterPage(client));
        }
    }
}

