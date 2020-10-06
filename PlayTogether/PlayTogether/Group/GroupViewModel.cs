using PlayTogether.Models;
using PlayTogether.Services.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PlayTogether.Group
{
    public class GroupViewModel : BaseViewModel
    {
        private Groups _group;
        private INavigationService _navigation;
        public Groups Group
        {
            get { return _group; }
            set
            {
                _group = value;
                OnPropertyChanged("Group");
            }
        }
        public ICommand PreviousPageCommand { get => new Command(async () => await PreviousPage()); }
        public async override Task InitializeAsync(object parameter)
        {
            Group = (Groups)parameter;
        }

        public GroupViewModel (INavigationService navigation)
        {
            _navigation = navigation;
        }

        private async Task PreviousPage()
        {
            await _navigation.PopAsync();
        }
    }
}
