using Acr.UserDialogs;
using BlockchainAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BlockchainApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SendEmailPage : ContentPage
	{
        String packageId;
        BlockchainClient client;
		public SendEmailPage (BlockchainClient client, String pid)
		{
            this.client = client;
            packageId = pid;
			InitializeComponent ();
		}

        public async Task SendEmail()
        {
            Regex matcher = new Regex(@"\b[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}\b");
            if (matcher.Match(EmailAddress.Text.Trim().ToUpper()).Success)
            {
                using (UserDialogs.Instance.Loading("Sending"))
                {
                    await client.SendQRCode(EmailAddress.Text.Trim(), packageId);
                    await DisplayAlert("Success", "Email was sent", "Ok");
                }
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Input Alert", "What was entered was not a valid email address", "Ok");
            }
        }
	}
}