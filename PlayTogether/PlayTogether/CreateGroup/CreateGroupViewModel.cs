using PlayTogether.Models;
using PlayTogether.Services.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PlayTogether.CreateGroup
{
    public class CreateGroupViewModel : BaseViewModel
    {
        private INavigationService _navigation;
        private double _sliderValue;
        public double SliderValue
        {
            get { return _sliderValue; }
            set
            {
                _sliderValue = Math.Round(value);
                OnPropertyChanged("SliderValue");

            }
        }
        public ICommand PreviousPageCommand { get => new Command(async () => await PreviousPage()); }
        //public ICommand ValueChangedCommand { get => new Command(async () => await ValueChanged()); }
        private Games _game;

        public Games Game
        {
            get { return _game; }
            set
            {
                _game = value;
                OnPropertyChanged("Game");
            }
        }
        public async override Task InitializeAsync(object parameter)
        {
            Game = (Games)parameter;
        }
        public CreateGroupViewModel(INavigationService navigation)
        {
            _navigation = navigation;
            SliderValue = 2;
        }
        private async Task PreviousPage()
        {
            await _navigation.PopAsync();
        }        
    }
}
