using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using GMapControl.Models;
using GMapControl.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GMapControl.Views
{
    /// <summary>
    /// MapCtrl.xaml 的交互逻辑
    /// </summary>
    public partial class MapCtrl : UserControl
    {
        public List<PointLatLng> waypoints = new List<PointLatLng>();
        public List<PointLatLng> dispoints = new List<PointLatLng>();
        //public PointLatLng PlanePoint;

        private string LatLng = "";
        private DispatcherTimer Timer_MouseMove;
        private CursorConvert cursorConvert = new CursorConvert();
        private GMapMarker AeroPlane;
        private List<PointLatLng> coursepoints = new List<PointLatLng>();
        private List<PointLatLng> polygonpoints = new List<PointLatLng>();

        private static void GestureChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MapCtrl)d).InvalidateVisual();
        }
        public double PlaneAngle
        {
            get { return (double)GetValue(PlaneAngleProperty); }
            set { SetValue(PlaneAngleProperty, value); }
        }
        public static readonly DependencyProperty PlaneAngleProperty =
            DependencyProperty.Register("PlaneAngle", typeof(double), typeof(MapCtrl), new FrameworkPropertyMetadata(
                (double)0, GestureChangedCallback));

        public double PlanePosition_Lat
        {
            get { return (double)GetValue(PlanePosition_LatProperty); }
            set { SetValue(PlanePosition_LatProperty, value); }
        }
        public static readonly DependencyProperty PlanePosition_LatProperty =
            DependencyProperty.Register("PlanePosition_Lat", typeof(double), typeof(MapCtrl), new FrameworkPropertyMetadata(
                (double)30.663, GestureChangedCallback));
        public double PlanePosition_Lng
        {
            get { return (double)GetValue(PlanePosition_LngProperty); }
            set { SetValue(PlanePosition_LngProperty, value); }
        }
        public static readonly DependencyProperty PlanePosition_LngProperty =
            DependencyProperty.Register("PlanePosition_Lng", typeof(double), typeof(MapCtrl), new FrameworkPropertyMetadata(
                (double)104.067, GestureChangedCallback));

        public MapCtrl()
        {
            InitializeComponent();
            InitializeMapCtrl();
        }
        private void InitializeMapCtrl()
        {
            //GMapCtrl.CacheLocation = @"F:\MapDownloader\MapCache";
            GMapCtrl.MapProvider = GoogleChinaSatelliteMapProvider.Instance;
            GMapCtrl.Manager.Mode = AccessMode.CacheOnly;
            GMapCtrl.Position = new PointLatLng(30.6898, 103.9468);
            GMapCtrl.ShowCenter = false;
            GMapCtrl.MouseWheelZoomType = MouseWheelZoomType.MousePositionWithoutCenter;
            GMapCtrl.Zoom = 14;
            GMapCtrl.DragButton = MouseButton.Right;
            LatLngText.Text = GMapCtrl.Position.Lng.ToString("0.000000000000") + "\n"
                + GMapCtrl.Position.Lat.ToString("0.0000000000000");
            myToolBar.Visibility = Visibility.Hidden;
            ///鼠标空闲
            this.Timer_MouseMove = new DispatcherTimer();
            this.Timer_MouseMove.Tick += new EventHandler(Timer_MouseMove_Tick);
            this.Timer_MouseMove.Interval = new TimeSpan(0, 0, 2);
            this.Timer_MouseMove.Start();

            //PlanePoint = new PointLatLng(30.6898, 103.9468);
            //PlaneAngle = 25.0;
            AeroPlane = new GMapMarker(new PointLatLng(PlanePosition_Lat, PlanePosition_Lng));
            {
                AeroPlane.ZIndex = 30000;
                AeroPlane.Shape = new Plane();
                AeroPlane.Shape.RenderTransform = new RotateTransform(PlaneAngle, 24, 24);
                AeroPlane.Offset = new Point(-24, -24);
                GMapCtrl.Markers.Add(AeroPlane);
            }
            //coursepoints.Add(new PointLatLng(PlanePosition_Lat, PlanePosition_Lng));
        }
        /// <summary>
        /// 鼠标空闲事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_MouseMove_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!MouseMonitorHelper.HaveUsedTo())
                {
                    MouseMonitorHelper.CheckCount++;
                    if (MouseMonitorHelper.CheckCount == 3)
                    {
                        MouseMonitorHelper.CheckCount = 0;
                        // toobar隐藏、鼠标隐藏
                        this.myToolBar.Visibility = Visibility.Hidden;
                        //Mouse.OverrideCursor = Cursors.None;
                    }
                }
                else MouseMonitorHelper.CheckCount = 0;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        private void GMapCtrl_MouseMove(object sender, MouseEventArgs e)
        {
            Point p = e.GetPosition(GMapCtrl);
            LatLng = GMapCtrl.FromLocalToLatLng((int)p.X, (int)p.Y).Lng.ToString("0.000000000000") + "\n" +
                GMapCtrl.FromLocalToLatLng((int)p.X, (int)p.Y).Lat.ToString("0.0000000000000");
            myToolBar.Visibility = Visibility.Visible;
            //Mouse.OverrideCursor = Cursors.Arrow;
            Marker_Red.isMarkerMove = false;
            PolygonPointMarker.isMarkerMove = false;

            if ((ToolTypes)myToolBar.Tag == ToolTypes.toolRuler)
            {
                //GMapCtrl.Cursor = Cursors.Arrow;
                GMapCtrl.Cursor = cursorConvert.ConvertToCursor(new Pin_Marker(), new Point(16, 32));
            }
            if ((ToolTypes)myToolBar.Tag == ToolTypes.toolZoomIn)
            {
                LatLngText.Text = LatLng;
                DistanceMarkerRemove();
                GMapCtrl.Cursor = cursorConvert.ConvertToCursor(new Cursor_ZoomIn(), new Point(12, 12));

            }
            if ((ToolTypes)myToolBar.Tag == ToolTypes.toolZoomOut)
            {
                LatLngText.Text = LatLng;
                DistanceMarkerRemove();
                GMapCtrl.Cursor = cursorConvert.ConvertToCursor(new Cursor_ZoomOut(), new Point(12, 12));
            }
            if ((ToolTypes)myToolBar.Tag == ToolTypes.toolCursor)
            {
                LatLngText.Text = LatLng;
                DistanceMarkerRemove();
                GMapCtrl.Cursor = Cursors.Arrow;
            }
            if ((ToolTypes)myToolBar.Tag == ToolTypes.toolAddPoint)
            {
                LatLngText.Text = LatLng;
                DistanceMarkerRemove();
                GMapCtrl.Cursor = cursorConvert.ConvertToCursor(new Cursor_Route(), new Point(16, 32));
            }
            if ((ToolTypes)myToolBar.Tag == ToolTypes.toolEditPoint)
            {
                LatLngText.Text = LatLng;
                DistanceMarkerRemove();
                GMapCtrl.Cursor = Cursors.Arrow;
                Marker_Red.isMarkerMove = true;

                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    waypoints.Clear();
                    WayPointsRouteRemove();
                    var reload_WayPointMakers = GMapCtrl.Markers.Where(q => q != null);

                    for (int i = 0; i < reload_WayPointMakers.Count(); i++)
                    {
                        if (reload_WayPointMakers.ElementAt(i).ZIndex < 3000)
                            waypoints.Add(reload_WayPointMakers.ElementAt(i).Position);
                        //i--;
                    }

                    GMapRoute reload_WayPointsRoute = new GMapRoute(waypoints);
                    {
                        reload_WayPointsRoute.ZIndex = waypoints.Count();
                        reload_WayPointsRoute.Shape = new Path() { Stroke = new SolidColorBrush(Colors.Lime), StrokeThickness = 4 };
                        GMapCtrl.Markers.Add(reload_WayPointsRoute);
                    }
                }
            }
            if ((ToolTypes)myToolBar.Tag == ToolTypes.toolAutoRoute)
            {
                LatLngText.Text = LatLng;
                DistanceMarkerRemove();
                GMapCtrl.Cursor = Cursors.Arrow;
                PolygonPointMarker.isMarkerMove = true;

                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    polygonpoints.Clear();
                    PolygonRouteRemove();
                    var reload_PolygonPointMakers = GMapCtrl.Markers.Where(q => q != null);

                    for (int i = 0; i < reload_PolygonPointMakers.Count(); i++)
                    {
                        if (reload_PolygonPointMakers.ElementAt(i).ZIndex > 6000&&reload_PolygonPointMakers.ElementAt(i).ZIndex < 6500)
                            polygonpoints.Add(reload_PolygonPointMakers.ElementAt(i).Position);
                        //i--;
                    }

                    GMapPolygon reload_PolygonPointsRoute = new GMapPolygon(polygonpoints);
                    {
                        reload_PolygonPointsRoute.ZIndex = 6000;
                        reload_PolygonPointsRoute.Shape = new Path() { Stroke = new SolidColorBrush(Colors.Lime), StrokeThickness = 4 };
                        GMapCtrl.Markers.Add(reload_PolygonPointsRoute);
                    }
                }
            }
            if ((ToolTypes)myToolBar.Tag == ToolTypes.toolDeletePoint)
            {
                LatLngText.Text = LatLng;
                DistanceMarkerRemove();
                GMapCtrl.Cursor = Cursors.Arrow;
            }
            if ((ToolTypes)myToolBar.Tag == ToolTypes.toolAutoRoute)
            {
                LatLngText.Text = LatLng;
                DistanceMarkerRemove();
                GMapCtrl.Cursor = Cursors.Arrow;
            }
        }

        private void GMapCtrl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(GMapCtrl);
            PointLatLng newWayPoint = GMapCtrl.FromLocalToLatLng((int)p.X, (int)p.Y);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if ((ToolTypes)myToolBar.Tag == ToolTypes.toolAddPoint)
                {
                    waypoints.Add(newWayPoint);

                    GMapMarker WayPointMarker = new GMapMarker(newWayPoint);
                    {
                        WayPointMarker.ZIndex = waypoints.Count() + 1000;
                        WayPointMarker.Shape = new Marker_Red(this, WayPointMarker, (WayPointMarker.ZIndex-1000).ToString());
                        WayPointMarker.Offset = new Point(-16, -16);

                        GMapCtrl.Markers.Add(WayPointMarker);
                    }
                    if (waypoints.Count() > 1)
                    {
                        GMapRoute WayPointsRoute = new GMapRoute(waypoints);
                        {
                            WayPointsRoute.ZIndex = waypoints.Count();
                            WayPointsRoute.Shape = new Path() { Stroke = new SolidColorBrush(Colors.Lime), StrokeThickness = 4 };
                            WayPointsRouteRemove();
                            GMapCtrl.Markers.Add(WayPointsRoute);
                        }
                    }
                }

                if ((ToolTypes)myToolBar.Tag == ToolTypes.toolAutoRoute)
                {
                    polygonpoints.Add(newWayPoint);

                    GMapMarker PolygonPointMarker = new GMapMarker(newWayPoint);
                    {
                        PolygonPointMarker.ZIndex = polygonpoints.Count() + 6000;
                        PolygonPointMarker.Shape = new PolygonPointMarker(this, PolygonPointMarker);
                        PolygonPointMarker.Offset = new Point(-16, -16);

                        GMapCtrl.Markers.Add(PolygonPointMarker);
                    }
                    if (polygonpoints.Count() > 2)
                    {
                        GMapPolygon myPolygon = new GMapPolygon(polygonpoints);
                        {
                            myPolygon.ZIndex = 6000;
                            myPolygon.Shape = new Path() { Stroke = new SolidColorBrush(Colors.Lime), StrokeThickness = 4 };
                            PolygonRouteRemove();
                            GMapCtrl.Markers.Add(myPolygon);
                        }
                    }
                }

                if ((ToolTypes)myToolBar.Tag == ToolTypes.toolDeletePoint)
                {
                    WayPointsMaker_RouteRemove();

                    GMapRoute reloadWayPointsRoute = new GMapRoute(waypoints);
                    {
                        reloadWayPointsRoute.ZIndex = waypoints.Count();
                        reloadWayPointsRoute.Shape = new Path() { Stroke = new SolidColorBrush(Colors.Lime), StrokeThickness = 4 };
                        GMapCtrl.Markers.Add(reloadWayPointsRoute);
                    }
                    for (int i = 0; i < waypoints.Count(); i++)
                    {
                        GMapMarker reloadWayPointsMarker = new GMapMarker(waypoints.ElementAt(i));
                        {
                            reloadWayPointsMarker.ZIndex = i + 1001;
                            reloadWayPointsMarker.Shape = new Marker_Red(this, reloadWayPointsMarker, (reloadWayPointsMarker.ZIndex-1000).ToString());
                            reloadWayPointsMarker.Offset = new Point(-16, -16);
                            GMapCtrl.Markers.Add(reloadWayPointsMarker);
                        }
                    }
                }
            }

        }
        public void WayPointsMaker_RouteRemove()
        {
            var clear = GMapCtrl.Markers.Where(p => p != null);
            for (int i = 0; i < clear.Count(); i++)
            {
                if (clear.ElementAt(i).ZIndex < 3000)
                {
                    if (clear.ElementAt(i).ZIndex > 2000)
                    {
                        waypoints.Remove(clear.ElementAt(i).Position);
                    }
                    GMapCtrl.Markers.Remove(clear.ElementAt(i));
                    i--;
                }

            }

        }
        private void PolygonRouteRemove()
        {
            var clear = GMapCtrl.Markers.Where(p => p != null && p.ZIndex == 6000);
            if (clear != null)
            {
                for (int i = 0; i < clear.Count(); i++)
                {
                    GMapCtrl.Markers.Remove(clear.ElementAt(i));
                    i--;
                }
            }
        }
        public void WayPointsRouteRemove()
        {
            var WayPointsRoute_clear = GMapCtrl.Markers.Where(p => p != null && p.ZIndex <= 1000);
            if (WayPointsRoute_clear != null)
            {
                for (int i = 0; i < WayPointsRoute_clear.Count(); i++)
                {
                    GMapCtrl.Markers.Remove(WayPointsRoute_clear.ElementAt(i));
                    i--;
                }
            }
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            Thread.Sleep(1000);
            myToolBar.Visibility = Visibility.Hidden;
        }

        private void GMapCtrl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if ((ToolTypes)myToolBar.Tag == ToolTypes.toolZoomIn && GMapCtrl.Zoom < GMapCtrl.MaxZoom)
            {
                GMapCtrl.Zoom++;
            }
            if ((ToolTypes)myToolBar.Tag == ToolTypes.toolZoomOut && GMapCtrl.Zoom > GMapCtrl.MinZoom)
            {
                GMapCtrl.Zoom--;
            }
            if ((ToolTypes)myToolBar.Tag == ToolTypes.toolRuler)
            {
                Point p = e.GetPosition(GMapCtrl);
                PointLatLng newPoint = GMapCtrl.FromLocalToLatLng((int)p.X, (int)p.Y);
                dispoints.Add(newPoint);
                if (dispoints.Count() == 1)
                {
                    DistanceMarkerRemove();
                }
                GMapMarker currentMarker = new GMapMarker(newPoint);
                {
                    currentMarker.ZIndex = dispoints.Count() + 3100;
                    currentMarker.Shape = new Pin_Marker();
                    currentMarker.Offset = new Point(-16, -32);
                    GMapCtrl.Markers.Add(currentMarker);
                }
                if (dispoints.Count() == 2)
                {
                    GMapRoute distanceRoute = new GMapRoute(dispoints);
                    {
                        distanceRoute.ZIndex =  3000;
                        distanceRoute.Shape = new Path() { Stroke = new SolidColorBrush(Colors.Yellow), StrokeThickness = 3 };
                        GMapCtrl.Markers.Add(distanceRoute);
                        LatLngText.Text = "距离：" + "\n" + GMapCtrl.MapProvider.Projection.GetDistance(dispoints[1], dispoints[0]).ToString("f4") + "KM";
                        dispoints.Clear();
                    }
                }
            }
        }
        private void DistanceMarkerRemove()
        {
            var clear = GMapCtrl.Markers.Where(p => p != null && p.ZIndex == 3000 || p.ZIndex == 3102 || p.ZIndex == 3101);
            if (clear != null)
            {
                for (int i = 0; i < clear.Count(); i++)
                {
                    GMapCtrl.Markers.Remove(clear.ElementAt(i));
                    i--;
                }
            }
        }
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            AeroPlane.Shape.RenderTransform = new RotateTransform(PlaneAngle, 24, 24);
            AeroPlane.Position = new PointLatLng(PlanePosition_Lat, PlanePosition_Lng);
            coursepoints.Add(AeroPlane.Position);
            Curse_Remove();
            GMapRoute curseRoute = new GMapRoute(coursepoints);
            {
                curseRoute.ZIndex = 5000;
                curseRoute.Shape = new Path() { Stroke = new SolidColorBrush(Colors.Blue), StrokeThickness = 3 };
                GMapCtrl.Markers.Add(curseRoute);
            }
        }
        private void Curse_Remove()
        {
            var clear = GMapCtrl.Markers.Where(p => p != null && p.ZIndex == 5000);
            if (clear != null)
            {
                for (int i = 0; i < clear.Count(); i++)
                {
                    GMapCtrl.Markers.Remove(clear.ElementAt(i));
                    i--;
                }
            }
        }
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            coursepoints.Clear();
            Curse_Remove();
        }

        private void Focus_Click(object sender, RoutedEventArgs e)
        {
            //GMapCtrl.ZoomAndCenterMarkers(null);//缩放有问题
            GMapCtrl.Position = AeroPlane.Position;
        }
    }
}
