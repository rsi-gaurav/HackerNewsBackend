using HackerNews.Domain.Interface;
using Microsoft.AspNetCore.Mvc;

namespace HackerNews.Controllers
{
    /// <summary>
    /// Controller for handling Hacker News related operations.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class HackerNewsController : ControllerBase
    {
        private readonly IHackerNewsService _hackerNewsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="HackerNewsController"/> class.
        /// </summary>
        /// <param name="hackerNewsService">The Hacker News service.</param>
        public HackerNewsController(IHackerNewsService hackerNewsService)
        {
            _hackerNewsService = hackerNewsService;
        }

        /// <summary>
        /// Gets the new stories from Hacker News.
        /// </summary>
        /// <returns>The new stories.</returns>
        [HttpGet("NewStories")]
        public async Task<IActionResult> GetNewStories()
        {
            var newStories = await _hackerNewsService.GetNewStoriesAsync();
            return Ok(newStories);
        }
    }
}
