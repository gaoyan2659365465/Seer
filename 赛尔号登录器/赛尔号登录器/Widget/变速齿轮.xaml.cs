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
    /// 变速齿轮.xaml 的交互逻辑
    /// </summary>
    public partial class 变速齿轮 : UserControl
    {
        public 变速齿轮()
        {
            InitializeComponent();
            bool a = Memory.SetSpeed("赛尔号登录器.exe", 2);
        }

        public object owner;
        赛尔号登录器.Memory Memory = new 赛尔号登录器.Memory();//创建内存类实例

        private void Close_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (this.owner != null)
            {
                MainWindow s = (MainWindow)this.owner;
                s.框体.Child = null;
                this.owner = null;
            }
        }
    }
}
