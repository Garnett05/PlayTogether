using PlayTogether.Models;
using PlayTogether.Network;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;
using System;
using PlayTogether.Services.Navigation;

namespace PlayTogether.Home
{
    public class HomeViewModel : BaseViewModel
    {
        private INetworkService _networkService;
        private INavigationService _navigation;
        
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

        public HomeViewModel(INetworkService networkService, INavigationService navigation)
        {
            _networkService = networkService;
            _navigation = navigation;
            GetGamesData();
        }
        private async Task GetGamesData()
        {
            try
            {
                var result = await _networkService.GetAsync<Games>(Constants.GetAllGames());
                GameList = new ObservableCollection<Games>();
                GameList.Add(result); //resultado obtido da informação da API local
                //Mokando valores para exibição
                GameList.Add(new Games { id = "2", name = "CS:GO" });
                GameList.Add(new Games { id = "3", name = "Apex Legends" });
                GameList.Add(new Games { id = "4", name = "Candy Crush" });
            }
            catch
            {
            }
        }
    }
}
