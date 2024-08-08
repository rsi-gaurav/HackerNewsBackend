using HackerNews.Controllers;
using HackerNews.Domain.DTO;
using HackerNews.Domain.Interface;
using Moq;
using Microsoft.AspNetCore.Mvc;

namespace HackerNews.Controllers
{
    /// <summary>
    /// Unit tests for the HackerNewsController class.
    /// </summary>
    public class HackerNewsControllerTests
    {
        private readonly Mock<IHackerNewsService> _hackerNewsServiceMock;
        private readonly HackerNewsController _controller;

        public HackerNewsControllerTests()
        {
            _hackerNewsServiceMock = new Mock<IHackerNewsService>();
            _controller = new HackerNewsController(_hackerNewsServiceMock.Object);
        }

        /// <summary>
        /// Test case to verify that GetNewStories action returns OkResult with expected data.
        /// </summary>
        [Fact]
        public async Task GetSomeData_ReturnsOkResult_WithExpectedData()
        {
            // Arrange
            var expectedData = new List<HackerNewsDTO> { new HackerNewsDTO { id = 1, title = "Test Story" } };
            _hackerNewsServiceMock.Setup(service => service.GetNewStoriesAsync()).ReturnsAsync(expectedData);

            // Act
            var result = await _controller.GetNewStories();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedData, okResult.Value);
        }
    }
}

