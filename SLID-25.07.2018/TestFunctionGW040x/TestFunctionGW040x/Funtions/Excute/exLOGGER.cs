using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.IO;

namespace TestFunctionGW040x.Funtions {
    public class exLOGGER {

        public bool SaveData() {
            try {
                string title = "DateTime,PCName,MAC,FirmwareResult,MACResult,LAN-PORT1Result,LAN-PORT2Result,LAN-PORT3Result,LAN-PORT4Result,USB2.0Result,USB3.0Result,SynOpticalResult,TXOpticalResult,RXOpticalResult,TXOptical-Value(dBm),RXOptical-Value(dBm),LEDsResult,WPSButtonResult,RESETButtonResult,ErrorCode,TotalJudged";
                string line = "";
                string datetime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                line += string.Format("{0},", datetime);
                string pcname = System.Environment.MachineName;
                line += string.Format("{0},", pcname);
                string mac = GlobalData.testingInfo.MACADDRESS;
                line += string.Format("{0},", mac);
                //-------------------------------------------------------------//
                //check Firmware //- PASS FAIL
                string cfw = "-";
                if (GlobalData.initSetting.ENABLEFW) cfw = string.Format("{0}", GlobalData.testingInfo.FWSTATUS);
                line += string.Format("{0},", cfw);

                //check MAC //- PASS FAIL
                string cmac = "-";
                if (GlobalData.initSetting.ENABLEMAC) cmac = string.Format("{0}", GlobalData.testingInfo.MACSTATUS);
                line += string.Format("{0},", cmac);

                //check LAN //- PASS FAIL
                string clan1 = "-", clan2 = "-", clan3 ="-", clan4="-";
                if (GlobalData.initSetting.ENABLELAN) {
                    clan1 = string.Format("{0}", GlobalData.testingInfo.LAN1STATUS);
                    clan2 = string.Format("{0}", GlobalData.testingInfo.LAN2STATUS);
                    clan3 = string.Format("{0}", GlobalData.testingInfo.LAN3STATUS);
                    clan4 = string.Format("{0}", GlobalData.testingInfo.LAN4STATUS);
                }
                line += string.Format("{0},{1},{2},{3},", clan1,clan2,clan3,clan4);

                //check USB //- PASS FAIL
                string cusb2 = "-", cusb3="-";
                if (GlobalData.initSetting.ENABLEUSB) {
                    cusb2 = string.Format("{0}", GlobalData.testingInfo.USB2STATUS);
                    cusb3 = string.Format("{0}", GlobalData.testingInfo.USB3STATUS);
                }
                line += string.Format("{0},{1},", cusb2, cusb3);

                //check SynOptical //- PASS FAIL
                string csyn = "-";
                if (GlobalData.initSetting.ENABLESYN) csyn = string.Format("{0}", GlobalData.testingInfo.SYNSTATUS);
                line += string.Format("{0},", csyn);

                //check PowerOptical //- PASS FAIL
                string cTXpower = "-", cRXpower = "-", cTXValue="-", cRXValue="-";
                if (GlobalData.initSetting.ENABLEPOWER) {
                    cTXpower = string.Format("{0}", GlobalData.testingInfo.POWERTXSTATUS);
                    cRXpower = string.Format("{0}", GlobalData.testingInfo.POWERRXSTATUS);
                    if (GlobalData.testingInfo.ONTPOWER.Contains("TX=")) {
                        string[] s = GlobalData.testingInfo.ONTPOWER.Replace("TX=", "").Replace("RX=", "").Replace("dBm", "").Replace("\"","").Split(',');
                        cTXValue = string.Format("{0}", s[0]);
                        cRXValue = string.Format("{0}", s[1]);
                    }
                }
                line += string.Format("{0},{1},{2},{3},", cTXpower, cRXpower, cTXValue, cRXValue);

                //check LEDs //- PASS FAIL
                string cled = "-";
                if (GlobalData.initSetting.ENABLELED) cled = string.Format("{0}", GlobalData.testingInfo.LEDSTATUS);
                line += string.Format("{0},", cled);

                //check Buttons //- PASS FAIL
                string cWPSbutton = "-", cRESbutton = "-";
                if (GlobalData.initSetting.ENABLEBUTTON) {
                    cWPSbutton = string.Format("{0}", GlobalData.testingInfo.BUTTONWPSSTATUS);
                    cRESbutton = string.Format("{0}", GlobalData.testingInfo.BUTTONRESETSTATUS);
                }
                line += string.Format("{0},{1},", cWPSbutton, cRESbutton);

                //Error //-
                string error = "--";
                if (GlobalData.testingInfo.TESTINGSTATUS == "FAIL") line += string.Format("{0},", GlobalData.testingInfo.ERRORCODE.Replace(",","+"));
                else line += string.Format("{0},", error);

                //TotalJudge //PASS FAIL
                string judged = GlobalData.testingInfo.TESTINGSTATUS;
                line += string.Format("{0}", judged);
                //-------------------------------------------------------------//
                string logDir = string.Format("{0}\\LOG", System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
                if (!System.IO.Directory.Exists(logDir)) { System.IO.Directory.CreateDirectory(logDir); Thread.Sleep(100); }
                string fileName =string.Format("{0}.csv", DateTime.Now.ToString("yyyyMMdd"));
                string filePath = string.Format("{0}\\{1}", logDir, fileName);

                if (!System.IO.File.Exists(filePath)) {
                    StreamWriter st = new StreamWriter(filePath, true);
                    st.WriteLine(title);
                    st.WriteLine(line);
                    st.Dispose();
                }
                else {
                    StreamWriter st = new StreamWriter(filePath, true);
                    st.WriteLine(line);
                    st.Dispose();
                }
                return true;
            }catch {
                return false;
            }
        }

        public bool SaveLogDetail() {
            try {
                string logDir = string.Format("{0}\\LogDetail", System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
                if (!System.IO.Directory.Exists(logDir)) { System.IO.Directory.CreateDirectory(logDir); Thread.Sleep(100); }
                string fileName = string.Format("{0}.txt", DateTime.Now.ToString("yyyyMMdd"));
                string filePath = string.Format("{0}\\{1}", logDir, fileName);
                StreamWriter st = new StreamWriter(filePath, true);
                st.WriteLine(GlobalData.testingInfo.LOGSYSTEM);
                st.Dispose();
                return true;
            } catch {
                return false;
            }
        }
    }
}
