using System.Threading.Tasks;

namespace PlayTogether.Network
{
    public interface INetworkService
    {
        Task<TResult> GetAsync<TResult>(string url);
    }
}
