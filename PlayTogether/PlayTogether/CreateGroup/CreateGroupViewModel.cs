using Newtonsoft.Json;
using PlayTogether.Group;
using PlayTogether.Models;
using PlayTogether.Network;
using PlayTogether.Services.DialogMessage;
using PlayTogether.Services.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using static PlayTogether.App;

namespace PlayTogether.CreateGroup
{
    public class CreateGroupViewModel : BaseViewModel
    {        
        private INavigationService _navigation;
        private INetworkService _networkService;
        private IDialogMessage _dialogMessage;
        private double _sliderValue;
        private string _groupName;
        public double MaximumSliderValue { get; set; }
        public double MinimumSliderValue { get; set; }
        public double SliderValue
        {
            get { return _sliderValue; }
            set
            {
                _sliderValue = Math.Round(value);
                OnPropertyChanged("SliderValue");

            }
        }
        public string GroupName
        {
            get { return _groupName; }
            set
            {
                _groupName = value;
                OnPropertyChanged("GroupName");
            }
        }

        public ICommand PreviousPageCommand { get => new Command(async () => await PreviousPage()); }
        public ICommand CreateGroupCommand { get => new Command(async () => await CreateGroup()); }        
        private Games _game;

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
            SliderValue = 2;            
        }
        public CreateGroupViewModel(INavigationService navigation, INetworkService networkService, IDialogMessage dialogMessage)
        {
            _navigation = navigation;
            _networkService = networkService;
            _dialogMessage = dialogMessage;
        }
        private async Task PreviousPage()
        {
            await _navigation.PopAsync();
        }
        private async Task CreateGroup()
        {
            try
            {
                var numberOfGroups = await _networkService.GetAsync<List<Groups>>(Constants.GetAllGroups());
                if (GroupName == null || GroupName.Trim().Length == 0)
                {
                    await _dialogMessage.DisplayAlert("Aviso", "O nome do grupo obrigatoriamente precisa ser preenchido.", "Ok");
                }
                else
                {
                    Groups group = new Groups()
                    {
                        id = numberOfGroups.Count + 1,
                        name = GroupName,
                        image_url = "https://images-na.ssl-images-amazon.com/images/I/61f1f0WHh9L._SY400_.png",
                        numberPlayer = SliderValue.ToString(),
                        idGame = Game.id.ToString()
                    };

                    string json = JsonConvert.SerializeObject(group);
                    var result = await _networkService.PostAsync<Groups>(Constants.GetAllGroups(), json);
                    if (result != null)
                    {
                        await _navigation.PushAsync<GroupViewModel>(group);
                    }
                }
            }
            catch (Exception e)
            {
                await _dialogMessage.DisplayAlert(e.GetType().Name, e.Message, "Ok");
            }
        }
        /*private async Task DefineMaxAndMin()
        {
            var result = await _networkService.GetAsync<List<Games>>(Constants.GetAllGames());
            Games gm = result.Where(x => x.id == Globais.idGame).FirstOrDefault();
            MinAndMax.max = double.Parse(gm.maxPlayers);
            MinAndMax.min = double.Parse(gm.minPlayers);
        }*/        
    }
}
