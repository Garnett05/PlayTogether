using PlayTogether.Models;
using Rg.Plugins.Popup.Pages;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlayTogether.Group
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GroupPlayerInfoPage : PopupPage
    {
        public GroupPlayerInfoPage(Users user)
        {
            InitializeComponent();
            BindingContext = user;
        }
    }
}