using Autofac;
using PlayTogether.Services.Navigation;
using System;
using System.Reflection;
using Xamarin.Forms;

namespace PlayTogether
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //class used for build the registration
            var builder = new ContainerBuilder();
            //scan and register all classes in the assembly
            var dataAccess = Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyTypes(dataAccess)
                .AsImplementedInterfaces()
                .AsSelf();
            //get container
            NavigationPage navigationPage = null;
            Func<INavigation> navigationFunc = () =>
            {
                return navigationPage.Navigation;
            };

            builder.RegisterType<NavigationService>().As<INavigationService>()
                .WithParameter("navigation", navigationFunc);

            var container = builder.Build();
            navigationPage = new NavigationPage(container.Resolve<Login.LoginPage>());
            MainPage = navigationPage;
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
