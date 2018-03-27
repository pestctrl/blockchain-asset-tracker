using BlockchainAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace BlockchainApp
{
    //delete comment
	public partial class App : Application
	{
		public App ()
		{
            //InitializeComponent();
            MainPage = new NavigationPage(new BlockchainApp.MainPage(new HyperledgerService())) {
                BarBackgroundColor = Color.FromRgb(5, 5, 5)
            };
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
