using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestFunctionGW040x.Funtions
{
    public static class initParameters
    {
        public static string passwordAdmin = "234";

        public static List<string> listBarcodeType = new List<string>() { "USB", "UART" };
        public static List<string> listBaudRate = new List<string>() { "-","50","75","110","134","150","200","300","600",
                                                                       "1200","1800","2400","4800","9600",
                                                                       "19200","28800","38400","57600","76800",
                                                                       "115200","230400","460800","576000","921600"};
        public static List<string> listUARTPort = new List<string>();
        static initParameters()
        {
            listUARTPort.Add("-");
            for (int i = 1; i < 100; i++) {
                listUARTPort.Add(string.Format("COM{0}", i));
            }
        }
    }

    public static class TestingStatuses {
        public static string Ignored = "X";
        public static string Pass = "PASS";
        public static string Fail = "FAIL";
        public static string Wait = "wait";
        public static string Ready = "Ready";
        public static string NULL = "";
        public static string Normal = "--";
        public static string None = "NONE";
    }

    public static class PowerTitles {
        public static string Power = "BẬT NGUỒN DUT";
        public static string Ping = string.Format("PING TỚI ONT {0}...", GlobalData.initSetting.DUTIP);
        public static string Telnet = string.Format("TELNET VÀO ONT {0}...", GlobalData.initSetting.DUTIP);
        public static string Login = string.Format("LOGIN VÀO ONT USER={0}, PASSWORD={1}", GlobalData.initSetting.DUTTELNETUSER, GlobalData.initSetting.DUTTELNETPASS);
        public static string Wifi = string.Format("CHỜ WIFI KHỞI ĐỘNG...");
    }

    public static class ButtonTitles {
        public static string Wps = "NHẤN NÚT WPS";
        public static string Reset = "NHẤN NÚT RESET";
    }

    public static class TimeOuts {
        public static int x180 = 180;
        public static int x150 = 150;
        public static int x120 = 120;
        public static int x90 = 90;
        public static int x60 = 60;
        public static int x45 = 45;
        public static int x30 = 30;
        public static int x20 = 20;
        public static int x15 = 15;
        public static int x10 = 10;
        public static int x5 = 5;
        public static int x3 = 3;
    }


    
}
