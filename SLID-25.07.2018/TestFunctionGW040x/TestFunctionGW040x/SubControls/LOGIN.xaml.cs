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

namespace TestFunctionGW040x {
    /// <summary>
    /// Interaction logic for LOGIN.xaml
    /// </summary>
    public partial class LOGIN : Window {
        public LOGIN() {
            //this.Top = GlobalData.thisLocation.top + (GlobalData.thisLocation.height / 2) - 75;
            //this.Left = GlobalData.thisLocation.left + (GlobalData.thisLocation.width - 250) / 2;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            txtUser.Focus();
        }

        private void btnGo_Click(object sender, RoutedEventArgs e) {
            GlobalData.testingInfo.USER = txtUser.Text;
            GlobalData.testingInfo.PASSWORD = txtPass.Password.ToString();
            this.Close();
        }

        private void txtPass_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                GlobalData.testingInfo.USER = txtUser.Text;
                GlobalData.testingInfo.PASSWORD = txtPass.Password.ToString();
                this.Close();
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e) {
            if (e.ClickCount == 2) {
                GlobalData.testingInfo.USER = "admin";
                GlobalData.testingInfo.PASSWORD = "vnpt";
                this.Close();
            }
        }
    }
}
