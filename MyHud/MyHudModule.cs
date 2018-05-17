using MyHud.Views;
using Prism.Modularity;
using Prism.Regions;

namespace MyHud
{
    public class MyHudModule : IModule
    {
        RegionManager _regionManager;
        public MyHudModule(RegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _regionManager.RegisterViewWithRegion("MyHudRegion",typeof(MyHudTest));
        }
    }
}
