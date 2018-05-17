using System.Drawing;
using System.Runtime.InteropServices;

namespace GMapControl.ViewModels
{
    public class MouseMonitorHelper
    {
        private static Point mousePosition;    //鼠标的位置
        public static int CheckCount;   //检测鼠标位置的次数

        //判断鼠标是否移动        
        public static bool HaveUsedTo()
        {
            Point point = GetMousePoint();
            if (point == mousePosition) return false;
            mousePosition = point; return true;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MPoint
        {
            public int X;
            public int Y;
            public MPoint(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool GetCursorPos(out MPoint mpt);

        // 获取当前屏幕鼠标位置           
        public static Point GetMousePoint()
        {
            MPoint mpt = new MPoint();
            GetCursorPos(out mpt);
            Point p = new Point(mpt.X, mpt.Y);
            return p;
        }
    }
}
