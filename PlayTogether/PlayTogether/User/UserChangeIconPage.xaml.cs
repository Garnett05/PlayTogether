using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlayTogether.User
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserChangeIconPage : ContentPage
    {
        public UserChangeIconPage(UserChangeIconViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}