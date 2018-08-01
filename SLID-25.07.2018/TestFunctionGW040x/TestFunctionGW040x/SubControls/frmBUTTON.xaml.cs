using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace TestFunctionGW040x
{
    /// <summary>
    /// Interaction logic for frmBUTTON.xaml
    /// </summary>
    public partial class frmBUTTON : Window
    {
        
        public frmBUTTON()
        {
            InitializeComponent();
            this.DataContext = GlobalData.testingInfo;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            ChangeLegendForeground();
        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e) {
            this.Close();
        }

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
