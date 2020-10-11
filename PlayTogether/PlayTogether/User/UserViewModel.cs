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

namespace PlayTogether.User
{
    public class UserViewModel : BaseViewModel
    {

        private INetworkService _networkService;
        private IDialogMessage _dialogMessage;
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

        public UserViewModel(INetworkService networkService, IDialogMessage dialogMessage)
        {
            _networkService = networkService;
            _dialogMessage = dialogMessage;
            GetUser();
            GetGames();
        }
        public async Task GetUser()
        {
            if (User == null)
            {
                var result = await _networkService.GetAsync<List<Users>>(Constants.GetAllUsers());
                //User = result.Where(x => x.id == Globais.userId).FirstOrDefault();
                User = result.Where(x => x.id == 1).FirstOrDefault(); //Alteração para deixar o login mais rápido. O correto é como está na linha de cima
            }
        }
        public async Task GetGames()
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
        private async Task UpdateUserAvatar()
        {
            await _dialogMessage.DisplayAlert("Aviso", "Este recurso ainda não está disponível. Aguarde a próxima atualização para poder utilizá-lo.", "Ok");
        }
    }
}
