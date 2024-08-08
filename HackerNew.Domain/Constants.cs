namespace HackerNews.Domain.Constants
{
    /// <summary>
    /// Contains the API URLs for Hacker News.
    /// </summary>
    public static class ApiUrls
    {
        /// <summary>
        /// The URL for retrieving the new stories.
        /// </summary>
        public const string NewStories = "https://hacker-news.firebaseio.com/v0/newstories.json?print=pretty";

        /// <summary>
        /// The URL for retrieving the details of a story.
        /// </summary>

        public const string StoryDetails = "https://hacker-news.firebaseio.com/v0/item/{0}.json?print=pretty";
    }
}
