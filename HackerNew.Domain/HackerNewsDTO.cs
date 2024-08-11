using System.Diagnostics.CodeAnalysis;

namespace HackerNews.Domain.DTO
{
    [ExcludeFromCodeCoverage]
    /// <summary>
    /// Represents a Hacker News DTO.
    /// </summary>
    public class HackerNewsDTO
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        /// <value>
        /// The Id.
        /// </value>
        public int id { get; set; }

        /// <summary>
        /// Gets or sets the Title.
        /// </summary>
        /// <value>
        /// The Title.
        /// </value>
        public string title { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        public string url { get; set; }
    }
}
