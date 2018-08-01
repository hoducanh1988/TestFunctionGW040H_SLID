using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TestFunctionGW040x {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        private void setStartupLocation() {
            //double scaleX = 0.75;
            //double scaleY = 0.95;
            //this.Height = SystemParameters.WorkArea.Height * scaleY;
            //this.Width = SystemParameters.WorkArea.Width * scaleX;
            //this.Top = (SystemParameters.WorkArea.Height * (1 - scaleY)) / 2;
            //this.Left = (SystemParameters.WorkArea.Width * (1 - scaleX)) / 2;

            //GlobalData.thisLocation.top = (SystemParameters.WorkArea.Height * (1 - scaleY)) / 2;
            //GlobalData.thisLocation.left = (SystemParameters.WorkArea.Width * (1 - scaleX)) / 2;
            //GlobalData.thisLocation.width = SystemParameters.WorkArea.Width * scaleX;
            //GlobalData.thisLocation.height = SystemParameters.WorkArea.Height * scaleY;

            GlobalData.thisLocation.top = this.Top;
            GlobalData.thisLocation.left = this.Left;
            GlobalData.thisLocation.width = 1000;
            GlobalData.thisLocation.height = 750;
        }

        private void bringUCtoFront(int index) {
            List<Control> list = new List<Control>() { ucTesting, ucStep, ucSetting, ucHelp, ucAbout, ucLogin };
            //If index != 2
            switch (index) {
                case 2: {
                        //disable all
                        for (int i = 0; i < list.Count; i++) {
                            list[i].Visibility = Visibility.Collapsed;
                            Canvas.SetZIndex(list[i], 0);
                        }
                        LOGIN login = new LOGIN();
                        login.ShowDialog();
                        //visible login
                        if (GlobalData.testingInfo.USER == "admin" && GlobalData.testingInfo.PASSWORD == "vnpt") {
                            ucSetting.Visibility = Visibility.Visible;
                            Canvas.SetZIndex(ucSetting, 1);
                        }
                        else {
                            ucLogin.Visibility = Visibility.Visible;
                            Canvas.SetZIndex(ucLogin, 1);
                        }
                        break;
                    }
                default: {
                        for (int i = 0; i < list.Count; i++) {
                            if (i == index) {
                                list[i].Visibility = Visibility.Visible;
                                Canvas.SetZIndex(list[i], 1);
                            }
                            else {
                                list[i].Visibility = Visibility.Collapsed;
                                Canvas.SetZIndex(list[i], 0);
                            }
                        }
                        break;
                    }
            }
        }

        public MainWindow() {
            InitializeComponent();
            this.setStartupLocation();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e) {
            try {
                //this.DragMove();
                GlobalData.thisLocation.top = this.Top;
                GlobalData.thisLocation.left = this.Left;
            }
            catch { }
        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e) {
            Label l = sender as Label;
            switch (l.Content.ToString()) {
                case "X": {
                        this.Close();
                        break;
                    }
                case "***01. [Open Log test]": {
                        Process.Start("explorer.exe", string.Format("{0}LOG", System.AppDomain.CurrentDomain.BaseDirectory));
                        break;
                    }
                case "***02. [Open Log detail]": {
                        Process.Start("explorer.exe", string.Format("{0}LogDetail", System.AppDomain.CurrentDomain.BaseDirectory));
                        break;
                    }
                case "?": {
                        if (l.Foreground != Brushes.Lime) {
                            l.Foreground = Brushes.Lime;
                            this.Cursor = Cursors.Help;
                            GlobalData.initSetting.HELP = true;
                        }
                        else {
                            l.Foreground = Brushes.White;
                            this.Cursor = Cursors.Arrow;
                            GlobalData.initSetting.HELP = false;
                        }
                        break;
                    }
                default: {
                        Dictionary<string, int> dictionary = new Dictionary<string, int>() { { "TEST ALL", 0 }, { "TEST ONE", 1 }, { "SETTING", 2 }, { "HELP", 3 }, { "ABOUT", 4 } };
                        int t;
                        bool ret = dictionary.TryGetValue(l.Content.ToString(), out t);
                        this.lblMinus.Margin = new Thickness(t * 100, 0, 0, 0);
                        this.bringUCtoFront(t);
                        break;
                    }
            }
        }
    }
}
