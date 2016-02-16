using Infrastructure;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webplayer.Modules.Youtube.Views;

namespace Webplayer.Modules.Youtube
{
    //https://github.com/PrismLibrary/Prism-Samples-Wpf/blob/master/HelloWorld/Modules/ModuleA/ModuleAModule.cs
    //https://github.com/PrismLibrary/Prism-Samples-Wpf
    public class YoutubeModule : IModule
    {
        private IRegionManager _rm;

        public YoutubeModule(IRegionManager rm)
        {
            _rm = rm;
        }

        public void Initialize()
        {
            _rm.RegisterViewWithRegion(RegionNames.FindRegion, typeof(YoutubeFindView));
        }
    }
}
