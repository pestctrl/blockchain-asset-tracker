using Acr.UserDialogs;
using BlockchainAPI;
using BlockchainAPI.Models;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BlockchainApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPropertyPage : ContentPage
    {
        private BlockchainClient client;

        public RegisterPropertyPage(BlockchainClient blockchainclient)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            client = blockchainclient;
        }

        async void CreateProperty(object sender, EventArgs e)
        {
            BlockchainClient.Result error;
            Property proeprty = new Property
            {
                PropertyId = property_id.Text,
                description = description.Text,
                owner = client.thisTrader.traderId
            };

            using (UserDialogs.Instance.Loading("Creating"))
            {
                error = await client.RegisterNewProperty(proeprty);
            }

            switch (error)
            {
                case BlockchainClient.Result.SUCCESS:
                    await DisplayAlert("Alert", "Sucessful create Asset", "Ok");
                    break;
                case BlockchainClient.Result.EXISTS:
                    await DisplayAlert("Alert", "Unsucessful create Asset: Asset id already exists", "Ok");
                    break;
                case BlockchainClient.Result.NETWORK:
                    await DisplayAlert("Alert", "Error: Network down. Please try again.", "Ok");
                    break;
            }

            await Navigation.PopAsync();
        }

        private async void CameraButton_Clicked(object sender, EventArgs e)
        {
            Plugin.Media.Abstractions.StoreCameraMediaOptions options = new Plugin.Media.Abstractions.StoreCameraMediaOptions();
            options.SaveToAlbum = true;

            var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(options);

            if (photo != null)
                PhotoImage.Source = ImageSource.FromStream(() => { return photo.GetStream(); });
        }
        
        async void Back(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}