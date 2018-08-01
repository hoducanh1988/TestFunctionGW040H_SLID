using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TestFunctionGW040x.Funtions {
    public class exPower : baseFunctions {

        frmPOWER frmpower = null;
        int timeout = 0;
        public exPower(int _timeout) {
            this.timeout = _timeout;
        }

        bool _active {
            set {
                if(value) {
                    frmpower = new frmPOWER(timeout);  frmpower.ShowDialog();}
                else frmpower.Close();
            }
        }

        public bool Excute() {
            GlobalData.testingInfo.PowerResult = "";
            this._active = true;
            return GlobalData.testingInfo.PowerResult == "PASS";
        }

    }
}
