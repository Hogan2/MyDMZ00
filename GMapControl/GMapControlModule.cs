using GMapControl.Views;
using Prism.Modularity;
using Prism.Regions;

namespace GMapControl
{
    public class GMapControlModule : IModule       
    {
        IRegionManager _regionManager;
        public GMapControlModule(RegionManager regionManager)
        {
            _regionManager = regionManager;
        }


        public void Initialize()
        {
            _regionManager.RegisterViewWithRegion("MapRegion", typeof(MyMapCtrlTest));
        }
    }
}
