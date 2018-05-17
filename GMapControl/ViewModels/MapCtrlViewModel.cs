using GMapControl.Models;
using Prism.Commands;
using Prism.Mvvm;

namespace GMapControl.ViewModels
{
    public class MapCtrlViewModel : BindableBase
    {
        private ToolTypes _toolType;

        public ToolTypes ToolType
        {
            get { return _toolType; }
            set { SetProperty(ref _toolType, value); }
        }

        private ToolBarTitles _toolBarTitle = new ToolBarTitles();

        public ToolBarTitles ToolBarTitle
        {
            get { return _toolBarTitle; }
            set { SetProperty(ref _toolBarTitle, value); }
        }
        private ToolBarBorders _toolBarBorder = new ToolBarBorders();

        public ToolBarBorders ToolBarBorder
        {
            get { return _toolBarBorder; }
            set { SetProperty(ref _toolBarBorder, value); }
        }

        private void ToolBarTitleInitialize()
        {
            ToolBarTitle.Cursor = "鼠标";
            ToolBarTitle.ZoomIn = "放大地图";
            ToolBarTitle.ZoomOut = "缩小地图";
            ToolBarTitle.AddPoint = "增加航点";
            ToolBarTitle.EditPoint = "编辑航点";
            ToolBarTitle.DeletePoint = "删除航点";
            ToolBarTitle.AutoRoute = "自动航线";
            ToolBarTitle.Focous = "飞机居中";
            ToolBarTitle.Ruler = "距离测量";
            ToolBarTitle.Clear = "清除航迹";
        }
        public DelegateCommand Cursor_Click { get; private set; }
        public DelegateCommand ZoomIn_Click { get; private set; }
        public DelegateCommand ZoomOut_Click { get; private set; }
        public DelegateCommand AddPoint_Click { get; private set; }
        public DelegateCommand EditPoint_Click { get; private set; }
        public DelegateCommand DeletePoint_Click { get; private set; }
        public DelegateCommand AutoRoute_Click { get; private set; }
        public DelegateCommand Ruler_Click { get; private set; }

        public MapCtrlViewModel()
        {
            ToolBarTitleInitialize();
            Cursor_Click = new DelegateCommand(Cursor_Execute, Cursor_CanExecute);
            ZoomIn_Click = new DelegateCommand(ZoomIn_Execut);
            ZoomOut_Click = new DelegateCommand(ZoomOut_Execute);
            AddPoint_Click = new DelegateCommand(AddPoint_Execute);
            EditPoint_Click = new DelegateCommand(EditPoint_Execute);
            DeletePoint_Click = new DelegateCommand(DeletePoint_Execute);
            AutoRoute_Click = new DelegateCommand(AutoRoute_Execute);
            Ruler_Click = new DelegateCommand(Ruler_Execute);
        }
        private void Cursor_Execute()
        {
            ToolType = ToolTypes.toolCursor;
            SetToolBarBorder();
            ToolBarBorder.Cursor = "Black";
        }

        private bool Cursor_CanExecute()
        {
            return true;
        }
        private void ZoomIn_Execut()
        {
            ToolType = ToolTypes.toolZoomIn;
            SetToolBarBorder();
            ToolBarBorder.ZoomIn = "Black";
        }
        private void ZoomOut_Execute()
        {
            ToolType = ToolTypes.toolZoomOut;
            SetToolBarBorder();
            ToolBarBorder.ZoomOut = "Black";
        }
        private void AddPoint_Execute()
        {
            ToolType = ToolTypes.toolAddPoint;
            SetToolBarBorder();
            ToolBarBorder.AddPoint = "Black";
        }
        private void EditPoint_Execute()
        {
            ToolType = ToolTypes.toolEditPoint;
            SetToolBarBorder();
            ToolBarBorder.EditPoint = "Black";
        }
        private void DeletePoint_Execute()
        {
            ToolType = ToolTypes.toolDeletePoint;
            SetToolBarBorder();
            ToolBarBorder.DeletePoint = "Black";
        }
        private void AutoRoute_Execute()
        {
            ToolType = ToolTypes.toolAutoRoute;
            SetToolBarBorder();
            ToolBarBorder.AutoRoute = "Black";
        }

        private void Ruler_Execute()
        {
            ToolType = ToolTypes.toolRuler;
            SetToolBarBorder();
            ToolBarBorder.Ruler = "Black";
        }

        private void SetToolBarBorder()
        {
            ToolBarBorder.Cursor = "Transparent";
            ToolBarBorder.ZoomIn = "Transparent";
            ToolBarBorder.ZoomOut = "Transparent";
            ToolBarBorder.AddPoint = "Transparent";
            ToolBarBorder.EditPoint = "Transparent";
            ToolBarBorder.DeletePoint = "Transparent";
            ToolBarBorder.AutoRoute = "Transparent";
            ToolBarBorder.Ruler = "Transparent";
        }
    }
}
