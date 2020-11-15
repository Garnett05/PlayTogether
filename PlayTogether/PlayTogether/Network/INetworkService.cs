using System.Threading.Tasks;

namespace PlayTogether.Network
{
    public interface INetworkService
    {
        Task<TResult> GetAsync<TResult>(string url);
        Task<TResult> PostAsync<TResult>(string url, string jsonData);
        Task<TResult> PutAsync<TResult>(string url, string jsonData);
        Task DeleteAsync(string url);
    }
}