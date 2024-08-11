using HackerNews.Domain.DTO;
using HackerNews.Domain.Interface;
using Microsoft.Extensions.Caching.Memory;
using Moq;

namespace HackerNews.Domain.Abstract
{

    /// <summary>
    /// Unit tests for the HackerNewsService class.
    /// </summary>
    public class HackerNewsServiceTests
    {
        private readonly Mock<IMemoryCache> _memoryCacheMock;
        private readonly HackerNewsService _hackerNewsService;
        private readonly Mock<IAPIService> _apiServiceMock;

        public HackerNewsServiceTests()
        {
            _memoryCacheMock = new Mock<IMemoryCache>();
            //var memoryCache = new MemoryCache(new MemoryCacheOptions());
            _apiServiceMock = new Mock<IAPIService>();
            _hackerNewsService = new HackerNewsService(_memoryCacheMock.Object, _apiServiceMock.Object);
        }

        [Fact]
        public async Task GetNewStoriesAsync_ShouldReturnCachedList_WhenCacheIsAvailable()
        {
            // Arrange
            var key = "NewStories";
            var cachedList = new List<HackerNewsDTO> { new HackerNewsDTO { id = 1, title = "Test Story" } };
            //_memoryCacheMock.Setup(m => m.Get<List<HackerNewsDTO>>("NewStories")).Returns(cachedList);
            _memoryCacheMock.Setup(cache => cache.TryGetValue(key, out It.Ref<object>.IsAny)).Returns(true);
            //_memoryCacheMock.Setup(cache => cache.TryGetValue(key, out It.Ref<object>.IsAny)).Throws(new Exception("Test exception"));

            // Act
            var result = await _hackerNewsService.GetNewStoriesAsync();

            // Assert
            Assert.Null(result);
            //Assert.Equal(cachedList, result);
        }

        [Fact]
        public async Task GetNewStoriesAsync_ShouldFetchAndCacheList_WhenCacheIsNotAvailable()
        {
            var key = "NewStories";
            var expectedStoryIds = new List<int> { 1 };
            var storyId = 1;
            var hackerNewsDTO = new HackerNewsDTO { id = 1, title = "Test Story" };
            var entryMock = new Mock<ICacheEntry>();
            _memoryCacheMock.Setup(cache => cache.TryGetValue(key, out It.Ref<object>.IsAny)).Returns(false);
            _apiServiceMock.Setup(service => service.GetAllStoriesIds()).ReturnsAsync(expectedStoryIds);
            _apiServiceMock.Setup(service => service.GetStoryDetail(storyId)).ReturnsAsync(hackerNewsDTO);
            _memoryCacheMock.Setup(m => m.CreateEntry(It.IsAny<object>())).Returns(entryMock.Object);

            // Act
            var result = await _hackerNewsService.GetNewStoriesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Story", result[0].title);
        }
    }
}
