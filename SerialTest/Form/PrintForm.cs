using System;
using System.Collections.Generic;
using System.Text;

namespace SerialTest.form
{
    class PrintForm
    {
        public struct BundleMenuParams
        {
            public BundleMenuParams(string inShopName, MenuParams[] inMenuList)
            {
                shopName = inShopName;
                menuList = inMenuList;
            }

            public string shopName;
            public MenuParams[] menuList;
        }

        public struct MenuParams
        {
            public MenuParams(string inMenuName, int inMenuCnt, int inMenuCost, MenuOptParams[] inMenuOptList)
            {
                menuName = inMenuName;
                menuCnt = inMenuCnt;
                menuCost = inMenuCost;
                menuOptList = inMenuOptList;
            }

            public string menuName;
            public int menuCnt;
            public int menuCost;
            public MenuOptParams[] menuOptList;
        }

        public struct MenuOptParams
        {
            public MenuOptParams(string inMenuOptName, int inMenuOptCost)
            {
                menuOptName = inMenuOptName;
                menuOptCost = inMenuOptCost;
            }

            public string menuOptName;
            public int menuOptCost;
        }

        public string OrderNumForm(string orderNo)
        {
            return $"주문번호: {orderNo}";
        }

        public string PaymentForm(int payTpcd)
        {
            string paymentType;

            switch (payTpcd)
            {
                case 1:
                    paymentType = "후불(현금)";
                    break;
                case 2:
                    paymentType = "후불(카드)";
                    break;
                case 3:
                    paymentType = "후불(제로페이)";
                    break;
                case 4:
                    paymentType = "후불(온누리상품권)";
                    break;
                case 5:
                    paymentType = "후불(지역화폐)";
                    break;
                case 6:
                    paymentType = "결제완료(카드)";
                    break;
                case 7:
                    paymentType = "결제완료(제로페이)";
                    break;
                case 8:
                    paymentType = "결제완료(온누리상품권)";
                    break;
                case 9:
                    paymentType = "결제완료(지역화폐)";
                    break;
                default:
                    throw new Exception();
            }

            return $"결제방식: {paymentType}";
        }

        public string OrderTsForm(string orderTs)
        {
            string conv = DateTime.ParseExact(orderTs, "yyyy-MM-dd HH:mm:ss", null).ToString("yyyy-MM-dd HH:mm");

            return $"주문일시: {conv}";
        }

        public string TelForm(string customerTel)
        {
            return $"연락처: {ConvertTelFormat(customerTel)}";
        }

        public string ShopMemoForm(string shopMemo)
        {
            StringBuilder sb = new StringBuilder();
            _ = sb.AppendLine("************   고객 요청   **************");
            _ = sb.Append(SetStrFormat(shopMemo, 22));

            return sb.ToString();
        }

        public string RiderMemoForm(string riderMemo)
        {
            StringBuilder sb = new StringBuilder();
            _ = sb.AppendLine("************   배달 요청   **************");
            _ = sb.Append(SetStrFormat(riderMemo, 22));

            return sb.ToString();
        }

        public string AddressForm(string jibunAddress, string roadAddress)
        {
            StringBuilder sb = new StringBuilder();
            _ = sb.AppendLine("************   배달 주소   **************");
            _ = sb.Append(SetStrFormat(jibunAddress, 25));

            if (!string.IsNullOrEmpty(roadAddress))
            {
                _ = sb.Append(string.Format("\n(도로명) {0}", SetStrFormat(roadAddress, 20)));
            }

            return sb.ToString();
        }

        public string MenuPrintForm(int printType, string shopName, string shopTel, List<MenuParams> menuList, int couponCost, int deliCost, int totalCost, string cooInfo, bool isBundle = false)
        {
            StringBuilder sb = new StringBuilder();
            _ = sb.AppendLine("-----------------------------------------");
            _ = sb.AppendLine(string.Format("{0,-25}{1,2}{2,5}", "메뉴명", "수량", "금액"));
            _ = sb.AppendLine("-----------------------------------------");

            foreach (MenuParams item in menuList)
            {
                _ = sb.AppendLine(
                    string.Format("{0}{1,3}{2,10:N0}",
                        SetStrFormat(item.menuName, 14),
                        item.menuCnt,
                        item.menuCost
                     ));

                if (item.menuOptList.Length > 0)
                {
                    foreach (MenuOptParams optItem in item.menuOptList)
                    {
                        _ = sb.AppendLine(SetStrFormat(string.Format(" + {0}({1:N0}원)",
                            optItem.menuOptName, optItem.menuOptCost), 25));
                    }
                }
            }

            // 1: 매장용, 2: 주방용, 3: 고객용
            if (printType != 2)
            {
                _ = sb.AppendLine(string.Format("{0,-30}{1,6:N0}", "쿠폰비용", couponCost * -1));
                _ = sb.AppendLine(string.Format("{0,-30}{1,6:N0}", "배달비용", deliCost));

                _ = sb.AppendLine("-----------------------------------------");
                _ = sb.AppendLine(string.Format("{0,-28}{1,10:N0}", "합계 : ", totalCost));
                _ = sb.AppendLine("-----------------------------------------");

                if (!isBundle)
                {
                    _ = sb.AppendLine(string.Format("주문가게: {0}", SetStrFormat(shopName, 15)));
                }
            }

            if (printType == 3)
            {
                _ = sb.AppendLine(string.Format("상점연락처: {0}", SetStrFormat(ConvertTelFormat(shopTel), 15)));
                _ = sb.AppendLine("-----------------------------------------");
                _ = sb.AppendLine(SetStrFormat(cooInfo, 22));
            }

            if (printType != 3)
            {
                _ = sb.AppendLine("-----------------------------------------");
            }

            return sb.ToString();
        }

        // 여기 변경 필요
        public string BundleMenuPrintForm(string marketName, string marketTel, List<BundleMenuParams> menuList, int couponCost, int deliCost, int totalCost)
        {
            StringBuilder sb = new StringBuilder();
            _ = sb.AppendLine("-----------------------------------------");
            _ = sb.AppendLine(string.Format("{0,-25}{1,2}{2,5}", "메뉴명", "수량", "금액"));
            _ = sb.AppendLine("-----------------------------------------");

            foreach(BundleMenuParams item in menuList)
            {
                _= sb.AppendLine(string.Format("{0}", SetStrFormat($"[{item.shopName}]", 22)));

                foreach (MenuParams menuItem in item.menuList)
                {
                    _ = sb.AppendLine(
                        string.Format("{0}{1,3}{2,10:N0}",
                            SetStrFormat(menuItem.menuName, 14),
                            menuItem.menuCnt,
                            menuItem.menuCost
                         ));

                    if (menuItem.menuOptList.Length > 0)
                    {
                        foreach (MenuOptParams optItem in menuItem.menuOptList)
                        {
                            _ = sb.AppendLine(SetStrFormat(string.Format(" + {0}({1:N0}원)",
                                optItem.menuOptName, optItem.menuOptCost), 25));
                        }
                    }
                }
            }

            _ = sb.AppendLine(string.Format("{0,-30}{1,6:N0}", "쿠폰비용", couponCost * -1));
            _ = sb.AppendLine(string.Format("{0,-30}{1,6:N0}", "배달비용", deliCost));
            _ = sb.AppendLine("-----------------------------------------");
            _ = sb.AppendLine(string.Format("{0,-28}{1,10:N0}", "합계 : ", totalCost));
            _ = sb.AppendLine("-----------------------------------------");
            _ = sb.AppendLine(string.Format("주문시장: {0}", SetStrFormat(marketName, 15)));
            _ = sb.AppendLine(string.Format("시장연락처: {0}", SetStrFormat(ConvertTelFormat(marketTel), 15)));
            _ = sb.AppendLine("-----------------------------------------");

            return sb.ToString();
        }
        
        private string ConvertTelFormat(string tel)
        {
            switch (tel.Length)
            {
                case 9:
                    return "0" + Convert.ToInt64(tel).ToString("##-###-####");
                case 10:
                    return "0" + Convert.ToInt64(tel).ToString("###-###-####");
                case 11:
                    return "0" + Convert.ToInt64(tel).ToString("###-####-####");
                default:
                    throw new Exception();
            }
        }

        private string CreateEmptyLeftPad(string text, int baseCnt)
        {
            return string.Format("{0}", text).PadLeft(Math.Abs(baseCnt - Encoding.Default.GetBytes(text).Length));
        }

        private string CreateEmptyRightPad(string text, int baseCnt = 27)
        {
            return "".PadRight(Math.Abs(baseCnt - Encoding.Default.GetBytes(text).Length));
        }

        private string SetStrFormat(string str, int strSize)
        {

            if (str.Length > strSize)
            {
                int size = (int)Math.Ceiling((double)str.Length / strSize);
                int index = 0;
                string[] subStringArray = new string[size];

                for (int startIndex = 0; startIndex < str.Length; startIndex += strSize)
                {
                    if ((startIndex + strSize) >= str.Length)
                    {
                        string s = str.Substring(startIndex).TrimStart();
                        subStringArray[index++] = s + CreateEmptyRightPad(s); ;
                    }
                    else
                    {
                        subStringArray[index++] = str.Substring(startIndex, strSize).TrimStart();
                    }
                }

                str = string.Join("\r\n", subStringArray);
            }
            else
            {
                str += CreateEmptyRightPad(str);
            }

            return str;
        }
    }
}
