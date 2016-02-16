using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public interface ISongModel
    {
        string Title { get; set; }

        string Artist { get; set; }
    }
}
