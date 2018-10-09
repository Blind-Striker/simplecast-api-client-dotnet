namespace Simplecast.Client.Core
{
    public class ApiOptions
    {
        public ApiOptions(string apiKey, string baseUrl)
        {
            ApiKey = apiKey;
            BaseUrl = baseUrl;
        }

        public string ApiKey { get; }
        public string BaseUrl { get; }
    }
}
