using System;
using GUIEnum;
using Model;
using DAL;
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
            Console.WriteLine(@"
_____ _____ _____ _____ _____ _____ _____ _____ _____ _____ _____ _____ _____ _____ _____ _____  
\____\\____\\____\\____\\____\\____\\____\\____\\____\\____\\____\\____\\____\\____\\____\\____\
");
        }

        public void TinyLine()
        {
            Console.WriteLine("\n========================================================================================================================");
        }
        public void Title(string? title, string? subTitle)
        {
            if (title != null)
            {
                ConsoleForegroundColor(ConsoleEnum.Color.White);
                Line();
                ConsoleForegroundColor(ConsoleEnum.Color.Red);
                Console.WriteLine("\n" + title);
                ConsoleForegroundColor(ConsoleEnum.Color.White);
                Line();
            }
            if (subTitle != null)
            {
                Line();
                ConsoleForegroundColor(ConsoleEnum.Color.Blue);
                Console.WriteLine("\n" + subTitle);
                ConsoleForegroundColor(ConsoleEnum.Color.White);
                Line();
            }
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
            Ultilities.PressEnterTo("Continue");
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
            int quantity = 0;
            foreach (var phonedetails in phone.PhoneDetails)
            {
                quantity += phonedetails.ListImei.Count();
            }
            Console.WriteLine("| {0, 10} | {1, 30} | {2, 15} | {3, 15} |", phone.PhoneID, phone.PhoneName, phone.Brand.BrandName, phone.OS);
        }
        public void PrintOrderInfo(Order order)
        {
            Console.WriteLine("| {0, 10} | {1, 30} | {2, 15} | {3, 15} |", order.OrderID, order.Customer.CustomerName, order.CreateAt, order.OrderStatus);
        }
        public void PrintPhoneDetailsInfoTitle()
        {
            Console.WriteLine("=====================================================================================================");
            Console.WriteLine("| PHONE DETAILS TYPE                                                                                |");
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
            Console.WriteLine("Price: {0}", new PhoneDAL().GetPhoneById(phoneDetail.Phone.PhoneID).PhoneDetails[0].Price);
            Console.WriteLine("Processor: {0}", phoneDetail.Phone.Processor);
            Console.WriteLine("Battery: {0}", phoneDetail.Phone.BatteryCapacity);
            Console.WriteLine("OS: {0}", phoneDetail.Phone.OS);
            Console.WriteLine("Sim Slot: {0}", phoneDetail.Phone.SimSlot);
            Console.WriteLine("Screen : {0}", phoneDetail.Phone.Screen);
            Console.WriteLine("Connection: {0}", phoneDetail.Phone.Connection);
            Console.WriteLine("Charge Port: {0}", phoneDetail.Phone.ChargePort);
            Console.WriteLine("Release Date: {0}", phoneDetail.Phone.ReleaseDate);
            Console.WriteLine("Description: {0}", phoneDetail.Phone.Description);
            TinyLine();
        }
        public void PrintPhoneDetailsType(PhoneDetail phoneDetail)
        {
            Console.WriteLine("| {0, 10} | {1, 13} | {2, 15} | {3, 15} | {4, 15} | {5, 14} |", phoneDetail.PhoneDetailID, phoneDetail.PhoneColor.Color, phoneDetail.ROMSize.ROM, phoneDetail.Price, phoneDetail.PhoneStatusType, phoneDetail.Quantity);
        }
        public void PrintOrderAndPhoneBorder(Dictionary<int, List<Phone>> phones, int currentPage, int countPage)
        {
            Console.Clear();
            Title(
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
        public static void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }
    }
}