using Autofac;
using Newtonsoft.Json;
using PlayTogether.Models;
using PlayTogether.Network;
using PlayTogether.Services.DialogMessage;
using PlayTogether.Services.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using static PlayTogether.App;

namespace PlayTogether.User
{
    public class UserChangeIconViewModel : BaseViewModel
    {
        private ObservableCollection<IconsUser> _icons;
        private IconsUser _selectedIcon;        
        private INetworkService _networkService;
        private IDialogMessage _dialogMessage;
        private INavigationService _navigation;
        public ObservableCollection<IconsUser> Icons
        {
            get { return _icons; }
            set
            {
                _icons = value;
                OnPropertyChanged("Icons");
            }
        }
        public IconsUser SelectedIcon
        {
            get { return _selectedIcon; }
            set
            {
                _selectedIcon = value;
                OnPropertyChanged("SelectedIcon");
            }
        }
        public async override Task InitializeAsync(object parameter)
        {
            
            await LoadIcons();
        }
        public ICommand PreviousPageCommand { get => new Command(async () => await PreviousPage()); }
        public ICommand UpdateUserIconCommand { get => new Command(async () => await UpdateUserIcon()); }       

        public UserChangeIconViewModel(INetworkService networkService, IDialogMessage dialogMessage, INavigationService navigation)
        {
            _networkService = networkService;
            _dialogMessage = dialogMessage;
            _navigation = navigation;
        }
        private async Task LoadIcons()
        {
            try
            {
                Icons = await _networkService.GetAsync<ObservableCollection<IconsUser>>(Constants.GetUsersIcons());
            }
            catch (Exception e)
            {
                await _dialogMessage.DisplayAlert("Erro", e.Message, "Ok");
            }
        }
        private async Task PreviousPage()
        {
            await _navigation.PopAsync();
        }
        private async Task UpdateUserIcon()
        {
            bool awnser = await _dialogMessage.DisplayAlertOptions("Atenção", "Você está prestes a alterar sua imagem. Tem certeza disso?", "Não", "Sim");
            if (awnser == true)
            {
                return;
            }
            else
            {
                try
                {
                    var result = await _networkService.GetAsync<List<Users>>(Constants.GetAllUsers());
                    Users user = new Users();
                    user = result.Where(x => x.id == Globais.userId).FirstOrDefault();
                    user.imageUrl = SelectedIcon.ImageUrl;
                    string json = JsonConvert.SerializeObject(user);
                    var result2 = await _networkService.PutAsync<Users>(Constants.GetUserById(Globais.userId), json);
                    await _dialogMessage.DisplayAlert("Alteração realizada", "Imagem alterada com sucesso", "Ok");
                    BackHomePage();
                }
                catch (NullReferenceException)
                {
                    await _dialogMessage.DisplayAlert("Erro", "Para trocar de a imagem do seu usuário você deve selecionar uma das imagens disponíveis", "Ok");
                }
                catch (Exception e)
                {
                    await _dialogMessage.DisplayAlert("Erro", e.Message, "Ok");
                }
            }
        }
        private void BackHomePage()
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
    }
}
