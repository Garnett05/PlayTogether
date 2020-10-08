using PlayTogether.Models;
using PlayTogether.Network;
using PlayTogether.Services.Navigation;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static PlayTogether.App;

namespace PlayTogether.User
{
    public class UserViewModel : BaseViewModel
    {
        private Users _user;
        private INetworkService _networkService;
        public Users User
        {
            get { return _user; }
            set
            {
                _user = value;
                OnPropertyChanged("User");
            }
        }
        public UserViewModel(INetworkService networkService)
        {
            _networkService = networkService;
            GetUser();
        }
        public async Task GetUser()
        {
            if (User == null)
            {
                var result = await _networkService.GetAsync<List<Users>>(Constants.GetAllUsers());
                //User = result.Where(x => x.id == Globais.userId).FirstOrDefault();
                User = result.Where(x => x.id == 1).FirstOrDefault(); //Alteração para deixar o login mais rápido
            }
        }
    }
}
