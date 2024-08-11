using HackerNews.Domain.DTO;
using HackerNews.Domain.Interface;
using Microsoft.Extensions.Caching.Memory;

namespace HackerNews.Domain.Abstract
{
    /// <summary>
    /// Represents a service for retrieving new stories from Hacker News.
    /// </summary>
    public class HackerNewsService : IHackerNewsService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IAPIService _apiService;
        private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(5);
        private const string NewStoriesCacheKey = "NewStories";

        /// <summary>
        /// Initializes a new instance of the <see cref="HackerNewsService"/> class.
        /// </summary>
        /// <param name="memoryCache">The memory cache.</param>
        /// <param name="apiService">The API service.</param>
        public HackerNewsService(IMemoryCache memoryCache, IAPIService apiService)
        {
            _memoryCache = memoryCache;
            _apiService = apiService;
        }

        /// <summary>
        /// Retrieves the new stories from Hacker News.
        /// </summary>
        /// <returns>A list of <see cref="HackerNewsDTO"/> representing the new stories.</returns>
        public async Task<List<HackerNewsDTO>> GetNewStoriesAsync()
        {
            // Check if the new stories are available in the cache
            if (_memoryCache.TryGetValue(NewStoriesCacheKey, out List<HackerNewsDTO>? hackernewslist))
            {
                return hackernewslist!;
            }
            hackernewslist = new List<HackerNewsDTO>(0);
            var storiesIDes = (await _apiService.GetAllStoriesIds()).Take(200);

            //fetch the details of the stories in parallel
            Parallel.ForEach(storiesIDes, id =>
            {
                var tasks = _apiService.GetStoryDetail(id);
                if (tasks != null)
                {
                    hackernewslist.Add(tasks.Result);
                }
            });

            // Sort the stories by ID
            hackernewslist = hackernewslist.OrderByDescending(o => o.id).ToList();
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(_cacheDuration)
                .SetAbsoluteExpiration(_cacheDuration);

            // Cache the new stories
            _memoryCache.Set(NewStoriesCacheKey, hackernewslist, cacheEntryOptions);

            // Add null-forgiving operator to indicate that the return value is not null
            return hackernewslist!; 
        }
    }
}
