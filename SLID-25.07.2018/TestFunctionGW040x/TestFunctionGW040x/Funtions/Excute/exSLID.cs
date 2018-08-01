using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestFunctionGW040x.Funtions {
    public class exSLID : IDisposable {

        private ONT ontdevice = new ONT(GlobalData.initSetting.DUTIP, 23);
        public exSLID() {
        }

        public void Dispose() {
            ontdevice.Close();
        }

        private bool Connect() {
            try {
                GlobalData.testingInfo.LOGSYSTEM += "Kết nối telnet vào ONT...\r\n";
                bool ret = ontdevice.Connection();
                GlobalData.testingInfo.LOGSYSTEM += string.Format("...{0}\r\n", ret == true ? "PASS" : "FAIL");
                if (ontdevice.IsConnected == true) {
                    string message = "";
                    GlobalData.testingInfo.LOGSYSTEM += "Đăng nhập vào ONT {User=" + GlobalData.initSetting.DUTTELNETUSER + ",Pass=" + GlobalData.initSetting.DUTTELNETPASS + "}...\r\n";
                    ret = ontdevice.Login0(GlobalData.initSetting.DUTTELNETUSER, GlobalData.initSetting.DUTTELNETPASS, out message);
                    GlobalData.testingInfo.LOGSYSTEM += message + "\r\n";
                    GlobalData.testingInfo.LOGSYSTEM += string.Format("...{0}\r\n", ret == true ? "PASS" : "FAIL");
                    return ret;
                }
                else { GlobalData.testingInfo.LOGSYSTEM = "\"don't connect\""; return false; }
            }
            catch {
                return false;
            }
        }

        public bool writeSLID(string _slid, ref string msg) {
            //Connection
            int count = 0;
            REPEAT:
            count++;
            if (!Connect()) {
                if (count <= 3) goto REPEAT;
                else {
                    return false;
                }
            }

            //Write SLID
            try {
                if (ontdevice.IsConnected == false) return false;
                ontdevice.WriteLine(string.Format("tcapi set GPON_ONU Password {0}", _slid));
                Thread.Sleep(100);
                ontdevice.WriteLine("tcapi set GPON_Common CurrentAttribute Password");
                Thread.Sleep(100);
                ontdevice.WriteLine("tcapi show GPON_Common");
                Thread.Sleep(100);
                ontdevice.WriteLine("tcapi commit GPON_ONU");
                Thread.Sleep(100);
                //base.WriteLine("tcapi save");
                //Thread.Sleep(200);
                ontdevice.WriteLine("tcapi show GPON_ONU");
                Thread.Sleep(100);

                msg = ontdevice.Read0();
                return true;
            }
            catch (Exception ex) {
                msg = ex.ToString();
                return false;
            }
        }

    }
}
