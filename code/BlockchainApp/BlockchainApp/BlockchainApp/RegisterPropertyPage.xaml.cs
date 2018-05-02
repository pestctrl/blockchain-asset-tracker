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
        Plugin.Media.Abstractions.MediaFile photo;

        public RegisterPropertyPage(BlockchainClient blockchainclient)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            client = blockchainclient;
        }

        private String empty(String s)
        {
            if (s == null)
                return "";
            else
                return s;
        }

        async void CreateProperty(object sender, EventArgs e)
        {
            BlockchainClient.Result error;
            Property property = new Property
            {
                PropertyId = empty(property_id.Text),
                description = empty(description.Text),
                owner = empty(client.thisTrader.traderId)
            };

            using (UserDialogs.Instance.Loading("Creating"))
            {
                error = await client.RegisterNewProperty(property);
            }

            switch (error)
            {
                case BlockchainClient.Result.EMPTY:
                    await DisplayAlert("Alert", "Unsuccessful create: Fields cannot be empty.", "Ok");
                    break;
                case BlockchainClient.Result.SUCCESS:
                    await DisplayAlert("Alert", "Sucessful create Asset", "Ok");
                    break;
                case BlockchainClient.Result.EXISTERROR:
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

            photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(options);

            if (photo != null)
                PhotoImage.Source = ImageSource.FromStream(() => { return photo.GetStream(); });
        }
        
        async void Back(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}