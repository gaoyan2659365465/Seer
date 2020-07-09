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

namespace 赛尔号登录器.Widget
{
    /// <summary>
    /// 鼠标录制.xaml 的交互逻辑
    /// </summary>
    public partial class 鼠标录制 : UserControl
    {
        public 鼠标录制()
        {
            InitializeComponent();
            MyHookGlobal.SwitchHook.OpenThread();
            MyHookGlobal.SwitchHook.HookHomeEnd1 += SetLabelText;
        }

        public object owner;

        private void Close_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (this.owner != null)
            {
                MainWindow s = (MainWindow)this.owner;
                s.框体.Child = null;
                this.owner = null;
            }
        }

        public void SetLabelText(int b)
        {
            if(b == 1)
            {
                Color color = Color.FromArgb(255,255,255,255);
                Brush brush = new SolidColorBrush(color);
                Label1.Foreground = brush;
                Label1.Content = "按[Home]开始录制";
            }
            else if(b == 2)
            {
                Color color = Color.FromArgb(255, 0, 243, 255);
                Brush brush = new SolidColorBrush(color);
                Label1.Foreground = brush;
                Label1.Content = "按[Home]结束录制";
            }
            else if (b == 3)
            {
                Color color = Color.FromArgb(255, 255, 255, 255);
                Brush brush = new SolidColorBrush(color);
                Label2.Foreground = brush;
                Label2.Content = "按[End]开始播放";
            }
            else if (b == 4)
            {
                Color color = Color.FromArgb(255, 0, 243, 255);
                Brush brush = new SolidColorBrush(color);
                Label2.Foreground = brush;
                Label2.Content = "按[End]结束播放";;
            }
        }
    }
}
