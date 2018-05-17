using DMZ00.Views;
using GMapControl;
using Microsoft.Practices.Unity;
using MyHud;
using Prism.Modularity;
using Prism.Unity;
using System.Windows;

namespace DMZ00
{
    class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }
        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();
        }
        protected override void ConfigureModuleCatalog()
        {
            var catalog = (ModuleCatalog)ModuleCatalog;
            catalog.AddModule(typeof(GMapControlModule));
            catalog.AddModule(typeof(MyHudModule));
        }
    }
}
