// 调用exe的函数
using System;
using System.Diagnostics;


namespace StartExe
{
    public partial class Startexe
    {
        public bool StartProcess(string runFilePath, params string[] args)
        {
            string s = "";
            foreach (string arg in args)
            {
                s = s + arg + " ";
            }
            s = s.Trim();
            Process process = new Process();//创建进程对象    
            ProcessStartInfo startInfo = new ProcessStartInfo(runFilePath, s); // 括号里是(程序名,参数)
            process.StartInfo = startInfo;
            try { process.Start(); }
            catch { }
            return true;
        }

        private void start_craw(object sender, EventArgs e)
        {
            string exe_path = "E:/example.exe";  // 被调exe
            string[] the_args = { "1", "2", "3", "4" };   // 被调exe接受的参数
            StartProcess(exe_path, the_args);
        }
    }

}
