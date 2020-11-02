using Newtonsoft.Json;
using PlayTogether.Models;
using PlayTogether.Network;
using PlayTogether.Services.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using static PlayTogether.App;

namespace PlayTogether.GroupChat
{
    public class GroupChatViewModel : BaseViewModel
    {
        private INetworkService _networkService;
        private INavigationService _navigation;
        private ObservableCollection<Messages> _Messages;
        private Groups _group;
        private string _myMessage;        
        public ObservableCollection<Messages> Messages
        {
            get { return _Messages; }
            set
            {
                _Messages = value;
                OnPropertyChanged("Messages");
            }
        }
        public Groups Group
        {
            get { return _group; }
            set
            {
                _group = value;
                OnPropertyChanged("Group");
            }
        }
        public string MyMessage
        {
            get { return _myMessage; }
            set
            {
                _myMessage = value;
                OnPropertyChanged("MyMessage");
            }
        }

        public async override Task InitializeAsync(object parameter)
        {
            Group = (Groups)parameter;
            await LoadGroupMessages();
        }
        public ICommand SendMessageCommand { get => new Command(async () => await SendMessage()); }
        public ICommand PreviousPageCommand { get => new Command(async () => await PreviousPage()); }        

        public GroupChatViewModel(INetworkService networkService, INavigationService navigation)
        {
            _networkService = networkService;
            _navigation = navigation;
        }
        private async Task LoadGroupMessages()
        {
            var result = await _networkService.GetAsync<List<Messages>>(Constants.GetMessages());
            Messages = new ObservableCollection<Messages>(result.Where(x => x.id_group == Group.id));
        }
        private async Task SendMessage()
        {            
            if (MyMessage.Trim().Length > 0)
            {
                var messages = await _networkService.GetAsync<List<Messages>>(Constants.GetMessages());//Quando estiver usando a API,
                Messages message = new Messages();                                                     //remover este trecho 
                message = messages.OrderByDescending(x => x.id).FirstOrDefault();                      //pois a API usará o autoIncrement da tabela
                Messages msg = new Messages()
                {
                    id = message.id + 1,
                    id_group = Group.id,
                    id_user = Globais.userId,
                    message = MyMessage
                };
                string json = JsonConvert.SerializeObject(msg);
                await _networkService.PostAsync<Messages>(Constants.GetMessages(), json);
                Messages.Add(msg);
                MyMessage = string.Empty;
            }
        }
        private async Task PreviousPage()
        {
            await _navigation.PopAsync();
        }
    }
}
