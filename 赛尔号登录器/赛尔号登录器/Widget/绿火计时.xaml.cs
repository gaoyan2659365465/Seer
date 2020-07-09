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
    /// 绿火计时.xaml 的交互逻辑
    /// </summary>
    public partial class 绿火计时 : UserControl
    {
        public 绿火计时()
        {
            InitializeComponent();
            Start();
        }

        public object owner;
        System.Timers.Timer timer1 = new System.Timers.Timer();//计时器
        int time_int = 1800;

        private void Close_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            timer1.Stop();
            if (this.owner != null)
            {
                MainWindow s = (MainWindow)this.owner;
                s.框体3.Child = null;
                this.owner = null;
            }
        }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //按下计时开关，开始30分钟倒计时
            time_int = 1800;
            timer1.Start();
        }

        private void Start()
        {
            string s_seconds = "1";
            decimal d_seconds = decimal.Parse(s_seconds);
            timer1.Interval = (int)(d_seconds * 1000);
            timer1.Elapsed += new System.Timers.ElapsedEventHandler(Tick);
        }

        private void Tick(object sender, EventArgs e)
        {
            //此事件每秒执行一次
            SetContent i = new SetContent(SetWidgetContent);
            this.Dispatcher.Invoke(i, new object[] { });//执行唤醒操作
        }
        public delegate void SetContent();//创建一个代理
        public void SetWidgetContent()
        {
            int i = time_int % 60;
            int i1 = time_int / 60;
            string s = i.ToString();
            if (i==0)
            {
                s = "00";
            }
            时间标签1.Content = i1.ToString() + ":" + s;
            time_int--;
        }
    }
}
