using System;
using GUIEnum;
using Model;
using DAL;
using BusinessEnum;
using System.Globalization; // thu vien format tien
using BL;
using UI;
using System.Text.RegularExpressions;
using Interface;

namespace Ults
{
    class ConsoleUlts
    {
        ConsoleUI consoleUI = new ConsoleUI();
        public Dictionary<int, List<Phone>> listAllPhones = new Dictionary<int, List<Phone>>();
        public Dictionary<int, List<PhoneDetail>> listAllPhonesModel = new Dictionary<int, List<PhoneDetail>>();
        public bool PressYesOrNo(string yesAction, string noAction)
        {
            string str = $"Press 'Y' To {yesAction} Or 'N' To {noAction}";
            int centeredPosition = (Console.WindowWidth - str.Length) / 2;
            string spaces = centeredPosition > 0 ? new string(' ', centeredPosition) : "";

            ConsoleKeyInfo input = new ConsoleKeyInfo();
            bool active = true;
            Console.Write(spaces + str);
            do
            {
                input = Console.ReadKey(true);
                if (input.Key == ConsoleKey.N)
                {
                    Console.Clear();
                    return false;
                }
                else if (input.Key == ConsoleKey.Y)
                {
                    Console.Clear();
                    return true;
                }
            } while (active);
            return false;
        }
        public int PressCharacterTo(string firstAction, string? secondAction, string? thirdAction, string? fourthAction)
        {
            string firstStr = $"Press 'Q' To {firstAction} - Press 'W' To {secondAction} - Press 'E' To {thirdAction}";
            string secondStr = $"Press 'Q' To {firstAction} - Press 'W' To {secondAction}";
            string thirdStr = $"Press 'Q' To {firstAction} - Press 'W' To {secondAction} - Press 'E' To {thirdAction} - Press 'R' To {fourthAction}";
            string firstSpaces = consoleUI.AlignCenter(firstStr);
            string secondSpaces = consoleUI.AlignCenter(secondStr);
            string thirdSpaces = consoleUI.AlignCenter(thirdStr);
            ConsoleKeyInfo input = new ConsoleKeyInfo();
            bool active = true;
            if (thirdAction != null && fourthAction == null) Console.Write(firstSpaces + firstStr);
            else if (fourthAction != null) Console.Write(thirdSpaces + thirdStr);
            else Console.Write(secondSpaces + secondStr);
            do
            {
                input = Console.ReadKey(true);
                if (input.Key == ConsoleKey.Q)
                {
                    Console.Clear();
                    return 0;
                }
                else if (input.Key == ConsoleKey.W)
                {
                    Console.Clear();
                    return 1;
                }
                else if (input.Key == ConsoleKey.E && thirdAction != null)
                {
                    Console.Clear();
                    return 2;
                }
                else if (input.Key == ConsoleKey.R && fourthAction != null)
                {
                    Console.Clear();
                    return 3;
                }
            } while (active);
            return 0;
        }
        public int MenuHandle(string? title, string? subTitle, string[] menuItem, Staff loginStaff)
        {
            Console.Clear();
            ConsoleUlts consoleUlts = new ConsoleUlts();
            string spaces = consoleUI.AlignCenter("|--------------------------------------------------------------------------------------------|");
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            ConsoleKeyInfo keyInfo;
            string iconBackhand = "   ";
            bool activeSelectedMenu = true;
            int currentChoice = 1;
            int renderCount = 0;
            int colorCount = 0;
            while (activeSelectedMenu)
            {
                colorCount++;
                if (title != null || subTitle != null)
                    consoleUI.PrintTitle(title, subTitle, loginStaff);
                if (currentChoice <= (menuItem.Count() + 1) && currentChoice >= 1)
                {
                    for (int i = 0; i < menuItem.Count(); i++)
                    {
                        if (currentChoice - 1 == i)
                        {
                            if (colorCount % 2 == 0) Console.ForegroundColor = ConsoleColor.DarkMagenta;
                            else if (colorCount % 3 == 0) Console.ForegroundColor = ConsoleColor.Blue;
                            else if (colorCount % 5 == 0) Console.ForegroundColor = ConsoleColor.Green;
                            else Console.ForegroundColor = ConsoleColor.DarkCyan;
                            if (menuItem[i].ToLower() == "Log Out".ToLower()) Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(spaces + "| {0, 50} |", (iconBackhand + " " + consoleUI.SetTextBolder(menuItem[i])).PadRight(90));
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        else
                        {
                            Console.WriteLine(spaces + "| {0, 50} |", menuItem[i].PadRight(90));
                        }
                    }
                    consoleUI.PrintLine();

                    keyInfo = Console.ReadKey();
                    if (keyInfo.Key == ConsoleKey.Enter)
                    {
                        Console.Clear();
                        return currentChoice;
                    }

                    if (keyInfo.Key == ConsoleKey.DownArrow || keyInfo.Key == ConsoleKey.UpArrow || keyInfo.Key == ConsoleKey.Enter)
                    {
                        if (currentChoice >= 1 && currentChoice <= menuItem.Count())
                            if (keyInfo.Key == ConsoleKey.DownArrow)
                                currentChoice++;
                            else if (keyInfo.Key == ConsoleKey.UpArrow)
                                currentChoice--;

                        if (currentChoice == (menuItem.Count() + 1))
                        {
                            renderCount++;
                            currentChoice = 1;
                            if (renderCount % 3 == 0)
                            {
                                consoleUlts.Alert(GUIEnum.ConsoleEnum.Alert.Warning, "This approach leads to excessive rendering of the program");
                            }
                        }
                        else if (currentChoice == 0)
                        {
                            renderCount++;
                            currentChoice = menuItem.Count();
                            if (renderCount % 3 == 0)
                            {
                                consoleUlts.Alert(GUIEnum.ConsoleEnum.Alert.Warning, "This approach leads to excessive rendering of the program");
                            }
                        }
                        Console.Clear();
                    }
                    Console.Clear();

                }
            }
            return currentChoice;
        }

        public bool Pagination<T>(List<T> listItem, int currentPhase, string[] timeLine, int toDo)
        {
            int currentPage = 1;
            int totalPages = (int)Math.Ceiling((double)listItem.Count / 5);
            int centeredPosition = (Console.WindowWidth - "|============================================================================================|".Length) / 2;
            string spaces = centeredPosition > 0 ? new string(' ', centeredPosition) : "";
            ConsoleKeyInfo key;
            do
            {
                if (typeof(T) == typeof(Phone))
                    DisplayCurrentPage((List<Phone>)(object)listItem, currentPage, 5, currentPhase, timeLine, toDo);
                else if (typeof(T) == typeof(PhoneDetail))
                    DisplayCurrentPage((List<PhoneDetail>)(object)listItem, currentPage, 5, currentPhase, timeLine, toDo);
                else if (typeof(T) == typeof(Order))
                    DisplayCurrentPage((List<Order>)(object)listItem, currentPage, 5, currentPhase, timeLine, toDo);
                new ConsoleUI().GetFooterPagination(currentPage, totalPages);

                key = Console.ReadKey();

                if (key.Key == ConsoleKey.RightArrow && currentPage < totalPages)
                {
                    currentPage++;
                }
                else if (key.Key == ConsoleKey.LeftArrow && currentPage > 1)
                {
                    currentPage--;
                }
                else if (key.Key == ConsoleKey.Spacebar) return true;
            } while (key.Key != ConsoleKey.B);
            return false;
        }

        public void DisplayCurrentPage<T>(List<T> items, int currentPage, int itemsPerPage, int currentPhase, string[] timeLine, int toDo)
        {
            int startIndex = (currentPage - 1) * itemsPerPage;
            int endIndex = Math.Min(startIndex + itemsPerPage, items.Count);
            if (toDo == 0)
            {
                for (int i = startIndex; i < endIndex; i++)
                {

                    if (typeof(T) == typeof(Phone))
                    {
                        if (i == startIndex)
                        {
                            new ConsoleUI().PrintTimeLine(timeLine, currentPhase);
                            new ConsoleUI().PrintTitle(new ConsoleUI().GetAppANSIText(), new ConsoleUI().GetAddPhoneToOrderANSIText(), null);
                            new ConsoleUI().PrintPhoneBorderLine();
                        }
                        new ConsoleUI().PrintPhoneInfo((Phone)(object)items[i]!);

                    }
                    else if (typeof(T) == typeof(PhoneDetail))
                    {
                        if (i == startIndex)
                        {
                            new ConsoleUI().PrintTimeLine(timeLine, currentPhase);
                            new ConsoleUI().PrintTitle(new ConsoleUI().GetAppANSIText(), new ConsoleUI().GetPhoneDetailANSIText(), null);
                            new ConsoleUI().PrintPhoneModelTitle();
                        }
                        new ConsoleUI().PrintPhoneModelInfo((PhoneDetail)(object)items[i]!);
                    }
                    else if (typeof(T) == typeof(Order))
                    {
                        if (i == startIndex)
                        {
                            new ConsoleUI().PrintTimeLine(timeLine, currentPhase);
                            new ConsoleUI().PrintTitle(new ConsoleUI().GetAppANSIText(), new ConsoleUI().GetAddPhoneToOrderANSIText(), null);
                            new ConsoleUI().PrintOrderBorderLine();
                        }
                        new ConsoleUI().PrintOrderInfo((Order)(object)items[i]!);
                    }
                }
            }
            else if (toDo == 1)
            {
                for (int i = startIndex; i < endIndex; i++)
                {

                    if (typeof(T) == typeof(Phone))
                    {
                        if (i == startIndex)
                        {
                            new ConsoleUI().PrintTimeLine(timeLine, currentPhase);
                            new ConsoleUI().PrintTitle(new ConsoleUI().GetAppANSIText(), new ConsoleUI().GetCheckCustomerPhoneANSIText(), null);
                            new ConsoleUI().PrintPhoneBorderLine();
                        }
                        new ConsoleUI().PrintPhoneInfo((Phone)(object)items[i]!);

                    }
                    else if (typeof(T) == typeof(PhoneDetail))
                    {
                        if (i == startIndex)
                        {
                            new ConsoleUI().PrintTimeLine(timeLine, currentPhase);
                            new ConsoleUI().PrintTitle(new ConsoleUI().GetAppANSIText(), new ConsoleUI().GetPhoneDetailANSIText(), null);
                            new ConsoleUI().PrintPhoneModelTitle();
                        }
                        new ConsoleUI().PrintPhoneModelInfo((PhoneDetail)(object)items[i]!);
                    }
                    else if (typeof(T) == typeof(Order))
                    {
                        if (i == startIndex)
                        {
                            new ConsoleUI().PrintTimeLine(timeLine, currentPhase);
                            new ConsoleUI().PrintTitle(new ConsoleUI().GetAppANSIText(), new ConsoleUI().GetCheckCustomerPhoneANSIText(), null);
                            new ConsoleUI().PrintOrderBorderLine();
                        }
                        new ConsoleUI().PrintOrderInfo((Order)(object)items[i]!);
                    }
                }
            }
            else if (toDo == 2)
            {
                for (int i = startIndex; i < endIndex; i++)
                {

                    if (typeof(T) == typeof(Phone))
                    {
                        if (i == startIndex)
                        {
                            new ConsoleUI().PrintTimeLine(timeLine, currentPhase);
                            new ConsoleUI().PrintTitle(new ConsoleUI().GetAppANSIText(), new ConsoleUI().GetPaymentANSIText(), null);
                            new ConsoleUI().PrintPhoneBorderLine();
                        }
                        new ConsoleUI().PrintPhoneInfo((Phone)(object)items[i]!);

                    }
                    else if (typeof(T) == typeof(PhoneDetail))
                    {
                        if (i == startIndex)
                        {
                            new ConsoleUI().PrintTimeLine(timeLine, currentPhase);
                            new ConsoleUI().PrintTitle(new ConsoleUI().GetAppANSIText(), new ConsoleUI().GetPhoneDetailANSIText(), null);
                            new ConsoleUI().PrintPhoneModelTitle();
                        }
                        new ConsoleUI().PrintPhoneModelInfo((PhoneDetail)(object)items[i]!);
                    }
                    else if (typeof(T) == typeof(Order))
                    {
                        if (i == startIndex)
                        {
                            new ConsoleUI().PrintTimeLine(timeLine, currentPhase);
                            new ConsoleUI().PrintTitle(new ConsoleUI().GetAppANSIText(), new ConsoleUI().GetPaymentANSIText(), null);
                            new ConsoleUI().PrintOrderBorderLine();
                        }
                        new ConsoleUI().PrintOrderInfo((Order)(object)items[i]!);
                    }
                }
            }
            else if (toDo == 3)
            {
                for (int i = startIndex; i < endIndex; i++)
                {

                    if (typeof(T) == typeof(Phone))
                    {
                        if (i == startIndex)
                        {
                            new ConsoleUI().PrintTimeLine(timeLine, currentPhase);
                            new ConsoleUI().PrintTitle(new ConsoleUI().GetAppANSIText(), new ConsoleUI().GetHandleOrderANSIText(), null);
                            new ConsoleUI().PrintPhoneBorderLine();
                        }
                        new ConsoleUI().PrintPhoneInfo((Phone)(object)items[i]!);

                    }
                    else if (typeof(T) == typeof(PhoneDetail))
                    {
                        if (i == startIndex)
                        {
                            new ConsoleUI().PrintTimeLine(timeLine, currentPhase);
                            new ConsoleUI().PrintTitle(new ConsoleUI().GetAppANSIText(), new ConsoleUI().GetPhoneDetailANSIText(), null);
                            new ConsoleUI().PrintPhoneModelTitle();
                        }
                        new ConsoleUI().PrintPhoneModelInfo((PhoneDetail)(object)items[i]!);
                    }
                    else if (typeof(T) == typeof(Order))
                    {
                        if (i == startIndex)
                        {
                            new ConsoleUI().PrintTimeLine(timeLine, currentPhase);
                            new ConsoleUI().PrintTitle(new ConsoleUI().GetAppANSIText(), new ConsoleUI().GetHandleOrderANSIText(), null);
                            new ConsoleUI().PrintOrderBorderLine();
                        }
                        new ConsoleUI().PrintOrderInfo((Order)(object)items[i]!);
                    }
                }
            }
        }

        public void Alert(ConsoleEnum.Alert alertType, string msg)
        {
            string spaces = consoleUI.AlignCenter(msg);
            switch (alertType)
            {
                case ConsoleEnum.Alert.Success:
                    consoleUI.ConsoleForegroundColor(ConsoleEnum.Color.Green);
                    Console.WriteLine(spaces + msg.ToUpper());
                    break;
                case ConsoleEnum.Alert.Warning:
                    consoleUI.ConsoleForegroundColor(ConsoleEnum.Color.Yellow);
                    Console.WriteLine(spaces + msg.ToUpper());
                    break;
                case ConsoleEnum.Alert.Error:
                    consoleUI.ConsoleForegroundColor(ConsoleEnum.Color.Red);
                    Console.WriteLine(spaces + msg.ToUpper());
                    break;
                default:
                    break;
            }
            consoleUI.ConsoleForegroundColor(ConsoleEnum.Color.White);
            PressEnterTo("Continue");
        }
        public void PressEnterTo(string? action)
        {
            string str = $"Press Enter To {action}...";
            string spaces = consoleUI.AlignCenter(str);
            if (action != null)
            {
                Console.Write("\n" + spaces + str);
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
        public void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }
        public string GetUserName()
        {
            string spaces = consoleUI.AlignCenter("|--------------------------------------------------------------------------------------------|");
            string userName = GetInputString(spaces + " User Name");
            return userName;
        }
        public string GetPassword()
        {
            string spaces = consoleUI.AlignCenter("|--------------------------------------------------------------------------------------------|");
            string pass = "";
            do
            {
                Console.Write("\n" + spaces + " Password: ");
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
                if (pass == "") Alert(ConsoleEnum.Alert.Error, "Password Can't Empty");
            } while (pass == "");
            pass = pass.Substring(0, pass.Length - 1);
            Console.Write("\n");
            return pass;
        }
        public Customer GetCustomerInfo()
        {
            string PatternName = @"^.{0,28}[a-zA-Z\s]$";
            string PatternPhone = @"^\d{6,15}$";

            string spaces = consoleUI.AlignCenter("|--------------------------------------------------------------------------------------------|");
            string customerName = GetInputString($"{spaces} Customer Name");
            while (!Regex.IsMatch(customerName, PatternName))
            {
                Alert(ConsoleEnum.Alert.Error, "Invalid Customer Name customer names are not allowed to have special characters, numbers or more than 30 characters");
                customerName = GetInputString($"{spaces} Customer Name");
            }
            string phoneNumber = GetInputString($"{spaces} Phone Number");
            while (!Regex.IsMatch(phoneNumber, PatternPhone, RegexOptions.IgnoreCase))
            {
                Alert(ConsoleEnum.Alert.Error, "INVALID Phone Number please enter again");
                phoneNumber = GetInputString($"{spaces} Phone Number");
            }
            string address = GetInputString($"{spaces} Address");
            Customer customer = new Customer(0, customerName, phoneNumber, address);
            return customer;
        }
        public string GetInputString(string requestToEnter)
        {
            string str = "";
            do
            {
                Console.Write(requestToEnter + ": ");
                str = Console.ReadLine() ?? "";
                if (str == "")
                {
                    Alert(GUIEnum.ConsoleEnum.Alert.Error, "You Haven't Entered Anything Yet");
                }
            } while (str == "");
            return str;
        }
        public int GetInputInt(string requestToEnter)
        {
            int intValue = 0;
            bool isValidInput = false;
            do
            {
                Console.Write(requestToEnter + ": ");
                isValidInput = int.TryParse(Console.ReadLine(), out intValue);
                if (!isValidInput)
                {
                    Alert(GUIEnum.ConsoleEnum.Alert.Error, "Can't Enter A String");
                }
            } while (!isValidInput);
            return intValue;
        }
        public int InputIDValidation(int maximumValue, string requestToEnter, string errorMessage, string spaceToCenter)
        {
            int intValue = 0;
            do
            {
                intValue = GetInputInt(spaceToCenter + requestToEnter);
                if (intValue <= 0 || intValue > maximumValue)
                {
                    Alert(ConsoleEnum.Alert.Error, errorMessage);
                }
            } while (intValue <= 0 || intValue > maximumValue);
            return intValue;
        }
        public bool CheckInputIDValid(string inputId, List<int> IDPattern)
        { // Ham nay de loc input xem co dung kieu va gia tri co trong list(list order, list phonedetail, list imei ..vv.)
            string listofid = "";
            foreach (var ID in IDPattern)
            {
                listofid += (ID + " ");
            }
            int id;
            bool IsIntType = int.TryParse(inputId, out id);
            if (IsIntType == true)
            {
                int count = 0;
                foreach (var i in IDPattern)
                {
                    if (id == i) count++;
                }
                if (count != 0) return true;
                else
                {
                    Alert(ConsoleEnum.Alert.Error, $"Please choose an id in list {listofid}");
                    return false;
                }
            }
            else
            {
                Alert(ConsoleEnum.Alert.Error, "Invalid Input!");
                return false;
            }
        }
        public string GenerateID() => Guid.NewGuid().ToString("N").Substring(0, 12).ToUpper();
        public decimal EnterMoney()
        {
            decimal output = 0;
            string append = "";
            ConsoleKeyInfo cki;
            int centeredPosition = (Console.WindowWidth - "|--------------------------------------------------------------------------------------------|".Length) / 2;
            string spaces = centeredPosition > 0 ? new string(' ', centeredPosition) : "";
            Console.Write(spaces + "Enter money: ");
            bool checkMoneyOutOfRangeDecimal = false;
            do
            {
                int numout;
                cki = Console.ReadKey(true);
                if (Char.IsNumber(cki.KeyChar))
                {
                    if (int.TryParse(cki.KeyChar.ToString(), out numout))
                    {
                        append += numout.ToString();
                    }
                    try
                    {
                        ClearCurrentConsoleLine(1, spaces + "Enter money: " + consoleUI.SetTextBolder(consoleUI.FormatPrice(Convert.ToDecimal(append))));
                    }
                    catch (Exception ex)
                    {
                        checkMoneyOutOfRangeDecimal = true;
                        Console.WriteLine();
                        Alert(ConsoleEnum.Alert.Warning, "Money input too large! This cant be apply for our Store!");
                        break;
                    }
                    if (append.Length > 9)
                    {
                        checkMoneyOutOfRangeDecimal = true;
                        Console.WriteLine();
                        Alert(ConsoleEnum.Alert.Warning, "Money input too large! This cant be apply for our Store!");
                        break;
                    }
                }
                else
                {
                    if (cki.Key == ConsoleKey.Backspace)
                    {
                        string temp = "";
                        for (int i = 0; i < append.Length; i++)
                        {
                            if (i == append.Length - 1) break;
                            temp += append[i];
                        }
                        append = temp;
                        if (append.Length != 0) ClearCurrentConsoleLine(2, spaces + "Enter money: " + consoleUI.SetTextBolder(consoleUI.FormatPrice(Convert.ToDecimal(append))));

                    }
                }
            }
            while (cki.Key != ConsoleKey.Enter);
            if (append == "") Console.WriteLine("");
            else
            {
                if (!checkMoneyOutOfRangeDecimal)
                    output = Convert.ToDecimal(append);
            }
            return output;
        }
        public void ClearCurrentConsoleLine(int ver, string input)
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            if (ver == 2)
            {
                Console.SetCursorPosition(0, currentLineCursor - 1);
                Console.Write(new string(' ', Console.WindowWidth));
                Console.Write(input);
            }
            if (ver == 1)
            {
                Console.SetCursorPosition(0, currentLineCursor);
                Console.Write(input);
            }
        }
        public Order GetAnOrder(IStaffBL loginManager)
        {
            int currentPhase = 1, centeredPosition = (Console.WindowWidth - "|--------------------------------------------------------------------------------------------|".Length) / 2, secondcenteredPosition = (Console.WindowWidth - "|===================================================================================================|".Length) / 2;
            string input = "", spaces = centeredPosition > 0 ? new string(' ', centeredPosition) : "", orderID = "";
            Order order = new Order();
            do
            {
                orderID = GetInputString($"{spaces} Choose An Order ID").ToUpper();
                order = new OrderBL().GetOrderById(orderID) ?? null;
                if (order!.OrderID == "") Alert(ConsoleEnum.Alert.Error, "Invalid Order ID");
                else order.Accountant = loginManager.LoggedInStaff;
            } while (order.OrderID == "");
            return order;
        }
        public bool CheckImeiValid(string imei)
        {
            if (imei.Length != 15)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < 15; i++)
                {
                    int numOut;
                    if (!int.TryParse(imei[i].ToString(), out numOut)) return false;
                }
            }
            return true;

        }
    }
}
