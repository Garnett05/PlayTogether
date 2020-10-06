using PlayTogether.Services.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlayTogether.CreateGroup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateGroupPage : ContentPage
    {        
        public CreateGroupPage(CreateGroupViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;            
        }        
    }
}