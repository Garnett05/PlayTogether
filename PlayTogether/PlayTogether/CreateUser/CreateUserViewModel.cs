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

namespace PlayTogether.CreateUser
{
    public class CreateUserViewModel : BaseViewModel
    {
        private INetworkService _networkService;
        private INavigationService _navigation;
        private IDialogMessage _dialogMessage;
        private ObservableCollection<UsersIcons> _icons;
        private UsersIcons _selectedIcon;
        private string _name;
        private string _email;
        private string _nickname;
        private string _password;
        private string _passwordConfirm;
        private int _age;
        public UsersIcons SelectedIcon
        {
            get { return _selectedIcon; }
            set
            {
                _selectedIcon = value;
                OnPropertyChanged("SelectedIcon");
            }
        }
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged("Email");
            }
        }
        public string Nickname
        {
            get { return _nickname; }
            set
            {
                _nickname = value;
                OnPropertyChanged("Nickname");
            }
        }
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged("Password");
            }
        }
        public string PasswordConfirm
        {
            get { return _passwordConfirm; }
            set
            {
                _passwordConfirm = value;
                OnPropertyChanged("PasswordConfirm");
            }
        }
        public int Age
        {
            get { return _age; }
            set
            {
                _age = value;
                OnPropertyChanged("Age");
            }
            
        }
        public ObservableCollection<UsersIcons> Icons
        {
            get { return _icons; }
            set
            {
                _icons = value;
                OnPropertyChanged("Icons");
            }
        }
        public async override Task InitializeAsync(object parameter)
        {
            await LoadIcons();
        }
        public ICommand CreateUserCommand { get => new Command(async () => await CreateUser()); }
        
        public CreateUserViewModel(INetworkService networkService, INavigationService navigation, IDialogMessage dialogMessage)
        {
            _networkService = networkService;
            _navigation = navigation;
            _dialogMessage = dialogMessage;
        }
        private async Task LoadIcons()
        {
            Icons = await _networkService.GetAsync<ObservableCollection<UsersIcons>>(Constants.GetUsersIcons());
        }
        //TODO - Validações no método abaixo (e-mail, senha, idade...)
        public async Task CreateUser()
        {            
            try
            {
                var result = await _networkService.GetAsync<List<Users>>(Constants.GetAllUsers());//Quando estiver usando a API,
                Users maxIdUser = new Users();                                                    //remover este trecho 
                maxIdUser = result.OrderByDescending(x => x.id).FirstOrDefault();                 //pois a API usará o autoIncrement da tabela                                
                Users newUser = new Users()
                {
                    id = maxIdUser.id + 1,
                    name = Name,
                    age = Age,
                    nickname = Nickname,
                    email = Email,
                    password = Password,
                    user_image = SelectedIcon.image_url
                };
                string json = JsonConvert.SerializeObject(newUser);
                await _networkService.PostAsync<Users>(Constants.GetAllUsers(), json);
                Globais.userId = newUser.id;
                Login();
            }
            catch(Exception e)
            {
                await _dialogMessage.DisplayAlert(e.GetType().ToString(), e.Message, "Ok");
            }
            
        }
        private void Login()
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
        public bool PasswordValidation()
        {
            if (Password == PasswordConfirm)
            {
                return true;
            }
            return false;
        }
        private bool AgeValidation()
        {
            if (Age > 18)
            {
                return true;
            }
            return false;
        }
        private bool EmailValidation()
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(Email);
                return addr.Address == Email;
            }
            catch
            {
                return false;
            }
        }
    }
}
