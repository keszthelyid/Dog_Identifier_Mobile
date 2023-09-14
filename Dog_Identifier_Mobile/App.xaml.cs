using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Dog_Identifier_Mobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Scan History"));

            MainPage = new NavigationPage(new MainPage())
            {
                BarBackgroundColor = Color.Black,
                BarTextColor = Color.White
            };
        }

        protected override void OnStart()
        {

        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

    }
}
