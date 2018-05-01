using BlockchainAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace BlockchainApp
{
	public partial class App : Application
	{
		public App ()
		{
            InitializeComponent();
            MainPage = new NavigationPage(new BlockchainApp.MainPage(new HyperledgerService())) {
                BarBackgroundColor = Color.FromRgb(5, 5, 5)
            };
		}

		protected override void OnStart () {}

		protected override void OnSleep () {}

		protected override void OnResume () {}
	}
}
