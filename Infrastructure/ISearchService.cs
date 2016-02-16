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
    public interface ISearchService : IEnumerator<ISongModel>
    {
        string Query { get; set; }


    }
}
