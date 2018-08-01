using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;


namespace TestFunctionGW040x.Funtions {
    public class exPowerOptical : baseFunctions, IDisposable, ISyn {

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
                else { GlobalData.testingInfo.ONTPOWER = "\"don't connect\""; return false; }
            }
            catch {
                return false;
            }
        }

        public exPowerOptical() {

        }

        private double _InttodBm(string _Number) {
            try {
                double value = double.Parse(_Number);
                double x = value / 10000;
                double logx = System.Math.Log(x);
                double log10 = System.Math.Log(10);
                double tmp = logx / log10;
                double ret = System.Math.Round(tmp * 100, 2) / 10;
                return ret;
            }
            catch {
                return double.MinValue;
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
                    object obj = new object();
                    lock (obj) { GlobalData.testingInfo.ERRORCODE += "Fpo0#0001, "; }
                    return false;
                }
            }

            //2.Excute commandline
            int count1 = 0;
            REP2:
            count1++;
            bool rxRet = false;
            bool txRet = false;

            try {
                //RX
                count = 0;
                ontdevice.WriteLine("cat proc/kmsg &");
                REPEAT0:
                count++;
                GlobalData.testingInfo.LOGSYSTEM += "Đọc giá trị công suất thu RX...\r\n";
                string rxData = ontdevice.Read0();
                //ontdevice.WriteLine("tcapi get Info_PonPhy RxPower");
                ontdevice.WriteLine("echo show_BoB_information >/proc/pon_phy/debug");
                Thread.Sleep(1000);
                
                rxData = ontdevice.Read0();
                string data = rxData;
                GlobalData.testingInfo.LOGSYSTEM += string.Format("...{0}\r\n", rxData);
                double rxPower = double.MinValue;

                if (rxData.Contains("Rx power =")) {
                    rxData = rxData.Split(new string[] { "Rx power =" }, StringSplitOptions.None)[1].Replace("\r","").Replace("\n","").Replace("dBm","").Replace("#","").Trim();
                    rxPower = double.Parse(rxData);
                    GlobalData.testingInfo.LOGSYSTEM += string.Format("...Giá trị RX {0}dBm\r\n", rxPower);
                    GlobalData.testingInfo.LOGSYSTEM += string.Format("...Tiêu chuẩn từ {0}dBm đến {1}dBm\r\n", GlobalData.initSetting.DUTRXMIN, GlobalData.initSetting.DUTRXMAX);
                    //compare rxData with standard
                    rxRet = (rxPower <= GlobalData.initSetting.DUTRXMAX) && (rxPower >= GlobalData.initSetting.DUTRXMIN);
                    if (rxRet == false) {
                        if (count < 3) goto REPEAT0;
                    }
                    GlobalData.testingInfo.LOGSYSTEM += string.Format("Kết quả đo công suất thu RX là: {0}\r\n", rxRet == true ? "PASS" : "FAIL");
                    GlobalData.testingInfo.POWERRXSTATUS = rxRet == true ? TestingStatuses.Pass : TestingStatuses.Fail;
                }
                else {
                    if (count < 3) goto REPEAT0;
                    GlobalData.testingInfo.POWERRXSTATUS = TestingStatuses.Fail;
                }

                //if (rxData.Contains("tcapi get Info_PonPhy RxPower")) {
                //    rxData = rxData.Replace("tcapi get Info_PonPhy RxPower", "").Replace("#", "").Replace("\r\n", "").Replace("\r", "").Trim();
                //    rxPower = _InttodBm(rxData);
                //    GlobalData.testingInfo.LOGSYSTEM += string.Format("...Giá trị quy đổi {0}dBm\r\n", rxPower);
                //    GlobalData.testingInfo.LOGSYSTEM += string.Format("...Tiêu chuẩn từ {0}dBm đến {1}dBm\r\n", GlobalData.initSetting.DUTRXMIN, GlobalData.initSetting.DUTRXMAX);
                //    //compare rxData with standard
                //    rxRet = (rxPower <= GlobalData.initSetting.DUTRXMAX) && (rxPower >= GlobalData.initSetting.DUTRXMIN);
                //    if (rxRet == false) {
                //        count++;
                //        if (count < 3) goto REPEAT0;
                //    }
                //    GlobalData.testingInfo.LOGSYSTEM += string.Format("Kết quả đo công suất thu RX là: {0}\r\n", rxRet == true ? "PASS" : "FAIL");
                //    GlobalData.testingInfo.POWERRXSTATUS = rxRet == true ? TestingStatuses.Pass : TestingStatuses.Fail;
                //}
                //else {
                //    GlobalData.testingInfo.POWERRXSTATUS = TestingStatuses.Fail;
                //}


                //TX - check lap lai 40 lan, neu 40 lan deu NG => hien thi NG
                count = 0;
                string txData = data;
                REPEAT1:
                count++;
                GlobalData.testingInfo.LOGSYSTEM += string.Format("\r\n\r\nĐọc giá trị công suất phát TX...Lần thứ {0}\r\n", count);
                double txPower = double.MinValue;
                GlobalData.testingInfo.LOGSYSTEM += string.Format("...{0}\r\n", txData);

                if (txData.Contains("Tx power =")) {
                    txData = txData.Split(new string[] { "Tx power =" }, StringSplitOptions.None)[1].Split('\n')[0].Replace("\r", "").Replace("\n", "").Replace("dBm", "").Replace("#","").Trim();
                    txPower = double.Parse(txData);
                    GlobalData.testingInfo.LOGSYSTEM += string.Format("...Giá trị TX {0}dBm\r\n", txPower);
                    GlobalData.testingInfo.LOGSYSTEM += string.Format("...Tiêu chuẩn từ {0}dBm đến {1}dBm\r\n", GlobalData.initSetting.DUTTXMIN, GlobalData.initSetting.DUTTXMAX);
                    //compare txData with standard
                    txRet = (txPower <= GlobalData.initSetting.DUTTXMAX) && (txPower >= GlobalData.initSetting.DUTTXMIN);
                    if (txRet == false) {
                        //ontdevice.WriteLine("tcapi get Info_PonPhy TxPower");
                        txData = ontdevice.Read0();
                        ontdevice.WriteLine("echo show_BoB_information >/proc/pon_phy/debug");
                        //ontdevice.WriteLine("cat proc/kmsg &");
                        Thread.Sleep(1000);
                        txData = ontdevice.Read0();
                        if (count < 40) goto REPEAT1;
                    }
                    GlobalData.testingInfo.LOGSYSTEM += string.Format("Kết quả đo công suất phát TX là: {0}\r\n", txRet == true ? "PASS" : "FAIL");
                    GlobalData.testingInfo.POWERTXSTATUS = txRet == true ? TestingStatuses.Pass : TestingStatuses.Fail;
                }
                else {
                    //ontdevice.WriteLine("tcapi get Info_PonPhy TxPower");
                    txData = ontdevice.Read0();
                    ontdevice.WriteLine("echo show_BoB_information >/proc/pon_phy/debug");
                    //ontdevice.WriteLine("cat proc/kmsg &");
                    Thread.Sleep(1000);
                    txData = ontdevice.Read0();
                    if (count < 40) goto REPEAT1;
                    GlobalData.testingInfo.POWERTXSTATUS = TestingStatuses.Fail;
                }

                //if (txData.Contains("tcapi get Info_PonPhy TxPower")) {
                //    txData = txData.Replace("tcapi get Info_PonPhy TxPower", "").Replace("#", "").Replace("\r\n", "").Replace("\r", "").Trim();
                //    txPower = _InttodBm(txData);
                //    GlobalData.testingInfo.LOGSYSTEM += string.Format("...Giá trị quy đổi {0}dBm\r\n", txPower);
                //    GlobalData.testingInfo.LOGSYSTEM += string.Format("...Tiêu chuẩn từ {0}dBm đến {1}dBm\r\n", GlobalData.initSetting.DUTTXMIN, GlobalData.initSetting.DUTTXMAX);
                //    //compare txData with standard
                //    txRet = (txPower <= GlobalData.initSetting.DUTTXMAX) && (txPower >= GlobalData.initSetting.DUTTXMIN);
                //    if (txRet == false) {
                //        count++;
                //        if (count < 20) goto REPEAT1; 
                //    }
                //    GlobalData.testingInfo.LOGSYSTEM += string.Format("Kết quả đo công suất phát TX là: {0}\r\n", txRet == true ? "PASS" : "FAIL");
                //    GlobalData.testingInfo.POWERTXSTATUS = txRet == true ? TestingStatuses.Pass : TestingStatuses.Fail;
                //}
                //else {
                //    GlobalData.testingInfo.POWERTXSTATUS = TestingStatuses.Fail;
                //}

                GlobalData.testingInfo.ONTPOWER = string.Format("\"TX={0}dBm,RX={1}dBm\"", txPower, rxPower);
                if (txRet == false || rxRet == false) {
                    object obj = new object();
                    lock (obj) { GlobalData.testingInfo.ERRORCODE += string.Format("Fpo1#{0}, ", GEN_ERRORCODE(txRet, rxRet)); }
                }
                return txRet && rxRet;
            }
            catch {
                if (count1 < 3) goto REP2;
                object obj = new object();
                lock (obj) { GlobalData.testingInfo.ERRORCODE += "Fpo0#0002, "; }
                GlobalData.testingInfo.POWERTXSTATUS = txRet == true ? TestingStatuses.Pass : TestingStatuses.Fail;
                GlobalData.testingInfo.POWERRXSTATUS = rxRet == true ? TestingStatuses.Pass : TestingStatuses.Fail;
                return false;
            }
        }

        public void Dispose() {
            ontdevice.Close();
        }
    }
}
