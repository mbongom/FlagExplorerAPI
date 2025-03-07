namespace OMiX.FlagExplorer.Service.Infrastructure
{
    public interface IHttpClientProvider
    {
        Task<HttpResponseMessage> GetAsync(HttpClient httpClient, string url);
    }
}
