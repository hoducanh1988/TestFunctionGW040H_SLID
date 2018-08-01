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
using TestFunctionGW040x.Funtions;

namespace TestFunctionGW040x {
    /// <summary>
    /// Interaction logic for ucStep.xaml
    /// </summary>
    public partial class ucStep : UserControl {
        public ucStep() {
            InitializeComponent();
            this.DataContext = GlobalData.initSetting;
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

        #region Change Tab Index

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            TabControl tb = sender as TabControl;
            Dispatcher.Invoke(new Action(() => {
                TabControl t = sender as TabControl;
                switch (t.SelectedIndex) {
                    case 0: { //Kiểm tra FW
                            InitialFWControl();
                            break;
                        }
                    case 1: { //Kiểm tra MAC
                            InitialMACControl();
                            break;
                        }
                    case 2: { //Check LAN
                            InitialLANControl();
                            break;
                        }
                    case 3: { //Check USB
                            InitialUSBControl();
                            break;
                        }
                    case 4: { //Kiểm tra đồng bộ quang
                            InitialSYNControl();
                            break;
                        }
                    case 5: { //Kiểm tra công suất quang
                            InitialPowerOpticalControl();
                            break;
                        }
                    case 6: { //Kiểm tra LED
                            rtbLED.Document.Blocks.Clear();
                            break;
                        }
                    case 7: { //Kiểm tra nút nhấn
                            InitialButtonControl();
                            break;
                        }
                }

            }));
        }

        #endregion

        #region WriteDebug
        RichTextBox rtb = new RichTextBox();

        private void debugWriteLine(string data) {
            Dispatcher.Invoke(new Action(() => {
                rtb.AppendText(data + "\n");
                rtb.ScrollToEnd();
            }));
        }
        private void debugWriteStep(string data) {
            Dispatcher.Invoke(new Action(() => {
                rtb.AppendText(string.Format("- {0}, {1}...\n", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss ffff"), data));
                rtb.ScrollToEnd();
            }));
        }
        private void debugWriteResult(bool result) {
            Dispatcher.Invoke(new Action(() => {
                rtb.AppendText(string.Format("...Kết quả={0}\n\n", result == true ? "Thành công" : "Thất bại"));
                rtb.ScrollToEnd();
            }));
        }
        #endregion

        #region Check Firmware
        //Clear FW
        void InitialFWControl() {
            rtbFirmware.Document.Blocks.Clear();
            tbFWResult.Text = "--";
            tbFWVersion.Text = "--";
        }
        //Check version FW
        private void btnCheckFW_Click(object sender, RoutedEventArgs e) {
            Button b = sender as Button;
            rtb = rtbFirmware;
            InitialFWControl();
            string message = "";
            debugWriteLine("KIỂM TRA VERSION FIRMWARE >>>>>>>>>>>>>>>>>>>>>>>>>>>>>\n");
            b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#3aed1e");
            b.Content = "Đang kiểm tra firmware";
            tbFWResult.Text = "wait";
            string fwversion = "";
            Thread t = new Thread(new ThreadStart(() => {
                bool ret = false;
                //Mo cong telnet vao ONT
                REPEAT:
                debugWriteStep("Mở cổng telnet tới ONT");
                ONT ontdevice = new ONT(GlobalData.initSetting.DUTIP, 23);
                debugWriteResult(true);

                //Connect vao ONT
                debugWriteStep("Kết nối telnet vào ONT");
                if (ontdevice.Connection()==false) {
                    debugWriteResult(false);
                    goto NG;
                }
                debugWriteResult(true);

                //Login vao ONT
                debugWriteStep("Login vào ONT");
                try {
                    ret = ontdevice.Login0(GlobalData.initSetting.DUTTELNETUSER, GlobalData.initSetting.DUTTELNETPASS, out message);
                } catch (Exception ex) {
                    debugWriteLine(ex.ToString());
                    goto REPEAT;
                }
                debugWriteLine(message);
                if (ret==false) {
                    debugWriteResult(false);
                    goto NG;
                }
                debugWriteResult(true);

                //Doc thong tin FW verion
                debugWriteStep("Lấy thông tin Version Firmware từ ONT");
                ontdevice.WriteLine("cat /etc/fwver.conf");
                Thread.Sleep(300);
                fwversion = ontdevice.Read();
                debugWriteLine(fwversion);
                fwversion = fwversion.Replace("cat /etc/fwver.conf", "").Replace("#", "").Replace("\r\n", "").Replace("\r", "").Trim();
                debugWriteResult(true);

                //So sanh voi tieu chuan
                debugWriteStep("So sánh Version Firmware đọc được từ ONT với tiêu chuẩn");
                string st = "";
                Dispatcher.Invoke(new Action(() => { st = txtFW.Text; }));
                debugWriteLine(string.Format("Tiêu chuẩn: {0}", st));
                debugWriteLine(string.Format("Thực tế: {0}", fwversion));
                Dispatcher.Invoke(new Action(() => {
                    ret = fwversion.ToUpper() == txtFW.Text.ToUpper();
                }));
                if (ret == false) {
                    debugWriteResult(false);
                    goto NG;
                }
                debugWriteResult(true);
                goto OK;
                NG:
                {
                    ontdevice.Close();
                    Dispatcher.Invoke(new Action(() => {
                        b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#EEEEEE");
                        b.Content = "Kiểm tra firmware";
                        tbFWResult.Text = "FAIL";
                        debugWriteLine("<");
                        tbFWVersion.Text = fwversion;
                    }));
                    MessageBox.Show(message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                OK:
                {
                    ontdevice.Close();
                    Dispatcher.Invoke(new Action(() => {
                        b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#EEEEEE");
                        b.Content = "Kiểm tra firmware";
                        tbFWResult.Text = "PASS";
                        debugWriteLine("<");
                        tbFWVersion.Text = fwversion;
                    }));
                    MessageBox.Show("Hoàn thành", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
            }));
            t.IsBackground = true;
            t.Start();
        }
        #endregion

        #region Check MAC
        //Clear MAC
        void InitialMACControl() {
            rtbMAC.Document.Blocks.Clear();
            tbMACResult.Text = "--";
            tbMACAddress.Text = "--";
        }
        //Check MAC
        private void btnCheckMAC_Click(object sender, RoutedEventArgs e) {
            Button b = sender as Button;
            rtb = rtbMAC;
            InitialMACControl();
            string message = "";
            debugWriteLine("KIỂM TRA ĐỊA CHỈ MAC >>>>>>>>>>>>>>>>>>>>>>>>>>>>>\n");
            b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#3aed1e");
            b.Content = "Đang kiểm tra MAC";
            tbMACResult.Text = "wait";
            string mac = "";
            Thread t = new Thread(new ThreadStart(() => {
                bool ret = false;
                //Mo cong telnet vao ONT
                REPEAT:
                debugWriteStep("Mở cổng telnet tới ONT");
                ONT ontdevice = new ONT(GlobalData.initSetting.DUTIP, 23);
                debugWriteResult(true);

                //Connect vao ONT
                debugWriteStep("Kết nối telnet vào ONT");
                if (ontdevice.Connection() == false) {
                    debugWriteResult(false);
                    goto NG;
                }
                debugWriteResult(true);

                //Login vao ONT
                debugWriteStep("Login vào ONT");
                try {
                    ret = ontdevice.Login0(GlobalData.initSetting.DUTTELNETUSER, GlobalData.initSetting.DUTTELNETPASS, out message);
                }
                catch (Exception ex) {
                    debugWriteLine(ex.ToString());
                    goto REPEAT;
                }
                debugWriteLine(message);
                if (ret == false) {
                    debugWriteResult(false);
                    goto NG;
                }
                debugWriteResult(true);

                //Doc thong tin MAC
                debugWriteStep("Lấy thông tin Mac Address từ ONT");
                ontdevice.WriteLine("ifconfig eth0");
                Thread.Sleep(500);
                mac = ontdevice.Read();
                debugWriteLine(mac);
                mac = mac.Replace("ifconfig eth0", "").Replace("#", "").Replace("\r\n", "").Replace("\r", "").Trim();
                string[] buffer = mac.Split(new string[] { "HWaddr" }, StringSplitOptions.None);
                mac = buffer[1].Trim();
                mac = mac.Substring(0, 17).Replace(":", "").ToUpper();
                debugWriteResult(true);

                //So sanh voi tieu chuan
                debugWriteStep("So sánh MAC đọc được từ ONT với tem");
                string st = "";
                Dispatcher.Invoke(new Action(() => { st = txtMAC.Text; }));
                debugWriteLine(string.Format("Tiêu chuẩn: {0}", st));
                debugWriteLine(string.Format("Thực tế: {0}", mac));
                ret = mac.ToUpper() == st.ToUpper();
                if (ret == false) {
                    debugWriteResult(false);
                    goto NG;
                }
                debugWriteResult(true);
                goto OK;
                NG:
                {
                    ontdevice.Close();
                    Dispatcher.Invoke(new Action(() => {
                        b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#EEEEEE");
                        b.Content = "Kiểm tra MAC";
                        tbMACResult.Text = "FAIL";
                        debugWriteLine("<");
                        tbMACAddress.Text = mac;
                    }));
                    MessageBox.Show(message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                OK:
                {
                    ontdevice.Close();
                    Dispatcher.Invoke(new Action(() => {
                        b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#EEEEEE");
                        b.Content = "Kiểm tra MAC";
                        tbMACResult.Text = "PASS";
                        debugWriteLine("<");
                        tbMACAddress.Text = mac;
                    }));
                    MessageBox.Show("Hoàn thành", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
            }));
            t.IsBackground = true;
            t.Start();
        }
        #endregion

        #region Check LAN
        //Clear LAN
        void InitialLANControl() {
            rtbLAN.Document.Blocks.Clear();
            lblLAN1Result.Content = "--";
            lblLAN2Result.Content = "--";
            lblLAN3Result.Content = "--";
            lblLAN4Result.Content = "--";
            tbLANResult.Text = "--";
            
        }
        //Check LAN
        private void btnCheckLAN_Click(object sender, RoutedEventArgs e) {
            Button b = sender as Button;
            rtb = rtbLAN;
            rtbLAN.Document.Blocks.Clear();
            lblLAN1Result.Content = "wait";
            lblLAN2Result.Content = "wait";
            lblLAN3Result.Content = "wait";
            lblLAN4Result.Content = "wait";
            tbLANResult.Text = "wait";
            string message = "";
            debugWriteLine("KIỂM TRA CỔNG LAN >>>>>>>>>>>>>>>>>>>>>>>>>>>>>\n");
            b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#3aed1e");
            b.Content = "Đang kiểm tra LAN";
            string data = "";
            Thread t = new Thread(new ThreadStart(() => {
                bool ret = false;
                //Mo cong telnet vao ONT
                REPEAT:
                debugWriteStep("Mở cổng telnet tới ONT");
                ONT ontdevice = new ONT(GlobalData.initSetting.DUTIP, 23);
                debugWriteResult(true);

                //Connect vao ONT
                debugWriteStep("Kết nối telnet vào ONT");
                if (ontdevice.Connection() == false) {
                    debugWriteResult(false);
                    goto NG;
                }
                debugWriteResult(true);

                //Login vao ONT
                debugWriteStep("Login vào ONT");
                try {
                    ret = ontdevice.Login0(GlobalData.initSetting.DUTTELNETUSER, GlobalData.initSetting.DUTTELNETPASS, out message);
                }
                catch (Exception ex) {
                    debugWriteLine(ex.ToString());
                    goto REPEAT;
                }
                debugWriteLine(message);
                if (ret == false) {
                    debugWriteResult(false);
                    goto NG;
                }
                debugWriteResult(true);

                //Kiem tra cong LAN 1
                debugWriteStep("Kiểm tra cổng LAN-Port1");
                bool ret1 = false;
                ontdevice.WriteLine("ethphxcmd eth0 media-type port 0");
                Thread.Sleep(300);
                data = ontdevice.Read();
                debugWriteLine(data);
                ret = data.Contains("Link is up") || data.Contains("Link is down");
                if (!ret) ret1 = false;
                else ret1 = data.Contains("Link is up");
                debugWriteResult(ret1);
                Dispatcher.Invoke(new Action(() => { lblLAN1Result.Content = ret1 == true ? "PASS" : "FAIL"; }));

                //Kiem tra cong LAN 2
                debugWriteStep("Kiểm tra cổng LAN-Port2");
                bool ret2 = false;
                ontdevice.WriteLine("ethphxcmd eth0 media-type port 1");
                Thread.Sleep(300);
                data = ontdevice.Read();
                debugWriteLine(data);
                ret = data.Contains("Link is up") || data.Contains("Link is down");
                if (!ret) ret2 = false;
                else ret2 = data.Contains("Link is up");
                debugWriteResult(ret2);
                Dispatcher.Invoke(new Action(() => { lblLAN2Result.Content = ret2 == true ? "PASS" : "FAIL"; }));

                //Kiem tra cong LAN 3
                debugWriteStep("Kiểm tra cổng LAN-Port3");
                bool ret3 = false;
                ontdevice.WriteLine("ethphxcmd eth0 media-type port 2");
                Thread.Sleep(300);
                data = ontdevice.Read();
                debugWriteLine(data);
                ret = data.Contains("Link is up") || data.Contains("Link is down");
                if (!ret) ret3 = false;
                else ret3 = data.Contains("Link is up");
                debugWriteResult(ret3);
                Dispatcher.Invoke(new Action(() => { lblLAN3Result.Content = ret3 == true ? "PASS" : "FAIL"; }));

                //Kiem tra cong LAN 4
                debugWriteStep("Kiểm tra cổng LAN-Port4");
                bool ret4 = false;
                ontdevice.WriteLine("ethphxcmd eth0 media-type port 3");
                Thread.Sleep(300);
                data = ontdevice.Read();
                debugWriteLine(data);
                ret = data.Contains("Link is up") || data.Contains("Link is down");
                if (!ret) ret4 = false;
                else ret4 = data.Contains("Link is up");
                debugWriteResult(ret4);
                Dispatcher.Invoke(new Action(() => {lblLAN4Result.Content = ret4 == true ? "PASS" : "FAIL";}));

                if (ret1 == false || ret2 == false || ret3 ==false || ret4 == false)  goto NG;
                goto OK;
                NG:
                {
                    ontdevice.Close();
                    Dispatcher.Invoke(new Action(() => {
                        b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#EEEEEE");
                        b.Content = "Kiểm tra LAN";
                        if (lblLAN1Result.Content.ToString()=="wait") {
                            lblLAN1Result.Content = "FAIL";
                            lblLAN2Result.Content = "FAIL";
                            lblLAN3Result.Content = "FAIL";
                            lblLAN4Result.Content = "FAIL";
                        }
                        tbLANResult.Text = "FAIL";
                        debugWriteLine("<");
                    }));
                    MessageBox.Show("Hoàn thành", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                OK:
                {
                    ontdevice.Close();
                    Dispatcher.Invoke(new Action(() => {
                        b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#EEEEEE");
                        b.Content = "Kiểm tra LAN";
                        tbLANResult.Text = "PASS";
                        debugWriteLine("<");
                    }));
                    MessageBox.Show("Hoàn thành", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
            }));
            t.IsBackground = true;
            t.Start();
        }

        #endregion

        #region Check USB
        //Clear USB
        void InitialUSBControl() {
            rtbUSB.Document.Blocks.Clear();
            lblUSB2Result.Content = "--";
            lblUSB3Result.Content = "--";
            tbUSBResult.Text = "--";
        }
        //Check USB
        private void btnCheckUSB_Click(object sender, RoutedEventArgs e) {
            Button b = sender as Button;
            rtb = rtbUSB;
            rtbUSB.Document.Blocks.Clear();
            lblUSB2Result.Content = "wait";
            lblUSB3Result.Content = "wait";
            tbUSBResult.Text = "wait";
            string message = "";
            debugWriteLine("KIỂM TRA CỔNG USB >>>>>>>>>>>>>>>>>>>>>>>>>>>>>\n");
            b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#3aed1e");
            b.Content = "Đang kiểm tra USB";
            Thread t = new Thread(new ThreadStart(() => {
                bool ret = false;
                //Mo cong telnet vao ONT
                REPEAT:
                debugWriteStep("Mở cổng telnet tới ONT");
                ONT ontdevice = new ONT(GlobalData.initSetting.DUTIP, 23);
                debugWriteResult(true);

                //Connect vao ONT
                debugWriteStep("Kết nối telnet vào ONT");
                if (ontdevice.Connection() == false) {
                    debugWriteResult(false);
                    goto NG;
                }
                debugWriteResult(true);

                //Login vao ONT
                debugWriteStep("Login vào ONT");
                try {
                    ret = ontdevice.Login0(GlobalData.initSetting.DUTTELNETUSER, GlobalData.initSetting.DUTTELNETPASS, out message);
                }
                catch (Exception ex) {
                    debugWriteLine(ex.ToString());
                    goto REPEAT;
                }
                debugWriteLine(message);
                if (ret == false) {
                    debugWriteResult(false);
                    goto NG;
                }
                debugWriteResult(true);

                //Kiem tra cong USB
                debugWriteStep("Kiểm tra cổng USB");
                ontdevice.WriteLine("mount -t usbfs usbfs /proc/bus/usb/");
                Thread.Sleep(1000);
                ontdevice.WriteLine("cat /proc/bus/usb/devices");
                Thread.Sleep(2000);
                string getStr = ontdevice.Read();
                debugWriteLine(getStr);
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

                ret = _format1 || _format2;
                if (ret == false) {
                    goto NG;
                }
                //Có USB HUB
                string[] buffer = null;
                string usb2 = null, usb3 = null;
                int IndexofUsb3 = getStr.IndexOf(_usb3text);
                int IndexofUsb2 = getStr.IndexOf(_usb2text);
                if (IndexofUsb3 < IndexofUsb2) {
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
                    ret = ret2 && ret3;
                    Dispatcher.Invoke(new Action(() => {
                        lblUSB2Result.Content = ret2 == true ? "PASS" : "FAIL";
                        lblUSB3Result.Content = ret3 == true ? "PASS" : "FAIL";
                    }));
                    if (ret == false) goto NG;
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
                    ret = ret2 && ret3;
                    Dispatcher.Invoke(new Action(() => {
                        lblUSB2Result.Content = ret2 == true ? "PASS" : "FAIL";
                        lblUSB3Result.Content = ret3 == true ? "PASS" : "FAIL";
                    }));
                    if (ret == false) goto NG;
                }
                goto OK;
                NG:
                {
                    ontdevice.Close();
                    Dispatcher.Invoke(new Action(() => {
                        b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#EEEEEE");
                        b.Content = "Kiểm tra USB";
                        if (lblUSB2Result.Content.ToString() == "wait") {
                            lblUSB2Result.Content = "FAIL";
                            lblUSB3Result.Content = "FAIL";
                        }
                        tbUSBResult.Text = "FAIL";
                        debugWriteLine("<");
                    }));
                    MessageBox.Show("Hoàn thành", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                OK:
                {
                    ontdevice.Close();
                    Dispatcher.Invoke(new Action(() => {
                        b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#EEEEEE");
                        b.Content = "Kiểm tra USB";
                        tbUSBResult.Text = "PASS";
                        debugWriteLine("<");
                    }));
                    MessageBox.Show("Hoàn thành", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
            }));
            t.IsBackground = true;
            t.Start();
        }

        #endregion

        #region Kiểm tra đồng bộ quang

        //Clear SYN
        void InitialSYNControl() {
            rtbSYN.Document.Blocks.Clear();
            lblONTResult.Content = "--";
            lblOLTResult.Content = "--";
            tbSYNResult.Text = "--";
        }

        /// <summary>
        /// Đọc thông tin đồng bộ quang từ OLT
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReadOLT_Click(object sender, RoutedEventArgs e) {
            Button b = sender as Button;
            rtb = rtbSYN;
            InitialSYNControl();
            tbSYNResult.Text = "wait";
            string message = "";
            debugWriteLine("ĐỌC DỮ LIỆU TỪ OLT >>>>>>>>>>>>>>>>>>>>>>>>>>>>>\n");
            b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#3aed1e");
            b.Content = "Đang đọc dữ liệu từ OLT";
            Thread t = new Thread(new ThreadStart(() => {
                bool ret = false;
                //Mở cổng telnet vào OLT
                REPEAT:
                debugWriteStep("Mở cổng telnet tới OLT");
                OLT oltdevice = new OLT(GlobalData.initSetting.OLTIP, 23);
                debugWriteResult(true);

                //Connect vào OLT
                debugWriteStep("Kết nối telnet vào OLT");
                if (oltdevice.Connection() == false) {
                    debugWriteResult(false);
                    goto NG;
                }
                debugWriteResult(true);

                //Login vào OLT
                debugWriteStep("Login vào OLT");
                try {
                    ret = oltdevice.Login0(GlobalData.initSetting.OLTTELNETUSER, GlobalData.initSetting.OLTTELNETPASS, out message);
                }
                catch (Exception ex) {
                    debugWriteLine(ex.ToString());
                    goto REPEAT;
                }
                debugWriteLine(message);
                if (ret == false) {
                    debugWriteResult(false);
                    goto NG;
                }
                debugWriteResult(true);

                //Lấy thông tin các ONT đang kết nối
                debugWriteStep("Lấy thông tin đồng bộ quang của OLT");
                oltdevice.WriteLine("environment inhibit-alarms");
                Thread.Sleep(300);
                string incomeData = string.Empty;
                oltdevice.WriteLine(string.Format("{0}{1}", GlobalData.initSetting.OLTCOMMAND, GlobalData.initSetting.OLTPORT));
                Thread.Sleep(300);
                incomeData = oltdevice.Read();
                debugWriteLine(incomeData);

                goto OK;
                NG:
                {
                    oltdevice.Close();
                    Dispatcher.Invoke(new Action(() => {
                        b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#EEEEEE");
                        b.Content = "Đọc dữ liệu từ OLT";
                        tbSYNResult.Text = "FAIL";
                        debugWriteLine("<");
                    }));
                    MessageBox.Show("Hoàn thành", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                OK:
                {
                    oltdevice.Close();
                    Dispatcher.Invoke(new Action(() => {
                        b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#EEEEEE");
                        b.Content = "Đọc dữ liệu từ OLT";
                        tbSYNResult.Text = "PASS";
                        debugWriteLine("<");
                    }));
                    MessageBox.Show("Hoàn thành", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
            }));
            t.IsBackground = true;
            t.Start();
        }

        private void btnCheckSYN_Click(object sender, RoutedEventArgs e) {
            Button b = sender as Button;
            rtb = rtbSYN;
            InitialSYNControl();
            tbSYNResult.Text = "wait";
            string message = "";
            string GPONSN = "";
            debugWriteLine("KIỂM TRA ĐỒNG BỘ QUANG >>>>>>>>>>>>>>>>>>>>>>>>>>>>>\n");
            b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#3aed1e");
            b.Content = "Đang kiểm tra đồng bộ quang";
            Thread t = new Thread(new ThreadStart(() => {
                bool ret = false;
                //Mo cong telnet vao ONT
                REPEAT:
                debugWriteStep("Mở cổng telnet tới ONT");
                ONT ontdevice = new ONT(GlobalData.initSetting.DUTIP, 23);
                debugWriteResult(true);

                //Connect vao ONT
                debugWriteStep("Kết nối telnet vào ONT");
                if (ontdevice.Connection() == false) {
                    debugWriteResult(false);
                    goto NG;
                }
                debugWriteResult(true);

                //Login vao ONT
                debugWriteStep("Login vào ONT");
                try {
                    ret = ontdevice.Login0(GlobalData.initSetting.DUTTELNETUSER, GlobalData.initSetting.DUTTELNETPASS, out message);
                }
                catch (Exception ex) {
                    debugWriteLine(ex.ToString());
                    goto REPEAT;
                }
                debugWriteLine(message);
                if (ret == false) {
                    debugWriteResult(false);
                    goto NG;
                }
                debugWriteResult(true);

                //Lấy mã GPON ONT
                debugWriteStep("Đọc mã GPON của ONT");
                ontdevice.WriteLine("prolinecmd gponsn display");
                Thread.Sleep(300);
                string tmpStr = ontdevice.Read();
                debugWriteLine(tmpStr);
                if (tmpStr.Contains("GPON")) {
                    string[] buffer = tmpStr.Split(new string[] { "GPON." }, StringSplitOptions.None);
                    tmpStr = buffer[1].Trim().Replace("\n", "").Replace("\r", "").Replace("#","").Replace("sn:","");
                    GPONSN = tmpStr;
                    Dispatcher.Invoke(new Action(() => { lblONTResult.Content = tmpStr; }));
                } else {
                    debugWriteResult(false);
                    goto NG;
                }
                debugWriteResult(true);

                //Đóng kêt nối telnet ONT
                debugWriteStep("Đóng kết nối telnet ONT");
                ontdevice.Close();
                debugWriteResult(true);

                //Mở cổng telnet vào OLT
                REPEAT2:
                debugWriteStep("Mở cổng telnet tới OLT");
                OLT oltdevice = new OLT(GlobalData.initSetting.OLTIP, 23);
                debugWriteResult(true);

                //Connect vào OLT
                debugWriteStep("Kết nối telnet vào OLT");
                if (oltdevice.Connection() == false) {
                    debugWriteResult(false);
                    goto NG;
                }
                debugWriteResult(true);

                //Login vào OLT
                debugWriteStep("Login vào OLT");
                try {
                    ret = oltdevice.Login0(GlobalData.initSetting.OLTTELNETUSER, GlobalData.initSetting.OLTTELNETPASS, out message);
                }
                catch (Exception ex) {
                    debugWriteLine(ex.ToString());
                    goto REPEAT2;
                }
                debugWriteLine(message);
                if (ret == false) {
                    debugWriteResult(false);
                    goto NG;
                }
                debugWriteResult(true);

                //Lấy thông tin các ONT đang kết nối
                int count = 0;
                GET:
                debugWriteStep("Lấy thông tin đồng bộ quang của OLT");
                oltdevice.WriteLine("environment inhibit-alarms");
                Thread.Sleep(300);
                string incomeData = string.Empty;
                oltdevice.WriteLine(string.Format("{0}{1}", GlobalData.initSetting.OLTCOMMAND, GlobalData.initSetting.OLTPORT));
                Thread.Sleep(300);
                incomeData = oltdevice.Read();
                debugWriteLine(incomeData);

                //Kiểm tra OLT đã nhận ONT hay chưa?
                debugWriteStep("Kiểm tra OLT có nhận ONT hay không");
                if (incomeData.Contains(GPONSN) == false) {
                    debugWriteResult(false);
                    count++;
                    if (count < 3) goto GET;
                    else goto NG;
                }
                debugWriteResult(true);

                goto OK;
                NG:
                {
                    ontdevice.Close();
                    Dispatcher.Invoke(new Action(() => {
                        b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#EEEEEE");
                        b.Content = "Kiểm tra đồng bộ quang";
                        tbSYNResult.Text = "FAIL";
                        debugWriteLine("<");
                    }));
                    MessageBox.Show("Hoàn thành", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                OK:
                {
                    ontdevice.Close();
                    Dispatcher.Invoke(new Action(() => {
                        b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#EEEEEE");
                        b.Content = "Kiểm tra đồng bộ quang";
                        tbSYNResult.Text = "PASS";
                        debugWriteLine("<");
                    }));
                    MessageBox.Show("Hoàn thành", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
            }));
            t.IsBackground = true;
            t.Start();
        }


        #endregion

        #region Kiểm tra công suất quang
        //Clear PowerOptical
        void InitialPowerOpticalControl() {
            rtbPowerOptical.Document.Blocks.Clear();
            lblTXResult.Content = "--";
            lblRXResult.Content = "--";
            tbPowerOpticalResult.Text = "--";
        }
        //Check PowerOptical
        private void btnCheckPowerOptical_Click(object sender, RoutedEventArgs e) {
            Button b = sender as Button;
            rtb = rtbPowerOptical;
            rtbPowerOptical.Document.Blocks.Clear();
            lblTXResult.Content = "wait";
            lblRXResult.Content = "wait";
            tbPowerOpticalResult.Text = "wait";
            string message = "";
            debugWriteLine("KIỂM TRA CÔNG SUẤT QUANG >>>>>>>>>>>>>>>>>>>>>>>>>>>>>\n");
            b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#3aed1e");
            b.Content = "Đang kiểm tra công suất quang";
            Thread t = new Thread(new ThreadStart(() => {
                bool ret = false;
                //Mo cong telnet vao ONT
                REPEAT:
                debugWriteStep("Mở cổng telnet tới ONT");
                ONT ontdevice = new ONT(GlobalData.initSetting.DUTIP, 23);
                debugWriteResult(true);

                //Connect vao ONT
                debugWriteStep("Kết nối telnet vào ONT");
                if (ontdevice.Connection() == false) {
                    debugWriteResult(false);
                    goto NG;
                }
                debugWriteResult(true);

                //Login vao ONT
                debugWriteStep("Login vào ONT");
                try {
                    ret = ontdevice.Login0(GlobalData.initSetting.DUTTELNETUSER, GlobalData.initSetting.DUTTELNETPASS, out message);
                }
                catch (Exception ex) {
                    debugWriteLine(ex.ToString());
                    goto REPEAT;
                }
                debugWriteLine(message);
                if (ret == false) {
                    debugWriteResult(false);
                    goto NG;
                }
                debugWriteResult(true);

                //Kiểm tra công suất quang
                debugWriteStep("Kiểm tra công suất phát TX");
                ontdevice.WriteLine("tcapi get Info_PonPhy TxPower");
                Thread.Sleep(300);
                bool txRet = false;
                string txData = ontdevice.Read0();
                debugWriteLine(txData);
                double txPower = double.MinValue;
                if (txData.Contains("tcapi get Info_PonPhy TxPower")) {
                    txData = txData.Replace("tcapi get Info_PonPhy TxPower", "").Replace("#", "").Replace("\r\n", "").Replace("\r", "").Trim();
                    txPower = _InttodBm(txData);
                    debugWriteLine(string.Format("Giá trị quy đổi {0}dBm", txPower));
                    debugWriteLine(string.Format("Tiêu chuẩn từ {0}dBm đến {1}dBm", GlobalData.initSetting.DUTTXMIN, GlobalData.initSetting.DUTTXMAX));
                    //compare txData with standard
                    txRet = (txPower <= GlobalData.initSetting.DUTTXMAX) && (txPower >= GlobalData.initSetting.DUTTXMIN);
                    debugWriteResult(txRet);
                    Dispatcher.Invoke(new Action(() => { lblTXResult.Content = txRet == true ? "PASS" : "FAIL"; }));
                } else {
                    debugWriteResult(false);
                    Dispatcher.Invoke(new Action(() => { lblTXResult.Content = "FAIL"; }));
                    }

                //Kiểm tra công suất thu
                debugWriteStep("Kiểm tra công suất phát RX");
                ontdevice.WriteLine("tcapi get Info_PonPhy RxPower");
                Thread.Sleep(300);
                bool rxRet = false;
                string rxData = ontdevice.Read0();
                debugWriteLine(rxData);
                double rxPower = double.MinValue;
                if (rxData.Contains("tcapi get Info_PonPhy RxPower")) {
                    rxData = rxData.Replace("tcapi get Info_PonPhy RxPower", "").Replace("#", "").Replace("\r\n", "").Replace("\r", "").Trim();
                    rxPower = _InttodBm(rxData);
                    debugWriteLine(string.Format("Giá trị quy đổi {0}dBm", rxPower));
                    debugWriteLine(string.Format("Tiêu chuẩn từ {0}dBm đến {1}dBm", GlobalData.initSetting.DUTRXMIN, GlobalData.initSetting.DUTRXMAX));
                    //compare rxData with standard
                    rxRet = (rxPower <= GlobalData.initSetting.DUTRXMAX) && (rxPower >= GlobalData.initSetting.DUTRXMIN);
                    debugWriteResult(rxRet);
                    Dispatcher.Invoke(new Action(() => { lblRXResult.Content = rxRet == true ? "PASS" : "FAIL"; }));
                }
                else {
                    debugWriteResult(false);
                    Dispatcher.Invoke(new Action(() => { lblRXResult.Content = "FAIL"; }));
                }
                if (txRet == false || rxRet == false) goto NG;
                goto OK;
                NG:
                {
                    ontdevice.Close();
                    Dispatcher.Invoke(new Action(() => {
                        b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#EEEEEE");
                        b.Content = "Kiểm tra công suất quang";
                        if (lblTXResult.Content.ToString() == "wait") {
                            lblTXResult.Content = "FAIL";
                            lblRXResult.Content = "FAIL";
                        }
                        tbPowerOpticalResult.Text = "FAIL";
                        debugWriteLine("<");
                    }));
                    MessageBox.Show("Hoàn thành", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                OK:
                {
                    ontdevice.Close();
                    Dispatcher.Invoke(new Action(() => {
                        b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#EEEEEE");
                        b.Content = "Kiểm tra công suất quang";
                        tbPowerOpticalResult.Text = "PASS";
                        debugWriteLine("<");
                    }));
                    MessageBox.Show("Hoàn thành", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
            }));
            t.IsBackground = true;
            t.Start();
        }
        #endregion

        #region Kiểm tra LEDs

        private void btnLed_Click(object sender, RoutedEventArgs e) {
            Button b = sender as Button;
            rtb = rtbLED;
            rtbLED.Document.Blocks.Clear();
            string message = "";
            b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#3aed1e");
            string temp = b.Content.ToString();
            b.Content = "Đang " + temp;
            debugWriteLine(string.Format("{0} >>>>>>>>>>>>>>>>>>>>>>>>>>>>>\n", temp.ToUpper()));
            Thread t = new Thread(new ThreadStart(() => {
                bool ret = false;
                //Mo cong telnet vao ONT
                REPEAT:
                debugWriteStep("Mở cổng telnet tới ONT");
                ONT ontdevice = new ONT(GlobalData.initSetting.DUTIP, 23);
                debugWriteResult(true);

                //Connect vao ONT
                debugWriteStep("Kết nối telnet vào ONT");
                if (ontdevice.Connection() == false) {
                    debugWriteResult(false);
                    goto NG;
                }
                debugWriteResult(true);

                //Login vao ONT
                debugWriteStep("Login vào ONT");
                try {
                    ret = ontdevice.Login0(GlobalData.initSetting.DUTTELNETUSER, GlobalData.initSetting.DUTTELNETPASS, out message);
                }
                catch (Exception ex) {
                    debugWriteLine(ex.ToString());
                    goto REPEAT;
                }
                debugWriteLine(message);
                if (ret == false) {
                    debugWriteResult(false);
                    goto NG;
                }
                debugWriteResult(true);
                Thread.Sleep(1000);

                //Bat LED
                debugWriteStep("Bật LED");
                switch (temp) {
                    case "Bật LED PON": {
                            ontdevice.WriteLine("sys memwl bfbf0204 0x2");
                            Thread.Sleep(200);
                            break;
                        }
                    case "Bật LED INET Xanh": {
                            ontdevice.WriteLine("echo \"1 0\" > proc/tc3162/led_internet");
                            Thread.Sleep(200);
                            break;
                        }
                    case "Bật LED INET Đỏ": {
                            ontdevice.WriteLine("echo \"0 1\" > proc/tc3162/led_internet");
                            Thread.Sleep(200);
                            break;
                        }
                    case "Bật LED WLAN": {
                            ontdevice.WriteLine("iwpriv ra0 set led_setting=00-00-00-00-00-00-00-00");
                            Thread.Sleep(200);
                            break;
                        }
                    case "Bật LED WPS": {
                            ontdevice.WriteLine("iwpriv ra0 set led_setting=01-00-00-00-00-00-00-00");
                            Thread.Sleep(200);
                            break;
                        }
                    case "Bật LED LOS": {
                            ontdevice.WriteLine("echo 1 > /proc/xpon/los_led");
                            Thread.Sleep(200);
                            break;
                        }
                }
                debugWriteResult(true);
                goto OK;
                NG:
                {
                    ontdevice.Close();
                    Dispatcher.Invoke(new Action(() => {
                        b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#EEEEEE");
                        b.Content = temp;
                        debugWriteLine("<");
                    }));
                    MessageBox.Show("Hoàn thành", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                OK:
                {
                    ontdevice.Close();
                    Dispatcher.Invoke(new Action(() => {
                        b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#EEEEEE");
                        b.Content = temp;
                        debugWriteLine("<");
                    }));
                    MessageBox.Show("Hoàn thành", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
            }));
            t.IsBackground = true;
            t.Start();
        }

        #endregion

        #region Kiểm tra nút nhấn
        //Clear Button
        void InitialButtonControl() {
            rtbButton.Document.Blocks.Clear();
            lblbuttonLegend.Content = "--";
            lblWPSResult.Content = "--";
            lblResetResult.Content = "--";
            tbButtonResult.Text = "--";
        }

        private void btnCheckButton_Click(object sender, RoutedEventArgs e) {
            Button b = sender as Button;
            rtb = rtbButton;
            rtbButton.Document.Blocks.Clear();
            lblWPSResult.Content = "wait";
            lblResetResult.Content = "wait";
            tbButtonResult.Text = "wait";
            string message = "";
            debugWriteLine("KIỂM TRA NÚT NHẤN >>>>>>>>>>>>>>>>>>>>>>>>>>>>>\n");
            b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#3aed1e");
            b.Content = "Đang kiểm tra nút nhấn";
            Thread t = new Thread(new ThreadStart(() => {
                bool ret = false;
                //Mo cong telnet vao ONT
                REPEAT:
                debugWriteStep("Mở cổng telnet tới ONT");
                ONT ontdevice = new ONT(GlobalData.initSetting.DUTIP, 23);
                debugWriteResult(true);

                //Connect vao ONT
                debugWriteStep("Kết nối telnet vào ONT");
                if (ontdevice.Connection() == false) {
                    debugWriteResult(false);
                    goto NG;
                }
                debugWriteResult(true);

                //Login vao ONT
                debugWriteStep("Login vào ONT");
                try {
                    ret = ontdevice.Login0(GlobalData.initSetting.DUTTELNETUSER, GlobalData.initSetting.DUTTELNETPASS, out message);
                }
                catch (Exception ex) {
                    debugWriteLine(ex.ToString());
                    goto REPEAT;
                }
                debugWriteLine(message);
                if (ret == false) {
                    debugWriteResult(false);
                    goto NG;
                }
                debugWriteResult(true);

                //Kiem tra nut nhan WPS
                debugWriteStep("Kiểm tra nút nhấn WPS");
                Dispatcher.Invoke(new Action(() => { lblbuttonLegend.Content = "VUI LÒNG NHẤN NÚT WPS"; }));
                string Result = "";
                int index = TimeOuts.x30;
                while (Result == "") {
                    //telnet vào ONT để lấy kết quả nút nhấn wps
                    ontdevice.WriteLine("cat proc/kmsg &");
                    Thread.Sleep(1000);
                    string tmpstr = "";
                    tmpstr = ontdevice.Read0();
                    debugWriteLine(tmpstr);
                    if (tmpstr.Contains("VNPTT-WPS button is pressed")) { Result = "PASS"; break; }
                    index--;
                    if (index == 0) { Result = "FAIL"; break; }
                }
                Dispatcher.Invoke(new Action(() => { lblWPSResult.Content = Result; }));
                bool ret1 = Result == "PASS";
                debugWriteResult(ret1);

                //Kiem tra nut Reset
                debugWriteStep("Kiểm tra nút nhấn Reset");
                Dispatcher.Invoke(new Action(() => { lblbuttonLegend.Content = "VUI LÒNG NHẤN NÚT RESET"; }));
                Result = "";
                index = TimeOuts.x30;
                while (Result == "") {
                    //telnet vào ONT để lấy kết quả nút nhấn wps
                    ontdevice.WriteLine("cat proc/kmsg &");
                    Thread.Sleep(1000);
                    string tmpstr = "";
                    tmpstr = ontdevice.Read0();
                    debugWriteLine(tmpstr);
                    if (tmpstr.Contains("VNPTT-RESET button is pressed")) { Result = "PASS"; break; }
                    index--;
                    if (index == 0) { Result = "FAIL"; break; }
                }
                Dispatcher.Invoke(new Action(() => { lblResetResult.Content = Result; }));
                bool ret2 = Result == "PASS";
                debugWriteResult(ret2);
                if (ret1 == false || ret2 == false) goto NG;
                goto OK;
                NG:
                {
                    ontdevice.Close();
                    Dispatcher.Invoke(new Action(() => {
                        b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#EEEEEE");
                        b.Content = "Kiểm tra nút nhấn";
                        lblbuttonLegend.Content = "--";
                        if (lblWPSResult.Content.ToString() == "wait") {
                            lblWPSResult.Content = "FAIL";
                            lblResetResult.Content = "FAIL";
                        }
                        tbButtonResult.Text = "FAIL";
                        debugWriteLine("<");
                    }));
                    MessageBox.Show("Hoàn thành", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                OK:
                {
                    ontdevice.Close();
                    Dispatcher.Invoke(new Action(() => {
                        b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#EEEEEE");
                        b.Content = "Kiểm tra nút nhấn";
                        lblbuttonLegend.Content = "--";
                        tbButtonResult.Text = "PASS";
                        debugWriteLine("<");
                    }));
                    MessageBox.Show("Hoàn thành", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
            }));
            t.IsBackground = true;
            t.Start();
        }

        #endregion

    }
}
