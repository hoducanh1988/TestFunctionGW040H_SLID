using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;

namespace TestFunctionGW040x.Funtions {
    public class exButton : baseFunctions, IDisposable {

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
                else { GlobalData.testingInfo.ONTBUTTON = "\"don't connect\""; return false; }
            }
            catch {
                return false;
            }
        }

        public exButton() {
           
        }

        frmBUTTON frmbutton = null;
        bool _active {
            set {
                Application.Current.Dispatcher.BeginInvoke(new Action(() => {
                    if (value) {
                        frmbutton = new frmBUTTON();
                        frmbutton.ShowDialog();
                    }
                    else frmbutton.Close();

                }));
            }
          
           
        }

        public bool Excute() {
            //1.Connect to ONT
            int count = 0;
            REPEAT:
            count++;
            if (!Connect()) {
                if (count <= 3) goto REPEAT;
                else {
                    GlobalData.testingInfo.ERRORCODE += "Fbu0#0001, ";
                    return false;
                }
            }

            //2.Excute commandline
            try {
                //kiem tra nut wps
                GlobalData.testingInfo.WpsResult = "";
                GlobalData.testingInfo.BUTTONTITLE = ButtonTitles.Wps;
                GlobalData.testingInfo.BUTTONTIMEOUT = TimeOuts.x30;
                GlobalData.testingInfo.LOGSYSTEM += "Hiển thị Form xác nhận nút nhấn WPS\r\n";
                this._active = true;
                while (GlobalData.testingInfo.WpsResult == "") {
                    //telnet vào ONT để lấy kết quả nút nhấn wps
                    GlobalData.testingInfo.LOGSYSTEM += "Đọc debugger log...\r\n";
                    ontdevice.WriteLine("cat proc/kmsg &");
                    Thread.Sleep(1000);
                    string tmpstr="";
                    tmpstr = ontdevice.Read0();
                    GlobalData.testingInfo.LOGSYSTEM +=string.Format("...{0}\r\n", tmpstr);
                    if (tmpstr.Contains("VNPTT-WPS button is pressed")) { GlobalData.testingInfo.WpsResult = "PASS"; break; }
                    GlobalData.testingInfo.BUTTONTIMEOUT--;
                    if (GlobalData.testingInfo.BUTTONTIMEOUT == 0) { GlobalData.testingInfo.WpsResult = "FAIL"; break; }
                }
                this._active = false;
                GlobalData.testingInfo.BUTTONWPSSTATUS = GlobalData.testingInfo.WpsResult == "PASS" ? TestingStatuses.Pass : TestingStatuses.Fail;
                GlobalData.testingInfo.LOGSYSTEM += string.Format("Kết quả kiểm tra nút WPS: {0}\r\n", GlobalData.testingInfo.WpsResult);

                if (GlobalData.testingInfo.WpsResult == "FAIL") {
                    GlobalData.testingInfo.ERRORCODE += "Fbu1#0001, ";
                    GlobalData.testingInfo.ONTBUTTON = string.Format("\"wps={0},reset={1}\"","FAIL","--");
                    return false;
                }

                //kiem tra nut reset
                GlobalData.testingInfo.ResetResult = "";
                GlobalData.testingInfo.BUTTONTITLE = ButtonTitles.Reset;
                GlobalData.testingInfo.BUTTONTIMEOUT = TimeOuts.x30;
                GlobalData.testingInfo.LOGSYSTEM += "Hiển thị Form xác nhận nút nhấn Reset\r\n";
                this._active = true;
                while (GlobalData.testingInfo.ResetResult == "") {
                    //telnet vào ONT để lấy kết quả nút nhấn reset
                    GlobalData.testingInfo.LOGSYSTEM += "Đọc debugger log...\r\n";
                    ontdevice.WriteLine("cat proc/kmsg &");
                    Thread.Sleep(1000);
                    string tmpstr = ontdevice.Read0();
                    GlobalData.testingInfo.LOGSYSTEM += string.Format("...{0}\r\n", tmpstr);
                    if (tmpstr.Contains("VNPTT-RESET button is pressed")) { GlobalData.testingInfo.ResetResult = "PASS"; break; }
                    GlobalData.testingInfo.BUTTONTIMEOUT--;
                    if (GlobalData.testingInfo.BUTTONTIMEOUT == 0) { GlobalData.testingInfo.ResetResult = "FAIL"; break; }
                }
                this._active = false;
                GlobalData.testingInfo.BUTTONRESETSTATUS = GlobalData.testingInfo.ResetResult == "PASS" ? TestingStatuses.Pass : TestingStatuses.Fail;
                GlobalData.testingInfo.LOGSYSTEM += string.Format("Kết quả kiểm tra nút Reset: {0}\r\n", GlobalData.testingInfo.ResetResult);
                if (GlobalData.testingInfo.ResetResult == "FAIL") {
                    GlobalData.testingInfo.ERRORCODE += "Fbu1#0002, ";
                    GlobalData.testingInfo.ONTBUTTON = string.Format("\"wps={0},reset={1}\"", "PASS", "FAIL");
                }
                else GlobalData.testingInfo.ONTBUTTON = string.Format("\"wps={0},reset={1}\"", "PASS", "PASS");
                return GlobalData.testingInfo.WpsResult == "PASS" && GlobalData.testingInfo.ResetResult == "PASS";
            }
            catch (Exception ex) {
                GlobalData.testingInfo.ERRORCODE += "Fbu0#0002, ";
                GlobalData.testingInfo.LOGSYSTEM += string.Format("...{0}\r\n", ex.ToString());
                return false;
            }
        }

        public void Dispose() {
            ontdevice.Close();
        }
    }
}
