using PlayTogether.Home;
using PlayTogether.Models;
using PlayTogether.Network;
using PlayTogether.Services.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;
using Xamarin.Forms;
using PlayTogether.Services.DialogMessage;
using PlayTogether.TabbedHome;
using static PlayTogether.App;
using System;
using System.Net;
using System.Threading.Tasks;
using Autofac;
using System.Reflection;
using PlayTogether.CreateUser;

namespace PlayTogether.Login
{
    public class LoginViewModel : BaseViewModel
    {
        private INavigationService _navigation;
        private INetworkService _networkService;
        private IDialogMessage _dialogMessage;
        private string _email;
        private string _password;
        public ObservableCollection<Users> UsersCollection { get; set; }
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged("Email");
            }
        }
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged("Password");
            }
        }
        private Users User { get; set; }

        public ICommand GoHomePage => new Command(PushHome);
        public ICommand ForgotPasswordCommand { get => new Command(async () => await ForgotPassword()); }
        public ICommand CreateAccountCommand { get => new Command(async () => await CreateAccount()); }

        public LoginViewModel (INavigationService navigation, INetworkService network, IDialogMessage dialogMessage)
        {
            _navigation = navigation;
            _networkService = network;
            _dialogMessage = dialogMessage;
        }

        private async void PushHome()
        {            
            try
            {
                var result = await _networkService.GetAsync<List<Users>>(Constants.GetAllUsers());
                User = result.Where(x => x.email == Email).FirstOrDefault();
                if (User == null)
                {
                    //await _navigation.PushAsync<TabbedHomeViewModel>();
                    await _dialogMessage.DisplayAlert("Não encontrado", "Não existe um usuário com este e-mail.", "Ok");
                }
                else if (User.password != Password)
                {
                    await _dialogMessage.DisplayAlert("Senha incorreta", "A senha digitada está incorreta. Tente novamente.", "Ok");
                }                
                else
                {
                    Globais.userId = User.id;
                    result.Clear();
                    Login();
                }
            }
            catch(WebException)
            {
                await _dialogMessage.DisplayAlert("Erro", "Verifique sua conexão com a internet para prosseguir com o login.", "Ok");
            }
        }
        private async Task ForgotPassword()
        {
            await _dialogMessage.DisplayAlert("Aviso", "Este recurso está desabilitado no momento. Entre em contato com os desenvolvedores para que seu acesso seja ajustado.", "Ok");
        }
        private async Task CreateAccount()
        {            
            await _navigation.PushAsync<CreateUserViewModel>();
        }
        private void Login()
        {
            var builder = new ContainerBuilder();
            var dataAccess = Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyTypes(dataAccess)
                .AsImplementedInterfaces()
                .AsSelf();
            NavigationPage navigationPage = null;
            Func<INavigation> navigationFunc = () =>
            {
                return navigationPage.Navigation;
            };
            builder.RegisterType<NavigationService>().As<INavigationService>()
                .WithParameter("navigation", navigationFunc);
            var container = builder.Build();
            navigationPage = new NavigationPage(container.Resolve<TabbedHome.TabbedHomePage>());            
            Application.Current.MainPage = navigationPage;
        }
    }
}