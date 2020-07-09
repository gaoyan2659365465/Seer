using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceProcess;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace 赛尔号登录器
{
    public partial class Web : UserControl
    {
        public Web()
        {
            InitializeComponent();
            webKitBrowser1_Load();
        }

        private void webKitBrowser1_Load()
        {
            //开启Flash服务
            try
            {
                serviceController1.Start();
            }
            catch { }
            Volume_Load(0);
            webBrowser1.Navigate("seer.61.com/Client.swf");
        }
        
        #region 静音
        public enum INTERNETFEATURELIST
        {
            FEATURE_OBJECT_CACHING = 0,
            FEATURE_ZONE_ELEVATION = 1,
            FEATURE_MIME_HANDLING = 2,
            FEATURE_MIME_SNIFFING = 3,
            FEATURE_WINDOW_RESTRICTIONS = 4,
            FEATURE_WEBOC_POPUPMANAGEMENT = 5,
            FEATURE_BEHAVIORS = 6,
            FEATURE_DISABLE_MK_PROTOCOL = 7,
            FEATURE_LOCALMACHINE_LOCKDOWN = 8,
            FEATURE_SECURITYBAND = 9,
            FEATURE_RESTRICT_ACTIVEXINSTALL = 10,
            FEATURE_VALIDATE_NAVIGATE_URL = 11,
            FEATURE_RESTRICT_FILEDOWNLOAD = 12,
            FEATURE_ADDON_MANAGEMENT = 13,
            FEATURE_PROTOCOL_LOCKDOWN = 14,
            FEATURE_HTTP_USERNAME_PASSWORD_DISABLE = 15,
            FEATURE_SAFE_BINDTOOBJECT = 16,
            FEATURE_UNC_SAVEDFILECHECK = 17,
            FEATURE_GET_URL_DOM_FILEPATH_UNENCODED = 18,
            FEATURE_TABBED_BROWSING = 19,
            FEATURE_SSLUX = 20,
            FEATURE_DISABLE_NAVIGATION_SOUNDS = 21,
            FEATURE_DISABLE_LEGACY_COMPRESSION = 22,
            FEATURE_FORCE_ADDR_AND_STATUS = 23,
            FEATURE_XMLHTTP = 24,
            FEATURE_DISABLE_TELNET_PROTOCOL = 25,
            FEATURE_FEEDS = 26,
            FEATURE_BLOCK_INPUT_PROMPTS = 27,
            FEATURE_ENTRY_COUNT = 28
        }

        private const int SET_FEATURE_ON_THREAD = 0x00000001;
        private const int SET_FEATURE_ON_PROCESS = 0x00000002;
        private const int SET_FEATURE_IN_REGISTRY = 0x00000004;
        private const int SET_FEATURE_ON_THREAD_LOCALMACHINE = 0x00000008;
        private const int SET_FEATURE_ON_THREAD_INTRANET = 0x00000010;
        private const int SET_FEATURE_ON_THREAD_TRUSTED = 0x00000020;
        private const int SET_FEATURE_ON_THREAD_INTERNET = 0x00000040;
        private const int SET_FEATURE_ON_THREAD_RESTRICTED = 0x00000080;

        [DllImport("urlmon.dll")]
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        static extern int CoInternetSetFeatureEnabled(
              INTERNETFEATURELIST FeatureEntry,
              [MarshalAs(UnmanagedType.U4)] int dwFlags,
              bool fEnable);


        [DllImport("winmm.dll")]
        public static extern int waveOutGetVolume(IntPtr h, out uint dwVolume);

        [DllImport("winmm.dll")]
        public static extern int waveOutSetVolume(IntPtr h, uint dwVolume);

        public void Volume_Load(uint v)
        {
            try
            {
                // waveOutSetVolume(0, 0xFFFF); //禁止当前进程发出声音
                CoInternetSetFeatureEnabled(INTERNETFEATURELIST.FEATURE_DISABLE_NAVIGATION_SOUNDS, SET_FEATURE_ON_PROCESS, true);
                uint _savedVolume;
                waveOutGetVolume(IntPtr.Zero, out _savedVolume);

                waveOutSetVolume(IntPtr.Zero, v);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        #endregion

        public void SetWebRefresh()
        {
            //刷新页面
            webBrowser1.Refresh();
        }
    }
}
