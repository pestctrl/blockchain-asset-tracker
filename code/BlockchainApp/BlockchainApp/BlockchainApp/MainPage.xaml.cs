using Acr.UserDialogs;
using BlockchainAPI;
using BlockchainAPI.Models;
using System;
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
            bool loginSuccess = true;

            user.username = login_id.Text;
            user.password = login_password.Text;

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

