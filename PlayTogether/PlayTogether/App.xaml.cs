using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlayTogether
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new Login.LoginPage();
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
