using HackerNews.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackerNews.Domain.Interface
{
    public interface IHackerNewsService
    {
        Task<List<HackerNewsDTO>> GetNewStoriesAsync();
    }
}
