using Microsoft.Practices.ServiceLocation;
using Prism.Unity;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Infrastructure.Dao;
using Infrastructure.Service;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Webplayer.Modules.Youtube;
using Queue;
using Webplayer.Modules.Spotify;

namespace Webplayer
{
    //https://github.com/PrismLibrary/Prism/blob/master/Source/Wpf/Prism.Unity.Wpf/UnityBootstrapper.cs#L50
    //https://github.com/PrismLibrary/Prism/blob/master/Source/Wpf/Prism.Wpf/Bootstrapper.cs
    public class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return ServiceLocator.Current.GetInstance<Shell>();
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();

            Application.Current.MainWindow = (Shell)this.Shell;
            Application.Current.MainWindow.Show();
        }

        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();

             var module = (ModuleCatalog)ModuleCatalog;
            module.AddModule(typeof(StructureModule));
            module.AddModule(typeof(YoutubeModule));
            module.AddModule(typeof(SpotifyModule));
        }

        protected override void InitializeModules()
        {
            
            var conn = new SQLiteConnection("Data Source=webplayer.db;Version=3;");
            conn.Open();
            Container.RegisterInstance<SQLiteConnection>(conn);
            Container.RegisterType<IPlaylistDao, PlaylistDao>();

            base.InitializeModules();
        }
    }
}
