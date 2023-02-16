using SerialTest.form;
using SerialTest.Helper;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using System.Windows;
using static SerialTest.form.PrintForm;

namespace SerialTest
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private static SerialPort _serialPort;
        private static string portName;
        private static int baudRate;

        private readonly PrintForm form = new PrintForm();
        private readonly Command command = new Command();

        int paymentType = 2;
        string jibunAddr = "광주 광산구 산정동 1056 부영애시앙 207동 2001호";
        string roadAddr = "광주 광산구 목련로 41 부영애시앙 207동 2001호";
        string memo = "벨누르지 마시고 문을 두드려 주세요. 리뷰할께요 콜라주세요. 수저 3개 주세요. 포크는 필요 없습니다.";
        string customerTel = "05078589300";
        List<MenuParams> menuList = new List<MenuParams>(new MenuParams[] {
                new MenuParams("후라이드치킨(뼈)", 1, 14000, new MenuOptParams[] {
                    new MenuOptParams("치즈볼", 1000)
                }),
                new MenuParams("탕수육 小", 1, 25000, new MenuOptParams[] {
                    new MenuOptParams("레몬간장 소스", 0),
                    new MenuOptParams("찹쌀탕수육으로 변경", 1000),
                    new MenuOptParams("콜라 추가", 1000)
                })
            });
        List<BundleMenuParams> bundleMenuList = new List<BundleMenuParams>(new BundleMenuParams[]
        {
            new BundleMenuParams("테스트 치킨집", new MenuParams[] {
                new MenuParams("후라이드치킨(뼈)", 1, 14000, new MenuOptParams[] {
                    new MenuOptParams("치즈볼", 1000)
                })
            }),
            new BundleMenuParams("테스트 중국집", new MenuParams[] {
                new MenuParams("탕수육 小", 1, 25000, new MenuOptParams[] {
                    new MenuOptParams("레몬간장 소스", 0),
                    new MenuOptParams("찹쌀탕수육으로 변경", 1000),
                    new MenuOptParams("콜라 추가", 1000)
                })
            })
        });
        int couponCost = 2000;
        int deliCost = 3000;
        int totalCost = 23000;
        string shopName = "후라이드참잘하는집 하남점";
        string shopTel = "0519220311";
        string orderTs = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string orderNo = "B0L900P3LT00AA";
        string cooInfo = "핫도그 반죽:쌀가루(미국산), 명량/체다치즈/반반모짜/모짜체다 핫도그: 돼지고기(소시지)";


        public MainWindow()
        {
            InitializeComponent();

            string[] portName = SerialPort.GetPortNames();
            Array.Sort(portName);

            ComPortCombBox.ItemsSource = portName;
            BaudRateCombBox.ItemsSource = new string[] { "300", "600", "1200", "2400", "4800", "9600", "14400", "19200", "28800", "38400", "57600", "115200" };
        }

        private void SerialConnectBtn_Click(object sender, RoutedEventArgs e)
        {
            portName = ComPortCombBox.SelectedItem.ToString();
            baudRate = int.Parse(BaudRateCombBox.SelectedItem.ToString());

            _serialPort = new SerialPort();

            if (!_serialPort.IsOpen)
            {
                _serialPort.PortName = portName;
                _serialPort.BaudRate = baudRate;
                _serialPort.DataBits = 8;
                _serialPort.StopBits = StopBits.One;
                _serialPort.Parity = Parity.None;
                _serialPort.Encoding = Encoding.Default;
                _serialPort.Handshake = Handshake.None;
                _serialPort.DtrEnable = true;
                _serialPort.NewLine = Environment.NewLine;

                _serialPort.Open();

                Console.WriteLine("Serial Connection Success");
                ComPortCombBox.IsEnabled = false;
                BaudRateCombBox.IsEnabled = false;
                TestPrintBtn.IsEnabled = true;
                ReceiptPrintBtn.IsEnabled = true;
                WrapPrintBtn.IsEnabled = true;
                BundlePrintBtn.IsEnabled = true;
                BundleSinglePrintBtn.IsEnabled = true;
            }
            else
            {
                Console.WriteLine("Serial Connection Fail");
            }
        }


        private void SerialDisConnectBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_serialPort.IsOpen)
            {
                _serialPort.Close();

                Console.WriteLine("Serial DisConnection Success");
                ComPortCombBox.IsEnabled = true;
                BaudRateCombBox.IsEnabled = true;
                TestPrintBtn.IsEnabled = false;
                ReceiptPrintBtn.IsEnabled = false;
                WrapPrintBtn.IsEnabled = false;
                BundlePrintBtn.IsEnabled = false;
                BundleSinglePrintBtn.IsEnabled = false;
            }
            else
            {
                Console.WriteLine("Serial DisConnection Fail");
            }
        }

        private void TestPrintBtn_Click(object sender, RoutedEventArgs e)
        {
            _serialPort.WriteLine(command.InitializePrinter);
            _serialPort.WriteLine(TestPrintForm(portName, baudRate));
            _serialPort.WriteLine("\n\n\n");
            _serialPort.WriteLine(command.Cut);
        }

        private void ReceiptPrintBtn_Click(object sender, RoutedEventArgs e)
        {
            string title = "[배달올레 - 배달]";

            // 매장용
            PrintInitTitle(title, 1);
            _serialPort.WriteLine(ShopPrintForm(paymentType, 1, jibunAddr, roadAddr, customerTel, memo, memo, menuList, couponCost, deliCost, totalCost, orderTs, shopName, shopTel, orderNo, cooInfo));
            _serialPort.WriteLine(command.Cut);

            // 주방용
            PrintInitTitle(title, 2);
            _serialPort.WriteLine(KitchenPrintForm(memo, menuList, couponCost, deliCost, totalCost, orderTs, shopName, shopTel, orderNo, cooInfo));
            _serialPort.WriteLine(command.Cut);

            // 고객용
            PrintInitTitle(title, 3);
            _serialPort.WriteLine(CustomerPrintForm(paymentType, 1, jibunAddr, roadAddr, customerTel, memo, memo, menuList, couponCost, deliCost, totalCost, orderTs, shopName, shopTel, orderNo, cooInfo));
            _serialPort.WriteLine(command.Cut);
        }

        private void WrapPrintBtn_Click(object sender, RoutedEventArgs e)
        {
            string title = "[배달올레 - 포장]";

            // 매장용
            PrintInitTitle(title, 1);
            _serialPort.WriteLine(ShopPrintForm(paymentType, 2, jibunAddr, roadAddr, customerTel, memo, memo, menuList, couponCost, deliCost, totalCost, orderTs, shopName, shopTel, orderNo, cooInfo));
            _serialPort.WriteLine(command.Cut);

            // 주방용
            PrintInitTitle(title, 2);
            _serialPort.WriteLine(KitchenPrintForm(memo, menuList, couponCost, deliCost, totalCost, orderTs, shopName, shopTel, orderNo, cooInfo));
            _serialPort.WriteLine(command.Cut);

            // 고객용
            PrintInitTitle(title, 3);
            _serialPort.WriteLine(CustomerPrintForm(paymentType, 2, jibunAddr, roadAddr, customerTel, memo, memo, menuList, couponCost, deliCost, totalCost, orderTs, shopName, shopTel, orderNo, cooInfo));
            _serialPort.WriteLine(command.Cut);
        }

        private void BundlePrintBtn_Click(object sender, RoutedEventArgs e)
        {
            string title = "[배달올레 - 묶음배달]";

            // 피커용
            PrintInitTitle(title, 4);
            _serialPort.WriteLine(PickerPrintForm(paymentType, jibunAddr, roadAddr, customerTel, memo, bundleMenuList, couponCost, deliCost, totalCost, orderTs, "초량전통시장", "0552213122", orderNo));
            _serialPort.WriteLine(command.Cut);

            // 고객용
            PrintInitTitle(title, 3);
            _serialPort.WriteLine(BundleCustomerPrintForm(paymentType, jibunAddr, roadAddr, customerTel, memo, memo, bundleMenuList, couponCost, deliCost, totalCost, orderTs, "초량전통시장", "0552213122", orderNo));
            _serialPort.WriteLine(command.Cut);
        }

        private void BundleSinglePrintBtn_Click(object sender, RoutedEventArgs e)
        {
            string title = "[배달올레 - 묶음배달]";

            PrintInitTitle(title, 1);
            _serialPort.WriteLine(BundleShopPrintForm(paymentType, jibunAddr, roadAddr, customerTel, memo, menuList, couponCost, deliCost, totalCost, orderTs, shopName, shopTel, orderNo));
            _serialPort.WriteLine(command.Cut);
        }

        private void PrintInitTitle(string title, int printType)
        {
            string printTitle = "";

            switch (printType)
            {
                case 1:
                    printTitle = "매장용";
                    break;
                case 2:
                    printTitle = "주방용";
                    break;
                case 3:
                    printTitle = "고객용";
                    break;
                case 4:
                    printTitle = "피커용";
                    break;
                default:
                    break;
            }

            _serialPort.WriteLine(command.InitializePrinter);
            _serialPort.WriteLine(command.LineSpaceRemove);
            _serialPort.WriteLine($"[{printTitle}]");
            _serialPort.WriteLine(command.AlignCenter);
            _serialPort.WriteLine(command.ConvertFontSize(2, 2));
            _serialPort.WriteLine(title);
            _serialPort.WriteLine("\n");
            _serialPort.WriteLine(command.InitializePrinter);
        }


        private string TestPrintForm(string portName, int baudRate)
        {
            StringBuilder sb = new StringBuilder();
            _ = sb.AppendLine("---------------------------------------");
            _ = sb.AppendLine($"Serial Port Name - {portName}");
            _ = sb.AppendLine($"BaudRate Number - {baudRate}");
            _ = sb.AppendLine("안녕하세요.");
            _ = sb.AppendLine("현재 영수증 프린터 테스트 중입니다.");
            _ = sb.AppendLine("감사합니다.");
            _ = sb.AppendLine("---------------------------------------");

            return sb.ToString();
        }

        private string ShopPrintForm(int paymentType, int deliveryType, string address, string roadAddress, string customerTel, string shopMemo, string riderMemo, List<MenuParams> menuList, int couponCost, int deliCost, int totalCost, string orderTs, string shopName, string shopTel, string orderNo, string cooInfo)
        {
            StringBuilder sb = new StringBuilder();

            _ = sb.AppendLine(form.OrderNumForm(orderNo));
            _ = sb.AppendLine(form.PaymentForm(paymentType));
            _ = sb.AppendLine(form.OrderTsForm(orderTs));
            _ = sb.AppendLine(form.TelForm(customerTel));
            _ = sb.AppendLine(form.ShopMemoForm(shopMemo));

            // 포장주문이 아닐 시 배달요청과 주소 값을 추가
            if (deliveryType != 2)
            {
                _ = sb.AppendLine(form.RiderMemoForm(riderMemo));
                _ = sb.AppendLine(form.AddressForm(address, roadAddress));
            }

            _ = sb.AppendLine(form.MenuPrintForm(1, shopName, shopTel, menuList, couponCost, deliCost, totalCost, cooInfo));
            _ = sb.AppendLine("\n\n");


            return sb.ToString();
        }

        private string KitchenPrintForm(string shopMemo, List<MenuParams> menuList, int couponCost, int deliCost, int totalCost, string orderTs, string shopName, string shopTel, string orderNo, string cooInfo)
        {
            StringBuilder sb = new StringBuilder();

            _ = sb.AppendLine(form.OrderNumForm(orderNo));
            _ = sb.AppendLine(form.OrderTsForm(orderTs));
            _ = sb.AppendLine(form.ShopMemoForm(shopMemo));
            _ = sb.AppendLine(form.MenuPrintForm(2, shopName, shopTel, menuList, couponCost, deliCost, totalCost, cooInfo));
            _ = sb.AppendLine("\n\n");

            return sb.ToString();
        }

        private string CustomerPrintForm(int paymentType, int deliveryType, string address, string roadAddress, string customerTel, string shopMemo, string riderMemo, List<MenuParams> menuList, int couponCost, int deliCost, int totalCost, string orderTs, string shopName, string shopTel, string orderNo, string cooInfo)
        {
            StringBuilder sb = new StringBuilder();

            _ = sb.AppendLine(form.OrderNumForm(orderNo));
            _ = sb.AppendLine(form.PaymentForm(paymentType));
            _ = sb.AppendLine(form.OrderTsForm(orderTs));
            _ = sb.AppendLine(form.TelForm(customerTel));
            _ = sb.AppendLine(form.ShopMemoForm(shopMemo));

            // 포장주문이 아닐 시 배달요청과 주소 값을 추가
            if (deliveryType != 2)
            {
                _ = sb.AppendLine(form.RiderMemoForm(riderMemo));
                _ = sb.AppendLine(form.AddressForm(address, roadAddress));
            }

            _ = sb.AppendLine(form.MenuPrintForm(3, shopName, shopTel, menuList, couponCost, deliCost, totalCost, cooInfo));
            _ = sb.AppendLine("\n\n");

            return sb.ToString();
        }

        private string PickerPrintForm(int paymentType, string address, string roadAddress, string customerTel, string shopMemo, List<BundleMenuParams> menuList, int couponCost, int deliCost, int totalCost, string orderTs, string marketName, string marketTel, string orderNo)
        {
            StringBuilder sb = new StringBuilder();

            _ = sb.AppendLine(form.OrderNumForm(orderNo));
            _ = sb.AppendLine(form.PaymentForm(paymentType));
            _ = sb.AppendLine(form.OrderTsForm(orderTs));
            _ = sb.AppendLine(form.TelForm(customerTel));
            _ = sb.AppendLine(form.AddressForm(address, roadAddress));
            _ = sb.AppendLine(form.ShopMemoForm(shopMemo));
            _ = sb.AppendLine(form.BundleMenuPrintForm(marketName, marketTel, menuList, couponCost, deliCost, totalCost));
            _ = sb.AppendLine("\n\n");

            return sb.ToString();
        }

        private string BundleCustomerPrintForm(int paymentType, string address, string roadAddress, string customerTel, string shopMemo, string riderMemo, List<BundleMenuParams> menuList, int couponCost, int deliCost, int totalCost, string orderTs, string marketName, string marketTel, string orderNo)
        {
            StringBuilder sb = new StringBuilder();

            _ = sb.AppendLine(form.OrderNumForm(orderNo));
            _ = sb.AppendLine(form.PaymentForm(paymentType));
            _ = sb.AppendLine(form.OrderTsForm(orderTs));
            _ = sb.AppendLine(form.TelForm(customerTel));
            _ = sb.AppendLine(form.AddressForm(address, roadAddress));
            _ = sb.AppendLine(form.ShopMemoForm(shopMemo));
            _ = sb.AppendLine(form.RiderMemoForm(riderMemo));
            _ = sb.AppendLine(form.BundleMenuPrintForm(marketName, marketTel, menuList, couponCost, deliCost, totalCost));
            _ = sb.AppendLine("\n\n");

            return sb.ToString();
        }

        private string BundleShopPrintForm(int paymentType, string address, string roadAddress, string customerTel, string shopMemo, List<MenuParams> menuList, int couponCost, int deliCost, int totalCost, string orderTs, string shopName, string shopTel, string orderNo)
        {
            StringBuilder sb = new StringBuilder();

            _ = sb.AppendLine(form.OrderNumForm(orderNo));
            _ = sb.AppendLine(form.PaymentForm(paymentType));
            _ = sb.AppendLine(form.OrderTsForm(orderTs));
            _ = sb.AppendLine(form.TelForm(customerTel));
            _ = sb.AppendLine(form.AddressForm(address, roadAddress));
            _ = sb.AppendLine(form.ShopMemoForm(shopMemo));
            _ = sb.AppendLine(form.MenuPrintForm(1, shopName, shopTel, menuList, couponCost, deliCost, totalCost, cooInfo));
            _ = sb.AppendLine("\n\n");

            return sb.ToString();
        }
    }
}
