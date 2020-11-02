using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlayTogether.GroupChat
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GroupChatPage : ContentPage
    {
        public GroupChatPage(GroupChatViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}