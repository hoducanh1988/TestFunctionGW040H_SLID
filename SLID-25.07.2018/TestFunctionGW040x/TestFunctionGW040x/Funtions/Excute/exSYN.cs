using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TestFunctionGW040x.Funtions {
    public class exSYN : baseFunctions, IDisposable, ISyn {

        private OLT oltdevice = new OLT(GlobalData.initSetting.OLTIP, 23);
        public exSYN() {
            int count = 0;
            REPEAT:
            count++;
            GlobalData.testingInfo.logstep.logSYN += "Kết nối telnet vào OLT...\r\n";
            bool ret = oltdevice.Connection();
            if (ret == false && count < 3) goto REPEAT;
            GlobalData.testingInfo.logstep.logSYN += string.Format("...{0}\r\n", ret == true ? "PASS" : "FAIL");
            if (oltdevice.IsConnected == true) {
                string message = "";
                GlobalData.testingInfo.logstep.logSYN += "Đăng nhập vào ONT {User=" + GlobalData.initSetting.OLTTELNETUSER + ",Pass=" + GlobalData.initSetting.OLTTELNETPASS + "}...\r\n";
                ret = oltdevice.Login0(GlobalData.initSetting.OLTTELNETUSER, GlobalData.initSetting.OLTTELNETPASS, out message);
                GlobalData.testingInfo.logstep.logSYN += message + "\r\n";
                GlobalData.testingInfo.logstep.logSYN += string.Format("...{0}\r\n", ret == true ? "PASS" : "FAIL");
                Thread.Sleep(500);
            }
            else GlobalData.testingInfo.ONTSYN = "\"don't connect\"";
        }

        public bool readONTPower(ref string msg) {
            try {
               
                oltdevice.WriteLine(string.Format("show equipment ont optics {0}", GlobalData.initSetting.OLTPORT));
                Thread.Sleep(300);
                msg = oltdevice.Read0();
                return true;
            }
            catch {
                return false;
            }
        }

        public bool refreshOLT() {
            try {
                //Get OLT Port follow SLID Name
                GlobalData.testingInfo.LOGSYSTEM += "get OLT Port follow SLID\r\n";
                string data = oltdevice.Read0();
                data = "";
                GlobalData.testingInfo.LOGSYSTEM += string.Format("info configure equipment ont flat | match exact:{0}", GlobalData.initSetting.DUTSLID);
                oltdevice.WriteLine(string.Format("info configure equipment ont flat | match exact:{0}", GlobalData.initSetting.DUTSLID));
                int count = 0;
                REP:
                count++;
                Thread.Sleep(1000);
                data += oltdevice.Read0();
                if (data.Contains("ont interface") == false) {
                    if (count > 10) return false;
                    else goto REP;
                }
                string _oltPort = data.Split(new string[] { "ont interface" }, StringSplitOptions.None)[1].Replace("\r", "").Replace("\n", "").Trim().Split(new string[] { "sw-ver" }, StringSplitOptions.None)[0] + "\r\n";
                GlobalData.testingInfo.LOGSYSTEM += data + "\r\n";
                GlobalData.initSetting.OLTPORT = _oltPort;

                //Refresh OLT
                GlobalData.testingInfo.LOGSYSTEM += "\r\nRefresh OLT....\r\n";
                GlobalData.testingInfo.LOGSYSTEM += string.Format("configure equipment ont interface {0}\r\n", GlobalData.initSetting.OLTPORT);
                oltdevice.WriteLine(string.Format("configure equipment ont interface {0}", GlobalData.initSetting.OLTPORT));
                Thread.Sleep(100);
                GlobalData.testingInfo.LOGSYSTEM += "admin-state down\r\n";
                oltdevice.WriteLine("admin-state down");
                Thread.Sleep(100);
                GlobalData.testingInfo.LOGSYSTEM += string.Format("configure equipment ont interface {0} sw-ver-pland DISABLED sernum ALCL:00000000 subslocid {1} sw-dnload-version DISABLED", GlobalData.initSetting.OLTPORT, GlobalData.initSetting.DUTSLID);
                oltdevice.WriteLine(string.Format("configure equipment ont interface {0} sw-ver-pland DISABLED sernum ALCL:00000000 subslocid {1} sw-dnload-version DISABLED", GlobalData.initSetting.OLTPORT, GlobalData.initSetting.DUTSLID));
                Thread.Sleep(100);

                return true;
            } catch {
                return false;
            }
        }

        public bool ClearSLID() {
            try {
                //Refresh OLT
                GlobalData.testingInfo.LOGSYSTEM += "\r\nRefresh OLT....\r\n";
                GlobalData.testingInfo.LOGSYSTEM += string.Format("configure equipment ont interface {0}\r\n", GlobalData.initSetting.OLTPORT);
                oltdevice.WriteLine(string.Format("configure equipment ont interface {0}", GlobalData.initSetting.OLTPORT));
                Thread.Sleep(100);
                GlobalData.testingInfo.LOGSYSTEM += "admin-state down\r\n";
                oltdevice.WriteLine("admin-state down");
                Thread.Sleep(100);
                GlobalData.testingInfo.LOGSYSTEM += string.Format("configure equipment ont interface {0} sw-ver-pland DISABLED sernum ALCL:00000000 subslocid {1} sw-dnload-version DISABLED", GlobalData.initSetting.OLTPORT, GlobalData.initSetting.DUTSLID);
                oltdevice.WriteLine(string.Format("configure equipment ont interface {0} sw-ver-pland DISABLED sernum ALCL:00000000 subslocid {1} sw-dnload-version DISABLED", GlobalData.initSetting.OLTPORT, GlobalData.initSetting.DUTSLID));
                Thread.Sleep(100);
                return true;
            } catch {
                return false;
            }
        }

        //Kiem tra dong bo quang 3 lan/ Neu 1 trong 3 lan OK => PASS
        public bool Excute() {
            try {
                int count = 0;
                REPEAT:
                //oltdevice.WriteLine("environment inhibit-alarms");
                oltdevice.WriteLine(string.Format("show equipment ont interface {0}", GlobalData.initSetting.OLTPORT));
                Thread.Sleep(1000);
                string incomeData = string.Empty;
                //oltdevice.WriteLine(string.Format("{0}{1}", GlobalData.initSetting.OLTCOMMAND, GlobalData.initSetting.OLTPORT));
                //Thread.Sleep(300);
               
                incomeData = oltdevice.Read();
                string _gpon = GEN_SERIAL_ONT(GlobalData.testingInfo.MACADDRESS);
                _gpon = _gpon.Replace("VNPT","VNPT:");
                GlobalData.testingInfo.logstep.logSYN += string.Format("ONT GPON: {0}\r\n\r\n", _gpon);
                GlobalData.testingInfo.logstep.logSYN += string.Format("OLT data: {0}\r\n", incomeData);
                bool ret = incomeData.Contains(_gpon);
                if (ret == false) {
                    count++;
                    if (count < 10) goto REPEAT;
                }
                return ret;
            }
            catch {
                return false;
            }
        }

        public void Dispose() {
            oltdevice.Close();
        }
    }
}
