using System.Net.Http;

public interface IHttpClientFactoryService
{
    HttpClient CreateClient();
}