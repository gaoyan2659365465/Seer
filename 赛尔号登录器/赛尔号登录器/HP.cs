using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 赛尔号登录器
{
    //对内存工具类Memory进行封装，供血量查看界面使用
    class HP
    {
        static 赛尔号登录器.Memory Memory = new 赛尔号登录器.Memory();//创建内存类实例

        public static uint GetHP_FeatureAddr()
        {
            //对游戏血量特征查询，返回找到的特征地址
            uint[] basicAddr1 = new uint[2];
            string str1 = "8B 85 60 FF FF FF 83 EC 04 52 6A 00 51 FF D0 83 C4 10 8B 8D 6C FF FF FF 89";
            uint int1 = Memory.Signature_search(str1, basicAddr1);
            if (int1 == 0) { return 0; }//没找到
            return basicAddr1[0];//应该有两个一样的，只返回一个就行
        }
        public static uint GetHP_CallAddr(uint FeatureAddr)
        {
            //特征位置+D = Call位置   #十进制13
            uint uint1 = FeatureAddr + 13;
            return uint1;
        }
        public static bool SetHP_HookCall(int ENABLE = 1)
        {
            //把当前游戏进程的Call改造，(自动汇编将自定义函数塞进Call)，用来获得Call内Eax值
            //1为运行[ENABLE], 0为运行[DISABLE]
            uint FeatureAddr = GetHP_FeatureAddr();
            uint CallAddr = GetHP_CallAddr(FeatureAddr);

            string str1 = Memory.ReadTxt("CEAA/CEAA代码_改造Call.txt");//获取函数指针
            str1 = str1.Replace("0239D140", (Memory.GetMyCall()).ToString("x8"));
            str1 = str1.Replace("290EDE2F", CallAddr.ToString("x8"));
            str1 = str1.Replace("290EDE2F", CallAddr.ToString("x8"));

            bool isHOOK = Memory.AutoAssemble(Memory.hProcess, str1, ENABLE);
            //在此以后已经成功改造游戏Call，并且可以直接获取到血量Call的真正地址
            return isHOOK;
        }
        public static uint GetHP_Initialize_Addr()
        {
            //血量初始化地址 = Call地址+3D  #61
            uint eax = Memory.Get_Call_Eax_Value();
            //uint addr = eax + 61; //mov eax,[ecx+1C]
            uint[] basicAddr1 = new uint[30];
            string str1 = "8B 45 10 8B 08 8B 41 1C 8B";
            uint int1 = Memory.Signature_search(str1, basicAddr1);
            if (int1 == 0) { return 0; }//没找到

            uint int2 = int1;
            for (; int2 > 0; int2--)
            {
                if(basicAddr1[int2]>eax)//mov eax,[ecx+1C]的地址肯定是比Call首地址大的
                {
                    if(basicAddr1[int2] - eax < 100)
                    {
                        return basicAddr1[int2]+5;//特征地址+5等于血量初始化地址
                    }
                }
            }
            return 0;
        }
        public static bool SetHP_HOOK_HP_Addr(uint HP_Initialize_Addr, int ENABLE = 1)
        {
            //把当前游戏进程的Call改造，(自动汇编将自定义函数塞进Call)，用来获得血量初始化地址内的[ecx+1C]
            string str1 = Memory.ReadTxt("CEAA/CEAA代码_读取血量新.txt");
            str1 = str1.Replace("0239D140", (Memory.GetMyCall()).ToString("x8"));
            str1 = str1.Replace("1174353B", HP_Initialize_Addr.ToString("x8"));
            str1 = str1.Replace("1174353B", HP_Initialize_Addr.ToString("x8"));

            bool isHOOK = Memory.AutoAssemble(Memory.hProcess, str1, ENABLE);
            return isHOOK;
        }
        public static uint GetHP_Addr(int ab)
        {
            return Memory.Get_Call_HP_Addr(ab);
        }
        public static uint GetHP_Value(uint Addr)
        {
            return Memory.GetCEValue(Memory.id, Addr);
        }
    }
}
