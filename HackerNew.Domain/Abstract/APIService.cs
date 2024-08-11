using HackerNews.Domain.Constants;
using HackerNews.Domain.DTO;
using HackerNews.Domain.Interface;
using System.Text.Json;

namespace HackerNews.Domain.Abstract
{
    /// <summary>
    /// Represents an API service for fetching Hacker News data.
    /// </summary>
    public class APIService : IAPIService
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="APIService"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client.</param>
        public APIService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Gets all the IDs of the top stories.
        /// </summary>
        /// <returns>An asynchronous operation that returns the list of story IDs.</returns>
        public async Task<IEnumerable<int>> GetAllStoriesIds()
        {
            // Fetch the top stories
            var response = await _httpClient.GetAsync(ApiUrls.NewStories);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();

            // Deserialize the list of story IDs
            var storyIds = JsonSerializer.Deserialize<List<int>>(responseContent);
            return storyIds;
        }

        /// <summary>
        /// Gets the details of a story.
        /// </summary>
        /// <param name="storyId">The ID of the story.</param>
        /// <returns>An asynchronous operation that returns the story details.</returns>
        public async Task<HackerNewsDTO> GetStoryDetail(int storyId)
        {
            var storyResponse = await _httpClient.GetAsync(string.Format(ApiUrls.StoryDetails, storyId));
            var storyContent = storyResponse.Content.ReadAsStringAsync();
            HackerNewsDTO? story = JsonSerializer.Deserialize<HackerNewsDTO>(storyContent.Result);
            return story;
        }
    }
}
