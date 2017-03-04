using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public static class GlobalCommands
    {
        public static CompositeCommand ShowSearchFieldInActiveCommand { get; private set; } = new CompositeCommand();
    }
}
