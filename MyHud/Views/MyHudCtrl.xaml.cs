using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MyHud.Views
{
    /// <summary>
    /// MyHudCtrl.xaml 的交互逻辑
    /// </summary>
    public partial class MyHudCtrl : UserControl
    {
        public MyHudCtrl()
        {
            InitializeComponent();
            MaxRollAngle = 60;
        }
        private static void GestureChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MyHudCtrl)d).InvalidateVisual();
        }
        public double RollAngle
        {
            get { return (double)GetValue(RollAngleProperty); }
            set { SetValue(RollAngleProperty, value); }
        }
        public static readonly DependencyProperty RollAngleProperty =
            DependencyProperty.Register("RollAngle", typeof(double), typeof(MyHudCtrl), new FrameworkPropertyMetadata(
                (double)0, GestureChangedCallback));
        public double MaxRollAngle
        {
            get { return (double)GetValue(MaxRollAngleProperty); }
            set { SetValue(MaxRollAngleProperty, value); }
        }
        public static readonly DependencyProperty MaxRollAngleProperty =
            DependencyProperty.Register("MaxRollAngle", typeof(double), typeof(MyHudCtrl), new FrameworkPropertyMetadata(
                (double)180, GestureChangedCallback));
        public double PitchAngle
        {
            get { return (double)GetValue(PitchAngleProperty); }
            set { SetValue(PitchAngleProperty, value); }
        }
        public static readonly DependencyProperty PitchAngleProperty =
            DependencyProperty.Register("PitchAngle", typeof(double), typeof(MyHudCtrl), new FrameworkPropertyMetadata((double)0, GestureChangedCallback), PitchAngleValidateValueCallback);
        private static bool PitchAngleValidateValueCallback(object value)
        {
            bool v = (double)value >= -90 && (double)value <= 90;
            if (!v) throw new Exception("俯仰角必须在-90~90度之间");
            return v;
        }
        private void DrawRollTick(double cycleR, double angle)
        {
            Line line = new Line();
            line.X1 = 0;
            line.X2 = 0;
            line.Y1 = 8;
            line.Y2 = 0;
            line.Stroke = Brushes.White;
            line.StrokeThickness = 2;
            Canvas.SetTop(line, -cycleR);
            line.RenderTransform = new RotateTransform(angle, 0, cycleR);
            Canvas_ViewPortMiddle.Children.Add(line);
            var textblock = new BorderTextLabel();
            textblock.Width = 14;
            textblock.Stroke = Brushes.DimGray;
            textblock.HorizontalContentAlignment = HorizontalAlignment.Center;
            textblock.Text = angle.ToString("##0");
            textblock.Foreground = Brushes.White;
            textblock.FontSize = 10;
            textblock.FontWeight = FontWeights.Light;
            Canvas.SetTop(textblock, -cycleR + 10);
            Canvas.SetLeft(textblock, -textblock.Width / 2);
            textblock.RenderTransform = new RotateTransform(angle, textblock.Width / 2, cycleR - 10);
            Canvas_ViewPortMiddle.Children.Add(textblock);
        }
        private void DrawPitchTick(double pitch, double offset)
        {
            Line line = new Line();
            line.X1 = 0;
            line.X2 = 30;
            line.Y1 = 0;
            line.Y2 = 0;
            line.Stroke = Brushes.White;
            line.StrokeThickness = 2;
            Canvas.SetLeft(line, -15);
            Canvas.SetTop(line, offset);
            Canvas_ViewPortMiddle.Children.Add(line);
            var textblock_left = new BorderTextLabel();
            var textblock_right = new BorderTextLabel();

            textblock_left.Width = 14;
            textblock_left.Stroke = Brushes.DimGray;
            textblock_left.HorizontalContentAlignment = HorizontalAlignment.Center;
            if (pitch != 0) textblock_left.Text = pitch.ToString("##0");
            textblock_left.Foreground = Brushes.White;
            textblock_left.FontSize = 10;
            textblock_left.FontWeight = FontWeights.Light;

            textblock_right.Width = 14;
            textblock_right.Stroke = Brushes.DimGray;
            textblock_right.HorizontalContentAlignment = HorizontalAlignment.Center;
            if (pitch != 0) textblock_right.Text = pitch.ToString("##0");
            textblock_right.Foreground = Brushes.White;
            textblock_right.FontSize = 10;
            textblock_right.FontWeight = FontWeights.Light;

            Canvas.SetTop(textblock_left, offset - 6);
            Canvas.SetLeft(textblock_left, -textblock_left.Width - 22);
            Canvas_ViewPortMiddle.Children.Add(textblock_left);

            Canvas.SetTop(textblock_right, offset - 6);
            Canvas.SetLeft(textblock_right, 22);
            Canvas_ViewPortMiddle.Children.Add(textblock_right);
        }
        private void DrawShortPitchTick(double offset)
        {
            Line line = new Line();
            line.X1 = 0;
            line.X2 = 20;
            line.Y1 = 0;
            line.Y2 = 0;
            line.Stroke = Brushes.White;
            line.StrokeThickness = 1;
            Canvas.SetLeft(line, -10);
            Canvas.SetTop(line, offset);
            Canvas_ViewPortMiddle.Children.Add(line);
        }
        private void DrawRollPitchCycle(double offset1)
        {
            Canvas_ViewPortMiddle.Children.Clear();
            double cycleR = Grid_Virwport.ActualWidth / 2 - 5;
            int tickangle = 15;
            DrawRollTick(cycleR, 0);
            for (int angle = tickangle; angle <= MaxRollAngle; angle += tickangle)
            {
                DrawRollTick(cycleR, angle);
                DrawRollTick(cycleR, -angle);
            }

            #region 俯仰角
            int pitch = (((int)PitchAngle) / 10) * 10;
            int maxPitchRange = 48;
            if (offset1 >= -maxPitchRange && offset1 <= maxPitchRange)
            {
                DrawPitchTick(0, offset1);

            }
            for (int i = 30; i < 100; i = i + 30)
            {
                if (offset1 - i >= -maxPitchRange && offset1 - i <= maxPitchRange)
                    DrawPitchTick(i, offset1 - i);

                if (offset1 + i >= -maxPitchRange && offset1 + i <= maxPitchRange)
                    DrawPitchTick(-i, offset1 + i);
            }
            for (int j = 15; j < 100; j = j + 15)
            {
                if (j % 2 == 1 && offset1 - j >= -maxPitchRange && offset1 - j <= maxPitchRange)
                    DrawShortPitchTick(offset1 - j);

                if (j % 2 == 1 && offset1 + j >= -maxPitchRange && offset1 + j <= maxPitchRange)
                    DrawShortPitchTick(offset1 + j);
            }
            #endregion

            Canvas_ViewPortMiddle.RenderTransform = new RotateTransform(-RollAngle);


        }

        protected virtual void RedrawRoll()
        {
            var bkbrush = (LinearGradientBrush)Grid_Virwport.Fill;
            double roll = (RollAngle % 360) * Math.PI / 180;
            if (roll > Math.PI) roll = Math.PI * 2 - roll;
            if (roll < -Math.PI) roll = Math.PI * 2 + roll;

            double offset = 0.5;
            double pitch = PitchAngle * Math.PI / 180;
            if (pitch > Math.PI / 2) pitch = Math.PI / 2;//设置俯仰视觉为120度
            if (pitch < -Math.PI / 2) pitch = -Math.PI / 2;
            offset = pitch / Math.PI + 0.5;
            double offset1 = offset * 180 - 90;
            double diameter = Grid_Virwport.ActualWidth;

            double startx = 0, starty = 0;
            startx = diameter/2 * (1 - Math.Sin(roll));
            starty = diameter / 2 * (1 - Math.Cos(roll));

            bkbrush.StartPoint = new Point(startx, starty);
            bkbrush.EndPoint = new Point(diameter - startx, diameter - starty);

            bkbrush.GradientStops[1].Offset = offset;
            bkbrush.GradientStops[2].Offset = offset;

            DrawRollPitchCycle(offset1);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            RedrawRoll();
        }
    }
}
