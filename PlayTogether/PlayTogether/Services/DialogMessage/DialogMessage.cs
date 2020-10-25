using Xamarin.Forms;
using System.Threading.Tasks;

namespace PlayTogether.Services.DialogMessage
{
    public class DialogMessage : IDialogMessage
    {
        public Task<string> DisplayActionSheet(string title, string description, params string[] buttons)
        {
            return Application.Current.MainPage.DisplayActionSheet(title, "cancel", description, buttons);
        }

        public async Task DisplayAlert(string title, string message, string cancel)
        {
            await Application.Current.MainPage.DisplayAlert(title, message, cancel);
        }

        public Task<bool> DisplayAlertOptions(string title, string message, string accept, string cancel)
        {
            return Application.Current.MainPage.DisplayAlert(title, message, accept, cancel);
        }

        public Task<string> DisplayPrompt(string title, string message)
        {
            return Application.Current.MainPage.DisplayPromptAsync(title, message);
        }
    }
}
