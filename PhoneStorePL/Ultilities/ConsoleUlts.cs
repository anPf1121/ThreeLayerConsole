using System;
using GUIEnum;
using Model;
using DAL;
using BusinessEnum;
using System.Globalization; // thu vien format tien
using BL;
using UI;
using System.Text.RegularExpressions;


namespace Ults
{
    class ConsoleUlts
    {
        ConsoleUI consoleUI = new ConsoleUI();
        int currentPageDetails = 1;
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
        public int PressCharacterTo(string firstAction, string? secondAction, string? thirdAction)
        {
            string firstStr = $"Press 'Q' To {firstAction} - Press 'W' To {secondAction} - Press 'E' To {thirdAction}";
            string secondStr = $"Press 'Q' To {firstAction} - Press 'W' To {secondAction}";
            int centeredPosition = (Console.WindowWidth - firstStr.Length) / 2;
            int secondCenteredPosition = (Console.WindowWidth - secondStr.Length) / 2;
            string firstSpaces = centeredPosition > 0 ? new string(' ', centeredPosition) : "";
            string secondSpaces = secondCenteredPosition > 0 ? new string(' ', secondCenteredPosition) : "";
            ConsoleKeyInfo input = new ConsoleKeyInfo();
            bool active = true;
            if (thirdAction != null)
                Console.Write(firstSpaces + firstStr);
            else
                Console.Write(secondSpaces + secondStr);
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
            } while (active);
            return 0;
        }
        public int MenuHandle(string? title, string? subTitle, string[] menuItem, Staff loginStaff)
        {
            ConsoleUlts consoleUlts = new ConsoleUlts();
            int centeredPosition = (Console.WindowWidth - "|--------------------------------------------------------------------------------------------|".Length) / 2;
            string spaces = centeredPosition > 0 ? new string(' ', centeredPosition) : "";
            Console.Clear();
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            ConsoleKeyInfo keyInfo;
            string iconBackhand = "👉";
            bool activeSelectedMenu = true;
            int currentChoice = 1;
            int renderCount = 0;
            while (activeSelectedMenu)
            {
                if (title != null || subTitle != null)
                    consoleUI.PrintTitle(title, subTitle, loginStaff);
                if (currentChoice <= (menuItem.Count() + 1) && currentChoice >= 1)
                {
                    for (int i = 0; i < menuItem.Count(); i++)
                        Console.WriteLine(spaces + "| {0, 50} |", (((currentChoice - 1 == i) ? (iconBackhand + " ") : "") + " " + consoleUI.SetTextBolder(menuItem[i]) + $" ({i + 1})").PadRight(98));
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

        public bool Pagination<T>(List<T> listItem, int currentPhase)
        {
            int currentPage = 1;
            int totalPages = (int)Math.Ceiling((double)listItem.Count / 5);
            int centeredPosition = (Console.WindowWidth - "|============================================================================================|".Length) / 2;
            string spaces = centeredPosition > 0 ? new string(' ', centeredPosition) : "";
            ConsoleKeyInfo key;
            do
            {
                Console.Clear();

                if (typeof(T) == typeof(Phone))
                    DisplayCurrentPage((List<Phone>)(object)listItem, currentPage, 5, currentPhase);
                else if (typeof(T) == typeof(PhoneDetail))
                    DisplayCurrentPage((List<PhoneDetail>)(object)listItem, currentPage, 5, currentPhase);
                else if (typeof(T) == typeof(Order))
                    DisplayCurrentPage((List<Order>)(object)listItem, currentPage, 5, currentPhase);
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

        public void DisplayCurrentPage<T>(List<T> items, int currentPage, int itemsPerPage, int currentPhase)
        {
            int startIndex = (currentPage - 1) * itemsPerPage;
            int endIndex = Math.Min(startIndex + itemsPerPage, items.Count);

            for (int i = startIndex; i < endIndex; i++)
            {

                if (typeof(T) == typeof(Phone))
                {
                    if (i == startIndex)
                    {
                        new ConsoleUI().PrintTimeLine(new ConsoleUI().GetCreateOrderTimeLine(), currentPhase);
                        new ConsoleUI().PrintTitle(new ConsoleUI().GetAppANSIText(), new ConsoleUI().GetAddPhoneToOrderANSIText(), null);
                        new ConsoleUI().PrintPhoneBorderLine();
                    }
                    new ConsoleUI().PrintPhoneInfo((Phone)(object)items[i]!);

                }
                else if (typeof(T) == typeof(PhoneDetail))
                {
                    if (i == startIndex)
                    {
                        new ConsoleUI().PrintTimeLine(new ConsoleUI().GetCreateOrderTimeLine(), currentPhase);
                        new ConsoleUI().PrintTitle(new ConsoleUI().GetAppANSIText(), new ConsoleUI().GetAddPhoneToOrderANSIText(), null);
                        new ConsoleUI().PrintPhoneModelTitle();
                    }
                    new ConsoleUI().PrintPhoneModelInfo((PhoneDetail)(object)items[i]!);
                }
                else if (typeof(T) == typeof(Order))
                {
                    if (i == startIndex)
                    {
                        new ConsoleUI().PrintTimeLine(new ConsoleUI().GetCreateOrderTimeLine(), currentPhase);
                        new ConsoleUI().PrintTitle(new ConsoleUI().GetAppANSIText(), new ConsoleUI().GetAddPhoneToOrderANSIText(), null);
                        new ConsoleUI().PrintOrderBorderLine();
                    }
                    new ConsoleUI().PrintOrderInfo((Order)(object)items[i]!);
                }
            }
        }

        public void Alert(ConsoleEnum.Alert alertType, string msg)
        {
            int centeredPosition = (Console.WindowWidth - msg.Length) / 2;
            string spaces = centeredPosition > 0 ? new string(' ', centeredPosition) : "";
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
            string str = $"👉 Press Enter To {action}...";
            int centeredPosition = (Console.WindowWidth - str.Length) / 2;
            string spaces = centeredPosition > 0 ? new string(' ', centeredPosition) : "";
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
            int centeredPosition = (Console.WindowWidth - "|--------------------------------------------------------------------------------------------|".Length) / 2;
            string spaces = centeredPosition > 0 ? new string(' ', centeredPosition) : "";
            string userName = GetInputString(spaces + "👉 User Name");
            return userName;
        }

        public string GetPassword()
        {
            int centeredPosition = (Console.WindowWidth - "|--------------------------------------------------------------------------------------------|".Length) / 2;
            string spaces = centeredPosition > 0 ? new string(' ', centeredPosition) : "";
            string pass = "";
            do
            {
                Console.Write("\n" + spaces + "👉 Password: ");
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
            string PatternName = "^[a-zA-Z\\s]+$";
            string PatternPhone = @"[0-9]$";
            int centeredPosition = (Console.WindowWidth - "|--------------------------------------------------------------------------------------------|".Length) / 2;
            string spaces = centeredPosition > 0 ? new string(' ', centeredPosition) : "";
            string customerName = GetInputString($"{spaces}Customer Name");
            while (!Regex.IsMatch(customerName, PatternName))
            {
                Alert(ConsoleEnum.Alert.Error, "Invalid Customer Name customer names are not allowed to have special characters and numbers");
                customerName = GetInputString($"{spaces}Customer Name");
            }
            string phoneNumber = GetInputString($"{spaces}Phone Number");
            while (!Regex.IsMatch(phoneNumber, PatternPhone, RegexOptions.IgnoreCase))
            {
                Alert(ConsoleEnum.Alert.Error, "INVALID Phone Number please enter again");
                phoneNumber = GetInputString($"{spaces}Phone Number");
            }
            string address = GetInputString($"{spaces}Address");
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
            int centeredPosition = (Console.WindowWidth - "|--------------------------------------------------------------------------------------------|".Length) / 2;
            string spaces = centeredPosition > 0 ? new string(' ', centeredPosition) : "";
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

        public DateTime GetDate(string requestToEnter)
        {
            Console.Write(requestToEnter + ": ");
            DateTime dateTime;
            while (!DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out dateTime))
            {
                Console.Write("Invalid Date Format, Please Re-enter (yyyy-MM-dd): ");
            }
            return dateTime;
        }
        public void PrintColumnChart(Dictionary<string, decimal> data)
        {
            StaffBL staffBL = new StaffBL();
            Console.WriteLine();
            int centeredPosition = (Console.WindowWidth - "|==================================================================================================================================================|".Length) / 2;
            string spaces = centeredPosition > 0 ? new string(' ', centeredPosition) : "";
            PhoneBL phoneBL = new PhoneBL();
            decimal maxValue = data.Values.Max();
            int count = 0;
            if (data.Count() == 2)
            {
                foreach (var value in data)
                {
                    count++;
                    for (int i = 0; i < value.Value / maxValue * 100; i++)
                    {
                        if (i == 0)
                        {
                            Console.Write(spaces + "    {0, -20} | ", value.Key);
                        }
                        else
                        {
                            Console.Write("█");
                        }
                        if (i >= value.Value / maxValue * 100 - 1)
                        {
                            Console.Write(" | {0, -15}", (value.Value / maxValue * 100).ToString("0.##") + "%");
                        }
                    }
                    Console.WriteLine("\n");
                    if (count == 5) break;
                    if (count == data.Count()) Console.WriteLine(spaces + "  * Your revenue accounts for {0} of the store's revenue", (value.Value / maxValue * 100).ToString("0.##") + "%");
                }
            }
            return;
        }
        public Dictionary<string, decimal> accountantDataHandleToPrintChart(string accountantName, decimal totalRevenue, decimal totalStoreRevenue)
        {
            Dictionary<string, decimal> result = new Dictionary<string, decimal>();
            result.Add("Store", totalStoreRevenue);
            result.Add(accountantName, totalRevenue);
            return result;
        }

        public void PrintColumnChart(Dictionary<int, decimal> data)
        {
            Console.WriteLine();
            int centeredPosition = (Console.WindowWidth - "|==================================================================================================================================================|".Length) / 2;
            string spaces = centeredPosition > 0 ? new string(' ', centeredPosition) : "";
            PhoneBL phoneBL = new PhoneBL();
            decimal maxValue = data.Values.Max();
            int count = 0;
            foreach (var value in data)
            {
                count++;
                for (int i = 0; i < value.Value / maxValue * 100; i++)
                {
                    if (i == 0)
                    {
                        Console.Write(spaces + "    {0, -20} | ", phoneBL.GetPhoneDetailByID(value.Key).Phone.PhoneName + " " + phoneBL.GetPhoneDetailByID(value.Key).PhoneColor.Color + " " + phoneBL.GetPhoneDetailByID(value.Key).ROMSize.ROM);
                    }
                    else
                    {
                        Console.Write("█");
                    }
                    if (i >= value.Value / maxValue * 100 - 1)
                    {
                        Console.Write(" | {0, -15}", consoleUI.FormatPrice(value.Value));
                    }
                }
                Console.WriteLine("\n");
                if (count == 5) break;
            }
        }
        public Dictionary<int, decimal> DataToPrintChartHandle(List<Order> orders)
        {
            Dictionary<int, decimal> phoneIdWithPrice = new Dictionary<int, decimal>();

            foreach (var order in orders)
            {
                foreach (var phone in order.PhoneDetails)
                {
                    if (phoneIdWithPrice.ContainsKey(phone.PhoneDetailID))
                    {
                        phoneIdWithPrice[phone.PhoneDetailID] += phone.Quantity * phone.Price;
                    }
                    else
                    {
                        phoneIdWithPrice[phone.PhoneDetailID] = phone.Quantity * phone.Price;
                    }
                }
            }
            Dictionary<int, decimal> sortedDictionary = new Dictionary<int, decimal>();
            List<KeyValuePair<int, decimal>> sortedList = new List<KeyValuePair<int, decimal>>(phoneIdWithPrice);
            sortedList.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));
            foreach (var pair in sortedList)
            {
                sortedDictionary.Add(pair.Key, pair.Value);
            }
            return sortedDictionary;
        }
        public Dictionary<PhoneDetail, Dictionary<PhoneEnum.Status, decimal>> GetListPhoneTradeIn()
        {
            List<PhoneDetail> ps = new PhoneBL().GetPhonesCanTradeIn();
            Dictionary<PhoneDetail, Dictionary<PhoneEnum.Status, decimal>> a = new Dictionary<PhoneDetail, Dictionary<PhoneEnum.Status, decimal>>();
            List<PhoneDetail> listcheck = new List<PhoneDetail>();
            foreach (var p in ps)
            {
                int count = 0;
                foreach (var l in listcheck)
                {
                    if (l.Phone.PhoneName == p.Phone.PhoneName && l.ROMSize.ROM == p.ROMSize.ROM && l.PhoneColor.Color == p.PhoneColor.Color) count++;
                }
                if (count == 0) listcheck.Add(p);
            }

            foreach (var l in listcheck)
            {
                Dictionary<PhoneEnum.Status, decimal> phonestatus = new Dictionary<PhoneEnum.Status, decimal>();
                foreach (var p in ps)
                {
                    if (l.Phone.PhoneName == p.Phone.PhoneName && l.ROMSize.ROM == p.ROMSize.ROM && l.PhoneColor.Color == p.PhoneColor.Color) phonestatus.Add(p.PhoneStatusType, p.Price);
                }
                a.Add(l, phonestatus);
            }
            return a;
        }
    }

}
