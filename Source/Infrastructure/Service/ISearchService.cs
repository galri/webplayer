using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Models;

namespace Infrastructure
{
    /// <summary>
    /// Interface for service that provide search functinality against music services.
    /// </summary>
    public interface ISearchService<T> : IDisposable where T : BaseSong
    {
        string Query { get; set; }

        /// <summary>
        /// Fetch list of search result, can be called multiple times to get more result.
        /// Will use <see cref="Query"/> to search first time.
        /// </summary>
        /// <returns></returns>
        Task<List<T>> FetchAsync();

        /// <summary>
        /// Fetch son info from id
        /// </summary>
        /// <param name="id">id to fetch sing info from</param>
        /// <returns></returns>
        Task<T> FetchSongAsync(string id);
    }
}
