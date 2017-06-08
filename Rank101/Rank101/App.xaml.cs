using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using Microsoft.Azure.Mobile;
using Microsoft.Azure.Mobile.Analytics;
using Microsoft.Azure.Mobile.Crashes;

using Rank101.Pages;

namespace Rank101
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
            MobileCenter.LogLevel = LogLevel.Verbose;

            MobileCenter.Start("android=c53edf74-42e6-45b9-aea2-e35081876ab4;" +
                   "ios=9cffcd71-bb6b-4d89-8fec-9a1f1187c1cb",
                   typeof(Analytics), typeof(Crashes));
        }
    }
}
