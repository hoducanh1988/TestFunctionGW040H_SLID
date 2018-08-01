using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace TestFunctionGW040x.Funtions {

    public class exMAC : baseFunctions, IDisposable, ISyn {

        private ONT ontdevice = new ONT(GlobalData.initSetting.DUTIP, 23);
        private bool Connect() {
            try {
                GlobalData.testingInfo.logstep.logMAC += "Kết nối telnet vào ONT...\r\n";
                bool ret = ontdevice.Connection();
                GlobalData.testingInfo.logstep.logMAC += string.Format("...{0}\r\n", ret == true ? "PASS" : "FAIL");
                if (ontdevice.IsConnected == true) {
                    string message = "";
                    GlobalData.testingInfo.logstep.logMAC += "Đăng nhập vào ONT {User=" + GlobalData.initSetting.DUTTELNETUSER + ",Pass=" + GlobalData.initSetting.DUTTELNETPASS + "}...\r\n";
                    ret = ontdevice.Login0(GlobalData.initSetting.DUTTELNETUSER, GlobalData.initSetting.DUTTELNETPASS, out message);
                    GlobalData.testingInfo.logstep.logMAC += message + "\r\n";
                    GlobalData.testingInfo.logstep.logMAC += string.Format("...{0}\r\n", ret == true ? "PASS" : "FAIL");
                    return ret;
                }
                else { GlobalData.testingInfo.ONTMAC = "\"don't connect\""; return false; }
            }
            catch {
                return false;
            }
        }

        public exMAC() {
          
        }

        public bool Excute() {
            //1.Connect to ONT
            int count = 0;
            REPEAT:
            count++;
            if (!Connect()) {
                if (count <= 10) goto REPEAT;
                else {
                    object obj = new object();
                    lock (obj) { GlobalData.testingInfo.ERRORCODE += "Fma0#0001, "; }
                    return false;
                }
                        
            }

            //2.Excute commandline
            try {
                GlobalData.testingInfo.logstep.logMAC += "Đọc thông tin địa chỉ MAC...\r\n";
                ontdevice.WriteLine("ifconfig eth0");
                Thread.Sleep(500);
                string tmpStr = ontdevice.Read();
                tmpStr = tmpStr.Replace("ifconfig eth0","").Replace("#","").Replace("\r\n","").Replace("\r","").Trim();
                string[] buffer = tmpStr.Split(new string[] {"HWaddr"}, StringSplitOptions.None);
                tmpStr = buffer[1].Trim();
                tmpStr = tmpStr.Substring(0, 17).Replace(":", "").ToUpper();
                GlobalData.testingInfo.logstep.logMAC += string.Format("...{0}\r\n", tmpStr);
                GlobalData.testingInfo.logstep.logMAC += string.Format("...Tiêu chuẩn: {0}\r\n", GlobalData.testingInfo.MACADDRESS);
                GlobalData.testingInfo.ONTMAC = string.Format("\"{0}\"", tmpStr);
                bool ret = tmpStr == GlobalData.testingInfo.MACADDRESS;
                if (ret == false) {
                    object obj = new object();
                    lock (obj) { GlobalData.testingInfo.ERRORCODE += "Fma1#0001, "; }
                }
                GlobalData.testingInfo.logstep.logMAC += string.Format("=> Phán định: {0}\r\n", ret == true ? "PASS" : "FAIL");
                return ret;
            }
            catch (Exception ex) {
                object obj = new object();
                lock (obj) { GlobalData.testingInfo.ERRORCODE += "Fma0#0002, "; }
                GlobalData.testingInfo.logstep.logMAC += string.Format("...{0}\r\n", ex.ToString());
                return false;
            }
        }

        public string macAddr;
        public exMAC(string _mac) {
            this.macAddr = _mac.ToUpper().Replace(":", "");
        }

        //check MAC Address
        public bool IsValid() {
            //////////////////
            GlobalData.testingInfo.LOGSYSTEM += string.Format("Kiểm tra địa chỉ MAC có hợp lệ...\r\n");
            try {
                //length of mac = 12 ???
                if (macAddr.Length != 12) {
                    object obj = new object();
                    lock (obj) { GlobalData.testingInfo.ERRORCODE += "Fma0#0003, "; }
                    GlobalData.testingInfo.LOGSYSTEM += string.Format("...Chiều dài={0}, Không hợp lệ\r\n", macAddr.Length);
                    goto NG;
                }
                GlobalData.testingInfo.LOGSYSTEM += string.Format("...Chiều dài={0}, Hợp lệ\r\n", macAddr.Length);

                //first 6 digits = A4F4C2 || A06518
                string sixDigits = macAddr.Substring(0, 6);
                string substr = GlobalData.initSetting.MAC6DIGIT;
                if (substr.Contains(":")) {
                    string[] buffer = substr.Split(':');
                    bool isSame = false;
                    foreach (string item in buffer) {
                        if (sixDigits == item) { isSame = true; break; }
                    }
                    if (!isSame) {
                        object obj = new object();
                        lock (obj) { GlobalData.testingInfo.ERRORCODE += "Fma0#0004, "; }
                        GlobalData.testingInfo.LOGSYSTEM += string.Format("...6 kí tự đầu={0}, Không hợp lệ\r\n", sixDigits);
                        goto NG;
                    }
                }
                else {
                    if (sixDigits != substr) {
                        object obj = new object();
                        lock (obj) { GlobalData.testingInfo.ERRORCODE += "Fma0#0004, "; }
                        GlobalData.testingInfo.LOGSYSTEM += string.Format("...6 kí tự đầu={0}, Không hợp lệ\r\n", sixDigits);
                        goto NG;
                    }
                }
                GlobalData.testingInfo.LOGSYSTEM += string.Format("...6 kí tự đầu={0}, Hợp lệ\r\n", sixDigits);

                //Mac digit format is [0-9,A-F]
                string patterns = "";
                for (int i = 0; i < 12; i++) {
                    patterns += "[0-9,A-F]";
                }
                if (!Regex.IsMatch(macAddr, patterns)) {
                    object obj = new object();
                    lock (obj) { GlobalData.testingInfo.ERRORCODE += "Fma0#0005, "; }
                    GlobalData.testingInfo.LOGSYSTEM += string.Format("...Định dạng kí tự [A-F,0-9], Không hợp lệ\r\n");
                    goto NG;
                }
                GlobalData.testingInfo.LOGSYSTEM += string.Format("...Định dạng kí tự [A-F,0-9], Hợp lệ\r\n");
                goto OK;
            }
            catch {
                goto NG;
            }
            //////////////////
            OK:
            {
                GlobalData.testingInfo.MACADDRESS = this.macAddr;
                return true;
            }
            //////////////////
            NG:
            {
                return false;
            }
        }

        public void Dispose() {
            ontdevice.Close();
        }
    }
}
