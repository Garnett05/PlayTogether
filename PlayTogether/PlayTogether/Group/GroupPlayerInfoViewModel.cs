using PlayTogether.Models;
using PlayTogether.Services.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PlayTogether.Group
{
    public class GroupPlayerInfoViewModel : BaseViewModel
    {
        private Users _user;
        public Users User
        {
            get { return _user; }
            set
            {
                _user = value;
                OnPropertyChanged("User");
            }
        }
        public async override Task InitializeAsync(object parameter)
        {            
            User = (Users)parameter;
        }
        public GroupPlayerInfoViewModel()
        {
            
        }
    }
}
