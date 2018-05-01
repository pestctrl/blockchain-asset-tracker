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
            CreatePackage package = new CreatePackage();
            // May need to change this
            package.packageId = Guid.NewGuid().ToString();
            package.sender = client.thisTrader.traderId;
            package.recipient = recipient.Text;
            package.contents = SelectedDataList.Where(prop => prop.selected)
                                         .Select(prop => prop.data.PropertyId)
                                         .ToList();
            // Error checking needed
            using (UserDialogs.Instance.Loading("Creating"))
            {
                await client.CreatePackage(package);
            }
            await DisplayAlert("Success","Package created!","Confirm");

            await Navigation.PopAsync();
        }

        async void Back(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}