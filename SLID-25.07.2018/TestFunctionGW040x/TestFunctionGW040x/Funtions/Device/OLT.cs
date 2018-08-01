using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestFunctionGW040x.Funtions
{
    public class OLT : Telnet
    {
        public OLT() : base() { }
        public OLT(string _host, int _port) : base(_host, _port) { }

    }
}
