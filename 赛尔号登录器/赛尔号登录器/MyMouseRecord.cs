using System;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Timers;
namespace MyHookGlobal
{
    public class SwitchHook
    {
        //-----------------------绑定快捷键Home与End-------------------------------
        #region 初始化声明
        //创建一个Hook标识
        private int hHook = 0;
        private GCHandle gc;
        //创建一个函数委托
        public delegate int HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        //传递数据所需要的结构
        [StructLayout(LayoutKind.Sequential)]
        public struct KeyInfoStruct
        {
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public int dwExtraInfo;
        }

        //按键的对应表
        public enum Keys
        {
            End = 35,
            Home = 36
        }
        #endregion

        #region WinAPI工具
        [DllImport("user32.dll")]
        public static extern int CallNextHookEx(int hHook, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern bool UnhookWindowsHookEx(int hHook);

        [DllImport("user32.dll")]
        public static extern int SetWindowsHookEx(int idHook, HookProc hProc, IntPtr hMod, int dwThreadId);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32")]
        private static extern int mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
        #endregion

        #region 初始化Hook
        //按键的事件会在此处调用
        public int MethodHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                KeyInfoStruct key_struct = (KeyInfoStruct)Marshal.PtrToStructure(lParam, typeof(KeyInfoStruct));
                if ((wParam == ((IntPtr)0x100)) && (((Keys)key_struct.vkCode).ToString() == "Home")){ MyHome(); }
                else if ((wParam == ((IntPtr)0x100)) && (((Keys)key_struct.vkCode).ToString() == "End")) { MyEnd(); }
            }
            return CallNextHookEx(this.hHook, nCode, wParam, lParam);
        }



        //初始化Hook,再次调用解除Hook
        public void Hook_Load()
        {
            if (0 == this.hHook)
            {
                HookProc hProc = new HookProc(this.MethodHookProc);
                this.hHook = SetWindowsHookEx(13, hProc, Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]), 0);
                if (this.hHook != 0) { this.gc = GCHandle.Alloc(hProc); }
            }
            else if (UnhookWindowsHookEx(this.hHook)) { this.hHook = 0; this.gc.Free(); }
        }
        #endregion

        #region 调用事件
        MyHookGlobal.MouseHook MouseHook1 = new MyHookGlobal.MouseHook();
        MyHookGlobal.LoadMouseHook LoadMouseHook1 = new MyHookGlobal.LoadMouseHook();

        public delegate void HookHomeEnd(int b);
        public static event HookHomeEnd HookHomeEnd1;
        public void MyHome()
        {
            if (MouseHook1.hMouseHook == 0)
            {
                MouseHook1.Start();
                HookHomeEnd1(2);
                LoadMouseHook1.StopAction(); HookHomeEnd1(3);//开始录制的时候需要关闭播放
            }
            else { MouseHook1.Stop(); HookHomeEnd1(1); }
        }

        public void MyEnd()
        {
            if (LoadMouseHook1.tmr.Enabled == false)
            {
                LoadMouseHook1.StartAction();
                HookHomeEnd1(4);
                MouseHook1.Stop(); HookHomeEnd1(1);//开始播放的时候需要关闭录制
            }
            else { LoadMouseHook1.StopAction(); HookHomeEnd1(3); }
        }

        #endregion

        #region 开启线程

        public static void OpenThread()
        {
            SwitchHook i = new SwitchHook();
            i.Hook_Load();
        }
        static ThreadStart childref = new ThreadStart(OpenThread);
        static Thread childThread = new Thread(childref);
        public SwitchHook()
        {
            //childThread.Start();
        }

        #endregion
    }

    public class MouseHook
    {
        //-----------------------录制鼠标脚本写入到文件-------------------------------

        #region 初始化声明

        private const int WM_LBUTTONDOWN = 0x201;
        private const int WM_LBUTTONDBLCLK = 0x203;
        private const int WM_MOUSEMOVE = 0x200;

        //默认事件文件
        StreamWriter sw;//文件
        public const string RecordFile = "工具/MouseRecorder.mrd";
        long currentMillis2;//初始时间

        //声明鼠标钩子事件类型.
        HookProc MouseHookProcedure;
        public delegate int HookProc(int nCode, Int32 wParam, IntPtr lParam);
        public int hMouseHook = 0;   //鼠标钩子句柄   

        //声明一个Point的封送类型   
        [StructLayout(LayoutKind.Sequential)]
        public class POINT
        {
            public int x;
            public int y;
        }

        //声明鼠标钩子的封送结构类型   
        [StructLayout(LayoutKind.Sequential)]
        public class MouseHookStruct
        {
            public POINT pt;
            public int hWnd;
            public int wHitTestCode;
            public int dwExtraInfo;
        }


        #endregion

        #region WinAPI工具
        //装置钩子的函数   
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

        //卸下钩子的函数   
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(int idHook);

        //下一个钩挂的函数   
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int CallNextHookEx(int idHook, int nCode, Int32 wParam, IntPtr lParam);
        #endregion

        #region 初始化Hook
        private int MouseHookProc(int nCode, Int32 wParam, IntPtr lParam)
        {
            int mousestate = wParam;
            if (mousestate == 513 || mousestate == 514)
            {
                //如果正常运行并且用户要监听鼠标的消息   
                MouseHookStruct MyMouseHookStruct = (MouseHookStruct)Marshal.PtrToStructure(lParam, typeof(MouseHookStruct));
                //将鼠标事件保存到文件，格式为   录制格式[时间,x,y,状态]。  
                /*if (!File.Exists(RecordFile))
                {
                sw = new StreamWriter(RecordFile);
                currentMillis2 = DateTime.Now.Ticks;
                }
                else
                    sw = File.AppendText(RecordFile);*/
                long currentMillis = (DateTime.Now.Ticks - currentMillis2) / 10000;
                sw.Write(currentMillis + ",");
                sw.Write(MyMouseHookStruct.pt.x.ToString());
                sw.Write(",");
                sw.Write(MyMouseHookStruct.pt.y.ToString());
                sw.Write("," + mousestate.ToString());
                sw.WriteLine("");
                //sw.Close();
                //Console.WriteLine(mousestate);
            }
            return CallNextHookEx(hMouseHook, nCode, wParam, lParam);
        }


        public void Start()
        {
            //安装鼠标钩子   
            if (hMouseHook == 0)
            {
                //生成一个HookProc的实例.   
                MouseHookProcedure = new HookProc(MouseHookProc);

                hMouseHook = SetWindowsHookEx(14, MouseHookProcedure, Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]), 0);

                //如果装置失败停止钩子   
                if (hMouseHook == 0)
                {
                    Stop();
                    throw new Exception("SetWindowsHookEx   failed.");
                }
                File.Delete(RecordFile);
                sw = new StreamWriter(RecordFile);
                currentMillis2 = DateTime.Now.Ticks;
            }
        }

        public void Stop()
        {
            bool retMouse = true;
            if (hMouseHook != 0)
            {
                retMouse = UnhookWindowsHookEx(hMouseHook);
                hMouseHook = 0;
            }
            if(sw != null) { sw.Close(); }//关闭
            //如果卸下钩子失败   
            if (!(retMouse)) throw new Exception("UnhookWindowsHookEx   failed.");
        }
        #endregion

        //析构函数.   
        ~MouseHook()
        {
            Stop();
        }
    }

    public class LoadMouseHook
    {
        //-----------------------播放鼠标脚本文件------------------------------------

        public LoadMouseHook()
        {
            //初始化tmr 防止调用报错
            tmr = new System.Timers.Timer(1);
        }

        //析构函数.   
        ~LoadMouseHook()
        {
            StopAction();
        }

        #region 初始化声明

        //鼠标消息数组   
        string[,] actions;

        //自定义TIMER事件   
        public System.Timers.Timer tmr;

        //时间执行位子标记   
        int actionflag = 0;

        //默认事件文件   
        public const string RecordFile = "工具/MouseRecorder.mrd";

        //默认执行速度   
        public const int runspeed = 1;
        #endregion

        #region WinAPI工具

        //该函数用于执行鼠标按键消息！   
        [DllImport("user32.dll", EntryPoint = "mouse_event")]
        public static extern void mouse_event(
        int dwFlags,
        int dx,
        int dy,
        int cButtons,
        int dwExtraInfo
        );

        //该函数用于执行鼠标定位   
        [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
        public extern static int SetCursorPos(int x, int y);
        #endregion

        #region 读取脚本文件
        //初始化鼠标消息数组   actions   
        public void ActionInitialize(string fname, int mSpeed)
        {
            actionflag = 0;

            StreamReader sr;
            if (File.Exists(fname))
                sr = new StreamReader(fname);
            else
            {
                //Console.WriteLine("找不到脚本文件");
                return;
            }
            string Filetxt = sr.ReadToEnd();
            sr.Close();
            string[] txtarr = Filetxt.Split('\n');
            string[] linearr;
            actions = new string[txtarr.Length, 4];
            for (int i = 0; i < txtarr.Length; i++)
            {
                linearr = txtarr[i].Split(',');
                for (int j = 0; j < linearr.Length; j++)
                {
                    actions[i, j] = linearr[j];
                }
            }
            //添加TIMER事件，执行鼠标消息。   
            tmr = new System.Timers.Timer(mSpeed);
            tmr.Elapsed += new System.Timers.ElapsedEventHandler(this.RunActions);
        }
        #endregion

        #region 执行脚本
        //时间执行过程。   
        public void RunActions(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (actionflag + 1 < actions.GetLength(0))//防止数组越界
            {
                //Console.WriteLine(actions.GetLength(0));
                if(actions[actionflag + 1, 0] != "")//防止脚本最后一个空值引发错误
                {
                    tmr.Interval = int.Parse(actions[actionflag + 1, 0]) - int.Parse(actions[actionflag, 0]);
                }
            }
            else { actionflag = 0; tmr.Interval = 500; return; }//如果越界就提前结束
            //将鼠标定位到指定位置
            if (actions[actionflag, 1] == null)//判断值是否为空
            {
                actionflag = 0; tmr.Interval = 500;return;//如果为空就提前结束
            }
            SetCursorPos(int.Parse(actions[actionflag, 1]), int.Parse(actions[actionflag, 2]));
            //调用mouse_event函数执行相应的操作。   
            switch (int.Parse(actions[actionflag, 3]) - 512)
            {   
                case 1://鼠标左键按下   
                    mouse_event(2, 0, 0, 0, 0);
                    break;
                case 2://鼠标左键弹起   
                    mouse_event(4, 0, 0, 0, 0);
                    break;
            }
            if (actionflag < actions.Length) { actionflag++; }
            else
            {
                actionflag = 0;
                //tmr.Elapsed -= new System.Timers.ElapsedEventHandler(this.RunActions);
                //tmr.Enabled = false;
                tmr.Interval = 500;
            }
        }
        #endregion

        //开始执行事件   
        public void StartAction()
        {
            ActionInitialize(RecordFile, 1);//重新加载脚本文件
            tmr.Enabled = true;
        }

        //停止执行事件   
        public void StopAction()
        {
            actionflag = 0;
            tmr.Elapsed -= new System.Timers.ElapsedEventHandler(this.RunActions);//临时修改*****
            tmr.Enabled = false;
        }
    }
}