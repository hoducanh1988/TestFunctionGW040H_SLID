using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using TestFunctionGW040x.Funtions;

namespace TestFunctionGW040x {
    /// <summary>
    /// Interaction logic for frmLED.xaml
    /// </summary>
    public partial class frmLED : Window {
        public frmLED() {
            InitializeComponent();
            this.DataContext = GlobalData.testingInfo;
        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e) {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            ChangeLegendForeground();
        }

        private void ChangeLegendForeground() {
            DispatcherTimer timer = new DispatcherTimer();
            int counter = 0;
            timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            timer.Tick += ((sd, ev) => {
                counter++;
                if (counter % 2 == 0) this.lblLegend.Foreground = Brushes.White;
                else this.lblLegend.Foreground = Brushes.Red;
                if (counter > 99) counter = 0;
            });
            timer.Start();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e) {
            Border gr = sender as Border;
            switch (gr.Name) {
                case "grPower": {
                        GlobalData.testingInfo.POWERJUD = !GlobalData.testingInfo.POWERJUD;
                        break;
                    }
                case "grPon": {
                        GlobalData.testingInfo.PONJUD = !GlobalData.testingInfo.PONJUD;
                        break;
                    }
                case "grInet": {
                        GlobalData.testingInfo.INETJUD = !GlobalData.testingInfo.INETJUD;
                        break;
                    }
                case "grWlan": {
                        GlobalData.testingInfo.WLANJUD = !GlobalData.testingInfo.WLANJUD;
                        break;
                    }
                case "grWlan5g": {
                        GlobalData.testingInfo.WLAN5GJUD = !GlobalData.testingInfo.WLAN5GJUD;
                        break;
                    }
                case "grWps": {
                        GlobalData.testingInfo.WPSJUD = !GlobalData.testingInfo.WPSJUD;
                        break;
                    }
                case "grLos": {
                        GlobalData.testingInfo.LOSJUD = !GlobalData.testingInfo.LOSJUD;
                        break;
                    }
                case "grLan1": {
                        GlobalData.testingInfo.LAN1JUD = !GlobalData.testingInfo.LAN1JUD;
                        break;
                    }
                case "grLan2": {
                        GlobalData.testingInfo.LAN2JUD = !GlobalData.testingInfo.LAN2JUD;
                        break;
                    }
                case "grLan3": {
                        GlobalData.testingInfo.LAN3JUD = !GlobalData.testingInfo.LAN3JUD;
                        break;
                    }
                case "grLan4": {
                        GlobalData.testingInfo.LAN4JUD = !GlobalData.testingInfo.LAN4JUD;
                        break;
                    }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            Button b = sender as Button;
            switch (b.Name) {
                case "btnMacDinh": {
                        GlobalData.testingInfo.POWERJUD = true;
                        GlobalData.testingInfo.PONJUD = true;
                        GlobalData.testingInfo.INETJUD = true;
                        GlobalData.testingInfo.WLANJUD = true;
                        GlobalData.testingInfo.WLAN5GJUD = true;
                        GlobalData.testingInfo.LAN1JUD = true;
                        GlobalData.testingInfo.LAN2JUD = true;
                        GlobalData.testingInfo.LAN3JUD = true;
                        GlobalData.testingInfo.LAN4JUD = true;
                        GlobalData.testingInfo.WPSJUD = true;
                        GlobalData.testingInfo.LOSJUD = true;
                        break;
                    }
                case "btnDongY": {
                        bool ret = GlobalData.testingInfo.POWERJUD &&
                                   GlobalData.testingInfo.PONJUD &&
                                   GlobalData.testingInfo.INETJUD &&
                                   GlobalData.testingInfo.WLANJUD &&
                                   GlobalData.testingInfo.WLAN5GJUD &&
                                   GlobalData.testingInfo.LAN1JUD &&
                                   GlobalData.testingInfo.LAN2JUD &&
                                   GlobalData.testingInfo.LAN3JUD &&
                                   GlobalData.testingInfo.LAN4JUD &&
                                   GlobalData.testingInfo.WPSJUD &&
                                   GlobalData.testingInfo.LOSJUD;

                        if (ret == false)
                            GlobalData.testingInfo.ERRORCODE += string.Format("Fle1#{0}, ", new baseFunctions().GEN_ERRORCODE(GlobalData.testingInfo.LOSJUD,
                                                                                                                              GlobalData.testingInfo.WPSJUD,
                                                                                                                              GlobalData.testingInfo.LAN4JUD,
                                                                                                                              GlobalData.testingInfo.LAN3JUD,
                                                                                                                              GlobalData.testingInfo.LAN2JUD,
                                                                                                                              GlobalData.testingInfo.LAN1JUD,
                                                                                                                              GlobalData.testingInfo.WLANJUD,
                                                                                                                               GlobalData.testingInfo.WLAN5GJUD,
                                                                                                                              GlobalData.testingInfo.INETJUD,
                                                                                                                              GlobalData.testingInfo.PONJUD,
                                                                                                                              GlobalData.testingInfo.POWERJUD));

                        GlobalData.testingInfo.LedResult = ret == true ? "PASS" : "FAIL";
                        GlobalData.testingInfo.LOGSYSTEM += string.Format("...\r\nLED POWER = {0}\r\n", GlobalData.testingInfo.POWERJUD == true ? "PASS" : "FAIL");
                        GlobalData.testingInfo.LOGSYSTEM += string.Format("LED PON = {0}\r\n", GlobalData.testingInfo.PONJUD == true ? "PASS" : "FAIL");
                        GlobalData.testingInfo.LOGSYSTEM += string.Format("LED INET = {0}\r\n", GlobalData.testingInfo.INETJUD == true ? "PASS" : "FAIL");
                        GlobalData.testingInfo.LOGSYSTEM += string.Format("LED 2G = {0}\r\n", GlobalData.testingInfo.WLANJUD == true ? "PASS" : "FAIL");
                        GlobalData.testingInfo.LOGSYSTEM += string.Format("LED 5G = {0}\r\n", GlobalData.testingInfo.WLAN5GJUD == true ? "PASS" : "FAIL");
                        GlobalData.testingInfo.LOGSYSTEM += string.Format("LED LAN1 = {0}\r\n", GlobalData.testingInfo.LAN1JUD == true ? "PASS" : "FAIL");
                        GlobalData.testingInfo.LOGSYSTEM += string.Format("LED LAN2 = {0}\r\n", GlobalData.testingInfo.LAN2JUD == true ? "PASS" : "FAIL");
                        GlobalData.testingInfo.LOGSYSTEM += string.Format("LED LAN3 = {0}\r\n", GlobalData.testingInfo.LAN3JUD == true ? "PASS" : "FAIL");
                        GlobalData.testingInfo.LOGSYSTEM += string.Format("LED LAN4 = {0}\r\n", GlobalData.testingInfo.LAN4JUD == true ? "PASS" : "FAIL");
                        GlobalData.testingInfo.LOGSYSTEM += string.Format("LED WPS = {0}\r\n", GlobalData.testingInfo.WPSJUD == true ? "PASS" : "FAIL");
                        GlobalData.testingInfo.LOGSYSTEM += string.Format("LED LOS = {0}\r\n...\r\n", GlobalData.testingInfo.LOSJUD == true ? "PASS" : "FAIL");
                        
                        //if(ret) GlobalData.testingInfo.ONTLED = string.Format("\"All led ok\"");
                        //else {
                        //    if (GlobalData.testingInfo.POWERJUD == false) GlobalData.testingInfo.ONTLED += "power,";
                        //    if (GlobalData.testingInfo.PONJUD == false) GlobalData.testingInfo.ONTLED += "pon,";
                        //    if (GlobalData.testingInfo.INETJUD == false) GlobalData.testingInfo.ONTLED += "inet,";
                        //    if (GlobalData.testingInfo.WLANJUD == false) GlobalData.testingInfo.ONTLED += "wlan,";
                        //    if (GlobalData.testingInfo.LAN1JUD == false) GlobalData.testingInfo.ONTLED += "lan1,";
                        //    if (GlobalData.testingInfo.LAN2JUD == false) GlobalData.testingInfo.ONTLED += "lan2,";
                        //    if (GlobalData.testingInfo.LAN3JUD == false) GlobalData.testingInfo.ONTLED += "lan3,";
                        //    if (GlobalData.testingInfo.LAN4JUD == false) GlobalData.testingInfo.ONTLED += "lan4,";
                        //    if (GlobalData.testingInfo.WPSJUD == false) GlobalData.testingInfo.ONTLED += "wps,";
                        //    if (GlobalData.testingInfo.LOSJUD == false) GlobalData.testingInfo.ONTLED += "los,";
                        //    GlobalData.testingInfo.ONTLED += "=NG";
                        //    GlobalData.testingInfo.ONTLED = string.Format("\"{0}\"", GlobalData.testingInfo.ONTLED);
                        //}
                        this.Close();
                        break;
                    }
            }
        }
    }
}
