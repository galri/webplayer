using Microsoft.Practices.ServiceLocation;
using Prism.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Prism.Modularity;
using Webplayer.Modules.Youtube;
using Queue;

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
        }
    }
}
