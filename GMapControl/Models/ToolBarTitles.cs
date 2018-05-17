using Prism.Mvvm;

namespace GMapControl.Models
{
    public enum ToolTypes
    {
        toolCursor,
        toolZoomIn,
        toolZoomOut,
        toolAddPoint,
        toolEditPoint,
        toolDeletePoint,
        toolAutoRoute,
        toolRuler
    }
    public class ToolBarTitles : BindableBase
    {
        private string _cursor;

        public string Cursor
        {
            get { return _cursor; }
            set { SetProperty(ref _cursor, value); }
        }
        private string _zoomin;

        public string ZoomIn
        {
            get { return _zoomin; }
            set { SetProperty(ref _zoomin, value); }
        }
        private string _zoomout;

        public string ZoomOut
        {
            get { return _zoomout; }
            set { SetProperty(ref _zoomout, value); }
        }
        private string _addPoint;

        public string AddPoint
        {
            get { return _addPoint; }
            set { SetProperty(ref _addPoint, value); }
        }

        private string _editPoint;

        public string EditPoint
        {
            get { return _editPoint; }
            set { SetProperty(ref _editPoint, value); }
        }
        private string _deletePoint;

        public string DeletePoint
        {
            get { return _deletePoint; }
            set { SetProperty(ref _deletePoint, value); }
        }

        private string _autoRoute;

        public string AutoRoute
        {
            get { return _autoRoute; }
            set { SetProperty(ref _autoRoute, value); }
        }
        private string _focous;

        public string Focous
        {
            get { return _focous; }
            set { SetProperty(ref _focous, value); }
        }
        private string _ruler;

        public string Ruler
        {
            get { return _ruler; }
            set { SetProperty(ref _ruler, value); }
        }
        private string _clear;

        public string Clear
        {
            get { return _clear; }
            set { SetProperty(ref _clear, value); }
        }

        public ToolBarTitles()
        {
        }
    }
}
