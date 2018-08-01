﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestFunctionGW040x.Funtions {
    public class baseFunctions {

        private Byte[] HexToBin(string pHexString) {
            if (String.IsNullOrEmpty(pHexString))
                return new Byte[0];

            if (pHexString.Length % 2 != 0)
                throw new Exception("Hexstring must have an even length");

            Byte[] bin = new Byte[pHexString.Length / 2];
            int o = 0;
            int i = 0;
            for (; i < pHexString.Length; i += 2, o++) {
                switch (pHexString[i]) {
                    case '0': bin[o] = 0x00; break;
                    case '1': bin[o] = 0x10; break;
                    case '2': bin[o] = 0x20; break;
                    case '3': bin[o] = 0x30; break;
                    case '4': bin[o] = 0x40; break;
                    case '5': bin[o] = 0x50; break;
                    case '6': bin[o] = 0x60; break;
                    case '7': bin[o] = 0x70; break;
                    case '8': bin[o] = 0x80; break;
                    case '9': bin[o] = 0x90; break;
                    case 'A': bin[o] = 0xa0; break;
                    case 'a': bin[o] = 0xa0; break;
                    case 'B': bin[o] = 0xb0; break;
                    case 'b': bin[o] = 0xb0; break;
                    case 'C': bin[o] = 0xc0; break;
                    case 'c': bin[o] = 0xc0; break;
                    case 'D': bin[o] = 0xd0; break;
                    case 'd': bin[o] = 0xd0; break;
                    case 'E': bin[o] = 0xe0; break;
                    case 'e': bin[o] = 0xe0; break;
                    case 'F': bin[o] = 0xf0; break;
                    case 'f': bin[o] = 0xf0; break;
                    default: throw new Exception("Invalid character found during hex decode");
                }

                switch (pHexString[i + 1]) {
                    case '0': bin[o] |= 0x00; break;
                    case '1': bin[o] |= 0x01; break;
                    case '2': bin[o] |= 0x02; break;
                    case '3': bin[o] |= 0x03; break;
                    case '4': bin[o] |= 0x04; break;
                    case '5': bin[o] |= 0x05; break;
                    case '6': bin[o] |= 0x06; break;
                    case '7': bin[o] |= 0x07; break;
                    case '8': bin[o] |= 0x08; break;
                    case '9': bin[o] |= 0x09; break;
                    case 'A': bin[o] |= 0x0a; break;
                    case 'a': bin[o] |= 0x0a; break;
                    case 'B': bin[o] |= 0x0b; break;
                    case 'b': bin[o] |= 0x0b; break;
                    case 'C': bin[o] |= 0x0c; break;
                    case 'c': bin[o] |= 0x0c; break;
                    case 'D': bin[o] |= 0x0d; break;
                    case 'd': bin[o] |= 0x0d; break;
                    case 'E': bin[o] |= 0x0e; break;
                    case 'e': bin[o] |= 0x0e; break;
                    case 'F': bin[o] |= 0x0f; break;
                    case 'f': bin[o] |= 0x0f; break;
                    default: throw new Exception("Invalid character found during hex decode");
                }
            }
            return bin;
        }

        private string BinToHex(string bin) {
            string output = "";
            try {
                int rest = bin.Length % 4;
                bin = bin.PadLeft(rest, '0'); //pad the length out to by divideable by 4

                for (int i = 0; i <= bin.Length - 4; i += 4) {
                    output += string.Format("{0:X}", Convert.ToByte(bin.Substring(i, 4), 2));
                }

                return output;
            }
            catch {
                return "ERROR";
            }
        }

        protected string GEN_SERIAL_ONT(string MAC) {
            try {
                string low_MAC = MAC.Substring(6, 6);
                string origalByteString = Convert.ToString(HexToBin(low_MAC)[0], 2).PadLeft(8, '0');
                string VNPT_SERIAL_ONT = null;

                origalByteString = origalByteString + "" + Convert.ToString(HexToBin(low_MAC)[1], 2).PadLeft(8, '0');
                origalByteString = origalByteString + "" + Convert.ToString(HexToBin(low_MAC)[2], 2).PadLeft(8, '0');
                //----HEX to BIN Cach 2-------
                string value = low_MAC;
                var s = String.Join("", low_MAC.Select(x => Convert.ToString(Convert.ToInt32(x + "", 16), 2).PadLeft(4, '0')));
                //----HEX to BIN Cach 2-------
                string shiftByteString = "";
                shiftByteString = origalByteString.Substring(1, origalByteString.Length - 1) + origalByteString[0];

                if (MAC.Contains("A06518")) {
                    VNPT_SERIAL_ONT = "VNPT" + "00" + BinToHex(shiftByteString); //"'00' --> dải MAC đang được đăng ký, sau này nếu đăng ký thêm dải mới thì giá trị này sẽ thành '01'"
                }
                else if (MAC.Contains("A4F4C2")) //Dải mác mới của VNPT. Hòa Add: 16/03/2017
                {
                    VNPT_SERIAL_ONT = "VNPT" + "01" + BinToHex(shiftByteString);
                }
                return VNPT_SERIAL_ONT;
            }
            catch {
                return "ERROR";
            }
        }

        /// <summary>
        /// Nhập dữ liệu vào từ mã có giá trị nhỏ nhất tới mã có giá trị lớn nhất
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string GEN_ERRORCODE (params bool[] data) {
            if (data.Length > 0) {
                int i = 0, sum = 0;
                foreach (var item in data) {
                    sum += (item == true ? 0 : 1) * ((int)Math.Pow(2, i));
                    i++;
                }
                string s = sum.ToString();
                int l = s.Length;
                for (int j = 0; j < 4 - l; j++) { s = "0" + s; }
                return s;
            } else  return "0000";
        }
    }
}
