using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TestFunctionGW040x.Funtions {
    public class exFirmware : baseFunctions, IDisposable, ISyn {

        private ONT ontdevice = new ONT(GlobalData.initSetting.DUTIP, 23);

        private bool Connect() {
            try {
                GlobalData.testingInfo.logstep.logFirmware += "Kết nối telnet vào ONT...\r\n";
                bool ret = ontdevice.Connection();
                GlobalData.testingInfo.logstep.logFirmware += string.Format("...{0}\r\n", ret == true ? "PASS" : "FAIL");
                if (ontdevice.IsConnected == true) {
                    string message = "";
                    GlobalData.testingInfo.logstep.logFirmware += "Đăng nhập vào ONT {User=" + GlobalData.initSetting.DUTTELNETUSER + ",Pass=" + GlobalData.initSetting.DUTTELNETPASS + "}...\r\n";
                    ret = ontdevice.Login0(GlobalData.initSetting.DUTTELNETUSER, GlobalData.initSetting.DUTTELNETPASS, out message);
                    GlobalData.testingInfo.logstep.logFirmware += message + "\r\n";
                    GlobalData.testingInfo.logstep.logFirmware += string.Format("...{0}\r\n", ret == true ? "PASS" : "FAIL");
                    return ret;
                }
                else { GlobalData.testingInfo.ONTFWVERSION = "\"don't connect\""; return false; }
            } catch {
                return false;
            }
        }

        public exFirmware() {

        }



        public bool Excute() {
            try {
                //1.Connect to ONT
                int count = 0;
                REPEAT:
                count++;
                if (!Connect()) {
                    if (count <= 10) goto REPEAT;
                    else {
                        object obj = new object();
                        lock (obj) { GlobalData.testingInfo.ERRORCODE += "Ffw0#0001, "; }
                        return false;
                    }
                }

                //2.Excute commandline
                GlobalData.testingInfo.logstep.logFirmware += "Đọc thông tin phiên bản Firmware...\r\n";
                ontdevice.WriteLine("cat /etc/fwver.conf");
                Thread.Sleep(300);
                string fwversion = ontdevice.Read();
                fwversion = fwversion.Replace("cat /etc/fwver.conf", "").Replace("#", "").Replace("\r\n", "").Replace("\r", "").Trim();
                GlobalData.testingInfo.logstep.logFirmware += string.Format("...{0}\r\n", fwversion);
                GlobalData.testingInfo.logstep.logFirmware += string.Format("...Tiêu chuẩn: {0}\r\n", GlobalData.initSetting.FWVERSION);
                GlobalData.testingInfo.ONTFWVERSION = string.Format("\"{0}\"", fwversion);
                bool ret = fwversion.ToUpper() == GlobalData.initSetting.FWVERSION.ToUpper();
                if (ret == false) {
                    object obj = new object();
                    lock (obj) { GlobalData.testingInfo.ERRORCODE += "Ffw1#0001, "; }
                }
                GlobalData.testingInfo.logstep.logFirmware += string.Format("=> Phán định: {0}\r\n", ret == true ? "PASS" : "FAIL");
                return ret;
            }
            catch (Exception ex) {
                object obj = new object();
                lock (obj) { GlobalData.testingInfo.ERRORCODE += "Ffw0#0002, "; }
                GlobalData.testingInfo.logstep.logFirmware += string.Format("...{0}\r\n", ex.ToString());
                return false;
            }
        }

        public void Dispose() {
            ontdevice.Close();
        }

    }
}
