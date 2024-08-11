using HackerNews.Domain.DTO;
using HackerNews.Test;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace HackerNews.Domain.Abstract
{
    /// <summary>
    /// Unit tests for the APIService class.
    /// </summary>
    public class APIServiceTests
    {
        private readonly HttpClient _httpClient1;
        private readonly HttpClient _httpClient2;
        private readonly APIService _apiService1;
        private readonly APIService _apiService2;

        /// <summary>
        /// Initializes a new instance of the <see cref="APIServiceTests"/> class.
        /// </summary>
        public APIServiceTests()
        {
            var httpMessageHandler1 = MockHttpHandler.GetMessageHandler1();
            _httpClient1 = new HttpClient(httpMessageHandler1.Object);
            _apiService1 = new APIService(_httpClient1);
            var httpMessageHandler2 = MockHttpHandler.GetMessageHandler2();
            _httpClient2 = new HttpClient(httpMessageHandler2.Object);
            _apiService2 = new APIService(_httpClient2);
        }

        /// <summary>
        /// Test case for GetAllStoriesIds method.
        /// </summary>
        [Fact]
        public async Task GetAllStoriesIds_ReturnsStoryIds_WhenResponseIsSuccessful()
        {
            // Arrange
            var expectedStoryIds = new List<int> { 1, 2, 3 };

            // Act
            var result = await _apiService1.GetAllStoriesIds();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(expectedStoryIds, result);
        }

        /// <summary>
        /// Test case for GetStoryDetail method.
        /// </summary>
        [Fact]
        public async Task GetStorieDetail_ReturnsStoryDetailDto_WhenResponseIsSuccessful()
        {
            // Arrange
            var storyId = 1;
            var hackerNewsDTO = new HackerNewsDTO { id = 1, title = "Test Story" };

            // Act
            var result = await _apiService2.GetStoryDetail(storyId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(hackerNewsDTO.id, result.id);
            Assert.Equal(hackerNewsDTO.title, result.title);
        }

        //[Fact]
        //public async Task GetStoryDetail_ReturnsNull_WhenResponseIsNotSuccessful()
        //{
        //    // Arrange
        //    var httpContext = new DefaultHttpContext();
        //    httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //    var httpContextAccessor = new HttpContextAccessor { HttpContext = httpContext };
            
        //    var apiService = new APIService(null);

        //    // Act
        //    var result = await apiService.GetStoryDetail(1);

        //    // Assert
        //    //Assert.IsNull(result);
        //}
    }
}
