using Autofac;
using Newtonsoft.Json;
using PlayTogether.Models;
using PlayTogether.Network;
using PlayTogether.Services.DialogMessage;
using PlayTogether.Services.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Reflection;
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
        private ObservableCollection<IconsUser> _icons;
        private IconsUser _selectedIcon;
        private string _name;
        private string _email;
        private string _nickname;
        private string _password;
        private string _passwordConfirm;
        private int _age;
        public IconsUser SelectedIcon
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
        public ObservableCollection<IconsUser> Icons
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
            try
            {
                Icons = await _networkService.GetAsync<ObservableCollection<IconsUser>>(Constants.GetUsersIcons());
            }
            catch(Exception e)
            {
                await _dialogMessage.DisplayAlert("Erro", e.Message, "Ok");
            }
        }        
        public async Task CreateUser()
        {            
            try
            {
                //var result = await _networkService.GetAsync<List<Users>>(Constants.GetAllUsers());//Quando estiver usando a API,
                //Users maxIdUser = new Users();                                                    //remover este trecho 
                //maxIdUser = result.OrderByDescending(x => x.id).FirstOrDefault();                 //pois a API usará o autoIncrement da tabela                                
                Users newUser = new Users()
                {                    
                    name = Name,
                    age = Age,
                    nickname = Nickname,
                    email = Email,
                    psw = Password, 
                    imageUrl = SelectedIcon.ImageUrl
                };
                bool emailIsValid = EmailValidation();
                //bool ageIsValid = AgeValidation();
                bool equalPassword = PasswordValidation();
                bool nameIsValid = NameValidation();
                if (equalPassword == true && emailIsValid == true && nameIsValid == true)
                {
                    string json = JsonConvert.SerializeObject(newUser);
                    var result = await _networkService.PostAsync<Users>(Constants.GetAllUsers(), json);
                    Globais.userId = result.id;
                    Login();
                }
                else if (nameIsValid == false)
                {
                    await _dialogMessage.DisplayAlert("Atenção", "O nome e nickname precisam obrigatoriamente ser preenchidos", "Ok");
                }
                else if (emailIsValid == false)
                {
                    await _dialogMessage.DisplayAlert("Atenção", "O e-mail informado não é válido. Por favor informe um endereço de e-mail válido", "Ok");
                }
                else if (equalPassword == false)
                {
                    await _dialogMessage.DisplayAlert("Atenção", "As senhas informadas são diferentes. Por favor preencha a mesma senha nos dois campos", "Ok");
                }                
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
        public bool EmailValidation()
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
        public bool NameValidation()
        {
            if (Name.Trim().Length > 0 && Nickname.Trim().Length > 0)
            {
                return true;
            }
            return false;
        }
    }
}
