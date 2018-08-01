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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TestFunctionGW040x.Funtions;

namespace TestFunctionGW040x.UserControls {
    /// <summary>
    /// Interaction logic for ucSetting.xaml
    /// </summary>
    public partial class ucSetting : UserControl {


        private void InitializeItemSource()
        {
            //Get list SLID
            string file = string.Format("{0}Setting\\SLID.ini", System.AppDomain.CurrentDomain.BaseDirectory);
            if (System.IO.File.Exists(file) == true) {
                string[] lines = System.IO.File.ReadAllLines(file);
                if (lines.Length == 0) return;

                GlobalData.listSLID = new List<string>();
                for (int i = 0; i < lines.Length; i++) {
                    try {
                        if (lines[i].Trim().Length > 0)
                            GlobalData.listSLID.Add(lines[i]);
                    }
                    catch { }
                }
            }

            this.cbbBarcodeType.ItemsSource = initParameters.listBarcodeType;
            this.cbbBRPort.ItemsSource = initParameters.listUARTPort;
            this.cbbBRBaudRate.ItemsSource = initParameters.listBaudRate;
            this.cbblistSLID.ItemsSource = GlobalData.listSLID;
        }


        public ucSetting() {
            InitializeComponent();
            InitializeItemSource();
            this.DataContext = GlobalData.initSetting;
           
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            GlobalData.initSetting.Save();
            GlobalData.testingInfo.initialization();
            MessageBox.Show("Thành công!", "Lưu cài đặt", MessageBoxButton.OK, MessageBoxImage.Information);
        }

    }
}
