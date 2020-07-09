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
    /// 代理器子项.xaml 的交互逻辑
    /// </summary>
    public partial class 代理器子项 : UserControl
    {
        public 代理器子项()
        {
            InitializeComponent();
        }
        public 代理器 owner;
        public 代理器.MyINI myINI;
        public string url1;
        public string url2;
        public string value4;
        static string path3 = System.IO.Directory.GetCurrentDirectory();
        public static string filePath = path3 + "\\科技代理器.ini";
        public void SetMyText(string i)
        {
            子1.SetMyText(i);
        }
        public void SetMyContent(代理器.MyINI a)
        {
            myINI = a;
            SetMyText(a.value);
            url1 = a.value1;
            url2 = a.value2;
            if(a.value3 == "真")
            {
                CheckBox1.IsChecked = true;
            }
            else if(a.value3 == "假")
            {
                CheckBox1.IsChecked = false;
            }
            value4 = a.value4;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
        }

        private void 子1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CheckBox1_Click(object sender, RoutedEventArgs e)
        {
            if (CheckBox1.IsChecked == false)
            {
                INIHelper.Write(value4, "激活", "假", filePath);
                myINI.value3 = "假";
            }
            else
            {
                INIHelper.Write(value4, "激活", "真", filePath);
                myINI.value3 = "真";
            }
            MainWindow s = (MainWindow)owner.owner;
            int i = 0;
            bool i1 = false;
            foreach (赛尔号登录器.Widget.代理器.MyINI j in s.f.myINI)
            {
                if(j.value == myINI.value)//如果发现里面存在自己的项
                {
                    i1 = true;
                    break;
                }
                i++;
            }
            if(i1 == false) { return; }
            s.f.myINI.RemoveAt(i);
            s.f.myINI.Add(myINI);
        }
    }
}
