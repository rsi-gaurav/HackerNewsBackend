using HackerNews.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackerNews.Domain.Interface
{
    /// <summary>
    /// Represents the interface for the Hacker News service.
    /// </summary>
    public interface IHackerNewsService
    {
        /// <summary>
        /// Retrieves a list of new stories from Hacker News asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the list of new stories.</returns>
        Task<List<HackerNewsDTO>> GetNewStoriesAsync();
    }
}
