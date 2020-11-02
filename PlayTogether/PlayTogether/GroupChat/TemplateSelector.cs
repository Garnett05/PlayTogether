using PlayTogether.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using static PlayTogether.App;

namespace PlayTogether.GroupChat
{
    class ChatTemplateSelector : DataTemplateSelector
    {
        DataTemplate incomingDataTemplate;
        DataTemplate outgoingDataTemplate;

        public ChatTemplateSelector()
        {
            this.incomingDataTemplate = new DataTemplate(typeof(IncomingVIewCell));
            this.outgoingDataTemplate = new DataTemplate(typeof(OutgoingViewCell));
        }
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var messageVm = item as Messages;
            if (messageVm == null)
                return null;

            return (messageVm.id_user == Globais.userId) ? outgoingDataTemplate : incomingDataTemplate;
        }
    }
}
