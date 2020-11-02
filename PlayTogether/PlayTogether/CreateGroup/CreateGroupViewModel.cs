using Newtonsoft.Json;
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

namespace PlayTogether.CreateGroup
{
    public class CreateGroupViewModel : BaseViewModel
    {        
        private INavigationService _navigation;
        private INetworkService _networkService;
        private IDialogMessage _dialogMessage;
        private GamesIcons _selectedIcon;
        private ObservableCollection<GamesIcons> _icons;
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
        public GamesIcons SelectedIcon
        {
            get { return _selectedIcon; }
            set
            {
                _selectedIcon = value;
                OnPropertyChanged("SelectedIcon");
            }
        }
        public ObservableCollection<GamesIcons> Icons
        {
            get { return _icons; }
            set
            {
                _icons = value;
                OnPropertyChanged("Icons");
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
            await LoadIcons();
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
                if (GroupName == null || GroupName.Trim().Length == 0)
                {
                    await _dialogMessage.DisplayAlert("Aviso", "O nome do grupo obrigatoriamente precisa ser preenchido.", "Ok");
                }
                else if (SelectedIcon == null)
                {
                    await _dialogMessage.DisplayAlert("Aviso", "Selecione um ícone para prosseguir com a criação do grupo.", "Ok");
                }
                else
                {
                    var numberOfGroups = await _networkService.GetAsync<List<Groups>>(Constants.GetAllGroups());//Quando estiver usando a API,
                    Groups maxIdGroup = new Groups();                                                           //remover este trecho 
                    maxIdGroup = numberOfGroups.OrderByDescending(x => x.id).FirstOrDefault();                  //pois a API usará o autoIncrement da tabela
                    Groups group = new Groups()
                    {
                        id = maxIdGroup.id+1,
                        name = GroupName,
                        image_url = SelectedIcon.image_url,
                        numberPlayer = SliderValue.ToString(),
                        idGame = Game.id.ToString(),
                        idUserGroupLeader = Globais.userId.ToString()
                    };

                    string json = JsonConvert.SerializeObject(group);
                    var result = await _networkService.PostAsync<Groups>(Constants.GetAllGroups(), json);
                    var result2 = await _networkService.GetAsync<List<GroupsxUsers>>(Constants.GetAllGroupsxUsers()); //Quando estiver usando a API, 
                    GroupsxUsers maxIdGroupxUser = new GroupsxUsers();                                                //remover este trecho 
                    maxIdGroupxUser = result2.OrderByDescending(x => x.id).FirstOrDefault();                          //pois a API usará o autoIncrement da tabela
                    GroupsxUsers userxGroup = new GroupsxUsers() { id = maxIdGroupxUser.id + 1, id_user = Globais.userId.ToString(), id_group = group.id.ToString() }; //Controlando o Max Id do GroupsxUsers por aqui
                    string json2 = JsonConvert.SerializeObject(userxGroup);
                    var result3 = await _networkService.PostAsync<GroupsxUsers>(Constants.GetAllGroupsxUsers(), json2);
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
        private async Task LoadIcons()
        {
            var result = await _networkService.GetAsync<List<GamesIcons>>(Constants.GetIcons());
            Icons = new ObservableCollection<GamesIcons>(result.Where(x => int.Parse(x.id_game) == Game.id));
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
