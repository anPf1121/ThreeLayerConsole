using System;
using GUIEnum;
using Model;
using DAL;
namespace Ults
{
    class ConsoleUlts
    {
//         public void ConsoleForegroundColor(ConsoleEnum.Color colorEnum)
//         {
//             switch (colorEnum)
//             {
//                 case ConsoleEnum.Color.Red:
//                     Console.ForegroundColor = ConsoleColor.Red;
//                     break;
//                 case ConsoleEnum.Color.Green:
//                     Console.ForegroundColor = ConsoleColor.Green;
//                     break;
//                 case ConsoleEnum.Color.Blue:
//                     Console.ForegroundColor = ConsoleColor.Blue;
//                     break;
//                 case ConsoleEnum.Color.Yellow:
//                     Console.ForegroundColor = ConsoleColor.Yellow;
//                     break;
//                 case ConsoleEnum.Color.White:
//                     Console.ForegroundColor = ConsoleColor.White;
//                     break;
//                 default:
//                     break;
//             }
//         }
//         public void Line()
//         {
//             Console.WriteLine(@"
// _____ _____ _____ _____ _____ _____ _____ _____ _____ _____ _____ _____ _____ _____ _____ _____  
// \____\\____\\____\\____\\____\\____\\____\\____\\____\\____\\____\\____\\____\\____\\____\\____\
// ");
//         }

        public void TinyLine()
        {
            Console.WriteLine("================================================================================================");
        }
//         public void Title(string? title, string? subTitle)
//         {
//             if (title != null)
//             {
//                 ConsoleForegroundColor(ConsoleEnum.Color.White);
//                 Line();
//                 ConsoleForegroundColor(ConsoleEnum.Color.Red);
//                 Console.WriteLine("\n" + title);
//                 ConsoleForegroundColor(ConsoleEnum.Color.White);
//                 Line();
//             }
//             if (subTitle != null)
//             {
//                 Line();
//                 ConsoleForegroundColor(ConsoleEnum.Color.Blue);
//                 Console.WriteLine("\n" + subTitle);
//                 ConsoleForegroundColor(ConsoleEnum.Color.White);
//                 Line();
//             }
//         }
//         public void Alert(ConsoleEnum.Alert alertType, string msg)
//         {
//             Ultilities Ultilities = new Ultilities();
//             switch (alertType)
//             {
//                 case ConsoleEnum.Alert.Success:
//                     ConsoleForegroundColor(ConsoleEnum.Color.Green);
//                     Console.WriteLine("\n" + "✅ " + msg.ToUpper());
//                     break;
//                 case ConsoleEnum.Alert.Warning:
//                     ConsoleForegroundColor(ConsoleEnum.Color.Yellow);
//                     Console.WriteLine("\n" + "⚠️  " + msg.ToUpper());
//                     break;
//                 case ConsoleEnum.Alert.Error:
//                     ConsoleForegroundColor(ConsoleEnum.Color.Red);
//                     Console.WriteLine("\n" + "❌ " + msg.ToUpper());
//                     break;
//                 default:
//                     break;
//             }
//             ConsoleForegroundColor(ConsoleEnum.Color.White);
//             Ultilities.PressEnterTo("Continue");
//         }
        public void PrintPhoneInfo(Phone phone)
        {
            Console.WriteLine("| {0, 10} | {1, 30} | {2, 15} | {3, 15} | {4, 15} | {5, 15}  |", phone.PhoneID, phone.PhoneName, phone.Brand.BrandName, new PhoneDetailsDAL().GetPhoneDetailsByPhoneID(phone.PhoneID)[0].Price, phone.OS, new PhoneDAL().GetPhoneById(phone.PhoneID).PhoneDetails.Count());
        }
        public void PrintOrderInfo(Order order)
        {
            Console.WriteLine("| {0, 10} | {1, 30} | {2, 15} | {3, 15} |", order.OrderID, order.Customer.CustomerName, order.CreateAt, order.OrderStatus);
        }
        public void PrintPhoneDetailsInfo(Phone phone)
        {
            Console.WriteLine("PHONE DETAILS");
            TinyLine();
            Console.WriteLine("Phone ID: {0}", phone.PhoneID);
            Console.WriteLine("Phone Name: {0}", phone.PhoneName);
            Console.WriteLine("Brand: {0}", phone.Brand.BrandName);
            Console.WriteLine("Camera: {0}", phone.Camera);
            Console.WriteLine("RAM: {0}", phone.RAM);
            Console.WriteLine("Weight: {0}", phone.Weight);
            Console.WriteLine("Price: {0}", new PhoneDAL().GetPhoneById(phone.PhoneID).PhoneDetails[0].Price);
            Console.WriteLine("Processor: {0}", phone.Processor);
            Console.WriteLine("Battery: {0}", phone.BatteryCapacity);
            Console.WriteLine("OS: {0}", phone.OS);
            Console.WriteLine("Sim Slot: {0}", phone.SimSlot);
            Console.WriteLine("Screen : {0}", phone.Screen);
            Console.WriteLine("Connection: {0}", phone.Connection);
            Console.WriteLine("Charge Port: {0}", phone.ChargePort);
            Console.WriteLine("Release Date: {0}", phone.ReleaseDate);
            Console.WriteLine("Description: {0}", phone.Description);
            TinyLine();
        }
    }
}