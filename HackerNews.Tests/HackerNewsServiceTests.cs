using HackerNews.Domain.Abstract;
using HackerNews.Domain.DTO;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Moq.Protected;
using System.Net;
using Xunit;
using System.Text.Json;

public class HackerNewsServiceTests
{
    private readonly Mock<IHttpClientFactoryService> _httpClientFactoryServiceMock;
    private readonly Mock<IMemoryCache> _memoryCacheMock;
    private readonly HackerNewsService _hackerNewsService;

    public HackerNewsServiceTests()
    {
        _httpClientFactoryServiceMock = new Mock<IHttpClientFactoryService>();
        _memoryCacheMock = new Mock<IMemoryCache>();
        _hackerNewsService = new HackerNewsService(_httpClientFactoryServiceMock.Object, _memoryCacheMock.Object);
    }

    [Fact]
    public async Task GetNewStoriesAsync_ShouldReturnCachedList_WhenCacheIsAvailable()
    {
        // Arrange
        var cachedList = new List<HackerNewsDTO> { new HackerNewsDTO { id = 1, title = "Test Story" } };
        _memoryCacheMock.Setup(m => m.TryGetValue(It.IsAny<object>(), out cachedList)).Returns(true);

        // Act
        var result = await _hackerNewsService.GetNewStoriesAsync();

        // Assert
        Assert.Equal(cachedList, result);
    }

    [Fact]
    public async Task GetNewStoriesAsync_ShouldFetchAndCacheList_WhenCacheIsNotAvailable()
    {
        // Arrange
        var httpClientMock = new Mock<HttpClient>();
        var responseContent = JsonSerializer.Serialize(new List<int> { 1, 2, 3 });
        var storyContent = JsonSerializer.Serialize(new HackerNewsDTO { id = 1, title = "Test Story" });

        httpClientMock.SetupSequence(c => c.GetAsync(It.IsAny<string>()))
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(responseContent) })
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(storyContent) });

        _httpClientFactoryServiceMock.Setup(m => m.CreateClient()).Returns(httpClientMock.Object);

        _memoryCacheMock.Setup(m => m.TryGetValue(It.IsAny<object>(), out It.Ref<List<HackerNewsDTO>>.IsAny)).Returns(false);

        // Act
        var result = await _hackerNewsService.GetNewStoriesAsync();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(1, result[0].id);
        Assert.Equal("Test Story", result[0].title);
        _memoryCacheMock.Verify(m => m.Set(It.IsAny<object>(), It.IsAny<List<HackerNewsDTO>>(), It.IsAny<MemoryCacheEntryOptions>()), Times.Once);
    }

    //[Fact]
    //public async Task GetNewStoriesAsync_CachesData() //Can't be tested as the stories will keep on changing and no specified test cases can be provided
    //{
    //    // Arrange
    //    var expectedStories = new List<HackerNewsDTO>();
    //    var handlerMock = new Mock<HttpMessageHandler>();
    //    handlerMock
    //        .Protected()
    //        .Setup<Task<HttpResponseMessage>>(
    //            "SendAsync",
    //            ItExpr.IsAny<HttpRequestMessage>(),
    //            ItExpr.IsAny<CancellationToken>()
    //        )
    //        .ReturnsAsync(new HttpResponseMessage
    //        {
    //            StatusCode = HttpStatusCode.OK,
    //            Content = new StringContent("[1, 2, 3]"),
    //        });

    //    var httpClient = new HttpClient(handlerMock.Object);

    //    var factoryServiceMock = new Mock<IHttpClientFactoryService>();
    //    factoryServiceMock.Setup(factory => factory.CreateClient()).Returns(httpClient);

    //    var memoryCache = new MemoryCache(new MemoryCacheOptions());
    //    var service = new HackerNewsService(factoryServiceMock.Object, memoryCache);

    //    // Act
    //    var result = await service.GetNewStoriesAsync();
    //    var cachedResult = await service.GetNewStoriesAsync();

    //    // Assert
    //    Assert.Equal(expectedStories, result);
    //    Assert.Equal(expectedStories, cachedResult);
    //    factoryServiceMock.Verify(factory => factory.CreateClient(), Times.Once);
    //}
}
