using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestFunctionGW040x.Funtions {

    public class exLAN : baseFunctions, IDisposable, ISyn {

        private ONT ontdevice = new ONT(GlobalData.initSetting.DUTIP, 23);
        private bool Connect() {
            try {
                GlobalData.testingInfo.logstep.logLAN += "Kết nối telnet vào ONT...\r\n";
                bool ret = ontdevice.Connection();
                GlobalData.testingInfo.logstep.logLAN += string.Format("...{0}\r\n", ret == true ? "PASS" : "FAIL");
                if (ontdevice.IsConnected == true) {
                    string message = "";
                    GlobalData.testingInfo.logstep.logLAN += "Đăng nhập vào ONT {User=" + GlobalData.initSetting.DUTTELNETUSER + ",Pass=" + GlobalData.initSetting.DUTTELNETPASS + "}...\r\n";
                    ret = ontdevice.Login0(GlobalData.initSetting.DUTTELNETUSER, GlobalData.initSetting.DUTTELNETPASS, out message);
                    GlobalData.testingInfo.logstep.logLAN += message + "\r\n";
                    GlobalData.testingInfo.logstep.logLAN += string.Format("...{0}\r\n", ret == true ? "PASS" : "FAIL");
                    return ret;
                }
                else { GlobalData.testingInfo.ONTLAN = "\"don't connect\""; return false; }
            }
            catch {
                return false;
            }
        }

        public exLAN() {
            
        }

        public bool Excute() {
            //1.Connect to ONT
            int count = 0;
            REPEAT:
            count++;
            if (!Connect()) {
                if (count <= 3) goto REPEAT;
                else {
                    object obj = new object();
                    lock (obj) { GlobalData.testingInfo.ERRORCODE += "Fla0#0001, "; }
                    return false;
                }
            }

            //2.Excute commandline
            try {
                string tmpstr = "";
                bool ret = false;
                //LAN1
                bool ret1 = false;
                GlobalData.testingInfo.logstep.logLAN += "Đọc thông tin trạng thái cổng LAN_1...\r\n";
                ontdevice.WriteLine("ethphxcmd eth0 media-type port 0");
                Thread.Sleep(300);
                tmpstr = ontdevice.Read();
                GlobalData.testingInfo.logstep.logLAN += string.Format("...{0}\r\n", tmpstr);
                ret = tmpstr.Contains("Link is up") || tmpstr.Contains("Link is down");
                if (!ret) ret1 = false;
                else ret1 = tmpstr.Contains("Link is up");
                GlobalData.testingInfo.logstep.logLAN += string.Format("...Trạng thái LAN_1: {0}\r\n", ret1 == true ? "PASS" : "FAIL");
                GlobalData.testingInfo.LAN1STATUS = ret1 == true? TestingStatuses.Pass : TestingStatuses.Fail;

                //LAN2
                bool ret2 = false;
                GlobalData.testingInfo.logstep.logLAN += "Đọc thông tin trạng thái cổng LAN_2...\r\n";
                ontdevice.WriteLine("ethphxcmd eth0 media-type port 1");
                Thread.Sleep(300);
                tmpstr = ontdevice.Read();
                GlobalData.testingInfo.logstep.logLAN += string.Format("...{0}\r\n", tmpstr);
                ret = tmpstr.Contains("Link is up") || tmpstr.Contains("Link is down");
                if (!ret) ret2 = false;
                else ret2 = tmpstr.Contains("Link is up");
                GlobalData.testingInfo.logstep.logLAN += string.Format("...Trạng thái LAN_2: {0}\r\n", ret2 == true ? "PASS" : "FAIL");
                GlobalData.testingInfo.LAN2STATUS = ret2 == true ? TestingStatuses.Pass : TestingStatuses.Fail;

                //LAN3
                bool ret3 = false;
                GlobalData.testingInfo.logstep.logLAN += "Đọc thông tin trạng thái cổng LAN_3...\r\n";
                ontdevice.WriteLine("ethphxcmd eth0 media-type port 2");
                Thread.Sleep(300);
                tmpstr = ontdevice.Read();
                GlobalData.testingInfo.logstep.logLAN += string.Format("...{0}\r\n", tmpstr);
                ret = tmpstr.Contains("Link is up") || tmpstr.Contains("Link is down");
                if (!ret) ret3 = false;
                else ret3 = tmpstr.Contains("Link is up");
                GlobalData.testingInfo.logstep.logLAN += string.Format("...Trạng thái LAN_3: {0}\r\n", ret3 == true ? "PASS" : "FAIL");
                GlobalData.testingInfo.LAN3STATUS = ret3 == true ? TestingStatuses.Pass : TestingStatuses.Fail;

                //LAN4
                bool ret4 = false;
                GlobalData.testingInfo.logstep.logLAN += "Đọc thông tin trạng thái cổng LAN_4...\r\n";
                ontdevice.WriteLine("ethphxcmd eth0 media-type port 3");
                Thread.Sleep(300);
                tmpstr = ontdevice.Read();
                GlobalData.testingInfo.logstep.logLAN += string.Format("...{0}\r\n", tmpstr);
                ret = tmpstr.Contains("Link is up") || tmpstr.Contains("Link is down");
                if (!ret) ret4 = false;
                else ret4 = tmpstr.Contains("Link is up");
                GlobalData.testingInfo.logstep.logLAN += string.Format("...Trạng thái LAN_4: {0}\r\n", ret4 == true ? "PASS" : "FAIL");
                GlobalData.testingInfo.LAN4STATUS = ret4 == true ? TestingStatuses.Pass : TestingStatuses.Fail;

                //
                GlobalData.testingInfo.ONTLAN = "{1,2,3,4}=" + string.Format("{0},{1},{2},{3}", ret1 == true ? "up" : "down", ret2 == true ? "up" : "down", ret3 == true ? "up" : "down", ret4 == true ? "up" : "down");
                GlobalData.testingInfo.logstep.logLAN += string.Format("=> Phán định: {0}\r\n", ret1 && ret2 && ret3 && ret4 == true ? "PASS" : "FAIL");
                if (ret1 == false || ret2 == false || ret3 == false || ret4 == false) {
                    object obj = new object();
                    lock (obj) { GlobalData.testingInfo.ERRORCODE += string.Format("Fla1#{0}, ", GEN_ERRORCODE(ret1, ret2, ret3, ret4)); }
                }
                return ret1 && ret2 && ret3 && ret4;
            }
            catch {
                object obj = new object();
                lock (obj) { GlobalData.testingInfo.ERRORCODE += "Fla0#0002, "; }
                return false;
            }
        }

        public void Dispose() {
            ontdevice.Close();
        }
    }
}
