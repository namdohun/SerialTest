using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SerialTest.Helper
{
    class Command
    {
        /// <summary>NUL</summary>
        public static string NUL = DecimalToCharString(0);
        /// <summary>SOH</summary>
        public static string SOH = DecimalToCharString(1);
        /// <summary>STX</summary>
        public static string STX = DecimalToCharString(2);
        /// <summary>ETX</summary>
        public static string ETX = DecimalToCharString(3);
        /// <summary>EOT</summary>
        public static string EOT = DecimalToCharString(4);
        /// <summary>ENQ</summary>
        public static string ENQ = DecimalToCharString(5);
        /// <summary>ACK</summary>
        public static string ACK = DecimalToCharString(6);
        /// <summary>BEL</summary>
        public static string BEL = DecimalToCharString(7);
        /// <summary>BS</summary>
        public static string BS = DecimalToCharString(8);
        /// <summary>TAB</summary>
        public static string TAB = DecimalToCharString(9);
        /// <summary>VT</summary>
        public static string VT = DecimalToCharString(11);
        /// <summary>FF</summary>
        public static string FF = DecimalToCharString(12);
        /// <summary>CR</summary>
        public static string CR = DecimalToCharString(13);
        /// <summary>SO</summary>
        public static string SO = DecimalToCharString(14);
        /// <summary>SI</summary>
        public static string SI = DecimalToCharString(15);
        /// <summary>DLE</summary>
        public static string DLE = DecimalToCharString(16);
        /// <summary>DC1</summary>
        public static string DC1 = DecimalToCharString(17);
        /// <summary>DC2</summary>
        public static string DC2 = DecimalToCharString(18);
        /// <summary>DC3</summary>
        public static string DC3 = DecimalToCharString(19);
        /// <summary>DC4</summary>
        public static string DC4 = DecimalToCharString(20);
        /// <summary>NAK</summary>
        public static string NAK = DecimalToCharString(21);
        /// <summary>SYN</summary>
        public static string SYN = DecimalToCharString(22);
        /// <summary>ETB</summary>
        public static string ETB = DecimalToCharString(23);
        /// <summary>CAN</summary>
        public static string CAN = DecimalToCharString(24);
        /// <summary>EM</summary>
        public static string EM = DecimalToCharString(25);
        /// <summary>SUB</summary>
        public static string SUB = DecimalToCharString(26);
        /// <summary>ESC</summary>
        public static string ESC = DecimalToCharString(27);
        /// <summary>FS</summary>
        public static string FS = DecimalToCharString(28);
        /// <summary>GS</summary>
        public static string GS = DecimalToCharString(29);
        /// <summary>RS</summary>
        public static string RS = DecimalToCharString(30);
        /// <summary>US</summary>
        public static string US = DecimalToCharString(31);
        /// <summary>Space</summary>
        public static string Space = DecimalToCharString(32);

        /// <summary> 프린터 초기화</summary>
        public string InitializePrinter = ESC + "@";
        /// <summary>볼드 On</summary>
        public string BoldOn = ESC + "E" + DecimalToCharString(1);
        /// <summary>볼드 Off</summary>
        public string BoldOff = ESC + "E" + DecimalToCharString(0);
        /// <summary>언더라인 On</summary>
        public string UnderlineOn = ESC + "-" + DecimalToCharString(1);
        /// <summary>언더라인 Off</summary>
        public string UnderlineOff = ESC + "-" + DecimalToCharString(0);
        /// <summary>흑백반전 On</summary>
        public string ReverseOn = GS + "B" + DecimalToCharString(1);
        /// <summary>흑백반전 Off</summary>
        public string ReverseOff = GS + "B" + DecimalToCharString(0);
        /// <summary>좌측정렬</summary>
        public string AlignLeft = ESC + "a" + DecimalToCharString(0);
        /// <summary>가운데정렬</summary>
        public string AlignCenter = ESC + "a" + DecimalToCharString(1);
        /// <summary>우측정렬</summary>
        public string AlignRight = ESC + "a" + DecimalToCharString(2);
        /// <summary>커트</summary>
        public string Cut = GS + "V" + DecimalToCharString(1);
        /// <summary>라인높이제거</summary>
        public string LineSpaceRemove = ESC + "3";

        /// <summary>
        /// FONT 명령어의 글자사이즈 속성을 변환 합니다.
        /// </summary>
        /// <param name="width">가로</param>
        /// <param name="height">세로</param>
        /// <returns>가로 x 세로</returns>
        public string ConvertFontSize(int width, int height)
        {
            int _w, _h;

            //가로변환
            switch (width)
            {
                case 1:
                    _w = 0;
                    break;
                case 2:
                    _w = 16;
                    break;
                case 3:
                    _w = 32;
                    break;
                case 4:
                    _w = 48;
                    break;
                case 5:
                    _w = 64;
                    break;
                case 6:
                    _w = 80;
                    break;
                case 7:
                    _w = 96;
                    break;
                case 8:
                    _w = 112;
                    break;
                default:
                    _w = 0;
                    break;
            }

            //세로변환
            switch (height)
            {
                case 1:
                    _h = 0;
                    break;
                case 2:
                    _h = 1;
                    break;
                case 3:
                    _h = 2;
                    break;
                case 4:
                    _h = 3;
                    break;
                case 5:
                    _h = 4;
                    break;
                case 6:
                    _h = 5;
                    break;
                case 7:
                    _h = 6;
                    break;
                case 8:
                    _h = 7;
                    break;
                default:
                    _h = 0;
                    break;
            }

            //가로x세로
            return GS + "!" + DecimalToCharString(_w + _h);
        }

        /// <summary>
        /// Decimal을 캐릭터 변환 후 스트링을 반환 합니다.
        /// </summary>
        /// <param name="val">커맨드 숫자</param>
        /// <returns>변환된 문자열</returns>
        private static string DecimalToCharString(decimal val)
        {
            return Convert.ToString((char)val);
        }
    }
}
