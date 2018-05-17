using GMap.NET.WindowsPresentation;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GMapControl.Views
{
    /// <summary>
    /// PolygonPointMarker.xaml 的交互逻辑
    /// </summary>
    public partial class PolygonPointMarker : UserControl
    {
        //Popup Popup;
        //Label Label;
        GMapMarker Marker;
        MapCtrl MainWindow;
        public static bool isMarkerMove = false;
        public PolygonPointMarker(MapCtrl window, GMapMarker marker)
        {
            InitializeComponent();

            this.MainWindow = window;
            this.Marker = marker;
            //this.wayPointIndex.Text = title;
            //Popup = new Popup();
            //Label = new Label();

            this.Loaded += new RoutedEventHandler(CustomMarkerDemo_Loaded);
            this.SizeChanged += new SizeChangedEventHandler(CustomMarkerDemo_SizeChanged);
            this.MouseEnter += new MouseEventHandler(MarkerControl_MouseEnter);
            this.MouseLeave += new MouseEventHandler(MarkerControl_MouseLeave);
            this.MouseMove += new MouseEventHandler(CustomMarkerDemo_MouseMove);
            this.MouseLeftButtonUp += new MouseButtonEventHandler(CustomMarkerDemo_MouseLeftButtonUp);
            this.MouseLeftButtonDown += new MouseButtonEventHandler(CustomMarkerDemo_MouseLeftButtonDown);

            //Popup.Placement = PlacementMode.Mouse;
            //{
            //    Label.Background = Brushes.Blue;
            //    Label.Foreground = Brushes.White;
            //    Label.BorderBrush = Brushes.WhiteSmoke;
            //    Label.BorderThickness = new Thickness(1);
            //    Label.Padding = new Thickness(3);
            //    Label.FontSize = 18;
            //    Label.FontFamily = new FontFamily("微软雅黑");
            //    Label.Content = title;
            //}
            //Popup.Child = Label;
        }
        void CustomMarkerDemo_Loaded(object sender, RoutedEventArgs e)
        {
            if (RedMarker.Source.CanFreeze)
            {
                RedMarker.Source.Freeze();
            }
        }

        void CustomMarkerDemo_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Marker.Offset = new Point(-e.NewSize.Width / 2, -e.NewSize.Height);
        }

        void CustomMarkerDemo_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && IsMouseCaptured && isMarkerMove)
            {
                Point p = e.GetPosition(MainWindow.GMapCtrl);
                Marker.Position = MainWindow.GMapCtrl.FromLocalToLatLng((int)p.X, (int)p.Y);
                //isMarkerMove = true;
            }
        }

        void CustomMarkerDemo_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!IsMouseCaptured)
            {
                Mouse.Capture(this);
                //Popup.IsOpen = false;
            }
        }

        void CustomMarkerDemo_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isMarkerMove = false;

            if (IsMouseCaptured)
            {
                Mouse.Capture(null);
                //Popup.IsOpen = true;
            }
        }

        void MarkerControl_MouseLeave(object sender, MouseEventArgs e)
        {
            Marker.ZIndex -= 200;
            //Popup.IsOpen = false;
        }

        void MarkerControl_MouseEnter(object sender, MouseEventArgs e)
        {
            Marker.ZIndex += 200;
            //Popup.IsOpen = true;
        }
    }
}
