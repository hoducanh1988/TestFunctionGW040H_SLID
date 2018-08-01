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
using TestFunctionGW040x.Funtions;

namespace TestFunctionGW040x
{
    /// <summary>
    /// Interaction logic for frmCONFIRM.xaml
    /// </summary>
    public partial class frmCONFIRM : Window
    {
        public frmCONFIRM()
        {
            InitializeComponent();
        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e) {
            this.Close();
        }

        private void Border_Loaded(object sender, RoutedEventArgs e)
        {
            this.passwordbox.Focus();
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            Button b = sender as Button;
            switch (b.Content.ToString()) {
                case "Hủy Bỏ": {
                        this.Close();
                        break;
                    }
                case "Đồng Ý": {
                        this.confirmPassword();
                        break;
                    }
            }
        }

        private void passwordbox_PasswordChanged(object sender, RoutedEventArgs e) {
            if (this.passwordbox.Password.ToString().Length > 0) this.tbMessage.Text = "";
        }

        private void confirmPassword() {
            if (this.passwordbox.Password.ToString() == initParameters.passwordAdmin) {
                GlobalData.testingInfo.initialization();
                GlobalData.testingInfo.ENABLEINPUTMAC = false;
                GlobalData.testingInfo.MACINPUTED = "";
                this.Close();
            }
            else {
                this.tbMessage.Text = string.Format("***LỖI: Mật khẩu người quản trị: '{0}' không đúng", this.passwordbox.Password.ToString());
                this.passwordbox.Clear();
                this.passwordbox.Focus();
            }
        }

        private void passwordbox_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) this.confirmPassword();
        }
    }
}
