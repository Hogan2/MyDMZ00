using MyCompass.Views;
using Prism.Modularity;
using Prism.Regions;

namespace MyCompass
{
    public class MyCompassModule : IModule
    {
        RegionManager _regionManager;

        public MyCompassModule(RegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _regionManager.RegisterViewWithRegion("MyCompassRegion", typeof(MyCompassCtrl));
        }
    }
}
