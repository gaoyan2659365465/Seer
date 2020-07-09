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
using System.Threading;
using System.Collections;

namespace 赛尔号登录器.Widget
{
    /// <summary>
    /// 代理器.xaml 的交互逻辑
    /// </summary>
    public partial class 代理器 : UserControl
    {
        public 代理器()
        {
            InitializeComponent();
            Thread th = new Thread(OpenThread);
            th.Start();
        }
        #region 默认
        public object owner;
        private void Close_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (this.owner != null)
            {
                MainWindow s = (MainWindow)this.owner;
                s.框体5.Child = null;
                this.owner = null;
            }
        }
        #endregion

        #region 开启线程
        public delegate void MyThread(MyINI a);
        private void OpenThread()
        {
            ArrayList b = GetAll();
            MainWindow s = (MainWindow)this.owner;
            s.f.myINI.Clear();//开始时清空所有Fiddler链接
            foreach (MyINI a in b)
            {
                MyThread c = new MyThread(SetMyListBox);
                this.Dispatcher.Invoke(c, new object[] { a});//执行唤醒操作
            }
        }
        public void SetMyListBox(MyINI a)
        {
            代理器子项 i = new 代理器子项();
            i.owner = this;
            i.SetMyContent(a);
            ListBox1.Items.Add(i);
            MainWindow s = (MainWindow)this.owner;
            s.f.myINI.Add(a);//新打开界面时把所有的项都放Fiddler里面
        }

        #endregion

        public struct MyINI
        {
            public string value;
            public string value1;
            public string value2;
            public string value3;
            public string value4;
        }
        static string path3 = System.IO.Directory.GetCurrentDirectory();
        public static string filePath = path3+"\\科技代理器.ini";
        public static ArrayList GetAll()
        {
            ArrayList b = new ArrayList();
            int i;
            string value = "";
            MyINI a = new MyINI();
            for (i=1; value != "未找到";i++)
            {
                value = INIHelper.Read(i.ToString(), "name", "未找到", filePath);
                a.value1 = INIHelper.Read(i.ToString(), "原链接", "未找到", filePath);
                a.value2 = INIHelper.Read(i.ToString(), "替换成", "未找到", filePath);
                a.value3 = INIHelper.Read(i.ToString(), "激活", "未找到", filePath);
                a.value = value;
                a.value4 = i.ToString();
                if (value != "未找到")
                {
                    b.Add(a);
                }
            }
            return b;
        }
    }
}
