using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TestFunctionGW040x.Funtions {
    public class exUSB : baseFunctions, IDisposable, ISyn {

        private ONT ontdevice = new ONT(GlobalData.initSetting.DUTIP, 23);
        private bool Connect() {
            try {
                GlobalData.testingInfo.logstep.logUSB += "Kết nối telnet vào ONT...\r\n";
                bool ret = ontdevice.Connection();
                GlobalData.testingInfo.logstep.logUSB += string.Format("...{0}\r\n", ret == true ? "PASS" : "FAIL");
                if (ontdevice.IsConnected == true) {
                    string message = "";
                    GlobalData.testingInfo.logstep.logUSB += "Đăng nhập vào ONT {User=" + GlobalData.initSetting.DUTTELNETUSER + ",Pass=" + GlobalData.initSetting.DUTTELNETPASS + "}...\r\n";
                    ret = ontdevice.Login0(GlobalData.initSetting.DUTTELNETUSER, GlobalData.initSetting.DUTTELNETPASS, out message);
                    GlobalData.testingInfo.logstep.logUSB += message + "\r\n";
                    GlobalData.testingInfo.logstep.logUSB += string.Format("...{0}\r\n", ret == true ? "PASS" : "FAIL");
                    return ret;
                }
                else { GlobalData.testingInfo.ONTUSB = "\"don't connect\""; return false; }
            }
            catch {
                return false;
            }
        }

        public exUSB() {
            
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
                    lock (obj) { GlobalData.testingInfo.ERRORCODE += "Fus0#0001, "; }
                    return false;
                }
            }

            //2.Excute commandline
            try {
                //
                GlobalData.testingInfo.logstep.logUSB += "Đọc thông tin cổng USB...\r\n";
                ontdevice.WriteLine("mount -t usbfs usbfs /proc/bus/usb/");
                Thread.Sleep(1000);
                ontdevice.WriteLine("cat /proc/bus/usb/devices");
                Thread.Sleep(2000);
                string getStr = ontdevice.Read();
                GlobalData.testingInfo.logstep.logUSB += string.Format("...{0}\r\n", getStr);
                //check usb
                string _usb2text = "", _usb3text = "";
                //Không có USB HUB
                bool _format1 = getStr.Contains("Product=USB3.0 Hub") || getStr.Contains("Product=USB2.0 Hub");
                bool _format2 = getStr.Contains("Product=4-Port USB 3.0 Hub") || getStr.Contains("Product=4-Port USB 2.0 Hub");

                if (_format1 == true) {
                    _usb2text = "Product=USB2.0 Hub";
                    _usb3text = "Product=USB3.0 Hub";
                }
                if (_format2 == true) {
                    _usb2text = "Product=4-Port USB 2.0 Hub";
                    _usb3text = "Product=4-Port USB 3.0 Hub";
                }

                bool ret = _format1 || _format2;
                if (ret == false) {
                    GlobalData.testingInfo.ONTUSB = string.Format("\"Chưa cắm USB HUB\"");
                    GlobalData.testingInfo.logstep.logUSB += string.Format("=> Phán định: FAIL\r\n");
                    GlobalData.testingInfo.USB2STATUS = TestingStatuses.Fail;
                    GlobalData.testingInfo.USB3STATUS = TestingStatuses.Fail;
                    object obj = new object();
                    lock (obj) { GlobalData.testingInfo.ERRORCODE += "Fus1#0003, "; }
                    return false;
                }
                //Có USB HUB
                string[] buffer = null;
                string usb2 = null, usb3 = null;
                int IndexofUsb3 = getStr.IndexOf(_usb3text);
                int IndexofUsb2 = getStr.IndexOf(_usb2text);
                if(IndexofUsb3 < IndexofUsb2) {
                    buffer = getStr.Split(new string[] { _usb3text }, StringSplitOptions.None);
                    getStr = buffer[1];
                    buffer = getStr.Split(new string[] { _usb2text }, StringSplitOptions.None);
                    usb3 = buffer[0];
                    usb2 = buffer[1];
                    //Kiểm tra có USB3.0 hay không?
                    buffer = usb3.Split(new string[] { "SerialNumber=xhc_mtk" }, StringSplitOptions.None);
                    usb3 = buffer[0];
                    bool ret3 = usb3.Contains("SerialNumber=");
                    //Kiểm tra có USB2.0 hay không?
                    bool ret2 = usb2.Contains("SerialNumber=");
                    GlobalData.testingInfo.ONTUSB = string.Format("\"USB2={0},USB3={1}\"", ret2 == true ? "PASS" : "FAIL", ret3 == true ? "PASS" : "FAIL");
                    GlobalData.testingInfo.USB2STATUS = ret2 == true? TestingStatuses.Pass : TestingStatuses.Fail;
                    GlobalData.testingInfo.USB3STATUS = ret3 == true? TestingStatuses.Pass : TestingStatuses.Fail;
                    ret = ret2 && ret3;
                    object obj = new object();
                    lock (obj) { GlobalData.testingInfo.ERRORCODE += string.Format("Fus1#{0}, ", GEN_ERRORCODE(ret2, ret3)); }
                    GlobalData.testingInfo.logstep.logUSB += string.Format("=> Phán định: {0}\r\n", ret == true ? "PASS" : "FAIL");
                }
                else {
                    buffer = getStr.Split(new string[] { _usb2text }, StringSplitOptions.None);
                    getStr = buffer[1];
                    buffer = getStr.Split(new string[] { _usb3text }, StringSplitOptions.None);
                    usb2 = buffer[0];
                    usb3 = buffer[1];
                    //Kiểm tra có USB2.0 hay không?
                    buffer = usb2.Split(new string[] { "SerialNumber=xhc_mtk" }, StringSplitOptions.None);
                    usb2 = buffer[0];
                    bool ret2 = usb2.Contains("SerialNumber=");
                    //Kiểm tra có USB3.0 hay không?
                    bool ret3 = usb3.Contains("SerialNumber=");
                    GlobalData.testingInfo.ONTUSB = string.Format("\"USB2={0},USB3={1}\"", ret2 == true ? "PASS" : "FAIL", ret3 == true ? "PASS" : "FAIL");
                    GlobalData.testingInfo.USB2STATUS = ret2 == true ? TestingStatuses.Pass : TestingStatuses.Fail;
                    GlobalData.testingInfo.USB3STATUS = ret3 == true ? TestingStatuses.Pass : TestingStatuses.Fail;
                    ret = ret2 && ret3;
                    if (ret == false) {
                        object obj = new object();
                        lock (obj) { GlobalData.testingInfo.ERRORCODE += string.Format("Fus1#{0}, ", GEN_ERRORCODE(ret2, ret3)); }
                    }
                    GlobalData.testingInfo.logstep.logUSB += string.Format("=> Phán định: {0}\r\n", ret == true ? "PASS" : "FAIL");
                }
                return ret;
            } catch (Exception ex) {
                object obj = new object();
                lock (obj) { GlobalData.testingInfo.ERRORCODE += "Fus0#0002, "; }
                GlobalData.testingInfo.logstep.logUSB += string.Format("...{0}\r\n", ex.ToString());
                return false;
            }
        }

        public void Dispose() {
            ontdevice.Close();
        }
    }
}
