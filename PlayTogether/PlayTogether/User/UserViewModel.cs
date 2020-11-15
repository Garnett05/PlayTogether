using PlayTogether.Models;
using PlayTogether.Network;
using PlayTogether.Services.DialogMessage;
using PlayTogether.Services.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using static PlayTogether.App;

namespace PlayTogether.User
{
    public class UserViewModel : BaseViewModel
    {

        private INetworkService _networkService;
        private IDialogMessage _dialogMessage;
        private INavigationService _navigation;
        private Users _user;
        private Games _game;
        private ObservableCollection<Games> _gamesCollection;
        public Users User
        {
            get { return _user; }
            set
            {
                _user = value;
                OnPropertyChanged("User");
            }
        }
        public Games Game
        {
            get { return _game; }
            set
            {
                _game = value;
                OnPropertyChanged("Games");
            }
        }
        public ObservableCollection<Games> GamesCollection
        {
            get { return _gamesCollection; }
            set
            {
                _gamesCollection = value;
                OnPropertyChanged("GamesCollection");
            }
        }
        public ICommand UpdateUserAvatarCommand { get => new Command(async () => await UpdateUserAvatar()); }

        public UserViewModel(INetworkService networkService, IDialogMessage dialogMessage, INavigationService navigation)
        {
            _networkService = networkService;
            _dialogMessage = dialogMessage;
            _navigation = navigation;
            GetUser();
            GetGames();
        }
        public async Task GetUser()
        {
            try
            {
                var result = await _networkService.GetAsync<List<Users>>(Constants.GetAllUsers());                
                User = result.Where(x => x.id == Globais.userId).FirstOrDefault();                
            }
            catch(Exception e)
            {
                await _dialogMessage.DisplayAlert("Erro", e.Message, "Ok");
            }
        }
        public async Task GetGames()
        {
            try
            {
                var result = await _networkService.GetAsync<List<Games>>(Constants.GetAllGames());
                GamesCollection = new ObservableCollection<Games>();
                foreach (Games x in result)
                {
                    if (!GamesCollection.Contains(x) && GamesCollection.Count < 3)
                    {
                        GamesCollection.Add(x);
                    }
                }
            }
            catch(Exception e)
            {
                await _dialogMessage.DisplayAlert("Erro", e.Message, "Ok");
            }
        }
        private async Task UpdateUserAvatar()
        {
            //await _dialogMessage.DisplayAlert("Aviso", "Este recurso ainda não está disponível. Aguarde a próxima atualização para poder utilizá-lo.", "Ok");            
            await _navigation.PushAsync<UserChangeIconViewModel>(User);
        }
    }
}