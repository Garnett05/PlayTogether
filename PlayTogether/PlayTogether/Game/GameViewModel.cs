using PlayTogether.CreateGroup;
using PlayTogether.Group;
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

namespace PlayTogether.Game
{
    public class GameViewModel : BaseViewModel
    {
        private INetworkService _networkService;
        private INavigationService _navigation;
        private IDialogMessage _dialogMessage;
        private bool _isRefreshing;
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
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                _isRefreshing = value;
                OnPropertyChanged("IsRefreshing");
            }
        }

        public ICommand PreviousPageCommand { get => new Command(async () => await PreviousPage()); }
        public ICommand CreateGroupCommand { get => new Command(async () => await CreateGroup()); }
        public ICommand EnterGroupCommand { get => new Command(async () => await EnterGroup()); }
        public ICommand RefreshCommand { get => new Command(async () => await GetGamesData()); }

        public async override Task InitializeAsync(object parameter)
        {
            Game = (Games)parameter;
            if (Game != null)
            {
                await GetGamesData();
            }
        }
        public GameViewModel(INetworkService networkService, INavigationService navigation, IDialogMessage dialogMessage)
        {
            _networkService = networkService;
            _navigation = navigation;
            _dialogMessage = dialogMessage;
        }
        private async Task GetGamesData()
        {
            try
            {
                IsRefreshing = true;
                var result = await _networkService.GetAsync<List<Groups>>(Constants.GetAllGroups());
                GroupsByGame = new ObservableCollection<Groups>();
                foreach (Groups x in result)
                {
                    if (int.Parse(x.idGame) == Game.id)
                    {
                        GroupsByGame.Add(x);
                    }
                }
                IsRefreshing = false;
            }
            catch(Exception e)
            {
                await _dialogMessage.DisplayAlert("Erro", e.Message, "Ok");
                IsRefreshing = false;
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
