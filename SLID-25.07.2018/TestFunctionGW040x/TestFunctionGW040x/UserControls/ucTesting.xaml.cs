using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using TestFunctionGW040x.Funtions;
using System.Reflection;
using System.Diagnostics;

namespace TestFunctionGW040x.UserControls {
    /// <summary>
    /// Interaction logic for ucTesting.xaml
    /// </summary>
    public partial class ucTesting : UserControl {

        DispatcherTimer timer = null;
        Stopwatch st = null;

        public ucTesting() {
            InitializeComponent();
            this.DataContext = GlobalData.testingInfo;

            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            timer.Tick += ((sender, e) => {
                if (GlobalData.testingInfo.DEVCOUNTER == true) {
                    _scrollViewer.ScrollToEnd();
                }
            });
            timer.Start();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e) {
            if (GlobalData.testingInfo.ERRORMESSAGE.Length > 0) return;
            if (e.Key == Key.Enter) {
                TextBox tb = sender as TextBox;
                switch (tb.Name) {
                    case "txtMacAddress": {
                            st = new Stopwatch();
                            st.Start();
                            string subStr = tb.Text.Trim();
                            if (subStr == string.Empty || subStr == "") return;
                            this.txtMacAddress.IsEnabled = false;
                            this.rtbResultInfo.Document.Blocks.Clear();
                            GlobalData.testingInfo.DEVCOUNTER = true;

                            //Nhập địa chỉ MAC vào phần mềm
                            string mac = tb.Text.Trim().Replace(":", "");
                            GlobalData.testingInfo.LOGSYSTEM += string.Format("NHẬP VÀ KIỂM TRA ĐỊA CHỈ MAC ADDRESS >>>>>>>>>>>>>>>>>>>>>>>>>>>\r\n...\r\n");
                            GlobalData.testingInfo.LOGSYSTEM += string.Format("...{0}\r\r\n", mac);
                            if (!(new exMAC(mac).IsValid())) {
                                GlobalData.testingInfo.ERRORMESSAGE = string.Format("LỖI {0}: Địa chỉ MAC '{1}' không hợp lệ.", GlobalData.testingInfo.ERRORCODE, mac);
                                GlobalData.testingInfo.LOGSYSTEM += string.Format("=> {0}\r\r\n", GlobalData.testingInfo.ERRORMESSAGE);
                                GlobalData.testingInfo.TESTINGSTATUS = TestingStatuses.Fail;
                                this._fail();
                                return;
                            }
                            GlobalData.testingInfo.LOGSYSTEM += string.Format("=> Địa chỉ MAC hợp lệ\r\n");

                            //Đợi DUT khởi động xong
                            GlobalData.testingInfo.TESTINGSTATUS = TestingStatuses.Wait;
                            GlobalData.testingInfo.LOGSYSTEM += string.Format("Bật nguồn ONT...\r\n");
                            int timeout = TimeOuts.x120;
                            if (!(new exPower(timeout).Excute())) {
                                if (GlobalData.testingInfo.ERRORCODE != "0x007") GlobalData.testingInfo.ERRORMESSAGE = string.Format("LỖI {0}: Bật nguồn DUT quá thời gian cho phép {1} giây.", GlobalData.testingInfo.ERRORCODE, timeout);
                                else GlobalData.testingInfo.ERRORMESSAGE = string.Format("LỖI {0}: WIFI chưa khởi động xong.", GlobalData.testingInfo.ERRORCODE);
                                GlobalData.testingInfo.LOGSYSTEM += string.Format("=> {0}\r\n", GlobalData.testingInfo.ERRORMESSAGE);
                                GlobalData.testingInfo.TESTINGSTATUS = TestingStatuses.Fail;
                                this._fail();
                                return;
                            }
                            GlobalData.testingInfo.LOGSYSTEM += "=> ONT khởi động hoàn thành\r\n";
                            GlobalData.testingInfo.LOGSYSTEM += "...\r\n<\r\n";

                            //Nhap ma SLID vao ONT
                            GlobalData.testingInfo.LOGSYSTEM += string.Format("Ghi mã SLID {0} vào ONT...\r\n", GlobalData.initSetting.DUTSLID);
                            string msg = "";
                            new exSLID().writeSLID(GlobalData.initSetting.DUTSLID, ref msg);
                            GlobalData.testingInfo.LOGSYSTEM += msg + "\r\n";

                            //Get OLT Port Follow SLID
                            Stopwatch tt0 = new Stopwatch();
                            tt0.Start();
                            var exsyn0 = new exSYN();
                            exsyn0.refreshOLT();
                            exsyn0.Dispose();
                            tt0.Stop();
                            GlobalData.testingInfo.LOGSYSTEM += string.Format("\r\nThời gian Refresh OLT: {0} ms\r\n", tt0.ElapsedMilliseconds);

                            //Kiểm tra fw version
                            if (GlobalData.initSetting.ENABLEFW == true) {
                                Thread f = new Thread(new ThreadStart(() => {
                                    GlobalData.testingInfo.FWSTATUS = TestingStatuses.Wait;
                                    GlobalData.testingInfo.logstep.logFirmware += "\r\nKIỂM TRA VERSION FIRMWARE >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>\r\n...\r\n";
                                    var exfirm = new exFirmware();
                                    if (!exfirm.Excute()) GlobalData.testingInfo.FWSTATUS = TestingStatuses.Fail;
                                    else GlobalData.testingInfo.FWSTATUS = TestingStatuses.Pass;
                                    GlobalData.testingInfo.logstep.logFirmware += "...\r\n<";
                                    exfirm.Dispose();
                                    object obj = new object();
                                    lock (obj) {
                                        GlobalData.testingInfo.LOGSYSTEM += string.Format("{0}\r\n", GlobalData.testingInfo.logstep.logFirmware);
                                    }
                                }));
                                f.IsBackground = true;
                                f.Start();
                            }

                            //Kiểm tra MAC Address
                            if (GlobalData.initSetting.ENABLEMAC == true) {
                                Thread m = new Thread(new ThreadStart(() => {
                                    GlobalData.testingInfo.MACSTATUS = TestingStatuses.Wait;
                                    GlobalData.testingInfo.logstep.logMAC += "\r\nKIỂM TRA ĐỊA CHỈ MAC >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>\r\n...\r\n";
                                    var exmac = new exMAC();
                                    if (!exmac.Excute()) GlobalData.testingInfo.MACSTATUS = TestingStatuses.Fail;
                                    else GlobalData.testingInfo.MACSTATUS = TestingStatuses.Pass;
                                    GlobalData.testingInfo.logstep.logMAC += "...\r\n<";
                                    exmac.Dispose();
                                    object obj = new object();
                                    lock (obj) {
                                        GlobalData.testingInfo.LOGSYSTEM += string.Format("{0}\r\n", GlobalData.testingInfo.logstep.logMAC);
                                    }
                                }));
                                m.IsBackground = true;
                                m.Start();
                            }

                            //Kiểm tra cổng LAN
                            if (GlobalData.initSetting.ENABLELAN == true) {
                                Thread l = new Thread(new ThreadStart(() => {
                                    GlobalData.testingInfo.LANSTATUS = TestingStatuses.Wait;
                                    GlobalData.testingInfo.LAN1STATUS = TestingStatuses.Wait;
                                    GlobalData.testingInfo.LAN2STATUS = TestingStatuses.Wait;
                                    GlobalData.testingInfo.LAN3STATUS = TestingStatuses.Wait;
                                    GlobalData.testingInfo.LAN4STATUS = TestingStatuses.Wait;
                                    GlobalData.testingInfo.logstep.logLAN += "\r\nKIỂM TRA CỔNG LAN >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>\r\n...\r\n";
                                    var exlan = new exLAN();
                                    if (!exlan.Excute()) GlobalData.testingInfo.LANSTATUS = TestingStatuses.Fail;
                                    else GlobalData.testingInfo.LANSTATUS = TestingStatuses.Pass;
                                    GlobalData.testingInfo.logstep.logLAN += "...\r\n<";
                                    exlan.Dispose();
                                    object obj = new object();
                                    lock (obj) {
                                        GlobalData.testingInfo.LOGSYSTEM += string.Format("{0}\r\n", GlobalData.testingInfo.logstep.logLAN);
                                    }
                                }));
                                l.IsBackground = true;
                                l.Start();
                            }

                            //Kiểm tra cổng USB
                            if (GlobalData.initSetting.ENABLEUSB == true) {
                                Thread u = new Thread(new ThreadStart(() => {
                                    GlobalData.testingInfo.USBSTATUS = TestingStatuses.Wait;
                                    GlobalData.testingInfo.USB2STATUS = TestingStatuses.Wait;
                                    GlobalData.testingInfo.USB3STATUS = TestingStatuses.Wait;
                                    GlobalData.testingInfo.logstep.logUSB += "\r\nKIỂM TRA CỔNG USB >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>\r\n...\r\n";
                                    var exusb = new exUSB();
                                    if (!exusb.Excute()) GlobalData.testingInfo.USBSTATUS = TestingStatuses.Fail;
                                    else GlobalData.testingInfo.USBSTATUS = TestingStatuses.Pass;
                                    GlobalData.testingInfo.logstep.logUSB += "...\r\n<";
                                    exusb.Dispose();
                                    object obj = new object();
                                    lock (obj) {
                                        GlobalData.testingInfo.LOGSYSTEM += string.Format("{0}\r\n", GlobalData.testingInfo.logstep.logUSB);
                                    }
                                }));
                                u.IsBackground = true;
                                u.Start();
                            }

                            //Kiểm tra đồng bộ quang
                            if (GlobalData.initSetting.ENABLESYN == true) {
                                Thread s = new Thread(new ThreadStart(() => {
                                    GlobalData.testingInfo.SYNSTATUS = TestingStatuses.Wait;
                                    GlobalData.testingInfo.logstep.logSYN += "\r\nKIỂM TRA ĐỒNG BỘ QUANG >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>\r\n...\r\n";
                                    var exsyn = new exSYN();
                                    if (!exsyn.Excute()) GlobalData.testingInfo.SYNSTATUS = TestingStatuses.Fail;
                                    else GlobalData.testingInfo.SYNSTATUS = TestingStatuses.Pass;
                                    GlobalData.testingInfo.logstep.logSYN += "...\r\n<";
                                    exsyn.Dispose();
                                    object obj = new object();
                                    lock (obj) {
                                        GlobalData.testingInfo.LOGSYSTEM += string.Format("{0}\r\n", GlobalData.testingInfo.logstep.logSYN);
                                    }
                                }));
                                s.IsBackground = true;
                                s.Start();
                            }

                           
                            //Kiểm tra nút nhấn, Kiểm tra LED
                            Thread bl = new Thread(new ThreadStart(() => {

                                //Chờ những bước trên hoàn thành
                                bool _flag = false;
                                while (!_flag) {
                                    if (SynFunctionsIsOk() != "WAIT") break;
                                    Thread.Sleep(200);
                                }
                                if (SynFunctionsIsOk() == "FAIL") {

                                    //Nhap ma SLID vao ONT
                                    GlobalData.testingInfo.LOGSYSTEM += string.Format("Ghi mã SLID {0} vào ONT...\r\n", "000001");
                                    new exSLID().writeSLID("000001", ref msg);
                                    GlobalData.testingInfo.LOGSYSTEM += msg + "\r\n";

                                    //Refresh OLT
                                    Stopwatch tt1 = new Stopwatch();
                                    tt1.Start();
                                    var exsyn1 = new exSYN();
                                    exsyn1.ClearSLID();
                                    exsyn1.Dispose();
                                    tt1.Stop();
                                    GlobalData.testingInfo.LOGSYSTEM += string.Format("\r\nThời gian Refresh OLT: {0} ms\r\n", tt1.ElapsedMilliseconds);

                                    GlobalData.testingInfo.TESTINGSTATUS = TestingStatuses.Fail;
                                    this._fail();
                                    return;
                                }

                                //Kiểm tra công suất quang
                                if (GlobalData.initSetting.ENABLEPOWER == true) {
                                    GlobalData.testingInfo.POWERSTATUS = TestingStatuses.Wait;
                                    GlobalData.testingInfo.POWERTXSTATUS = TestingStatuses.Wait;
                                    GlobalData.testingInfo.POWERRXSTATUS = TestingStatuses.Wait;
                                    GlobalData.testingInfo.LOGSYSTEM += "\r\nKIỂM TRA CÔNG SUẤT QUANG >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>\r\n...\r\n";
                                    var expower = new exPowerOptical();
                                    if (!expower.Excute()) {
                                        //Nhap ma SLID vao ONT
                                        GlobalData.testingInfo.LOGSYSTEM += string.Format("Ghi mã SLID {0} vào ONT...\r\n", "000001");
                                        new exSLID().writeSLID("000001", ref msg);
                                        GlobalData.testingInfo.LOGSYSTEM += msg + "\r\n";

                                        //Refresh OLT
                                        Stopwatch tt2 = new Stopwatch();
                                        tt2.Start();
                                        var exsyn2 = new exSYN();
                                        exsyn2.ClearSLID();
                                        exsyn2.Dispose();
                                        tt2.Stop();
                                        GlobalData.testingInfo.LOGSYSTEM += string.Format("\r\nThời gian Refresh OLT: {0} ms\r\n", tt2.ElapsedMilliseconds);

                                        //
                                        GlobalData.testingInfo.POWERSTATUS = TestingStatuses.Fail;
                                        GlobalData.testingInfo.TESTINGSTATUS = TestingStatuses.Fail;
                                        this._fail();
                                        expower.Dispose();
                                        return;
                                    }
                                    else GlobalData.testingInfo.POWERSTATUS = TestingStatuses.Pass;
                                    GlobalData.testingInfo.LOGSYSTEM += "...\r\n<";
                                    expower.Dispose();
                                    object obj = new object();
                                }

                                //
                                //Nhap ma SLID vao ONT
                                GlobalData.testingInfo.LOGSYSTEM += string.Format("Ghi mã SLID {0} vào ONT...\r\n", "000001");
                                new exSLID().writeSLID("000001", ref msg);
                                GlobalData.testingInfo.LOGSYSTEM += msg + "\r\n";

                                //Refresh OLT
                                Stopwatch tt3 = new Stopwatch();
                                tt3.Start();
                                var exsyn3 = new exSYN();
                                exsyn3.ClearSLID();
                                exsyn3.Dispose();
                                tt3.Stop();
                                GlobalData.testingInfo.LOGSYSTEM += string.Format("\r\nThời gian Refresh OLT: {0} ms\r\n", tt3.ElapsedMilliseconds);


                                //Kiểm tra LEDs
                                if (GlobalData.initSetting.ENABLELED == true) {
                                    GlobalData.testingInfo.LEDSTATUS = TestingStatuses.Wait;
                                    GlobalData.testingInfo.LOGSYSTEM += "\r\nKIỂM TRA LED >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>\r\n...\r\n";
                                    var exled = new exLED();
                                    if (!exled.Excute()) {
                                        GlobalData.testingInfo.LEDSTATUS = TestingStatuses.Fail;
                                        GlobalData.testingInfo.TESTINGSTATUS = TestingStatuses.Fail;
                                        this._fail();
                                        exled.Dispose();
                                        return;
                                    }
                                    else GlobalData.testingInfo.LEDSTATUS = TestingStatuses.Pass;
                                    GlobalData.testingInfo.LOGSYSTEM += "...\r\n<";
                                    exled.Dispose();
                                }

                                //Kiểm tra nút nhấn
                                if (GlobalData.initSetting.ENABLEBUTTON == true) {
                                    GlobalData.testingInfo.BUTTONSTATUS = TestingStatuses.Wait;
                                    GlobalData.testingInfo.BUTTONWPSSTATUS = TestingStatuses.Wait;
                                    GlobalData.testingInfo.BUTTONRESETSTATUS = TestingStatuses.Wait;
                                    GlobalData.testingInfo.LOGSYSTEM += "\r\nKIỂM TRA NÚT NHẤN >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>\r\n...\r\n";
                                    var exbutton = new exButton();
                                    if (!exbutton.Excute()) {
                                        GlobalData.testingInfo.BUTTONSTATUS = TestingStatuses.Fail;
                                        GlobalData.testingInfo.TESTINGSTATUS = TestingStatuses.Fail;
                                        this._fail();
                                        exbutton.Dispose();
                                        return;
                                    }
                                    else GlobalData.testingInfo.BUTTONSTATUS = TestingStatuses.Pass;
                                    GlobalData.testingInfo.LOGSYSTEM += "...\r\n<";
                                    exbutton.Dispose();
                                }

                                //Total OK
                                this._pass();
                                return;
                            }));
                            bl.IsBackground = true;
                            bl.Start();
                            break;
                        }
                    default: { break; }
                }
            }
        }

        void _fail() {
            Dispatcher.BeginInvoke(new Action(() => {
                this.rtbResultInfo.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFD50000");
                st.Stop();
                this.rtbResultInfo.AppendText(string.Format("1. Sản phẩm: {0} kiểm tra hoàn thành, tổng thời gian kiểm tra là {1} giây. \n\r\n", GlobalData.testingInfo.MACADDRESS, st.ElapsedMilliseconds / 1000.0));
                st.Reset();
                this.rtbResultInfo.AppendText(string.Format("2. Kết quả : {0}\n", "FAIL"));
                this.rtbResultInfo.AppendText(string.Format("3. Mã lỗi  : {0}\n", GlobalData.testingInfo.ERRORCODE));
                this.txtMacAddress.IsEnabled = true;
                this.lblOldMAC.Content = string.Format("=> Old MAC: {0}", this.txtMacAddress.Text);
                this.txtMacAddress.Clear();
                this.txtMacAddress.Focus();
            }));
            GlobalData.testingInfo.LOGSYSTEM += "\r\n";
            GlobalData.testingInfo.LOGSYSTEM += "***************************************************************************************\r\n";
            GlobalData.testingInfo.LOGSYSTEM += "PHÁN ĐỊNH TỔNG: FAIL\r\n\r\n";

            new exLOGGER().SaveData();
            new exLOGGER().SaveLogDetail();
            GlobalData.testingInfo.DEVCOUNTER = false;
        }

        void _pass() {
            GlobalData.testingInfo.LOGSYSTEM += "\r\n";
            GlobalData.testingInfo.LOGSYSTEM += "***************************************************************************************\r\n";
            GlobalData.testingInfo.LOGSYSTEM += "PHÁN ĐỊNH TỔNG: PASS\r\n\r\n";
            GlobalData.testingInfo.TESTINGSTATUS = TestingStatuses.Pass;
            Dispatcher.BeginInvoke(new Action(() => {
                this.rtbResultInfo.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#000000");
                st.Stop();
                this.rtbResultInfo.AppendText(string.Format("1. Sản phẩm: {0} kiểm tra hoàn thành, tổng thời gian kiểm tra là {1} giây. \n\r\n", GlobalData.testingInfo.MACADDRESS, st.ElapsedMilliseconds / 1000.0));
                st.Reset();
                this.rtbResultInfo.AppendText(string.Format("2. Kết quả : {0}\n", "PASS"));
                this.rtbResultInfo.AppendText(string.Format("3. Mã lỗi  : {0}\n", "--"));
                this.txtMacAddress.IsEnabled = true;
                this.lblOldMAC.Content = string.Format("=> Old MAC: {0}", this.txtMacAddress.Text);
                this.txtMacAddress.Clear();
                this.txtMacAddress.Focus();

            }));
            new exLOGGER().SaveData();
            new exLOGGER().SaveLogDetail();
            GlobalData.testingInfo.DEVCOUNTER = false;
        }

      
        /// <summary>
        /// 
        /// </summary>
        /// <returns>PASS,FAIL,WAIT</returns>
        private string SynFunctionsIsOk() {
            try {
                bool ret = false;
                ret = GlobalData.testingInfo.FWSTATUS == TestingStatuses.Wait ||
                      GlobalData.testingInfo.MACSTATUS == TestingStatuses.Wait ||
                      GlobalData.testingInfo.LAN1STATUS == TestingStatuses.Wait ||
                      GlobalData.testingInfo.LAN2STATUS == TestingStatuses.Wait ||
                      GlobalData.testingInfo.LAN3STATUS == TestingStatuses.Wait ||
                      GlobalData.testingInfo.LAN4STATUS == TestingStatuses.Wait ||
                      GlobalData.testingInfo.USB2STATUS == TestingStatuses.Wait ||
                      GlobalData.testingInfo.USB3STATUS == TestingStatuses.Wait ||
                      GlobalData.testingInfo.SYNSTATUS == TestingStatuses.Wait;
                if (ret) return "WAIT";
                ret = (GlobalData.testingInfo.FWSTATUS == TestingStatuses.Pass || GlobalData.testingInfo.FWSTATUS == TestingStatuses.Ignored) &&
                      (GlobalData.testingInfo.MACSTATUS == TestingStatuses.Pass || GlobalData.testingInfo.MACSTATUS == TestingStatuses.Ignored) &&
                      (GlobalData.testingInfo.LAN1STATUS == TestingStatuses.Pass || GlobalData.testingInfo.LAN1STATUS == TestingStatuses.Ignored) &&
                      (GlobalData.testingInfo.LAN2STATUS == TestingStatuses.Pass || GlobalData.testingInfo.LAN2STATUS == TestingStatuses.Ignored) &&
                      (GlobalData.testingInfo.LAN3STATUS == TestingStatuses.Pass || GlobalData.testingInfo.LAN3STATUS == TestingStatuses.Ignored) &&
                      (GlobalData.testingInfo.LAN4STATUS == TestingStatuses.Pass || GlobalData.testingInfo.LAN4STATUS == TestingStatuses.Ignored) &&
                      (GlobalData.testingInfo.USB2STATUS == TestingStatuses.Pass || GlobalData.testingInfo.USB2STATUS == TestingStatuses.Ignored) &&
                      (GlobalData.testingInfo.USB3STATUS == TestingStatuses.Pass || GlobalData.testingInfo.USB3STATUS == TestingStatuses.Ignored) &&
                      (GlobalData.testingInfo.SYNSTATUS == TestingStatuses.Pass || GlobalData.testingInfo.SYNSTATUS == TestingStatuses.Ignored);
                if (ret) return "PASS";
                return "FAIL";
            }
            catch {
                return "FAIL";
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e) {
            txtMacAddress.Focus();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e) {
            TextBox tb = sender as TextBox;
            switch (tb.Name) {
                case "txtMacAddress": {
                        if (tb.Text.Trim().Length > 0) {
                            GlobalData.testingInfo.initialization();
                            this.rtbResultInfo.Document.Blocks.Clear();
                            this.lblOldMAC.Content = "";
                        }
                        break;
                    }
                default: { break; }
            }
        }

    }
}
