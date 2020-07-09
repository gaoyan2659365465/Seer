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
using System.Runtime.InteropServices; //引入动态链接库

namespace 赛尔号登录器
{
    /// <summary>
    /// 鼠标连点器.xaml 的交互逻辑
    /// </summary>
    public partial class 鼠标连点器 : UserControl
    {
        public 鼠标连点器()
        {
            InitializeComponent();
        }

        public object owner;

        [DllImport("鼠标连点器.dll")]
        public static extern void MouseLeftClick();//动态链接库中方法
        [DllImport("鼠标连点器.dll")]
        public static extern void Stop();//动态链接库中方法
        鼠标连点器库.MouseClick i = new 鼠标连点器库.MouseClick();

        private void Label_Initialized(object sender, EventArgs e)
        {
            //开始的时候激活钩子，此时按Home键可以实现连点
            i.MouseLeftClick();
        }

        private void CloseHook()
        {
            if(i.GetHookValue()!=0)
            {
                i.MouseLeftClick();
            }
            i.Stop();
        }

        private void Close_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(this.owner != null)
            {
                CloseHook();

                MainWindow s = (MainWindow)this.owner;
                s.框体.Child = null;
                this.owner = null;
            }
        }
    }
}
