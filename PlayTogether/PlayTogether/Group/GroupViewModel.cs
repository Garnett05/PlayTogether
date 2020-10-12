using PlayTogether.Models;
using PlayTogether.Network;
using PlayTogether.Services.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Linq;
using Xamarin.Forms;
using System;
using static PlayTogether.App;
using Newtonsoft.Json;
using PlayTogether.Services.DialogMessage;

namespace PlayTogether.Group
{
    public class GroupViewModel : BaseViewModel
    {
        private Groups _group;
        private INavigationService _navigation;
        private INetworkService _networkService;
        private IDialogMessage _dialogMessage;
        private string _groupParticipants;
        private ObservableCollection<Users> _groupUsers;
        public bool userOnCurrentGroup { get; set; }
        public ObservableCollection<Users> GroupUsers
        {
            get { return _groupUsers; }
            set
            {
                _groupUsers = value;
                OnPropertyChanged("GroupUsers");
            }
        }
        public ObservableCollection<GroupsxUsers> GroupxUsers;
        public Groups Group
        {
            get { return _group; }
            set
            {
                _group = value;
                OnPropertyChanged("Group");
            }
        }
        public string GroupParticipants
        {
            get { return _groupParticipants; }
            set
            {
                _groupParticipants = value;
                OnPropertyChanged("GroupParticipants");
            }
        }

        public ICommand PreviousPageCommand { get => new Command(async () => await PreviousPage()); }
        public ICommand JoinGroupCommand { get => new Command(async () => await JoinGroup()); }
                

        public async override Task InitializeAsync(object parameter)
        {
            Group = (Groups)parameter;
            await LoadUsersFromThisGroup();
            GroupParticipants = $"Integrantes - {GroupxUsers.Count}/{Group.numberPlayer}";
        }

        public GroupViewModel (INavigationService navigation, INetworkService networkService, IDialogMessage dialogMessage)
        {
            _navigation = navigation;
            _networkService = networkService;
            _dialogMessage = dialogMessage;
        }

        private async Task PreviousPage()
        {
            await _navigation.PopAsync();
        }
        private async Task LoadUsersFromThisGroup()
        {
            var result = await _networkService.GetAsync<List<GroupsxUsers>>(Constants.GetAllGroupsxUsers());
            GroupxUsers = new ObservableCollection<GroupsxUsers>();
            foreach (GroupsxUsers g in result)
            {
                if (int.Parse(g.id_group) == Group.id)
                {
                    GroupxUsers.Add(g);
                }
            }
            var result2 = await _networkService.GetAsync<List<Users>>(Constants.GetAllUsers());
            GroupUsers = new ObservableCollection<Users>();
            foreach (Users u in result2)
            {
                if (GroupxUsers.Any(x => int.Parse(x.id_user) == u.id)) {
                    GroupUsers.Add(u);
                }
            }
        }
        private async Task JoinGroup()
        {
            var result3 = await _networkService.GetAsync<List<Users>>(Constants.GetAllUsers());
            Users user = new Users();
            user = result3.Where(x => x.id == Globais.userId).FirstOrDefault();
            if (GroupUsers.Any(x => x.id == user.id))
            {
                await _dialogMessage.DisplayAlert("Aviso", "Você já está nesse grupo!", "Ok");                
            }
            else if (int.Parse(Group.numberPlayer) == GroupxUsers.Count())
            {
                await _dialogMessage.DisplayAlert("Aviso", "Este grupo já está cheio. Aguarde que ele fique vazio, ou procure um outro grupo deste mesmo jogo.", "Ok");
            }
            else
            {
                try
                {
                    var result = await _networkService.GetAsync<List<GroupsxUsers>>(Constants.GetAllGroupsxUsers()); //Quando estiver usando a API, remover este trecho e o de baixo que está informando o id do registro, pois a API usará o autoIncrement da tabela                
                    GroupsxUsers userxGroup = new GroupsxUsers() { id = result.Count + 1, id_user = Globais.userId.ToString(), id_group = Group.id.ToString() };
                    string json = JsonConvert.SerializeObject(userxGroup);
                    var result2 = await _networkService.PostAsync<GroupsxUsers>(Constants.GetAllGroupsxUsers(), json);
                    GroupxUsers.Add(userxGroup);                    
                    GroupUsers.Add(user);
                    GroupParticipants = $"Integrantes - {GroupxUsers.Count}/{Group.numberPlayer}";
                }
                catch (Exception e)
                {
                    await _dialogMessage.DisplayAlert(e.GetType().Name, e.Message, "Ok");
                }
            }            
        }
    }
}
