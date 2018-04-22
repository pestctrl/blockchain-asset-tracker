using BlockchainAPI;
using BlockchainAPI.Models;
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

        public CreatePackagePage (List<Property> props)
		{
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

        void CreatePackage()
        {
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
            DisplayActionSheet("Package Test", "Cancel", null, test);
        }
	}
}