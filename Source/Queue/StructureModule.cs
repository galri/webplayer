using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using Infrastructure.Models;
using Infrastructure.Service;
using Webplayer.Modules.Structure.ViewModels;
using Webplayer.Modules.Structure.Views;

namespace Queue
{
    public class StructureModule : IModule
    {
        private readonly IRegionManager _rm;
        private readonly IUnityContainer _container;

        public StructureModule(IRegionManager rm, IUnityContainer container)
        {
            _rm = rm;
            _container = container;
        }

        public void Initialize()
        {
            _container.RegisterInstance(new List<ISongServicePlaylistSaver>());
            _container.RegisterType<IPlaylistService, PlaylistService>();

            _container.RegisterInstance<IQueueController>(new QueueController());

            var queueController = _container.Resolve<IQueueController>();
            var playlistService = _container.Resolve<IPlaylistService>();

            _container.RegisterType<IQueueView, QueueView>().
                RegisterType<IQueueViewModel, QueueViewModel>();
            _container.RegisterType<IStatusView, StatusView>().
                RegisterType<IStatusViewModel, StatusViewModel>();

            _rm.RegisterViewWithRegion(RegionNames.QueueRegion, typeof(IQueueView));
            _rm.RegisterViewWithRegion(RegionNames.StatusRegion, typeof (IStatusView));
        }
    }
}
