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
		public CreatePackagePage (List<Property> props)
		{
            properties = new ObservableCollection<Property>();
            props.ForEach(prop => properties.Add(prop));
			InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this, false);
            property_list.ItemsSource = properties;
		}
	}
}