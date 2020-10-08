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

        public LoginViewModel (INavigationService navigation, INetworkService network, IDialogMessage dialogMessage)
        {
            _navigation = navigation;
            _networkService = network;
            _dialogMessage = dialogMessage;
        }

        private async void PushHome()
        {
            var result = await _networkService.GetAsync<List<Users>>(Constants.GetAllUsers());            
            User = result.Where(x => x.email == Email).FirstOrDefault();
            if (User == null)
            {
                await _navigation.PushAsync<TabbedHomeViewModel>();
                //await _dialogMessage.DisplayAlert("Não encontrado", "Não existe um usuário com este e-mail.", "Ok");
            }
            else if (User.password != Password)
            {
                await _dialogMessage.DisplayAlert("Senha incorreta", "A senha digitada está incorreta. Tente novamente.", "Ok");
            }
            else
            {
                Globais.userId = User.id;
                await _navigation.PushAsync<TabbedHomeViewModel>(User);                
                result.Clear();
            }            
        }
    }
}