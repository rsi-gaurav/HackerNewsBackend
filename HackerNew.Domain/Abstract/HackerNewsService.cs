using HackerNews.Domain.DTO;
using HackerNews.Domain.Interface;
using HackerNews.Domain.Constants;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;
using System.Net;
using System.Threading.Tasks;

namespace HackerNews.Domain.Abstract
{
    /// <summary>
    /// Represents a service for retrieving new stories from Hacker News.
    /// </summary>
    public class HackerNewsService : IHackerNewsService
    {
        private readonly IHttpClientFactoryService _httpClientFactoryService;
        private readonly IMemoryCache _memoryCache;
        private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(5);
        private const string NewStoriesCacheKey = "NewStories";

        /// <summary>
        /// Initializes a new instance of the <see cref="HackerNewsService"/> class.
        /// </summary>
        /// <param name="httpClientFactoryService">The HTTP client factory service.</param>
        /// <param name="memoryCache">The memory cache.</param>
        public HackerNewsService(IHttpClientFactoryService httpClientFactoryService, IMemoryCache memoryCache)
        {
            _httpClientFactoryService = httpClientFactoryService;
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// Retrieves the list of new stories from Hacker News.
        /// </summary>
        /// <returns>The list of new stories.</returns>
        public async Task<List<HackerNewsDTO>> GetNewStoriesAsync()
        {

            if (_memoryCache.TryGetValue(NewStoriesCacheKey, out List<HackerNewsDTO>? hackernewslist))
            {
                return hackernewslist;
            }

            var client = _httpClientFactoryService.CreateClient();

            // Fetch the top stories
            var response = await client.GetAsync(ApiUrls.NewStories);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();

            // Deserialize the list of story IDs
            var storyIds = JsonSerializer.Deserialize<List<int>>(responseContent);

            hackernewslist = new List<HackerNewsDTO>();
            // Fetch details for each story in parallel
            Parallel.ForEach(storyIds.Take(200), id =>
            {
                var storyResponse = client.GetAsync(string.Format(ApiUrls.StoryDetails, id));
                var storyContent = storyResponse.Result.Content.ReadAsStringAsync();
                HackerNewsDTO? story = JsonSerializer.Deserialize<HackerNewsDTO>(storyContent.Result);
                if (story != null)
                {
                    hackernewslist.Add(story);
                }
            });
            hackernewslist = hackernewslist.OrderByDescending(o => o.id).ToList();

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(_cacheDuration)
                .SetAbsoluteExpiration(_cacheDuration);

            _memoryCache.Set(NewStoriesCacheKey, hackernewslist, cacheEntryOptions);

            return hackernewslist;
        }
    }
}
