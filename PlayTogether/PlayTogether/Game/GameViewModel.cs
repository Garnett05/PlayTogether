using PlayTogether.CreateGroup;
using PlayTogether.Group;
using PlayTogether.Models;
using PlayTogether.Network;
using PlayTogether.Services.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using static PlayTogether.App;

namespace PlayTogether.Game
{
    public class GameViewModel : BaseViewModel
    {
        private INetworkService _networkService;
        private INavigationService _navigation;
        private Games _game;
        private Groups _selectedGroup;
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
        public Groups SelectedGroup
        {
            get { return _selectedGroup; }
            set
            {
                _selectedGroup = value;
                OnPropertyChanged("SelectedGroup");
            }
        }
        public ICommand PreviousPageCommand { get => new Command(async () => await PreviousPage()); }
        public ICommand CreateGroupCommand { get => new Command(async () => await CreateGroup()); }
        public ICommand EnterGroupCommand { get => new Command(async () => await EnterGroup()); }

        public async override Task InitializeAsync(object parameter)
        {
            Game = (Games)parameter;
            if (Game != null)
            {
                await GetGamesData();
            }
        }
        public GameViewModel(INetworkService networkService, INavigationService navigation)
        {
            _networkService = networkService;
            _navigation = navigation;
        }
        private async Task GetGamesData()
        {
            var result = await _networkService.GetAsync<List<Groups>>(Constants.GetAllGroups());
            GroupsByGame = new ObservableCollection<Groups>();
            foreach (Groups x in result)
            {
                if (int.Parse(x.idGame) == Game.id)
                {
                    GroupsByGame.Add(x);
                }
            }
            //GroupsByGame = new ObservableCollection<Groups>(result);
            //GameList = new ObservableCollection<Games>(result);
        }
        private async Task PreviousPage()
        {
            await _navigation.PopAsync();
        }
        private async Task CreateGroup()
        {
            if (Game == null)
            {
                return;
            }
            Globais.idGame = Game.id;
            await _navigation.PushAsync<CreateGroupViewModel>(Game);            
        }
        private async Task EnterGroup()
        {
            if (SelectedGroup == null)
            {
                return;
            }
            await _navigation.PushAsync<GroupViewModel>(SelectedGroup);
            SelectedGroup = null;
        }

    }
}
