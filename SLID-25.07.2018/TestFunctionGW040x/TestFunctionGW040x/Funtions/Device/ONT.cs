using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TestFunctionGW040x.Funtions
{
    public class ONT : Telnet
    {
        public ONT() : base() { }
        public ONT(string _host, int _port) : base(_host, _port) { }

        public bool writeSLID(string _slid, ref string msg) {
            try {
                if (base.IsConnected == false) return false;
                base.WriteLine(string.Format("tcapi set GPON_ONU Password {0}", _slid));
                Thread.Sleep(100);
                base.WriteLine("tcapi set GPON_Common CurrentAttribute Password");
                Thread.Sleep(100);
                base.WriteLine("tcapi show GPON_Common");
                Thread.Sleep(100);
                base.WriteLine("tcapi commit GPON_ONU");
                Thread.Sleep(100);
                //base.WriteLine("tcapi save");
                //Thread.Sleep(200);
                base.WriteLine("tcapi show GPON_ONU");
                Thread.Sleep(100);

                msg = base.Read0();
                return true;
            } catch (Exception ex) {
                msg = ex.ToString();
                return false;
            }
        }


        public bool clearSLID(ref string msg) {
            try {
                if (base.IsConnected == false) return false;
                base.WriteLine("prolinecmd restore default");
                Thread.Sleep(100);
                msg = base.Read0();
                return true;
            } catch (Exception ex) {
                msg = ex.ToString();
                return false;
            }
        }

        public string readSLID() {
            try {
                if (base.IsConnected == false) return "";
                //base.WriteLine("prolinecmd gponpasswd display");
                base.WriteLine("tcapi show GPON_ONU");
                Thread.Sleep(100);
                string tmp = base.Read0();
                tmp = tmp.Replace("\r", "").Replace("\n", "");
                tmp = tmp.Split(new string[] { "passwd:" }, StringSplitOptions.None)[1];
                return tmp;
            }
            catch {
                return "";
            }
        }
    }
}
