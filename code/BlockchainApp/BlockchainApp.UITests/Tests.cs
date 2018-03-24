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
            app.Tap("clicky");
            app.WaitForElement("message");

            var message = app.Query("message").First().Text;

            Assert.AreEqual(message, "Login Failed");
        }
    }
}

