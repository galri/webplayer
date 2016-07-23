﻿using Infrastructure;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webplayer.Modules.Youtube.ViewModels;
using Webplayer.Modules.Youtube.Views;
using Webplayer.Modules.Youtube.Services;
using Google.Apis.YouTube.v3;
using Infrastructure.Models;
using Infrastructure.Service;
using Prism.Logging;

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
            var youtubeService = new YouTubeService(new Google.Apis.Services.BaseClientService.Initializer()
            {
                ApiKey = "AIzaSyAtuwBQDfoweQqFuxNmXUNH-n70J1KL_54",
                ApplicationName = "Web playlist"
            });

            var serviceSaverList = _container.Resolve<List<ISongServicePlaylistSaver>>();
            var dbConn = _container.Resolve<SQLiteConnection>();
            serviceSaverList.Add(new YoutubePlaylistService(dbConn, _container.Resolve<ILoggerFacade>()));

            _container.RegisterInstance<YouTubeService>(youtubeService);

            _container.RegisterType<IYoutubeFindViewModel, YoutubeFindViewModel>().
                RegisterType<IYoutubeSongSearchService, YoutubeSongSearch>().
                RegisterType<IYoutubePlaylistSearchService,YoutubePlaylistSearchService>();

            _rm.RegisterViewWithRegion(RegionNames.FindRegion, typeof(YoutubeFindView));

            _container.RegisterType<IVIdeoInfoView, VideoInfoView>();
            _container.RegisterType<IVideoInfoViewModel, VideoInfoViewModel>();
            _rm.RegisterViewWithRegion(RegionNames.InfoRegion, typeof (VideoInfoView));

        }
    }
}
