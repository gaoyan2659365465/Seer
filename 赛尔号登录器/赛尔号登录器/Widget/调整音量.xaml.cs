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
    /// 调整音量.xaml 的交互逻辑
    /// </summary>
    public partial class 调整音量 : UserControl
    {
        public 调整音量()
        {
            InitializeComponent();
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

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MainWindow s = (MainWindow)this.owner;
            s.Slider_ValueChanged((uint)Slider1.Value);
        }
    }
}
