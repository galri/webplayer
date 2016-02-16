using Infrastructure;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webplayer.Modules.Youtube.ViewModels;
using Webplayer.Modules.Youtube.Views;
using Webplayer.Modules.Youtube.Services;

namespace Webplayer.Modules.Youtube
{
    //https://github.com/PrismLibrary/Prism-Samples-Wpf/blob/master/HelloWorld/Modules/ModuleA/ModuleAModule.cs
    //https://github.com/PrismLibrary/Prism-Samples-Wpf
    public class YoutubeModule : IModule
    {
        private IRegionManager _rm;
        private IUnityContainer _container;

        public YoutubeModule(IRegionManager rm, IUnityContainer container)
        {
            _rm = rm;
            _container = container;
        }

        public void Initialize()
        {
            _container.RegisterType<IYoutubeFindViewModel, YoutubeFindViewModel>().
                RegisterType<IYoutubeSongSearchService, YoutubeSongSearch>();

            _rm.RegisterViewWithRegion(RegionNames.FindRegion, typeof(YoutubeFindView));
        }
    }
}
