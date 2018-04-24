using Acr.UserDialogs;
using BlockchainAPI;
using BlockchainAPI.Models;
using BlockchainAPI.Transactions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BlockchainApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CreatePackagePage : ContentPage
	{
        ObservableCollection<Property> properties;
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

        void OnSelection(object sender, SelectedItemChangedEventArgs e)
        {
            int collectionPosition = SelectedDataList.IndexOf(sender as SelectedData<Property>);
            //DisplayAlert("ItemSelected", (sender as SelectedData<Property>).ToString(), "ok");
            //SelectedDataList[collectionPosition].selected = !SelectedDataList[collectionPosition].selected;
        }

        async void CreatePackage()
        {

            

            CreatePackage p = new CreatePackage();
            // May need to change this
            p.packageId = Guid.NewGuid().ToString();
            p.sender = client.thisTrader.traderId;
            // Add UI element for the below element
            p.recipient = recipient.Text;
            p.contents = SelectedDataList.Where(prop => prop.selected)
                                         .Select(prop => prop.data.PropertyId)
                                         .ToList();
            // Error checking needed
            using (UserDialogs.Instance.Loading("Creating"))
            {
                await client.CreatePackage(p);
            }
            await DisplayAlert("Success","Package created!","Confirm");

            await Navigation.PopAsync();
            /*
            List<Property> packagedList = new List<Property>();
            string test = "";
            //SelectedDataList.ToList().ForEach(data => test += data.data.ToString() + " " + data.selected.ToString() + ", ");
            //DisplayActionSheet("Package Test", "Cancel", null, test);
            SelectedDataList.ToList().ForEach(property =>
            {
                if (property.selected)
                    packagedList.Add(property.data);
            });
            packagedList.ForEach(listItem => test += listItem.PropertyId.ToString() + ", ");
            DisplayActionSheet("Package Test", "Cancel", null, test);*/
        }
	}
}