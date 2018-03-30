using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace BlockchainApp.UITests
{
    [TestFixture(Platform.Android)]
    //[TestFixture(Platform.iOS)]
    public class Tests
    {
        IApp app;
        Platform platform;

        public Tests(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            //app = AppInitializer.StartApp(platform);
            app = ConfigureApp
                .Android
                .InstalledApp("com.companyname.BlockchainApp")
                .PreferIdeSettings()
                .EnableLocalScreenshots()
                .StartApp();
        }

        [Test]
        public void CanaryTest()
        {
            Assert.IsTrue(true);
        }

        [Test]
        public void Logging_in_as_TRADER1_should_display_name_in_next_screen()
        {
            app.EnterText("login", "TRADER1");
            app.DismissKeyboard();
            app.Tap("clicky");
            app.WaitForElement("button2");
            app.Tap("button2");
            app.WaitForElement("welcome");
            
            string welcomeMessage = app.Query("welcome").First().Text;

            Assert.AreEqual(welcomeMessage, "Hello, Alice Margatroid!");
        }

        [Test]
        public void Logging_in_as_trader_does_not_work()
        {
            app.EnterText("login", "trader");
            app.DismissKeyboard();
            app.Tap("clicky");
            app.WaitForElement("message");

            var message = app.Query("message").First().Text;

            Assert.AreEqual(message, "Login Failed");
        }

        [Test]
        public void ClickRegisterWithExistingUserAndGoBackToMainPage()
        {
            app.Tap("clicky2");
            app.EnterText(c => c.Id("NoResourceEntry-15"), "TRADER1");
            app.EnterText(c => c.Id("NoResourceEntry-16"), "tom");
            app.EnterText(c => c.Id("NoResourceEntry-17"), "Vuong");
            app.Tap(c => c.Id("NoResourceEntry-18"));
            app.Tap(c => c.Text("Ok"));
            app.Back();
        }

        [Test]
        public void LoginWithTrader1ClickSendFirstAssetGoBackLogout()
        {
            app.EnterText(c => c.Marked("login"), "TRADER1");
            app.DismissKeyboard();
            app.Tap("clicky");
            app.Tap("button2");
            app.Tap(c => c.Id("NoResourceEntry-30"));
            app.WaitForElement(c => c.Marked("Send"));
            app.Back();
            app.Tap(c => c.Text("Completed"));
            app.Tap(c => c.Text("Back"));
            app.Tap(c => c.Text("Logout"));
        }

        [Test]
        public void LoginWithTrader1SendFirstAssetToTrader2ScollDown()
        {
            app.EnterText("login", "TRADER1");
            app.DismissKeyboard();
            app.Tap("clicky");
            app.Tap("button2");
            app.Tap(c => c.Id("NoResourceEntry-24"));
            app.EnterText("NoResourceEntry-47", "TRADER2");
            app.DismissKeyboard();
            app.Tap(c => c.Text("Send"));
            app.Tap(c => c.Marked("Confirm"));
            app.Tap(c => c.Text("Logout"));
        }
    }
}

