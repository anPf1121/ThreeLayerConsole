using System;
using GUIEnum;

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
            Console.WriteLine("================================================================================================");
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
                    Console.WriteLine("\n" + msg.ToUpper() + "✅");
                    break;
                case ConsoleEnum.Alert.Warning:
                    ConsoleForegroundColor(ConsoleEnum.Color.Yellow);
                    Console.WriteLine("\n" + msg.ToUpper() + "⚠️");
                    break;
                case ConsoleEnum.Alert.Error:
                    ConsoleForegroundColor(ConsoleEnum.Color.Red);
                    Console.WriteLine("\n" + msg.ToUpper() + "❌");
                    break;
                default:
                    break;
            }
            ConsoleForegroundColor(ConsoleEnum.Color.White);
            Ultilities.PressEnterTo("Continue");
        }
        // public void PrintPhoneInfo(Phone phone)
        // {
        //     Console.WriteLine("| {0, 10} | {1, 30} | {2, 15} | {3, 15} | {4, 15} | {5, 15}  |", phone.PhoneID, phone.PhoneName, phone.Brand, phone.Price, phone.OS, phone.Quantity);
        // }
        // public void PrintOrderInfo(Order order)
        // {
        //     Console.WriteLine("| {0, 10} | {1, 30} | {2, 15} | {3, 15} |", order.OrderID, order.OrderCustomer.CustomerName, order.OrderDate, order.OrderStatus);
        // }
        // public void PrintPhoneDetailsInfo(Phone phone)
        // {
        //     Console.WriteLine("PHONE DETAILS");
        //     TinyLine();
        //     Console.WriteLine("Phone ID: {0}", phone.PhoneID);
        //     Console.WriteLine("Phone Name: {0}", phone.PhoneName);
        //     Console.WriteLine("Brand: {0}", phone.Brand);
        //     Console.WriteLine("CPU: {0}", phone.CPU);
        //     Console.WriteLine("RAM: {0}", phone.RAM);
        //     Console.WriteLine("Discount Price: {0}", phone.DiscountPrice);
        //     Console.WriteLine("Price: {0}", phone.Price);
        //     Console.WriteLine("Battery: {0}", phone.BatteryCapacity);
        //     Console.WriteLine("OS: {0}", phone.OS);
        //     Console.WriteLine("Sim Slot: {0}", phone.SimSlot);
        //     Console.WriteLine("Screen Hz: {0}", phone.ScreenHz);
        //     Console.WriteLine("Screen Resolution: {0}", phone.ScreenResolution);
        //     Console.WriteLine("ROM: {0}", phone.ROM);
        //     Console.WriteLine("Storage Memory: {0}", phone.StorageMemory);
        //     Console.WriteLine("Mobile Network: {0}", phone.MobileNetwork);
        //     Console.WriteLine("Quantity: {0}", phone.Quantity);
        //     Console.WriteLine("Phone Size: {0}", phone.PhoneSize);
        //     TinyLine();
        // }
    }
}