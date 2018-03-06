using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BlockChain
{
	public partial class UserPage : ContentPage
	{
        public class AssetView
        {
            public string title { get; set; }
            public string subtitle { get; set; }
        }

		public UserPage ()
		{
			InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this, false);
            populateAssetList();
        }

        void populateAssetList()
        {
            var assets = new List<AssetView>
          {
             new AssetView { title = "First Asset", subtitle ="This is a nice description"},
             new AssetView { title = "Second Asset", subtitle ="This is a nice description"},
             new AssetView { title = "Third Asset", subtitle ="This is a nice description"},
             new AssetView { title = "Fourth Asset", subtitle ="This is a nice description"},
             new AssetView { title = "Fifth Asset", subtitle ="This is a nice description"},
             new AssetView { title = "Sixth Asset", subtitle ="This is a nice description"},
             new AssetView { title = "Seventh Asset", subtitle ="This is a nice description"},
             new AssetView { title = "Eighth Asset", subtitle ="This is a nice description"},
          };

            current_asset_list.ItemsSource = assets;
        }
	}
}