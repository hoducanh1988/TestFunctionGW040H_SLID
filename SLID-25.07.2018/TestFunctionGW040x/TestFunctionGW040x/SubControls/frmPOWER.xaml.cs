using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;
using System.Windows.Threading;
using TestFunctionGW040x.Funtions;

namespace TestFunctionGW040x {
    /// <summary>
    /// Interaction logic for frmPOWER.xaml
    /// </summary>
    public partial class frmPOWER : Window {

        private class BindingData : INotifyPropertyChanged {
            public event PropertyChangedEventHandler PropertyChanged;
            protected virtual void OnPropertyChanged(string propertyName = null) {
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null) {
                    handler(this, new PropertyChangedEventArgs(propertyName));
                }
            }

            int _timeout;
            public int TimeOut {
                get { return _timeout; }
                set {
                    _timeout = value;
                    OnPropertyChanged(nameof(TimeOut));
                }
            }
            string _powertitle;
            public string PowerTitle {
                get { return _powertitle; }
                set {
                    _powertitle = value;
                    OnPropertyChanged(nameof(PowerTitle));
                }
            }
        }

        BindingData bindingdata = new BindingData();
        
        public frmPOWER(int _timeout) {
            InitializeComponent();
            bindingdata.TimeOut = _timeout;
            bindingdata.PowerTitle = PowerTitles.Power;
            this.DataContext = bindingdata;
        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e) {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            this.ChangeLegendForeground();
            //----------------------------------//
            Thread t = new Thread(new ThreadStart(() => {
                //---------// Ping to 192.168.1.1 
                bool pingOk = false;
                //bindingdata.PowerTitle = PowerTitles.Ping;
                while (bindingdata.TimeOut > 0) {
                    if (new Network().PingNetwork(GlobalData.initSetting.DUTIP)) { pingOk = true; break; }
                    Thread.Sleep(1000);
                    bindingdata.TimeOut--;
                }
                if (pingOk == false) {
                    GlobalData.testingInfo.ERRORCODE = "0x004";
                    GlobalData.testingInfo.PowerResult = "FAIL";
                    Dispatcher.BeginInvoke(new Action(() => {
                        this.Close();
                        return;
                    }));
                }
                //---------// Login 192.168.1.1
                bindingdata.PowerTitle = PowerTitles.Login;
                ONT ontdevice = new ONT(GlobalData.initSetting.DUTIP, 23);
                bool loginOk = false;
                while (bindingdata.TimeOut > 0) {
                    string message = "";
                    try {
                        if(ontdevice.Connection()) {
                            if (ontdevice.Login0(GlobalData.initSetting.DUTTELNETUSER, GlobalData.initSetting.DUTTELNETPASS, out message)) { loginOk = true; break; }
                        }
                    } catch { }
                    Thread.Sleep(1000);
                    bindingdata.TimeOut--;
                }
                if (loginOk == false) {
                    GlobalData.testingInfo.ERRORCODE = "0x006";
                    GlobalData.testingInfo.PowerResult = "FAIL";
                    Dispatcher.BeginInvoke(new Action(() => {
                        this.Close();
                        return;
                    }));
                }
                
                else {
                    GlobalData.testingInfo.PowerResult = "PASS";
                    Dispatcher.BeginInvoke(new Action(() => {
                        this.Close();
                        return;
                    }));
                }
            }));
            t.IsBackground = true;
            t.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        private void ChangeLegendForeground() {
            DispatcherTimer timer = new DispatcherTimer();
            int counter = 0;
            timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            timer.Tick += ((sd, ev) => {
                counter++;
                if (counter % 2 == 0) this.lblLegend.Foreground = Brushes.Red;
                else this.lblLegend.Foreground = Brushes.Yellow;
                if (counter > 99) counter = 0;
            });
            timer.Start();
        }
    }

}
