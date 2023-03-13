namespace Api.Tests.Extensions
{
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    public static class HttpClientExtensions
    {
        public static async Task<K> PostAsync<T, K>(this HttpClient client, string url, T data) where K : class
        {
            var json = JsonConvert.SerializeObject(data);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            var response = await client.PostAsync(url, stringContent);
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
    }
}
