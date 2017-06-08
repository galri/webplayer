using Infrastructure;
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
using Google.Apis.Services;
using System.IO;
using Newtonsoft.Json;
using Webplayer.Modules.Youtube.Models;
using Prism.Mvvm;

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
            var applicationSettingsData = File.ReadAllText("YoutubeApplicationSettings.json");
            var applicationSettings = JsonConvert.DeserializeObject<YoutubeApiSettings>(applicationSettingsData);

            var youtubeService = new YouTubeService(new BaseClientService.Initializer
            {
                ApiKey = applicationSettings.ApiKey,
                ApplicationName = applicationSettings.ApplicationName,
            });

            var serviceSaverList = _container.Resolve<List<ISongServicePlaylistSaver>>();
            var dbConn = _container.Resolve<SQLiteConnection>();
            serviceSaverList.Add(new YoutubePlaylistService(dbConn, _container.Resolve<ILoggerFacade>()));

            _container.RegisterInstance<YouTubeService>(youtubeService);

            _container.RegisterType<IYoutubeFindViewModel, YoutubeFindViewModel>().
                RegisterType<IYoutubeFindView,YoutubeFindView>().
                RegisterType<IYoutubeSongSearchService, YoutubeSongSearch>().
                RegisterType<IYoutubePlaylistSearchService,YoutubePlaylistSearchService>();

            _container.RegisterType<IYoutubeFindUploader, IYoutubeFindUploader>().
                RegisterType<IYoutubeFindUploaderViewModel, YoutubeFindUploaderViewModel>().
                RegisterType<IYoutubeChannelService, YoutubeChannelService>();

            _container.RegisterType<IYoutubeSearchDialogView, YoutubeSearchDialogView>().
                RegisterType<IYoutubeSearchDialogViewModel, YoutubeSearchDialogViewModel>();

              _rm.RegisterViewWithRegion(RegionNames.FindRegion, typeof(IYoutubeFindView));

            //_container.RegisterType<IVIdeoInfoView, VideoInfoView>();
            _container.RegisterType<IVIdeoInfoView, YoutubeLibView>();
            _container.RegisterType<IVideoInfoViewModel, VideoInfoViewModel>();
            _rm.RegisterViewWithRegion(RegionNames.InfoRegion, typeof (YoutubeLibView));
            
            //var ys = new YoutubeAccountService();
            //ys.Login();

        }

        public static void ConfigureViewModelLocator(IUnityContainer container)
        {
            ViewModelLocationProvider.Register(typeof(YoutubeLibView).ToString(), () => container.Resolve<IVideoInfoViewModel>());
        }
    }
}
