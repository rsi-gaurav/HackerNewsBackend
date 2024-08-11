using HackerNews.Domain.DTO;

namespace HackerNews.Domain.Interface
{
    /// <summary>
    /// Represents an API service for retrieving Hacker News data.
    /// </summary>
    public interface IAPIService
    {
        /// <summary>
        /// Retrieves all the story IDs from Hacker News.
        /// </summary>
        /// <returns>An asynchronous operation that returns a collection of story IDs.</returns>
        Task<IEnumerable<int>> GetAllStoriesIds();

        /// <summary>
        /// Retrieves the details of a specific story from Hacker News.
        /// </summary>
        /// <param name="StoryId">The ID of the story to retrieve.</param>
        /// <returns>An asynchronous operation that returns the details of the story.</returns>
        Task<HackerNewsDTO> GetStoryDetail(int StoryId);
    }
}
