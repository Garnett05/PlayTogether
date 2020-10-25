using System.Threading.Tasks;

namespace PlayTogether.Services.DialogMessage
{
    public interface IDialogMessage
    {
        Task DisplayAlert(string title, string message, string cancel);
        Task<bool> DisplayAlertOptions(string title, string message, string accept, string cancel);
        Task<string> DisplayPrompt(string title, string message);
        Task<string> DisplayActionSheet(string title, string description, params string[] buttons);
    }
}
