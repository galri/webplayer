using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    /// <summary>
    /// Interface for service that provide search functinality against music services.
    /// </summary>
    public interface ISearchService<T> where T : ISongModel
    {
        string Query { get; set; }

        IEnumerable<T> FetchNextSearchResult();

        T GetSong(string id);
    }
}
