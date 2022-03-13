using System.Net.Http.Headers;
using System.Text.Json;

namespace Library.Utils.Generals
{
    public static class General
    {
        public static StringContent BuildJsonRequestContent<T>(T data, MediaTypeHeaderValue mediaType)
        {
            var dataAsString = JsonSerializer.Serialize(data);
            var content = new StringContent(dataAsString);

            content.Headers.ContentType = mediaType;
            return content;
        }

        public static AuthenticationHeaderValue SetRequestBearerToken(string token)
        {
            if(string.IsNullOrEmpty(token))
                return new AuthenticationHeaderValue("Bearer", "Without authorization");

            return new AuthenticationHeaderValue("Bearer", token);
        }
    }
}