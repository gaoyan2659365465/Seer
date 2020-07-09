using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading.Tasks;

namespace 赛尔号登录器
{
    public class MyFiddler
    {
        public ArrayList myINI = new ArrayList();
        public string path3 = System.IO.Directory.GetCurrentDirectory();
        public void OpenFiddler()
        {
            Fiddler.FiddlerApplication.Startup(12345, true, false, false);
            Fiddler.FiddlerApplication.BeforeRequest += delegate (Fiddler.Session oS)
            {
                oS.bBufferResponse = true;
                ReplaceUrl(oS);
            };
            Fiddler.FiddlerApplication.BeforeResponse += delegate (Fiddler.Session oS)
            {
                ReplaceUrl2(oS);
            };
        }

        public void ReplaceUrl(Fiddler.Session oS)
        {
            foreach (赛尔号登录器.Widget.代理器.MyINI j in myINI)
            {
                if (j.value2.IndexOf("工具") == -1)//判断是不是本地的资源
                {
                    if (j.value3 == "真" && oS.uriContains(j.value1))
                    {
                        oS.url = j.value2;
                    }
                }
            }
        }
        public void ReplaceUrl2(Fiddler.Session oS)
        {
            foreach (赛尔号登录器.Widget.代理器.MyINI j in myINI)
            {
                if (j.value2.IndexOf("工具") != -1)//判断是不是本地的资源
                {
                    if (j.value3 == "真" && oS.uriContains(j.value1))
                    {
                        oS.LoadResponseFromFile(path3 + j.value2);
                    }
                }
            }
        }


        //析构函数.   
        ~MyFiddler()
        {
            Fiddler.FiddlerApplication.Shutdown();
        }
    }
}
