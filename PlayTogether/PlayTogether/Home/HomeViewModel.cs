using PlayTogether.Models;
using PlayTogether.Network;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using PlayTogether.Services.Navigation;
using System.Collections.Generic;
using PlayTogether.Services.DialogMessage;
using System.Windows.Input;
using System.Linq;
using Xamarin.Forms;
using System;
using PlayTogether.Game;

namespace PlayTogether.Home
{
    public class HomeViewModel : BaseViewModel
    {
        private INetworkService _networkService;
        private INavigationService _navigation;
        //private IDialogMessage _dialogMessage; 
        private Games _selectedGame;
        private int _selectedGameId;
        private ObservableCollection<Games> GameData { get; set; }

        private ObservableCollection<Games> _gameList;
        public ObservableCollection<Games> GameList
        {
            get { return _gameList; }
            set
            {
                _gameList = value;
                OnPropertyChanged("GameList");
            }
        }
        public string _searchTerm;
        public string SearchTerm
        {
            get { return _searchTerm; }
            set
            {
                _searchTerm = value;
                OnPropertyChanged("SearchTerm");
                if (SearchTerm is null || SearchTerm.Length == 0)
                {
                    GameList.Clear();
                    GameList = GameData;
                }
            }
        }
        public Games SelectedGame
        {
            get { return _selectedGame; }
            set
            {
                _selectedGame = value;
                OnPropertyChanged("SelectedGame");
            }
        }
        public int SelectedGameId
        {
            get { return _selectedGameId; }
            set
            {
                _selectedGameId = value;
                OnPropertyChanged("SelectedGameId");
            }
        }

        public ICommand PerformSearchCommand { get => new Command(() => PerformSearch()); }
        public ICommand GameChangedCommand { get => new Command(async () => await GoToGameDetails()); }
        

        public HomeViewModel(INetworkService networkService, INavigationService navigation)
        {
            _networkService = networkService;
            _navigation = navigation;
            //_dialogMessage = dialogMessage;
            GetGamesData();
        }
        private async Task GetGamesData()
        {
            var result = await _networkService.GetAsync<List<Games>>(Constants.GetAllGames());
            GameData = new ObservableCollection<Games>(result);
            GameList = new ObservableCollection<Games>(result);
        }
        private void PerformSearch()
        {
            if (SearchTerm.Length > 0)
            {
                var v = GameData;
                GameList = new ObservableCollection<Games>(v.Where(x => x.name == SearchTerm));
            }
            else
            {
                GameList.Clear();
                GameList = GameData;
            }
        }
        private async Task GoToGameDetails()
        {
            if (SelectedGame == null)
            {
                return;
            }
            await _navigation.PushAsync<GameViewModel>(SelectedGame);
            SelectedGame = null;
        }
    }
}