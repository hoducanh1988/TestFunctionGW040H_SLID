using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections;
using System.Windows.Media;

namespace TestFunctionGW040x.Funtions
{

    public class MyINotifyPropertyChanged : INotifyPropertyChanged {

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public class defaultSetting : MyINotifyPropertyChanged {


        public string MAC6DIGIT {
            get { return Properties.Settings.Default.Mac6digit; }
            set {
                Properties.Settings.Default.Mac6digit = value;
                OnPropertyChanged(nameof(MAC6DIGIT));
            }
        }
        public string FWVERSION {
            get { return Properties.Settings.Default.FwVersion; }
            set {
                Properties.Settings.Default.FwVersion = value;
                OnPropertyChanged(nameof(FWVERSION));
            }
        }
        public string DUTIP {
            get { return Properties.Settings.Default.DutIP; }
            set {
                Properties.Settings.Default.DutIP = value;
                OnPropertyChanged(nameof(DUTIP));
            }
        }
        public string DUTTELNETUSER {
            get { return Properties.Settings.Default.DutTelnetUser; }
            set {
                Properties.Settings.Default.DutTelnetUser = value;
                OnPropertyChanged(nameof(DUTTELNETUSER));
            }
        }
        public string DUTTELNETPASS {
            get { return Properties.Settings.Default.DutTelnetPass; }
            set {
                Properties.Settings.Default.DutTelnetPass = value;
                OnPropertyChanged(nameof(DUTTELNETPASS));
            }
        }
        public double DUTTXMIN {
            get { return Properties.Settings.Default.DutTxMin; }
            set {
                Properties.Settings.Default.DutTxMin = value;
                OnPropertyChanged(nameof(DUTTXMIN));
            }
        }
        public double DUTTXMAX {
            get { return Properties.Settings.Default.DutTxMax; }
            set {
                Properties.Settings.Default.DutTxMax = value;
                OnPropertyChanged(nameof(DUTTXMAX));
            }
        }
        public double DUTRXMIN {
            get { return Properties.Settings.Default.DutRxMin; }
            set {
                Properties.Settings.Default.DutRxMin = value;
                OnPropertyChanged(nameof(DUTRXMIN));
            }
        }
        public double DUTRXMAX {
            get { return Properties.Settings.Default.DutRxMax; }
            set {
                Properties.Settings.Default.DutRxMax = value;
                OnPropertyChanged(nameof(DUTRXMAX));
            }
        }
        public string DUTSLID {
            get { return Properties.Settings.Default.DutSLID; }
            set {
                Properties.Settings.Default.DutSLID = value;
                OnPropertyChanged(nameof(DUTSLID));
            }
        }
        public string OLTIP {
            get { return Properties.Settings.Default.OltIP; }
            set {
                Properties.Settings.Default.OltIP = value;
                OnPropertyChanged(nameof(OLTIP));
            }
        }
        public string OLTTELNETUSER {
            get { return Properties.Settings.Default.OltTelnetUser; }
            set {
                Properties.Settings.Default.OltTelnetUser = value;
                OnPropertyChanged(nameof(OLTTELNETUSER));
            }
        }
        public string OLTTELNETPASS {
            get { return Properties.Settings.Default.OltTelnetPass; }
            set {
                Properties.Settings.Default.OltTelnetPass = value;
                OnPropertyChanged(nameof(OLTTELNETPASS));
            }
        }
        public string OLTCOMMAND {
            get { return Properties.Settings.Default.OltCommand; }
            set {
                Properties.Settings.Default.OltCommand = value;
                OnPropertyChanged(nameof(OLTCOMMAND));
            }
        }
        string _oltPort;
        public string OLTPORT {
            get { return _oltPort; }
            set {
                _oltPort = value;
                OnPropertyChanged(nameof(OLTPORT));
            }
        }
        public string BARCODETYPE {
            get { return Properties.Settings.Default.BarcodeType; }
            set {
                Properties.Settings.Default.BarcodeType = value;
                OnPropertyChanged(nameof(BARCODETYPE));
            }
        }
        public string BARCODESPORT {
            get { return Properties.Settings.Default.BarcodeSPort; }
            set {
                Properties.Settings.Default.BarcodeSPort = value;
                OnPropertyChanged(nameof(BARCODESPORT));
            }
        }
        public string BARCODEBAUDRATE {
            get { return Properties.Settings.Default.BarcodeBaudRate; }
            set {
                Properties.Settings.Default.BarcodeBaudRate = value;
                OnPropertyChanged(nameof(BARCODEBAUDRATE));
            }
        }

        //-------------------------------------------------------------------------------------//
        public bool ENABLEFW {
            get { return Properties.Settings.Default.EnableFW; }
            set {
                Properties.Settings.Default.EnableFW = value;
                GlobalData.testingInfo.FWSTATUS = value == true ? TestingStatuses.Ready : TestingStatuses.Ignored;
                OnPropertyChanged(nameof(ENABLEFW));
            }
        }
        public bool ENABLEMAC {
            get { return Properties.Settings.Default.EnableMAC; }
            set {
                Properties.Settings.Default.EnableMAC = value;
                GlobalData.testingInfo.MACSTATUS = value == true ? TestingStatuses.Ready : TestingStatuses.Ignored;
                OnPropertyChanged(nameof(ENABLEMAC));
            }
        }
        public bool ENABLELAN {
            get { return Properties.Settings.Default.EnableLAN; }
            set {
                Properties.Settings.Default.EnableLAN = value;
                GlobalData.testingInfo.LANSTATUS = value == true ? TestingStatuses.Ready : TestingStatuses.Ignored;
                OnPropertyChanged(nameof(ENABLELAN));
            }
        }
        public bool ENABLEUSB {
            get { return Properties.Settings.Default.EnableUSB; }
            set {
                Properties.Settings.Default.EnableUSB = value;
                GlobalData.testingInfo.USBSTATUS = value == true ? TestingStatuses.Ready : TestingStatuses.Ignored;
                OnPropertyChanged(nameof(ENABLEUSB));
            }
        }
        public bool ENABLESYN {
            get { return Properties.Settings.Default.EnableSYN; }
            set {
                Properties.Settings.Default.EnableSYN = value;
                GlobalData.testingInfo.SYNSTATUS = value == true ? TestingStatuses.Ready : TestingStatuses.Ignored;
                OnPropertyChanged(nameof(ENABLESYN));
            }
        }
        public bool ENABLEPOWER {
            get { return Properties.Settings.Default.EnablePOWER; }
            set {
                Properties.Settings.Default.EnablePOWER = value;
                GlobalData.testingInfo.POWERSTATUS = value == true ? TestingStatuses.Ready : TestingStatuses.Ignored;
                OnPropertyChanged(nameof(ENABLEPOWER));
            }
        }
        public bool ENABLEBUTTON {
            get { return Properties.Settings.Default.EnableBUTTON; }
            set {
                Properties.Settings.Default.EnableBUTTON = value;
                GlobalData.testingInfo.BUTTONSTATUS = value == true ? TestingStatuses.Ready : TestingStatuses.Ignored;
                OnPropertyChanged(nameof(ENABLEBUTTON));
            }
        }
        public bool ENABLELED {
            get { return Properties.Settings.Default.EnableLED; }
            set {
                Properties.Settings.Default.EnableLED = value;
                GlobalData.testingInfo.LEDSTATUS = value == true ? TestingStatuses.Ready : TestingStatuses.Ignored;
                OnPropertyChanged(nameof(ENABLELED));
            }
        }
        //-------------------------------------------------------------------------------------//
        private bool _help;
        public bool HELP {
            get { return _help; }
            set {
                _help = value;
                OnPropertyChanged(nameof(HELP));
            }
        }

        public void Save() {
            Properties.Settings.Default.Save();
        }
    }

    public class TestInfomation : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
                Properties.Settings.Default.Save();
            }
        }
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
        #region No1
        //public bool ENABLEFW {
        //    get { return Properties.Settings.Default.EnableFW; }
        //    set {
        //        Properties.Settings.Default.EnableFW = value;
        //        this.FWSTATUS = value == true ? TestingStatuses.Ready : TestingStatuses.Ignored;
        //        OnPropertyChanged(nameof(ENABLEFW));
        //    }
        //}
        //public bool ENABLEMAC {
        //    get { return Properties.Settings.Default.EnableMAC; }
        //    set {
        //        Properties.Settings.Default.EnableMAC = value;
        //        this.MACSTATUS = value == true ? TestingStatuses.Ready : TestingStatuses.Ignored;
        //        OnPropertyChanged(nameof(ENABLEMAC));
        //    }
        //}
        //public bool ENABLELAN {
        //    get { return Properties.Settings.Default.EnableLAN; }
        //    set {
        //        Properties.Settings.Default.EnableLAN = value;
        //        this.LANSTATUS = value == true ? TestingStatuses.Ready : TestingStatuses.Ignored;
        //        OnPropertyChanged(nameof(ENABLELAN));
        //    }
        //}
        //public bool ENABLEUSB {
        //    get { return Properties.Settings.Default.EnableUSB; }
        //    set {
        //        Properties.Settings.Default.EnableUSB = value;
        //        this.USBSTATUS = value == true ? TestingStatuses.Ready : TestingStatuses.Ignored;
        //        OnPropertyChanged(nameof(ENABLEUSB));
        //    }
        //}
        //public bool ENABLESYN {
        //    get { return Properties.Settings.Default.EnableSYN; }
        //    set {
        //        Properties.Settings.Default.EnableSYN = value;
        //        this.SYNSTATUS = value == true ? TestingStatuses.Ready : TestingStatuses.Ignored;
        //        OnPropertyChanged(nameof(ENABLESYN));
        //    }
        //}
        //public bool ENABLEPOWER {
        //    get { return Properties.Settings.Default.EnablePOWER; }
        //    set {
        //        Properties.Settings.Default.EnablePOWER = value;
        //        this.POWERSTATUS = value == true ? TestingStatuses.Ready : TestingStatuses.Ignored;
        //        OnPropertyChanged(nameof(ENABLEPOWER));
        //    }
        //}
        //public bool ENABLEBUTTON {
        //    get { return Properties.Settings.Default.EnableBUTTON; }
        //    set {
        //        Properties.Settings.Default.EnableBUTTON = value;
        //        this.BUTTONSTATUS = value == true ? TestingStatuses.Ready : TestingStatuses.Ignored;
        //        OnPropertyChanged(nameof(ENABLEBUTTON));
        //    }
        //}
        //public bool ENABLELED {
        //    get { return Properties.Settings.Default.EnableLED; }
        //    set {
        //        Properties.Settings.Default.EnableLED = value;
        //        this.LEDSTATUS = value == true ? TestingStatuses.Ready : TestingStatuses.Ignored;
        //        OnPropertyChanged(nameof(ENABLELED));
        //    }
        //}

        #endregion
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
        #region No2
       
        string _testingstatus;
        public string TESTINGSTATUS {
            get { return _testingstatus; }
            set {
                _testingstatus = value;
                OnPropertyChanged(nameof(TESTINGSTATUS));
            }
        }

        string _fwstatus;
        public string FWSTATUS {
            get { return _fwstatus; }
            set {
                _fwstatus = value;
                OnPropertyChanged(nameof(FWSTATUS));
            }
        }
        string _macstatus;
        public string MACSTATUS {
            get { return _macstatus; }
            set {
                _macstatus = value;
                OnPropertyChanged(nameof(MACSTATUS));
            }
        }
        string _lanstatus;
        public string LANSTATUS {
            get { return _lanstatus; }
            set {
                _lanstatus = value;
                OnPropertyChanged(nameof(LANSTATUS));
            }
        }

        string _lan1status;
        public string LAN1STATUS {
            get { return _lan1status; }
            set {
                _lan1status = value;
                OnPropertyChanged(nameof(LAN1STATUS));
            }
        }
        string _lan2status;
        public string LAN2STATUS {
            get { return _lan2status; }
            set {
                _lan2status = value;
                OnPropertyChanged(nameof(LAN2STATUS));
            }
        }
        string _lan3status;
        public string LAN3STATUS {
            get { return _lan3status; }
            set {
                _lan3status = value;
                OnPropertyChanged(nameof(LAN3STATUS));
            }
        }
        string _lan4status;
        public string LAN4STATUS {
            get { return _lan4status; }
            set {
                _lan4status = value;
                OnPropertyChanged(nameof(LAN4STATUS));
            }
        }

        string _usbstatus;
        public string USBSTATUS {
            get { return _usbstatus; }
            set {
                _usbstatus = value;
                OnPropertyChanged(nameof(USBSTATUS));
            }
        }
        string _usb2status;
        public string USB2STATUS {
            get { return _usb2status; }
            set {
                _usb2status = value;
                OnPropertyChanged(nameof(USB2STATUS));
            }
        }
        string _usb3status;
        public string USB3STATUS {
            get { return _usb3status; }
            set {
                _usb3status = value;
                OnPropertyChanged(nameof(USB3STATUS));
            }
        }

        string _synstatus;
        public string SYNSTATUS {
            get { return _synstatus; }
            set {
                _synstatus = value;
                OnPropertyChanged(nameof(SYNSTATUS));
            }
        }
        string _powerstatus;
        public string POWERSTATUS {
            get { return _powerstatus; }
            set {
                _powerstatus = value;
                OnPropertyChanged(nameof(POWERSTATUS));
            }
        }
        string _powertxstatus;
        public string POWERTXSTATUS {
            get { return _powertxstatus; }
            set {
                _powertxstatus = value;
                OnPropertyChanged(nameof(POWERTXSTATUS));
            }
        }
        string _powerrxstatus;
        public string POWERRXSTATUS {
            get { return _powerrxstatus; }
            set {
                _powerrxstatus = value;
                OnPropertyChanged(nameof(POWERRXSTATUS));
            }
        }
        string _buttonstatus;
        public string BUTTONSTATUS {
            get { return _buttonstatus; }
            set {
                _buttonstatus = value;
                OnPropertyChanged(nameof(BUTTONSTATUS));
            }
        }
        string _buttonwpsstatus;
        public string BUTTONWPSSTATUS {
            get { return _buttonwpsstatus; }
            set {
                _buttonwpsstatus = value;
                OnPropertyChanged(nameof(BUTTONWPSSTATUS));
            }
        }
        string _buttonresetstatus;
        public string BUTTONRESETSTATUS {
            get { return _buttonresetstatus; }
            set {
                _buttonresetstatus = value;
                OnPropertyChanged(nameof(BUTTONRESETSTATUS));
            }
        }

        string _ledstatus;
        public string LEDSTATUS {
            get { return _ledstatus; }
            set {
                _ledstatus = value;
                OnPropertyChanged(nameof(LEDSTATUS));
            }
        }
        string _logtelnet;
        public string LOGTELNET {
            get { return _logtelnet; }
            set {
                _logtelnet = value;
                OnPropertyChanged(nameof(LOGTELNET));
            }
        }
        string _logsystem;
        public string LOGSYSTEM {
            get { return _logsystem; }
            set {
                _logsystem = value;
                OnPropertyChanged(nameof(LOGSYSTEM));
            }
        }
        string _macaddress;
        public string MACADDRESS {
            get { return _macaddress; }
            set {
                _macaddress = value;
                OnPropertyChanged(nameof(MACADDRESS));
            }
        }
        string _errormessage;
        public string ERRORMESSAGE {
            get { return _errormessage; }
            set {
                _errormessage = value;
                OnPropertyChanged(nameof(ERRORMESSAGE));
            }
        }
        public string ERRORCODE { get; set; }

        #endregion
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
        #region No3
        
        string _user;
        public string USER {
            get { return _user; }
            set {
                _user = value;
                OnPropertyChanged(nameof(USER));
            }
        }
        string _pass;
        public string PASSWORD {
            get { return _pass; }
            set {
                _pass = value;
                OnPropertyChanged(nameof(PASSWORD));
            }
        }

        #endregion
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
        #region No4

        public string PowerResult { get; set; }
        public string LedResult { get; set; }
        public string WpsResult { get; set; }
        public string ResetResult { get; set; }

        bool _powerjud;
        public bool POWERJUD {
            get { return _powerjud; }
            set {
                _powerjud = value;
                OnPropertyChanged(nameof(POWERJUD));
            }
        }
        bool _ponjud;
        public bool PONJUD {
            get { return _ponjud; }
            set {
                _ponjud = value;
                OnPropertyChanged(nameof(PONJUD));
            }
        }
        bool _inetjud;
        public bool INETJUD {
            get { return _inetjud; }
            set {
                _inetjud = value;
                OnPropertyChanged(nameof(INETJUD));
            }
        }
        bool _wlanjud;
        public bool WLANJUD {
            get { return _wlanjud; }
            set {
                _wlanjud = value;
                OnPropertyChanged(nameof(WLANJUD));
            }
        }
        bool _wlan5gjud;
        public bool WLAN5GJUD {
            get { return _wlan5gjud; }
            set {
                _wlan5gjud = value;
                OnPropertyChanged(nameof(WLAN5GJUD));
            }
        }
        bool _lan1jud;
        public bool LAN1JUD {
            get { return _lan1jud; }
            set {
                _lan1jud = value;
                OnPropertyChanged(nameof(LAN1JUD));
            }
        }
        bool _lan2jud;
        public bool LAN2JUD {
            get { return _lan2jud; }
            set {
                _lan2jud = value;
                OnPropertyChanged(nameof(LAN2JUD));
            }
        }
        bool _lan3jud;
        public bool LAN3JUD {
            get { return _lan3jud; }
            set {
                _lan3jud = value;
                OnPropertyChanged(nameof(LAN3JUD));
            }
        }
        bool _lan4jud;
        public bool LAN4JUD {
            get { return _lan4jud; }
            set {
                _lan4jud = value;
                OnPropertyChanged(nameof(LAN4JUD));
            }
        }
        bool _losjud;
        public bool LOSJUD {
            get { return _losjud; }
            set {
                _losjud = value;
                OnPropertyChanged(nameof(LOSJUD));
            }
        }
        bool _wpsjud;
        public bool WPSJUD {
            get { return _wpsjud; }
            set {
                _wpsjud = value;
                OnPropertyChanged(nameof(WPSJUD));
            }
        }

        #endregion
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
        #region No5
        
        string _ontfwversion;
        public string ONTFWVERSION {
            get { return _ontfwversion; }
            set {
                _ontfwversion = value;
                OnPropertyChanged(nameof(ONTFWVERSION));
            }
        }
        string _ontmac;
        public string ONTMAC {
            get { return _ontmac; }
            set {
                _ontmac = value;
                OnPropertyChanged(nameof(ONTMAC));
            }
        }
        string _ontlan;
        public string ONTLAN {
            get { return _ontlan; }
            set {
                _ontlan = value;
                OnPropertyChanged(nameof(ONTLAN));
            }
        }
        string _ontbutton;
        public string ONTBUTTON {
            get { return _ontbutton; }
            set {
                _ontbutton = value;
                OnPropertyChanged(nameof(ONTBUTTON));
            }
        }
        string _ontled;
        public string ONTLED {
            get { return _ontled; }
            set {
                _ontled = value;
                OnPropertyChanged(nameof(ONTLED));
            }
        }
        string _ontusb;
        public string ONTUSB {
            get { return _ontusb; }
            set {
                _ontusb = value;
                OnPropertyChanged(nameof(ONTUSB));
            }
        }
        string _ontsyn;
        public string ONTSYN {
            get { return _ontsyn; }
            set {
                _ontsyn = value;
                OnPropertyChanged(nameof(ONTSYN));
            }
        }
        string _ontpower;
        public string ONTPOWER {
            get { return _ontpower; }
            set {
                _ontpower = value;
                OnPropertyChanged(nameof(ONTPOWER));
            }
        }

        #endregion
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
        #region No6
       
        bool _enableinputmac;
        public bool ENABLEINPUTMAC {
            get { return _enableinputmac; }
            set {
                _enableinputmac = value;
                OnPropertyChanged(nameof(ENABLEINPUTMAC));
            }
        }
        string _macinputed;
        public string MACINPUTED {
            get { return _macinputed; }
            set {
                _macinputed = value;
                OnPropertyChanged(nameof(MACINPUTED));
            }
        }

        #endregion
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
        #region No7
        
        string _buttontitle;
        public string BUTTONTITLE {
            get { return _buttontitle; }
            set {
                _buttontitle = value;
                OnPropertyChanged(nameof(BUTTONTITLE));
            }
        }
        int _buttontimeout;
        public int BUTTONTIMEOUT {
            get { return _buttontimeout; }
            set {
                _buttontimeout = value;
                OnPropertyChanged(nameof(BUTTONTIMEOUT));
            }
        }

        #endregion
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
        public LogStep logstep;
        private bool _devcounter;
        public bool DEVCOUNTER {
            get { return _devcounter; }
            set {
                _devcounter = value;
                OnPropertyChanged(nameof(DEVCOUNTER));
            }
        }
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
        public TestInfomation() {
            initialization();
        }

        public void initialization() {
            this.TESTINGSTATUS = TestingStatuses.Normal;
            this.FWSTATUS = GlobalData.initSetting.ENABLEFW == true ? TestingStatuses.None : TestingStatuses.Ignored;
            this.MACSTATUS = GlobalData.initSetting.ENABLEMAC == true ? TestingStatuses.None : TestingStatuses.Ignored;

            this.LANSTATUS = GlobalData.initSetting.ENABLELAN == true ? TestingStatuses.None : TestingStatuses.Ignored;
            this.LAN1STATUS = GlobalData.initSetting.ENABLELAN == true ? TestingStatuses.None : TestingStatuses.Ignored;
            this.LAN2STATUS = GlobalData.initSetting.ENABLELAN == true ? TestingStatuses.None : TestingStatuses.Ignored;
            this.LAN3STATUS = GlobalData.initSetting.ENABLELAN == true ? TestingStatuses.None : TestingStatuses.Ignored;
            this.LAN4STATUS = GlobalData.initSetting.ENABLELAN == true ? TestingStatuses.None : TestingStatuses.Ignored;

            this.USBSTATUS = GlobalData.initSetting.ENABLEUSB == true ? TestingStatuses.None : TestingStatuses.Ignored;
            this.USB2STATUS = GlobalData.initSetting.ENABLEUSB == true ? TestingStatuses.None : TestingStatuses.Ignored;
            this.USB3STATUS = GlobalData.initSetting.ENABLEUSB == true ? TestingStatuses.None : TestingStatuses.Ignored;

            this.SYNSTATUS = GlobalData.initSetting.ENABLESYN == true ? TestingStatuses.None : TestingStatuses.Ignored;

            this.POWERSTATUS = GlobalData.initSetting.ENABLEPOWER == true ? TestingStatuses.None : TestingStatuses.Ignored;
            this.POWERTXSTATUS = GlobalData.initSetting.ENABLEPOWER == true ? TestingStatuses.None : TestingStatuses.Ignored;
            this.POWERRXSTATUS = GlobalData.initSetting.ENABLEPOWER == true ? TestingStatuses.None : TestingStatuses.Ignored;

            this.BUTTONSTATUS = GlobalData.initSetting.ENABLEBUTTON == true ? TestingStatuses.None : TestingStatuses.Ignored;
            this.BUTTONWPSSTATUS = GlobalData.initSetting.ENABLEBUTTON == true ? TestingStatuses.None : TestingStatuses.Ignored;
            this.BUTTONRESETSTATUS = GlobalData.initSetting.ENABLEBUTTON == true ? TestingStatuses.None : TestingStatuses.Ignored;

            this.LEDSTATUS = GlobalData.initSetting.ENABLELED == true ? TestingStatuses.None : TestingStatuses.Ignored;

            this.ERRORMESSAGE = "";
            this.MACADDRESS = "";
            this.LOGSYSTEM = "";
            this.LOGTELNET = "";
            this.ERRORCODE = "";
            this.PowerResult = "";
            this.ONTFWVERSION = "";
            this.ONTMAC = "";
            this.ONTLAN = "";
            this.ONTLED = "";
            this.ONTBUTTON = "";
            this.ONTUSB = "";
            this.ONTSYN = "";
            this.ONTPOWER = "";
            this.ENABLEINPUTMAC = true;
            //this.MACINPUTED = "";

            this.POWERJUD = true;
            this.PONJUD = true;
            this.INETJUD = true;
            this.WLANJUD = true;
            this.WLAN5GJUD = true;
            this.LAN1JUD = true;
            this.LAN2JUD = true;
            this.LAN3JUD = true;
            this.LAN4JUD = true;
            this.WPSJUD = true;
            this.LOSJUD = true;

            this.logstep = new LogStep();
            this.DEVCOUNTER = true;
        }
    }

    public class LogStep {
        public string logFirmware { get; set; }
        public string logMAC { get; set; }
        public string logLAN { get; set; }
        public string logUSB { get; set; }
        public string logSYN { get; set; }
        public string logPowerOptical { get; set; }
        public string logLED { get; set; }
        public string logButton { get; set; }

        public LogStep() {
            logFirmware = "";
            logMAC = "";
            logLAN = "";
            logUSB = "";
            logSYN = "";
            logPowerOptical = "";
            logLED = "";
            logButton = "";
        }
    }

    public class mainLocation {
        public double top { get; set; }
        public double left { get; set; }
        public double width { get; set; }
        public double height { get; set; }
    }
}
