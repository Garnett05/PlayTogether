using Autofac;
using PlayTogether.CreateGroup;
using PlayTogether.Game;
using PlayTogether.Group;
using PlayTogether.Home;
using PlayTogether.TabbedHome;
using PlayTogether.CreateUser;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using PlayTogether.GroupChat;
using Rg.Plugins.Popup.Services;
using Rg.Plugins.Popup.Pages;

namespace PlayTogether.Services.Navigation
{
    public interface INavigationService
    {
        Task PushAsync<TViewModel>(object parameter = null) where TViewModel : BaseViewModel;
        Task PopAsync();
    }

    class NavigationService : INavigationService
    {
        private Func<INavigation> _navigation;
        private IComponentContext _container;
        private readonly Dictionary<Type, Type> _pageMap = new Dictionary<Type, Type>
        {
            // TODO: URL mapping goes here
              { typeof(HomeViewModel), typeof(HomePage) },
            { typeof(GameViewModel), typeof(GamePage)},
            { typeof(CreateGroupViewModel), typeof(CreateGroupPage)},
            { typeof(GroupViewModel), typeof(GroupPage)},
            { typeof(TabbedHomeViewModel), typeof(TabbedHomePage)},
            {typeof(CreateUserViewModel), typeof(CreateUserPage) },
            {typeof(GroupChatViewModel), typeof(GroupChatPage) },
            {typeof(GroupPlayerInfoViewModel), typeof(GroupPlayerInfoPage) }
        };

        public NavigationService(Func<INavigation> navigation, IComponentContext container)
        {
            _navigation = navigation;
            _container = container;
        }

        public async Task PopAsync()
        {
            await _navigation().PopAsync();
        }

        public async Task PushAsync<TViewModel>(object parameter = null) where TViewModel : BaseViewModel
        {
            var pageType = _pageMap[typeof(TViewModel)];
            Page page = _container.Resolve(pageType) as Page;
            await _navigation().PushAsync(page);
            await (page.BindingContext as BaseViewModel).InitializeAsync(parameter);
        }
    }
}
