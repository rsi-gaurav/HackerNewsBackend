using HackerNews.Domain.DTO;
using HackerNews.Domain.Interface;
using Microsoft.AspNetCore.Mvc;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="HackerNewsServiceTests"/> class.
        /// </summary>
        public HackerNewsServiceTests()
        {
            _memoryCacheMock = new Mock<IMemoryCache>();
            _apiServiceMock = new Mock<IAPIService>();
            _hackerNewsService = new HackerNewsService(_memoryCacheMock.Object, _apiServiceMock.Object);
        }

        /// <summary>
        /// Test case for the GetNewStoriesAsync method when cache is available.
        /// </summary>
        [Fact]
        public async Task GetNewStoriesAsync_ShouldReturnCachedList_WhenCacheIsAvailable()
        {
            // Arrange
            var key = "NewStories";
            object? cachedList = new List<HackerNewsDTO> { new HackerNewsDTO { id = 1, title = "Test Story" } }; ;
            _memoryCacheMock.Setup(cache => cache.TryGetValue(key, out cachedList)).Returns(true);

            // Act
            var result = await _hackerNewsService.GetNewStoriesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Count > 0);
            Assert.Equal(1, result[0].id);    
            Assert.Equal("Test Story", result[0].title);
        }

        /// <summary>
        /// Test case for the GetNewStoriesAsync method when cache is not available.
        /// </summary>
        [Fact]
        public async Task GetNewStoriesAsync_ShouldFetchAndCacheList_WhenCacheIsNotAvailable()
        {
            var key = "NewStories";
            var storyId = 1;
            object? outValue = null;
            var entryMock = new Mock<ICacheEntry>();
            var expectedStoryIds = new List<int> { 1 };
            var hackerNewsDTO = new HackerNewsDTO { id = 1, title = "Test Story" };
            _memoryCacheMock.Setup(cache => cache.TryGetValue(key, out outValue)).Returns(false);
            _apiServiceMock.Setup(service => service.GetAllStoriesIds()).ReturnsAsync(expectedStoryIds);
            _apiServiceMock.Setup(service => service.GetStoryDetail(storyId)).ReturnsAsync(hackerNewsDTO);
            _memoryCacheMock.Setup(m => m.CreateEntry(It.IsAny<object>())).Returns(entryMock.Object);

            // Act
            var result = await _hackerNewsService.GetNewStoriesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result[0].id);
            Assert.Equal("Test Story", result[0].title);
        }
    }
}
