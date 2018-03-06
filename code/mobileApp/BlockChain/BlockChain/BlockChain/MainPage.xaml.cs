using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BlockChain
{
	public partial class MainPage : ContentPage
	{
        static int loginCode = 0;

        public MainPage()
		{
			InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        async void LoginButton(object sender, EventArgs args)
        {
            switch (loginCode)
            {
                case 0:
                    DisplayAlert("Alert", "Login Successful" + login_id.Text + " " + login_password.Text, "OK");
                    Navigation.PushAsync(new UserPage());
                    break;
                case 1:
                    DisplayAlert("Alert", "Login Failed", "OK");
                    break;
                case 2:
                    DisplayAlert("Alert", "Connection Failed", "OK");
                    break;
                default:
                    DisplayAlert("Alert", "System Error", "OK");
                    break;
            }
            
            ++loginCode;
        }

        async void RegisterButton(object sender, EventArgs args)
        {

        }
    }
}
