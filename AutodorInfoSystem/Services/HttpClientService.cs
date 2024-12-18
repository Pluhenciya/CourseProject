using System.Net.Http.Headers;

namespace AutodorInfoSystem.Services
{
    public class HttpClientService
    {
        private readonly HttpClient httpClient;

        public HttpClientService()
        {
            httpClient = new HttpClient { BaseAddress = new Uri("http://adis_api:5239/api/") };
        }

        public void SetAuthorization(string token)
        {
            httpClient.DefaultRequestHeaders.Authorization = string.IsNullOrEmpty(token)
                ? null
                : new AuthenticationHeaderValue("Bearer", token);
        }

        public HttpClient GetHttpClient()
        {
            return httpClient;
        }
    }
}
