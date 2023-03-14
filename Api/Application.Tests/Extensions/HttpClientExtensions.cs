namespace Api.Tests.Extensions
{
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using Api.Tests.ApplicationFactory;
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json;

    public static class HttpClientExtensions
    {
        public static async Task<K> PostAsync<T, K>(this HttpClient client, string url, T data) where K : class
        {
            var json = JsonConvert.SerializeObject(data);

            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Content = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            var accessToken = FakeJwtManager.GenerateJwtToken();
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await client.SendAsync(request);


            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<K>(result);
            }
            else
            {
                throw new ApiException(response.StatusCode, "Error");
            }
        }

        public static async Task<K> PutAsync<T, K>(this HttpClient client, string url, T data) where K : class
        {
            var json = JsonConvert.SerializeObject(data);

            var request = new HttpRequestMessage(HttpMethod.Put, url);
            request.Content = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            var accessToken = FakeJwtManager.GenerateJwtToken();
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await client.SendAsync(request);


            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<K>(result);
            }
            else
            {
                throw new ApiException(response.StatusCode, "Error");
            }
        }

        public static async Task<K> GetAsync<K>(this HttpClient client, string url) where K : class
        {

            var request = new HttpRequestMessage(HttpMethod.Get, url);

            var accessToken = FakeJwtManager.GenerateJwtToken();
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await client.SendAsync(request);


            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<K>(result);
            }
            else
            {
                throw new ApiException(response.StatusCode, "Error");
            }
        }

        public static async Task<HttpResponseMessage> DeleteAsyncWithHeader(this HttpClient client, string url)
        {

            var request = new HttpRequestMessage(HttpMethod.Delete, url);

            var accessToken = FakeJwtManager.GenerateJwtToken();
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await client.SendAsync(request);


            if (response.IsSuccessStatusCode)
            {
                return response;
            }
            else
            {
                throw new ApiException(response.StatusCode, "Error");
            }
        }
    }
}
