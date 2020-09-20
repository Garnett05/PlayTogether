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

namespace PlayTogether.Home
{
    public class HomeViewModel : BaseViewModel
    {
        private INetworkService _networkService;
        private INavigationService _navigation;
        private IDialogMessage _dialogMessage;

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
        public string SearchTerm { get; set; }
        //TODO: Criar os métodos de pesquisa na lista public ICommand PerformSearchCommand { get => new Command(async () => await PerformSearch()); }        
        public HomeViewModel(INetworkService networkService, INavigationService navigation, DialogMessage dialogMessage)
        {
            _networkService = networkService;
            _navigation = navigation;
            _dialogMessage = dialogMessage;
            GetGamesData();
        }
        private async Task GetGamesData()
        {
            var result = await _networkService.GetAsync<List<Games>>(Constants.GetAllGames());
            GameList = new ObservableCollection<Games>(result);
        }
        //TODO: Criar os métodos de pesquisa na lista
        /*private async Task PerformSearch()
        {
            GameList.Select(x => x.name == SearchTerm);
        }*/

    }
}