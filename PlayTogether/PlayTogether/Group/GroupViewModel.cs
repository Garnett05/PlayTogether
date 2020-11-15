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
using Autofac;
using System.Reflection;
using PlayTogether.GroupChat;
using Rg.Plugins.Popup.Services;

namespace PlayTogether.Group
{
    public class GroupViewModel : BaseViewModel
    {
        private Groups _group;
        private INavigationService _navigation;
        private INetworkService _networkService;
        private IDialogMessage _dialogMessage;
        private ObservableCollection<Users> _groupUsers;
        private Users _selectedUser;
        private string _groupParticipants;
        private bool _entrarButton;
        private bool _deleteButton;
        private bool _messagesButton;
        private bool _isRefreshing;
        public bool MessagesButton
        {
            get { return _messagesButton; }
            set
            {
                _messagesButton = value;
                OnPropertyChanged("MessagesButton");
            }
        }
        public bool DeleteButton
        {
            get { return _deleteButton; }
            set
            {
                _deleteButton = value;
                OnPropertyChanged("DeleteButton");
            }
        }
        public bool EntrarButton
        {
            get { return _entrarButton; }
            set
            {
                _entrarButton = value;
                OnPropertyChanged("EntrarButton");
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
        public Users SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
                OnPropertyChanged("SelectedUser");
            }
        }

        public ICommand PreviousPageCommand { get => new Command(async () => await PreviousPage()); }
        public ICommand JoinGroupCommand { get => new Command(async () => await JoinGroup()); }
        public ICommand DeleteGroupCommand { get => new Command(async () => await DeleteGroup()); }
        public ICommand NavigateChatPageCommand { get => new Command(async () => await NavigateChatPage()); }
        public ICommand ShowPlayerInfoCommand { get => new Command(async () => await NavigatePlayerInfoPage()); }
        public ICommand RefreshCommand { get => new Command(async () => await LoadUsersFromThisGroup()); }        

        public async override Task InitializeAsync(object parameter)
        {
            Group = (Groups)parameter;
            await LoadUsersFromThisGroup();
            GroupParticipants = $"Integrantes - {GroupxUsers.Count}/{Group.numberPlayers}";
        }

        public GroupViewModel(INavigationService navigation, INetworkService networkService, IDialogMessage dialogMessage)
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
            try
            {
                IsRefreshing = true;
                var result = await _networkService.GetAsync<List<GroupsxUsers>>(Constants.GetAllGroupsxUsers());
                GroupxUsers = new ObservableCollection<GroupsxUsers>();
                foreach (GroupsxUsers g in result)
                {
                    if (g.idGroup == Group.id)
                    {
                        GroupxUsers.Add(g);
                    }
                }
                var result2 = await _networkService.GetAsync<List<Users>>(Constants.GetAllUsers());
                GroupUsers = new ObservableCollection<Users>();
                foreach (Users u in result2)
                {
                    if (GroupxUsers.Any(x => x.idUser == u.id))
                    {
                        GroupUsers.Add(u);
                    }
                }

                EntrarButton = true;
                DeleteButton = false;
                MessagesButton = false;
                Users userAux = new Users();
                userAux = GroupUsers.Where(x => x.id == Globais.userId).FirstOrDefault();
                if (userAux != null && userAux.id == Globais.userId)
                {
                    EntrarButton = false;
                    MessagesButton = true;
                }
                GroupParticipants = $"Integrantes - {GroupxUsers.Count}/{Group.numberPlayers}";
                IsRefreshing = false;

                if (int.Parse(Group.idUserGroupLeader) == Globais.userId)
                {
                    DeleteButton = true;
                }
            }
            catch (Exception e)
            {
                await _dialogMessage.DisplayAlert("Erro", e.Message, "Ok");
            }

        }
        private async Task JoinGroup()
        {
            try
            {
                var result3 = await _networkService.GetAsync<List<Users>>(Constants.GetAllUsers());
                Users user = new Users();
                user = result3.Where(x => x.id == Globais.userId).FirstOrDefault();
                if (GroupUsers.Any(x => x.id == user.id))
                {
                    await _dialogMessage.DisplayAlert("Aviso", "Você já está nesse grupo!", "Ok");
                }
                else if (int.Parse(Group.numberPlayers) == GroupxUsers.Count())
                {
                    await _dialogMessage.DisplayAlert("Aviso", "Este grupo já está cheio. Aguarde que ele fique vazio, ou procure um outro grupo deste mesmo jogo.", "Ok");
                }
                else
                {
                    GroupsxUsers userxGroup = new GroupsxUsers() { idUser = Globais.userId, idGroup = Group.id };
                    string json = JsonConvert.SerializeObject(userxGroup);
                    var result2 = await _networkService.PostAsync<GroupsxUsers>(Constants.GetAllGroupsxUsers(), json);
                    GroupxUsers.Add(userxGroup);
                    GroupUsers.Add(user);
                    GroupParticipants = $"Integrantes - {GroupxUsers.Count}/{Group.numberPlayers}";
                    EntrarButton = false;
                    MessagesButton = true;
                }
            }
            catch (Exception e)
            {
                await _dialogMessage.DisplayAlert("Erro", e.Message, "Ok");
            }
        }
        private async Task DeleteGroup()
        {
            try
            {
                var result = await _networkService.GetAsync<List<GroupsxUsers>>(Constants.GetAllGroupsxUsers());
                ObservableCollection<GroupsxUsers> result2 = new ObservableCollection<GroupsxUsers>(result.Where(x => x.idGroup == Group.id));
                result.Clear();

                foreach (GroupsxUsers gu in result2)
                {
                    await _networkService.DeleteAsync(Constants.DeleteGroupxUser(gu.id));
                }

                bool s = await _dialogMessage.DisplayAlertOptions("Atenção", "Você está prestes a excluir o grupo. Deseja prosseguir com essa ação?", "Não", "Sim");
                if (s == false)
                {
                    await _networkService.DeleteAsync(Constants.DeleteGroup(Group.id));
                    await _dialogMessage.DisplayAlert("Ação realizada", "O grupo foi excluído.", "Ok");
                    GoBackHomePage();
                }
            }
            catch(Exception e)
            {
                await _dialogMessage.DisplayAlert("Erro", e.Message, "Ok");
            }
        }
        private void GoBackHomePage()
        {
            var builder = new ContainerBuilder();
            var dataAccess = Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyTypes(dataAccess)
                .AsImplementedInterfaces()
                .AsSelf();
            NavigationPage navigationPage = null;
            Func<INavigation> navigationFunc = () =>
            {
                return navigationPage.Navigation;
            };
            builder.RegisterType<NavigationService>().As<INavigationService>()
                .WithParameter("navigation", navigationFunc);
            var container = builder.Build();
            navigationPage = new NavigationPage(container.Resolve<TabbedHome.TabbedHomePage>());
            Application.Current.MainPage = navigationPage;
        }
        private async Task NavigateChatPage()
        {
            await _navigation.PushAsync<GroupChatViewModel>(Group);
        }
        private async Task NavigatePlayerInfoPage()
        {
            if (SelectedUser == null)
            {
                return;
            }
            try
            {
                int idSelectedUser = SelectedUser.id;
                var result = await _networkService.GetAsync<List<Users>>(Constants.GetAllUsers());
                Users user = new Users();
                user = result.Where(x => x.id == idSelectedUser).FirstOrDefault();
                //await _navigation.PushAsync<GroupPlayerInfoViewModel>(user);
                //var viewModel = new GroupPlayerInfoViewModel(user);
                var popupPage = new GroupPlayerInfoPage(user);
                await PopupNavigation.Instance.PushAsync(popupPage);
                SelectedUser = null;
            }
            catch(Exception e)
            {
                await _dialogMessage.DisplayAlert("Erro", e.Message, "Ok");
            }
        }

    }
}
