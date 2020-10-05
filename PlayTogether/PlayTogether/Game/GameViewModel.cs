using PlayTogether.Models;
using PlayTogether.Network;
using PlayTogether.Services.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace PlayTogether.Game
{
    public class GameViewModel : BaseViewModel
    {
        private INetworkService _networkService;
        private Games _game;
        private ObservableCollection<Groups> _groupsByGame;
        public ObservableCollection<Groups> GroupsByGame
        {
            get { return _groupsByGame; }
            set
            {
                _groupsByGame = value;
                OnPropertyChanged("GroupsByGame");
            }
        }
        public Games Game
        {
            get { return _game; }
            set
            {
                _game = value;
                OnPropertyChanged("Game");
            }
        }
        public async override Task InitializeAsync(object parameter)
        {
            Game = (Games)parameter;
        }
        public GameViewModel(INetworkService networkService)
        {
            _networkService = networkService;
            GetGamesData();
        }
        private async Task GetGamesData()
        {
            var result = await _networkService.GetAsync<List<Groups>>(Constants.GetAllGroups());
            GroupsByGame = new ObservableCollection<Groups>(result);
            //GameList = new ObservableCollection<Games>(result);
        }
    }
}
