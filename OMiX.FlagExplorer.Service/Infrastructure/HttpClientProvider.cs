namespace OMiX.FlagExplorer.Service.Infrastructure
{
    public class HttpClientProvider : IHttpClientProvider
    {
        public Task<HttpResponseMessage> GetAsync(HttpClient httpClient, string url)
        {
            return httpClient.GetAsync(url);
        }
    }
}
