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
using System.Threading;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;//进程类
using System.Runtime.InteropServices; //引入动态链接库


namespace 赛尔号登录器.Widget
{
    /// <summary>
    /// 血量查看.xaml 的交互逻辑
    /// </summary>
    public partial class 血量查看 : UserControl
    {
        unsafe public 血量查看()
        {
            InitializeComponent();
            Start();//界面生成时就开始计时器
        }

        public object owner;
        System.Timers.Timer timer1 = new System.Timers.Timer();//计时器，用来查看血量
        赛尔号登录器.HP HP = new 赛尔号登录器.HP();//创建HP实例

        private void Close_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (this.owner != null)
            {
                MainWindow s = (MainWindow)this.owner;
                s.框体2.Child = null;
                this.owner = null;
            }
        }

        private void Start()
        {
            string s_seconds = "1";
            decimal d_seconds = decimal.Parse(s_seconds);
            timer1.Interval = (int)(d_seconds * 1000);
            timer1.Elapsed += new System.Timers.ElapsedEventHandler(Tick);
            timer1.Start();
        }

        private void Tick(object sender, EventArgs e)
        {
            //此事件每秒执行一次，从内存地址中获取血量



            Hook_HP();



            SetContent i = new SetContent(SetWidgetContent);
            this.Dispatcher.Invoke(i, new object[] {  });//执行唤醒操作
        }
        public delegate void SetContent();//创建一个代理
        public void SetWidgetContent()
        {
            //委托函数，用来更新UI
            AHP.Content = hp1.ToString() + "/" + HP.GetHP_Value(HP.GetHP_Addr(1)+4).ToString();
            BHP.Content = hp2.ToString() + "/" + HP.GetHP_Value(HP.GetHP_Addr(2)+4).ToString(); ;

            TextBox1.Text = Hook_a.ToString();
            TextBox2.Text = Hook_b.ToString();
            TextBox3.Text = Hook_addr.ToString();
            TextBox4.Text = Hook_c.ToString();
        }

        public static uint Hook_a;
        public static bool Hook_b;
        public static uint Hook_addr;
        public static bool Hook_c;
        public static uint hp1;
        public static uint hp2;

        public static void Hook_HP()
        {
            if (Hook_a == 0)
            {
                Hook_a = HP.GetHP_FeatureAddr();
                if (Hook_a == 0) { return; }
            }
            if(Hook_b == false)
            {
                Hook_b = HP.SetHP_HookCall(1);//自动改Call
                if (Hook_b == false) { return; }
            }
            if(Hook_addr == 0)
            {
                Hook_addr = HP.GetHP_Initialize_Addr();
                if (Hook_addr == 0) { return; }
            }
            if(Hook_c == false)
            {
                Hook_c = HP.SetHP_HOOK_HP_Addr(Hook_addr);
                if (Hook_c == false) { return; }
            }
            hp1 = HP.GetHP_Value(HP.GetHP_Addr(1));
            hp2 = HP.GetHP_Value(HP.GetHP_Addr(2));
        }
    }
}
