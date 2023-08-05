using System;
using GUIEnum;
using Model;
using DAL;
using BusinessEnum;
using System.Globalization; // thu vien format tien
using BL;

namespace Ults
{
    class ConsoleUlts
    {
        public void ConsoleForegroundColor(ConsoleEnum.Color colorEnum)
        {
            switch (colorEnum)
            {
                case ConsoleEnum.Color.Red:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case ConsoleEnum.Color.Green:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case ConsoleEnum.Color.Blue:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case ConsoleEnum.Color.Yellow:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case ConsoleEnum.Color.White:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                default:
                    break;
            }
        }

        public void Line()
        {
            Console.WriteLine(@"----------------------------------------------------------------------------------------------");
        }

        public void PrintPhoneDetailsInfo(List<PhoneDetail> phoneDetails)
        {
            foreach (PhoneDetail pd in phoneDetails)
            {
                PrintPhoneDetailsInfo(pd);
                break;
            }
            PrintPhoneModelTitle();
            foreach (PhoneDetail pd in phoneDetails)
            {
                PrintPhoneModelInfo(pd);
            }
            Console.WriteLine("=====================================================================================================");
        }

        public void TinyLine()
        {
            Console.WriteLine("----------------------------------------------------------------------------------------------");
        }

        public void Alert(ConsoleEnum.Alert alertType, string msg)
        {
            switch (alertType)
            {
                case ConsoleEnum.Alert.Success:
                    ConsoleForegroundColor(ConsoleEnum.Color.Green);
                    Console.WriteLine("\n" + "✅ " + msg.ToUpper());
                    break;
                case ConsoleEnum.Alert.Warning:
                    ConsoleForegroundColor(ConsoleEnum.Color.Yellow);
                    Console.WriteLine("\n" + "⚠️  " + msg.ToUpper());
                    break;
                case ConsoleEnum.Alert.Error:
                    ConsoleForegroundColor(ConsoleEnum.Color.Red);
                    Console.WriteLine("\n" + "❌ " + msg.ToUpper());
                    break;
                default:
                    break;
            }
            ConsoleForegroundColor(ConsoleEnum.Color.White);
            PressEnterTo("Continue");
        }

        public void PressEnterTo(string? action)
        {
            if (action != null)
            {
                Console.Write($"\n👉 Press Enter To {action}...");
            }
            ConsoleKeyInfo key = Console.ReadKey();
            if (key.Key == ConsoleKey.Enter)
            {
                ClearCurrentConsoleLine();
                return;
            }
            else
                PressEnterTo(null);
        }

        public void PrintPhoneBorderLine()
        {
            Console.WriteLine("================================================================================================");
            Console.WriteLine("| {0, 10} | {1, 30} | {2, 15} | {3, 15} |","ID","Phone Name","Brand", "OS");
            Console.WriteLine("================================================================================================");
        }

        public void PrintOrderBorderLine()
        {
            Console.WriteLine("========================================================================================================================");
            Console.WriteLine("| {0, 10} | {1, 30} | {2, 35} | {3, 32} |","ID","Customer Name","Order Date","Status");
            Console.WriteLine("========================================================================================================================");
        }

        public void PrintPhoneInfo(Phone phone)
        {
            Console.WriteLine("| {0, 10} | {1, 30} | {2, 15} | {3, 15} |",phone.PhoneID,phone.PhoneName,phone.Brand.BrandName,phone.OS);
        }

        public void PrintOrderInfo(Order order)
        {
            Console.WriteLine("| {0, 10} | {1, 30} | {2, 35} | {3, 32} |",order.OrderID, order.Customer.CustomerName,order.CreateAt,order.OrderStatus);
        }

        public void PrintPhoneModelTitle()
        {
            Console.WriteLine("=====================================================================================================");
            Console.WriteLine("| PHONE MODEL                                                                                       |");
            Console.WriteLine("=====================================================================================================");
            Console.WriteLine("| {0, 10} | {1, 13} | {2, 15} | {3, 15} | {4, 15} | {5, 14} |","Detail ID","Phone Color", "ROM Size","Price", "Phone Status","Quantity");
            Console.WriteLine("=====================================================================================================");
        }

        public void PrintPhoneDetailsInfo(PhoneDetail phoneDetail)
        {
            TinyLine();
            Console.WriteLine("PHONE DETAILS INFOMATION");
            TinyLine();
            Console.WriteLine("Phone Name: {0}", phoneDetail.Phone.PhoneName);
            Console.WriteLine("Brand: {0}", phoneDetail.Phone.Brand.BrandName);
            Console.WriteLine("Camera: {0}", phoneDetail.Phone.Camera);
            Console.WriteLine("RAM: {0}", phoneDetail.Phone.RAM);
            Console.WriteLine("Weight: {0}", phoneDetail.Phone.Weight);
            Console.WriteLine("Processor: {0}", phoneDetail.Phone.Processor);
            Console.WriteLine("Battery: {0}", phoneDetail.Phone.BatteryCapacity);
            Console.WriteLine("OS: {0}", phoneDetail.Phone.OS);
            Console.WriteLine("Sim Slot: {0}", phoneDetail.Phone.SimSlot);
            Console.WriteLine("Screen : {0}", phoneDetail.Phone.Screen);
            Console.WriteLine("Connection: {0}", phoneDetail.Phone.Connection);
            Console.WriteLine("Charge Port: {0}", phoneDetail.Phone.ChargePort);
            Console.WriteLine("Release Date: {0}", phoneDetail.Phone.ReleaseDate);
            Console.WriteLine("Description: {0}", phoneDetail.Phone.Description);
        }

        public void PrintListPhase(string[] phase, int itemCount, int currentPhase)
        {
            Console.Clear();
            TinyLine();
            foreach (string item in phase)
            {
                itemCount++;
                if (itemCount == currentPhase)
                {
                    ConsoleForegroundColor(ConsoleEnum.Color.Green);
                    Console.Write(((itemCount == currentPhase) ? " 👉 " + item : " > " + item));
                    ConsoleForegroundColor(ConsoleEnum.Color.White);
                }
                else
                {
                    Console.Write(((itemCount == currentPhase) ? " 👉 " + item : " > " + item));
                }
                if (itemCount == phase.Length)
                    Console.Write("\n");
            }
            TinyLine();
            itemCount = 0;
        }

        public void PrintSellerOrder(Order ord)
        {
            int totalQuantity = 0;
            Console.WriteLine("\n=======================================================================================================================");
            Console.WriteLine( "--------------------------------------------------- VTC Mobile --------------------------------------------------------");
            Console.WriteLine("=======================================================================================================================");
            Console.WriteLine("                                          Order ID: " + ord.OrderID);
            Console.WriteLine("Website: https://vtc.edu.vn/");
            Console.WriteLine("Address: 18 Tam Trinh, Quận Hai Bà Trưng, Thành Phố Hà Nội");
            Console.WriteLine("Phone Number: 0999999999");
            Console.WriteLine("=======================================================================================================================");
            Console.WriteLine("Seller: " + ord.Seller.StaffName + " - ID: " + ord.Seller.StaffID);
            Console.WriteLine("=======================================================================================================================");
            Console.WriteLine("Order Create Time: " + DateTime.Now);
            Console.WriteLine("Customer: " + ord.Customer.CustomerName);
            Console.WriteLine("Address: " + ord.Customer.Address);
            Console.WriteLine("Phone Number: " + ord.Customer.PhoneNumber);
            PrintOrderDetails(ord);
            Console.WriteLine(@"{0, 35} {1, 60}", "Customer", "Seller");
            Console.WriteLine(@"{0, 38} {1, 60}", ord.Customer.CustomerName, ord.Seller.StaffName);
            Console.WriteLine("                                                                                                                       ");
        }

        private string[] ones = {"","một","hai","ba","bốn","năm","sáu","bảy","tám","chín"};
        private string[] teens = {"mười","mười một","mười hai","mười ba","mười bốn","mười lăm","mười sáu","mười bảy","mười tám","mười chín"};
        private string[] tens = { "", "", "hai mươi", "ba mươi", "bốn mươi", "năm mươi", "sáu mươi", "bảy mươi", "tám mươi", "chín mươi" };

        public string ConvertNumberToWords(decimal number)
        {
            if (number == 0)
                return "không đồng";

            long nguyen = (long)Math.Truncate(number);
            int thapPhan = (int)((number - nguyen) * 100);

            string result = ConvertToWords(nguyen) + " đồng";

            if (thapPhan > 0)
            {
                result += " và " + ConvertToWords(thapPhan) + " Hào";
            }

            return result;
        }

        public string ConvertToWords(long number)
        {
            if (number < 10)
            {
                return ones[number];
            }
            else if (number < 20)
            {
                return teens[number - 10];
            }
            else if (number < 100)
            {
                return tens[number / 10] + (number % 10 > 0 ? " " + ones[number % 10] : "");
            }
            else if (number < 1000)
            {
                return ones[number / 100]
                    + " trăm"
                    + (number % 100 > 0 ? " " + ConvertToWords(number % 100) : "");
            }
            else if (number < 1000000)
            {
                return ConvertToWords(number / 1000)
                    + " nghìn"
                    + (number % 1000 > 0 ? " " + ConvertToWords(number % 1000) : "");
            }
            else if (number < 1000000000)
            {
                return ConvertToWords(number / 1000000)
                    + " triệu"
                    + (number % 1000000 > 0 ? " " + ConvertToWords(number % 1000000) : "");
            }
            else
            {
                throw new ArgumentException("Out of range: " + number);
            }
        }

        public void PrintAccountantOrder(Order ord)
        {
            Console.WriteLine("\n=======================================================================================================================");
            Console.WriteLine("--------------------------------------------------- VTC Mobile --------------------------------------------------------");
            Console.WriteLine("=======================================================================================================================");
            Console.WriteLine("Website: https://vtc.edu.vn/");
            Console.WriteLine("Address: 18 Tam Trinh, Quận Hai Bà Trưng, Thành Phố Hà Nội");
            Console.WriteLine("=======================================================================================================================");
            Console.WriteLine("Accountant: " + ord.Accountant.StaffName + " - ID: " + ord.Accountant.StaffID );
            Console.WriteLine("=======================================================================================================================");
            Console.WriteLine("Customer: " + ord.Customer.CustomerName);
            Console.WriteLine("Address: " + ord.Customer.Address);
            Console.WriteLine("Phone Number: " + ord.Customer.PhoneNumber);
            PrintOrderDetailsInfo(ord);
        }

        public string FormatPrice(decimal price)
        {
            CultureInfo cultureInfo = new CultureInfo("vi-VN");
            return string.Format(cultureInfo, "{0:N0} ₫", price);
        }

        public void PrintOrderDetails(Order ord)
        {
            CultureInfo cultureInfo = new CultureInfo("vi-VN");
            int printImeiHandle = 0;
            int printImeiHandle2 = 0;
            Console.WriteLine("=============================================================================================================================");
            Console.WriteLine("                                                  LIST PRODUCT");
            Console.WriteLine( "=============================================================================================================================");
            Console.WriteLine("| {0, 16} | {1, 30} | {2, 15} | {3, 15} | {4, 15} | {5, 15} |", "Phone Detail ID","Phone Name", "Quantity", "Imei","Status", "Price");
            Console.WriteLine("=============================================================================================================================" );
            foreach (var pd in ord.PhoneDetails)
            {
                printImeiHandle = pd.PhoneDetailID;
                Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------------");
                foreach (var ims in pd.ListImei)
                {
                    Console.WriteLine("| {0, 16} | {1, 30} | {2, 15} | {3, 15} | {4, 15} | {5, 15} |",(printImeiHandle != printImeiHandle2) ? pd.PhoneDetailID : "",(printImeiHandle != printImeiHandle2) ? pd.Phone.PhoneName : "", (printImeiHandle != printImeiHandle2) ? pd.Quantity : "", ims.PhoneImei, (printImeiHandle != printImeiHandle2) ? pd.PhoneStatusType : "", (printImeiHandle != printImeiHandle2) ? FormatPrice(pd.Price) : "" );
                    printImeiHandle2 = printImeiHandle;
                }
            }
            Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------------" );
            Console.WriteLine( "  {0, 16}   {1, 20}   {2, 5}   {3, 5}   {4, 15}   {5, 15}  ", "", "", "", "", "Total Due:", FormatPrice(ord.GetTotalDue()));
            Console.WriteLine( "  {0, 16}   {1, 20}   {2, 5}   {3, 5}   {4, 15}   {5, 15}", "", "", "", "", "To String:", ConvertNumberToWords(ord.GetTotalDue()));
            Console.WriteLine("=============================================================================================================================");
        }

        public void PrintDiscountPolicyDetail(DiscountPolicy discountPolicy)
        {
            Console.WriteLine($"Title: {discountPolicy.Title}");
            Console.WriteLine($"FromDate: {discountPolicy.FromDate}");
            Console.WriteLine($"ToDate: {discountPolicy.ToDate}");
            if (discountPolicy.PhoneDetail.PhoneDetailID != 0)
                Console.WriteLine( $"Phone Information: {discountPolicy.PhoneDetail.Phone.PhoneName} {discountPolicy.PhoneDetail.PhoneColor.Color} {discountPolicy.PhoneDetail.ROMSize.ROM}" );
            if (discountPolicy.PaymentMethod != "Not Have")
                Console.WriteLine($"Apply for Paymentmethod: {discountPolicy.PaymentMethod}");
            if ( discountPolicy.MinimumPurchaseAmount > 0 && discountPolicy.MaximumPurchaseAmount > 0)
            {
                Console.WriteLine( $"Maximum purchase amount: {discountPolicy.MinimumPurchaseAmount}");
                Console.WriteLine( $"Minimum purchase amount: {discountPolicy.MaximumPurchaseAmount}" );
            }
            if (discountPolicy.DiscountPrice != 0)
                Console.WriteLine($"DiscountPrice: {discountPolicy.DiscountPrice}");
            if (discountPolicy.MoneySupported != 0)
                Console.WriteLine($"Money supported: {discountPolicy.MoneySupported}");
        }

        public void PrintPhoneModelInfo(PhoneDetail phoneDetail)
        {
            CultureInfo cultureInfo = new CultureInfo("vi-VN");
            Console.WriteLine("| {0, 10} | {1, 13} | {2, 15} | {3, 15} | {4, 15} | {5, 14} |",phoneDetail.PhoneDetailID,phoneDetail.PhoneColor.Color, phoneDetail.ROMSize.ROM, string.Format(cultureInfo, "{0:N0} ₫", phoneDetail.Price), phoneDetail.PhoneStatusType,(phoneDetail.Quantity != 0) ? phoneDetail.Quantity : "Out Of Stock");
        }

        public void PrintOrderDetailsInfo(Order order)
        {
            Console.WriteLine("\n=======================================================================================================================");
            Console.WriteLine("--------------------------------------------------- VTC Mobile --------------------------------------------------------");
            Console.WriteLine("=======================================================================================================================");
            Console.WriteLine("Website: https://vtc.edu.vn/");
            Console.WriteLine("Address: 18 Tam Trinh, Quận Hai Bà Trưng, Thành Phố Hà Nội");
            Console.WriteLine("=======================================================================================================================");
            Console.WriteLine("ORDER DETAIL INFORMATION");
            Console.WriteLine("=======================================================================================================================");
            Console.WriteLine("👉 Cutomer Information");
            Console.WriteLine($"- Name: {order.Customer.CustomerName}");
            Console.WriteLine($"- Address: {order.Customer.Address}");
            Console.WriteLine($"- PhoneNumber: {order.Customer.PhoneNumber}");
            Console.WriteLine("👉 Seller Information");
            Console.WriteLine($"- SellerID: {order.Seller.StaffID}");
            Console.WriteLine($"- Name: {order.Seller.StaffName}");
            Console.WriteLine($"- Address: {order.Seller.Address}");
            Console.WriteLine($"- PhoneNumber: {order.Seller.PhoneNumber}");
            Console.WriteLine("👉 Accountant Information");
            Console.WriteLine($"- AccountantID: {order.Accountant.StaffID}");
            Console.WriteLine($"- Name: {order.Accountant.StaffName}");
            Console.WriteLine($"- Address: {order.Accountant.Address}");
            Console.WriteLine($"- PhoneNumber: {order.Accountant.PhoneNumber}");
            Console.WriteLine("👉 Show All Phones in Cart");
            if (order.PhoneDetails.Count() == 0)
            {
                Console.WriteLine("Doesnt have any phone in cart");
            }
            else
            {
                Console.WriteLine( "===================================================================================================");
                Console.WriteLine( "| {0, 10} | {1, 30} | {2, 15} | {3, 15} | {4, 16}", "PhoneName", "Color", "RomSize", "Quantity", " Unit Price  |" );
                Console.WriteLine( "===================================================================================================" );

                foreach (var phone in order.PhoneDetails)
                {
                    Console.WriteLine("| {0, 10} | {1, 30} | {2, 15} | {3, 15} | {4, 15}|",phone.Phone.PhoneName, phone.PhoneColor.Color,phone.ROMSize.ROM, phone.Quantity, FormatPrice(phone.Price));
                }
                Console.WriteLine( "===================================================================================================" );
                Console.WriteLine($"✅ Total Due: {FormatPrice(order.TotalDue)} VND");
                if (order.DiscountPolicies.Count() != 0)
                {
                    Console.WriteLine("👉 All DiscountPolicy be apply for this order is: ");
                    foreach (var dp in order.DiscountPolicies)
                    {
                        Console.WriteLine("- " + dp.Title);
                        order.TotalDue -= dp.DiscountPrice;
                    }
                    Console.WriteLine($"✅ Total Due After discount: {FormatPrice(order.TotalDue)}");
                }
            }
        }

        public void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }

        public string GetUserName()
        {
            Console.Write("\n👉 User Name: ");
            string userName = Console.ReadLine() ?? "";
            return userName;
        }

        public int GetPhoneModelQuantity()
        {
            int quantity = 0;
            Console.Write("Input Quantity: ");
            int.TryParse(Console.ReadLine(), out quantity);
            return quantity;
        }

        public string GetPassword()
        {
            Console.Write("\n👉 Password: ");
            string pass = "";
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.Backspace)
                {
                    pass += key.KeyChar;
                    if (key.Key != ConsoleKey.Enter)
                        Console.Write("*");
                }
                else if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
                {
                    // Xóa ký tự cuối cùng trong chuỗi pass khi người dùng nhấn phím Backspace
                    pass = pass.Substring(0, pass.Length - 1);
                    Console.Write("\b \b");
                }
            } while (key.Key != ConsoleKey.Enter);
            pass = pass.Substring(0, pass.Length - 1);
            return pass;
        }

        public void Title(string? title, string? subTitle, Staff? staffLoggedIn)
        {
            if (title != null)
            {
                Line();
                ConsoleForegroundColor(ConsoleEnum.Color.Red);
                Console.WriteLine(title);
                ConsoleForegroundColor(ConsoleEnum.Color.White);
                Line();
            }
            if (subTitle != null)
            {
                Line();
                ConsoleForegroundColor(ConsoleEnum.Color.Blue);
                Console.WriteLine(subTitle);
                ConsoleForegroundColor(ConsoleEnum.Color.White);
                Line();
            }
            if(staffLoggedIn != null)
            {
                ConsoleForegroundColor(ConsoleEnum.Color.Green);
                Console.WriteLine("                                                              " + ((staffLoggedIn.Role == StaffEnum.Role.Accountant)? "Accountant: ": "Seller: " )+ staffLoggedIn.StaffName + " - ID: " + staffLoggedIn.StaffID );
                ConsoleForegroundColor(ConsoleEnum.Color.White);
            }
            Line();
        }
        public string GetAppTitle()
        {
            return @"
                              ┌─┐┬ ┬┌─┐┌┐┌┌─┐  ┌─┐┌┬┐┌─┐┬─┐┌─┐
                              ├─┘├─┤│ ││││├┤   └─┐ │ │ │├┬┘├┤ 
                              ┴  ┴ ┴└─┘┘└┘└─┘  └─┘ ┴ └─┘┴└─└─┘
                              ";
        }

        public void GetFooterPagination(int currentPage, int countPage)
        {
            Console.WriteLine( "================================================================================================");
            Console.WriteLine("{0,45}" + "< " + $"{currentPage}/{countPage}" + " >", " ");
            Console.WriteLine( "================================================================================================");
            Console.WriteLine( "Press 'Left Arrow' To Back Previous Page, 'Right Arror' To Next Page");
            Console.WriteLine("Press 'Space' To Choose a phone, 'B' To Back Previous Menu");
        }

        public string GetSearchANSIText()
        {
            string searchANSIText = @"
                                      ┌─┐┌─┐┌─┐┬─┐┌─┐┬ ┬
                                      └─┐├┤ ├─┤├┬┘│  ├─┤
                                      └─┘└─┘┴ ┴┴└─└─┘┴ ┴
                                      ";
            return searchANSIText;
        }

        public string[] GetMenuItemSearch()
        {
            return new string[] {"Search All Phone","Search Phone By Information", "Back To Previous Menu"};
        }

        public string[] GetCreateOrderTimeLine()
        {
            return new string[] { "Search Phone", "Add Phone To Order", "Add More Phone?", "Enter Customer Info", "Confirm Order"};
        }

        public string GetCustomerInfoANSITitle()
        {
            string title =
                @"                    
            ┌─┐┌┐┌┌┬┐┌─┐┬─┐  ┌─┐┬ ┬┌─┐┌┬┐┌─┐┌┬┐┌─┐┬─┐  ┬┌┐┌┌─┐┌─┐┬─┐┌┬┐┌─┐┌┬┐┬┌─┐┌┐┌
            ├┤ │││ │ ├┤ ├┬┘  │  │ │└─┐ │ │ ││││├┤ ├┬┘  ││││├┤ │ │├┬┘│││├─┤ │ ││ ││││
            └─┘┘└┘ ┴ └─┘┴└─  └─┘└─┘└─┘ ┴ └─┘┴ ┴└─┘┴└─  ┴┘└┘└  └─┘┴└─┴ ┴┴ ┴ ┴ ┴└─┘┘└┘";
            return title;
        }

        public Customer GetCustomerInfo()
        {
            Console.Write(" Customer Name: ");
            string customerName = Console.ReadLine() ?? "";
            Console.Write(" Phone Number: ");
            string phoneNumber = Console.ReadLine() ?? "";
            Console.Write(" Address: ");
            string address = Console.ReadLine() ?? "";
            Customer customer = new Customer(0, customerName, phoneNumber, address);
            return customer;
        }

        public string GetInputString(string requestToEnter) {
            string str = "";
            do
            {
                Console.Write(requestToEnter + ": ");
                str = Console.ReadLine() ?? "";
                if(str == "") {
                    Alert(GUIEnum.ConsoleEnum.Alert.Error, "You Haven't Entered Anything Yet");
                }
            } while (str == "");
            return str;
        }
        public int GetInputInt(string requestToEnter) {
            int intValue = 0;
            bool isValidInput = false;
            do
            {
                Console.Write(requestToEnter + ": ");
                isValidInput = int.TryParse(Console.ReadLine(), out intValue);
                if(!isValidInput) {
                    Alert(GUIEnum.ConsoleEnum.Alert.Error, "Can't Enter A String");
                }
            } while (!isValidInput);
            return intValue;
        }
    }
}
