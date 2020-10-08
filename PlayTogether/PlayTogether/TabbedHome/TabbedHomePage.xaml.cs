using PlayTogether.Home;
using PlayTogether.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlayTogether.TabbedHome
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabbedHomePage : TabbedPage
    {
        public TabbedHomePage(HomePage homeView, TabbedHomeViewModel viewModel, UserPage userView)
        {
            InitializeComponent();
            BindingContext = viewModel;            
            Children.Add(homeView); { BarTextColor = Color.White; }
            Children.Add(userView); { BarTextColor = Color.White; }
        }
    }
}