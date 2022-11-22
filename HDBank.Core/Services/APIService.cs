using HDBank.Core.Interfaces;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace HDBank.Core.Services
{
    public class APIService : IAPIService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public APIService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> GetKey()
        {
            var client = _httpClientFactory.CreateClient("HDBank");
            var response = await client.GetAsync($"/get_key");
            return await response.Content.ReadAsStringAsync()!; ;
        }

    }
}