using System.Threading.Tasks;

namespace PlayTogether.Services.DialogMessage
{
    public interface IDialogMessage
    {
        Task DisplayAlert(string title, string message, string cancel);
        Task DisplayPrompt(string title, string message);
        Task DisplayActionSheet(string title, string description, params string[] buttons);
    }
}
