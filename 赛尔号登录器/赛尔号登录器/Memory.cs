using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;//进程类
using System.Runtime.InteropServices; //引入动态链接库

namespace 赛尔号登录器
{
    class Memory
    {
        public int id = Process.GetCurrentProcess().Id;//当前进程的PID
        public IntPtr hProcess = (IntPtr)OpenProcess(PROCESS_ALL_ACCESS, false, Process.GetCurrentProcess().Id);//进程句柄

        #region 特征码搜索
        [DllImport("kernel32.dll")]
        public static extern int OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);
        const int PROCESS_ALL_ACCESS = 0x1F0FFF;
        const int PROCESS_VM_READ = 0x0010;
        const int PROCESS_VM_WRITE = 0x0020;

        [DllImport("工具/特征码搜索.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint FindMatchingCode(IntPtr hProcess, string markCode, uint memBeginAddr, uint memEndAddr, uint[] retAddr, int deviation, bool isCall, bool isAll);

        //特征码搜索
        public uint Signature_search(string str,uint[] basicAddr)
        {
            //转换文本编码
            System.Text.Encoding.Default.GetBytes(str);
            //开始查询特征码
            uint a = FindMatchingCode(hProcess, str, 0x000000, 0x70000000, basicAddr, 0, false, true);
            //MessageBox.Show(basicAddr[0].ToString());
            return a;
        }
        #endregion

        #region 调用易语言dll
        //调用易语言dll
        [DllImport("工具/CEAA运行.dll")]
        public static extern bool GetCE(uint 目标内存地址, int 临时内存地址, int 进程PID, int 是否开启);

        [DllImport("工具/CEAA运行.dll")]
        public static extern int alloc();//申请内存

        [DllImport("工具/CEAA运行.dll")]
        public static extern void release(int 目标地址);//释放内存

        [DllImport("工具/CEAA运行.dll")]
        public static extern uint GetCEValue(int 进程PID, uint 目标内存地址);//获取内存整数

        [DllImport("工具/CEAA运行.dll")]
        public static extern bool SetCall(uint 目标内存地址, int 临时内存地址, int 进程PID, int 是否开启);//改造Call

        [DllImport("工具/CEAA运行.dll")]
        public static extern uint GetMyCall();//获取函数指针

        [DllImport("工具/CEAA运行.dll")]
        public static extern uint Get_Call_Eax_Value();//获取全局变量，Call的地址   mov eax,[ecx+1C]

        [DllImport("工具/CEAA运行.dll")]
        public static extern uint Get_Call_HP_Addr(int ab);//获取全局变量，剩余血量的地址

        [DllImport("变速齿轮.dll")]
        public static extern bool BeginTimeHook();//初始化变速齿轮

        [DllImport("变速齿轮.dll")]
        public static extern bool SetTimeHook(Double a);//设置变速齿轮

        [DllImport("变速齿轮.dll")]
        public static extern void _Uninstall();//卸载变速齿轮

        [DllImport("变速齿轮.dll")]
        public static extern bool SetSpeed(string 进程名称, Double 速度);//卸载变速齿轮

        #endregion

        #region 调用CEAA库

        [DllImport("工具/aa_engine.dll")]
        public static extern bool AutoAssemble(IntPtr 进程句柄, string 脚本代码, int 是否运行);//改造Call

        public string ReadTxt(string path)//读取text文件的方法
        {
            StreamReader sr = new StreamReader(path, Encoding.Default);
            //Console.WriteLine(sr.ReadToEnd());
            //str1.Replace();//替换文本
            return sr.ReadToEnd();
        }

        #endregion
    }
}
