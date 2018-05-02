using Acr.UserDialogs;
using BlockchainAPI;
using BlockchainAPI.Models;
using BlockchainAPI.Transactions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BlockchainApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CreatePackagePage : ContentPage
	{
        ObservableCollection<SelectedData<Property>> SelectedDataList { get; set; }
        BlockchainClient client;

        public CreatePackagePage (BlockchainClient client, List<Property> props)
		{
            this.client = client;
            SelectedDataList = new ObservableCollection<SelectedData<Property>>();

            props.ForEach(prop => SelectedDataList.Add(new SelectedData<Property>() {data = prop, selected = false }));
			InitializeComponent ();

            NavigationPage.SetHasNavigationBar(this, false);
            property_list.ItemsSource = SelectedDataList;
		}

        void OnSelection(object sender, SelectedItemChangedEventArgs selectItem)
        {
            int collectionPosition = SelectedDataList.IndexOf(sender as SelectedData<Property>);
        }

        async void CreatePackage()
        {
            BlockchainClient.Result error;
            CreatePackage package = new CreatePackage
            {
                packageId = Guid.NewGuid().ToString(),
                sender = client.thisTrader.traderId,
                recipient = recipient.Text,
                contents = SelectedDataList.Where(prop => prop.selected)
                                         .Select(prop => prop.data.PropertyId)
                                         .ToList()
            };

            using (UserDialogs.Instance.Loading("Creating"))
            {
                error = await client.CreatePackage(package);
            }

            switch (error)
            {
                case BlockchainClient.Result.SUCCESS:
                    await DisplayAlert("Alert", "Sucessful create Package. Please print label before initial departure scan", "Ok");
                    break;
                case BlockchainClient.Result.NETWORK:
                    await DisplayAlert("Alert", "Error: Network down. Please try again.", "Ok");
                    break;
            }

            await Navigation.PopAsync();
        }

        async void Back(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}