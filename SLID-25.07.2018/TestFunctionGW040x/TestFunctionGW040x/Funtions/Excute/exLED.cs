using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace TestFunctionGW040x.Funtions {
    public class exLED : baseFunctions, IDisposable {

        frmLED frmled = null;
        int counter = 0;

        bool _active {
            set {
                Application.Current.Dispatcher.BeginInvoke(new Action(() => {
                    if (value) {
                        frmled = new frmLED();
                        frmled.ShowDialog();
                    }
                    else frmled.Close();
                }));
            }
        }

        private ONT ontdevice = new ONT(GlobalData.initSetting.DUTIP, 23);
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

        public exLED() {
           
        }

        public bool Excute() {
            //1.Connect to ONT
            int count = 0;
            REPEAT:
            count++;
            if (!Connect()) {
                if (count <= 30) goto REPEAT;
                else {
                    GlobalData.testingInfo.ERRORCODE += "Fle0#0001, ";
                    return false;
                }
            }

            //2.Excute commandline
            try {
                GlobalData.testingInfo.LedResult = "";
                GlobalData.testingInfo.LOGSYSTEM += "Hiển thị Form xác nhận LED\r\n";
                this._active = true;
                GlobalData.testingInfo.LOGSYSTEM += "Bật sáng tất cả các LED.\r\n";
                while (GlobalData.testingInfo.LedResult == "") {
                    OnlineAllLED();
                    Thread.Sleep(1000);
                }
                GlobalData.testingInfo.LOGSYSTEM += string.Format("=> Phán định: {0}\r\n", GlobalData.testingInfo.LedResult);
                return GlobalData.testingInfo.LedResult == "PASS";
            }
            catch {
                GlobalData.testingInfo.ERRORCODE += "Fle0#0002, ";
                return false;
            }
        }

        bool OnlineAllLED() {
            try {
                counter++;
                if (counter<3) {
                    //bat den LOS
                    GlobalData.testingInfo.LOGSYSTEM += "...Bật LED LOS.\r\n";
                    ontdevice.WriteLine("echo 1 > /proc/xpon/los_led");
                    Thread.Sleep(100);
                    //bat den WLAN
                    GlobalData.testingInfo.LOGSYSTEM += "...Bật LED 2G.\r\n";
                    ontdevice.WriteLine("iwpriv ra0 set led_setting=00-00-00-00-00-00-00-00");
                    Thread.Sleep(100);
                    //bat den WLAN 5G
                    GlobalData.testingInfo.LOGSYSTEM += "...Bật LED 5G.\r\n";
                    ontdevice.WriteLine("echo 1 > proc/tc3162/led_wlan5g");
                    Thread.Sleep(100);
                    //bat den WPS
                    GlobalData.testingInfo.LOGSYSTEM += "...Bật LED WPS.\r\n";
                    ontdevice.WriteLine("iwpriv ra0 set led_setting=01-00-00-00-00-00-00-00");
                    Thread.Sleep(100);
                    //bat led PON
                    GlobalData.testingInfo.LOGSYSTEM += "...Bật LED PON.\r\n";
                    ontdevice.WriteLine("sys memwl bfbf0204 0x2");
                    Thread.Sleep(100);
                }
                //INET
                GlobalData.testingInfo.LOGSYSTEM += "...Bật LED INET xanh.\r\n";
                ontdevice.WriteLine("echo \"1 0\" > proc/tc3162/led_internet");
                Thread.Sleep(1000);
                GlobalData.testingInfo.LOGSYSTEM += "...Bật LED INET đỏ.\r\n";
                ontdevice.WriteLine("echo \"0 1\" > proc/tc3162/led_internet");
                Thread.Sleep(1000);
                return true;
            } catch {
                return false;
            }
        }

        public void Dispose() {
            ontdevice.Close();
        }
    }
}
