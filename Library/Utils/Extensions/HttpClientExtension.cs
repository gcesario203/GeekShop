using System.Text.Json;
using System.Net.Http.Headers;
using Library.Utils.Generals;

namespace Library.Utils.Extensions
{
    public static class HttpClientExtension
    {
        public static MediaTypeHeaderValue _contentType = new MediaTypeHeaderValue("application/json");
        public static async Task<T> ReadContentAs<T>(this HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode) throw new ApplicationException($"Something goes wrong calling the api: {response.ReasonPhrase}");

            var dataAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);


            return JsonSerializer.Deserialize<T>(dataAsString,
            new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });
        }
        public static Task<HttpResponseMessage> PostAsJson<T>(this HttpClient httpClient, string url, T data)
        {
            return httpClient.PostAsync(url, General.BuildJsonRequestContent<T>(data, _contentType));
        }
        public static Task<HttpResponseMessage> PutAsJson<T>(this HttpClient httpClient, string url, T data)
        {
            return httpClient.PutAsync(url, General.BuildJsonRequestContent<T>(data, _contentType));
        }
    }
}