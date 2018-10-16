using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MonkeyCache.SQLite;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace ExemploMonkeyCache
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            Barrel.ApplicationId = "NewsfeedAppId";

            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
