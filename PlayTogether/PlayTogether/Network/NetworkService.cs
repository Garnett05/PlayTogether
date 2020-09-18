using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PlayTogether.Network
{
    public class NetworkService : INetworkService
    {
        private HttpClient _httpClient;
        public NetworkService()
        {
            _httpClient = new HttpClient();
        }
        
        public async Task<TResult> GetAsync<TResult>(string url)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            string serialize = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<TResult>(serialize);
            return result;
        }
    }
}