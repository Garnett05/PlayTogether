using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlayTogether.CreateUser
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateUserPage : ContentPage
    {
        public CreateUserPage(CreateUserViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}