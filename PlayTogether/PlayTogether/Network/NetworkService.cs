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

        public async Task<TResult> PostAsync<TResult>(string url, string jsonData)
        {
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync(url, content);

            string serialized = await response.Content.ReadAsStringAsync();
            TResult result = JsonConvert.DeserializeObject<TResult>(serialized);

            return result;
        }
        public async Task<TResult> PutAsync<TResult>(string url, string jsonData)
        {
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PutAsync(url, content);

            string serialized = await response.Content.ReadAsStringAsync();
            TResult result = JsonConvert.DeserializeObject<TResult>(serialized);

            return result;
        }
        public async Task DeleteAsync(string url)
        {
            await _httpClient.DeleteAsync(url);
        }
        
    }
}