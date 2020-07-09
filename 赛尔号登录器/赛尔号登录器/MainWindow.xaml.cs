using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.InteropServices;

namespace 赛尔号登录器
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            f.OpenFiddler();
        }

        #region 窗口布局
        //点击关闭窗口
        private void Border_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }
        //点击最小化
        private void Border_MouseLeftButtonDown_2(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        #endregion

        #region 拖动窗口控制
        [DllImport("user32.dll")]
        public static extern IntPtr WindowFromPoint(POINT Point);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetCursorPos(out POINT lpPoint);

        [DllImportAttribute("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
        }

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        //拖动窗口
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            POINT curPos;
            IntPtr hWndPopup;

            GetCursorPos(out curPos);
            hWndPopup = WindowFromPoint(curPos);

            ReleaseCapture();
            SendMessage(hWndPopup, WM_NCLBUTTONDOWN, new IntPtr(HT_CAPTION), IntPtr.Zero);
        }
        #endregion

        public MyFiddler f = new MyFiddler();
        private Web web = new Web();
        private void Window_Initialized(object sender, EventArgs e)
        {
            this.Form.Child = this.web;
        }

        private void 功能按钮_MouseEnter(object sender, MouseEventArgs e)
        {
            MenuItem a = (MenuItem)sender;
            Brush br = new SolidColorBrush(Color.FromArgb(125,130,203,182));
            a.Background = br;
        }

        private void 功能按钮_MouseLeave(object sender, MouseEventArgs e)
        {
            MenuItem a = (MenuItem)sender;
            Brush br = new SolidColorBrush(Color.FromArgb(255, 85, 178, 151));
            a.Background = br;
        }

        #region 工具
        public void Slider_ValueChanged(uint a)
        {
            //调整音量，在其他界面被调用
            this.web.Volume_Load(a);
        }

        private void 刷新_Click(object sender, RoutedEventArgs e)
        {
            this.web.SetWebRefresh();
        }

        private void 鼠标连点器_Click(object sender, RoutedEventArgs e)
        {
            鼠标连点器 s = new 鼠标连点器();
            s.owner = this;
            框体.Child = s;
        }

        private void 调整音量_Click(object sender, RoutedEventArgs e)
        {
            赛尔号登录器.Widget.调整音量 s = new 赛尔号登录器.Widget.调整音量();
            s.owner = this;
            框体.Child = s;
        }

        private void 血量查看_Click(object sender, RoutedEventArgs e)
        {
            赛尔号登录器.Widget.血量查看 s = new 赛尔号登录器.Widget.血量查看();
            s.owner = this;
            框体2.Child = s;
        }
        #endregion

        private void 绿火计时_Click(object sender, RoutedEventArgs e)
        {
            赛尔号登录器.Widget.绿火计时 s = new 赛尔号登录器.Widget.绿火计时();
            s.owner = this;
            框体3.Child = s;
        }

        private void 变速齿轮_Click(object sender, RoutedEventArgs e)
        {
            赛尔号登录器.Widget.变速齿轮 s = new 赛尔号登录器.Widget.变速齿轮();
            s.owner = this;
            框体4.Child = s;
        }

        private void 菜单子项2_Click(object sender, RoutedEventArgs e)
        {
            StartExe.Startexe start1 = new StartExe.Startexe();
            string exe_path = "工具\\修改注册表.exe";  // 被调exe
            string[] the_args = { "1", "2", "3", "4" };   // 被调exe接受的参数
            start1.StartProcess(exe_path, the_args);
        }

        private void 录制鼠标_Click(object sender, RoutedEventArgs e)
        {
            赛尔号登录器.Widget.鼠标录制 s = new 赛尔号登录器.Widget.鼠标录制();
            s.owner = this;
            框体.Child = s;
        }

        private void 菜单子项2_Click_1(object sender, RoutedEventArgs e)
        {
            赛尔号登录器.Widget.代理器 s = new 赛尔号登录器.Widget.代理器();
            s.owner = this;
            框体5.Child = s;
        }
    }
}
