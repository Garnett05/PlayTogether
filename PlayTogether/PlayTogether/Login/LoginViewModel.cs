using PlayTogether.Home;
using PlayTogether.Services.Navigation;
using System.Windows.Input;
using Xamarin.Forms;

namespace PlayTogether.Login
{
    public class LoginViewModel : BaseViewModel
    {
        private INavigationService _navigation;
        public ICommand GoHomePage => new Command(PushHome);

        public LoginViewModel (INavigationService navigation)
        {
            _navigation = navigation;
        }

        private async void PushHome()
        {
            await _navigation.PushAsync<HomeViewModel>();
        }
    }
}