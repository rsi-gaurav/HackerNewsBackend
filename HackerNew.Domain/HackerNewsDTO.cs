using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackerNews.Domain.DTO
{
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
