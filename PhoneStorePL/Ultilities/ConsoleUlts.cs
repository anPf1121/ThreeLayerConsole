using System;
using GUIEnum;
using Model;
using DAL;
using System.Globalization; // thu vien format tien 
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
            Console.WriteLine(@"-----------------------------------------------------------------------------------------------");
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
            TinyLine();
        }
        public void TinyLine()
        {
            Console.WriteLine("______________________________________________________________________________________________________");
        }
        public void Alert(ConsoleEnum.Alert alertType, string msg)
        {
            Ultilities Ultilities = new Ultilities();
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
            else PressEnterTo(null);
        }
        public void PrintPhoneBorderLine()
        {
            Console.WriteLine("================================================================================================");
            Console.WriteLine("| {0, 10} | {1, 30} | {2, 15} | {3, 15} |", "ID", "Phone Name", "Brand", "OS");
            Console.WriteLine("================================================================================================");
        }
        public void PrintOrderBorderLine()
        {
            Console.WriteLine("========================================================================================================================");
            Console.WriteLine("| {0, 10} | {1, 30} | {2, 20} | {3, 15} |", "ID", "Customer Name", "Order Date", "Status");
            Console.WriteLine("========================================================================================================================");
        }
        public void PrintPhoneInfo(Phone phone)
        {
            Console.WriteLine("| {0, 10} | {1, 30} | {2, 15} | {3, 15} |", phone.PhoneID, phone.PhoneName, phone.Brand.BrandName, phone.OS);
        }
        public void PrintOrderInfo(Order order)
        {
            Console.WriteLine("| {0, 10} | {1, 30} | {2, 15} | {3, 15} |", order.OrderID, order.Customer.CustomerName, order.CreateAt, order.OrderStatus);
        }
        public void PrintPhoneModelTitle()
        {
            Console.WriteLine("=====================================================================================================");
            Console.WriteLine("| PHONE MODEL                                                                                |");
            Console.WriteLine("=====================================================================================================");
            Console.WriteLine("| {0, 10} | {1, 13} | {2, 15} | {3, 15} | {4, 15} | {5, 14} |", "Detail ID", "Phone Color", "ROM Size", "Price", "Phone Status", "Quantity");
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
            TinyLine();
            foreach (string item in phase)
            {
                itemCount++;
                Console.Write(((itemCount == currentPhase) ? " 👉 " + item : " > " + item));
                if (itemCount == phase.Length) Console.Write("\n");
            }
            TinyLine();
            itemCount = 0;
        }
        public void PrintSellerOrder(Order ord)
        {
            int totalQuantity = 0;
            CultureInfo cultureInfo = new CultureInfo("vi-VN");
            Console.WriteLine("\n=======================================================================================================================");
            Console.WriteLine("------------------------------------------------------------ VTC Mobile -----------------------------------------------");
            Console.WriteLine("=======================================================================================================================");
            Console.WriteLine("Website: https://vtc.edu.vn/");
            Console.WriteLine("Address: 18 Tam Trinh, Quận Hai Bà Trưng, Thành Phố Hà Nội");
            Console.WriteLine("=======================================================================================================================");
            Console.WriteLine("Seller: " + ord.Seller.StaffName + " - ID: " + ord.Seller.StaffID);
            Console.WriteLine("=======================================================================================================================");
            Console.WriteLine("Customer: " + ord.Customer.CustomerName);
            Console.WriteLine("Address: " + ord.Customer.Address);
            Console.WriteLine("Phone Number: " + ord.Customer.PhoneNumber);
            Console.WriteLine("=======================================================================================================================");
            Console.WriteLine("| {0, 10} | {1, 30} | {2, 15} | {3, 15} | {4, 15} | {5, 15} |", "Phone ID", "Phone Name", "Quantity", "Imei", "Status", "Price");
            Console.WriteLine("=======================================================================================================================");
            int printImeiHandle = 0;
            int printImeiHandle2 = 0;
            foreach (var pd in ord.PhoneDetails)
            {
                printImeiHandle = pd.Phone.PhoneID;
                Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------");
                foreach (var ims in pd.ListImei)
                {
                    Console.WriteLine("| {0, 10} | {1, 30} | {2, 15} | {3, 15} | {4, 15} | {5, 15} |", (printImeiHandle != printImeiHandle2) ? pd.Phone.PhoneID : "", (printImeiHandle != printImeiHandle2) ? pd.Phone.PhoneName : "", (printImeiHandle != printImeiHandle2) ? pd.Quantity : "", ims.PhoneImei, (printImeiHandle != printImeiHandle2) ? pd.PhoneStatusType : "", (printImeiHandle != printImeiHandle2) ? pd.Price : "");
                    printImeiHandle2 = printImeiHandle;
                }
            }
            Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("  {0, 10}   {1, 30}   {2, 15}   {3, 15}   {4, 15}   {5, 15}  ", "", "", "", "", "Total Due:", string.Format(cultureInfo, "{0:N0} ₫", ord.GetTotalDue()));
            Console.WriteLine("  {0, 10}   {1, 30}   {2, 15}   {3, 15}   {4, 15}   {5, 15}  ", "", "", "", "", "Discount Price:", string.Format(cultureInfo, "{0:N0} ₫", 0)); // hard coded
            Console.WriteLine("=======================================================================================================================");

        }
        public void PrintPhoneModelInfo(PhoneDetail phoneDetail)
        {
            CultureInfo cultureInfo = new CultureInfo("vi-VN");
            Console.WriteLine("| {0, 10} | {1, 13} | {2, 15} | {3, 15} | {4, 15} | {5, 14} |", phoneDetail.PhoneDetailID, phoneDetail.PhoneColor.Color, phoneDetail.ROMSize.ROM, string.Format(cultureInfo, "{0:N0} ₫", phoneDetail.Price), phoneDetail.PhoneStatusType, phoneDetail.Quantity);
        }
        public void PrintOrderAndPhoneBorder(Dictionary<int, List<Phone>> phones, int currentPage, int countPage)
        {
            Ultilities ults = new Ultilities();
            Console.Clear();
            ults.Title(
                        null,
    @"
      █████╗ ██████╗ ██████╗     ████████╗ ██████╗      ██████╗ ██████╗ ██████╗ ███████╗██████╗ 
    ██╔══██╗██╔══██╗██╔══██╗    ╚══██╔══╝██╔═══██╗    ██╔═══██╗██╔══██╗██╔══██╗██╔════╝██╔══██╗
    ███████║██║  ██║██║  ██║       ██║   ██║   ██║    ██║   ██║██████╔╝██║  ██║█████╗  ██████╔╝
    ██╔══██║██║  ██║██║  ██║       ██║   ██║   ██║    ██║   ██║██╔══██╗██║  ██║██╔══╝  ██╔══██╗
    ██║  ██║██████╔╝██████╔╝       ██║   ╚██████╔╝    ╚██████╔╝██║  ██║██████╔╝███████╗██║  ██║
    ╚═╝  ╚═╝╚═════╝ ╚═════╝        ╚═╝    ╚═════╝      ╚═════╝ ╚═╝  ╚═╝╚═════╝ ╚══════╝╚═╝  ╚═╝
                                                                                           "
                    );
            PrintPhoneBorderLine();
            foreach (Phone phone in phones[currentPage])
            {
                PrintPhoneInfo(phone);
            }

            Console.WriteLine("================================================================================================");
            Console.WriteLine("{0,45}" + "< " + $"{currentPage}/{countPage}" + " >", " ");
            Console.WriteLine("================================================================================================");
        }
        public void PrintOrderDetailsInfo(Order order)
        {
            Console.Clear();
            TinyLine();
            Console.WriteLine("ORDER DETAILS INFOMATION");
            TinyLine();
            Console.WriteLine($"Order ID: {order.OrderID}");
            Console.WriteLine($"Create At: {order.CreateAt}");
            Console.WriteLine($"Seller: {order.Seller.StaffName}");
            Console.WriteLine($"Seller id : {order.Seller.StaffID}");
            Console.WriteLine($"Accountant: {order.Accountant.StaffName}");
            Console.WriteLine($"Accountant id: {order.Accountant.StaffID}");
            Console.WriteLine($"Customer: {order.Customer.CustomerName}");
            Console.WriteLine($"Customer id: {order.Customer.CustomerID}");
            foreach (PhoneDetail phoneDetail in order.PhoneDetails)
            {
                Console.WriteLine("ROM Size: ", phoneDetail.ROMSize.ROM);
                Console.WriteLine("Phone Color: ", phoneDetail.PhoneColor.Color);
            }
            Console.WriteLine($"Phone Quantity in order:{order.PhoneDetails.Count()}");
            Console.WriteLine($"Total Due:{order.GetTotalDue()}");
            // Phải hiển thị thêm DiscountPolicy các chính sách giảm giá được áp dụng cho order
            Console.WriteLine($"Status: {order.OrderStatus}");


        }
        public static void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }
    }
}