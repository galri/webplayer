using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public static class RegionNames
    {
        /// <summary>
        /// region for searching after songs
        /// </summary>
        public static string FindRegion = "MainRegion";

        /// <summary>
        /// Top region for new open etc...
        /// </summary>
        public static string ToolbarRegion = "ToolbarRegion";

        /// <summary>
        /// How long the song has been playing, play, pause etc..
        /// </summary>
        public static string StatusRegion = "StatusRegion";

        /// <summary>
        /// Video, album image
        /// </summary>
        public static string InfoRegion = "InfoRegion";

        /// <summary>
        /// Main playlist area.
        /// </summary>
        public static string QueueRegion = "QueueRegion";
    }
}
