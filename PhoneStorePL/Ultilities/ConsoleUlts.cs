using System;
using GUIEnum;
using Model;
using DAL;
using BusinessEnum;
using System.Globalization; // thu vien format tien
using BL;
using UI;

namespace Ults
{
    class ConsoleUlts
    {
        ConsoleUI consoleUI = new ConsoleUI();
        int currentPageDetails = 1;
        public Dictionary<int, List<Phone>> listAllPhones = null;

        public bool PressYesOrNo(string yesAction, string noAction)
        {
            string str = $"Press 'Y' To {yesAction} Or 'N' To {noAction}";
            int centeredPosition = (Console.WindowWidth - str.Length) / 2;
            string spaces = new string(' ', centeredPosition);

            ConsoleKeyInfo input = new ConsoleKeyInfo();
            bool active = true;
            char ch = 'c';
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
            string firstSpaces = new string(' ', centeredPosition);
            string secondSpaces = new string(' ', secondCenteredPosition);
            ConsoleKeyInfo input = new ConsoleKeyInfo();
            bool active = true;
            char ch = 'c';
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
            int centeredPosition = (Console.WindowWidth - "|--------------------------------------------------------------------------------------------|".Length) / 2;
            string spaces = new string(' ', centeredPosition);
            Console.Clear();
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            ConsoleKeyInfo keyInfo;
            string iconBackhand = "👉";
            bool activeSelectedMenu = true;
            int currentChoice = 1;
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
                            currentChoice = 1;
                        else if (currentChoice == 0)
                            currentChoice = menuItem.Count();
                        Console.Clear();
                    }
                    Console.Clear();
                }
            }
            return currentChoice;
        }

        public bool? ListOrderPagination(List<Order> listOrder, string[] phases, int itemCount, int currentPhase, Staff staff)
        {
            if (listOrder != null)
            {
                string title = consoleUI.GetAllOrderANSIText();
                bool active = true;
                Dictionary<int, List<Order>> orders = new Dictionary<int, List<Order>>();
                orders = OrderMenuPaginationHandle(listOrder);
                if (orders != null && orders.Count > 0)
                {
                    int countPage = orders.Count(),
                        currentPage = 1;
                    ConsoleKeyInfo input = new ConsoleKeyInfo();
                    while (true)
                    {
                        Console.Clear();
                        consoleUI.PrintTitle(consoleUI.GetAppANSIText(), title, staff);
                        while (active)
                        {
                            consoleUI.PrintTimeLine(phases, itemCount, currentPhase);
                            consoleUI.PrintTitle(consoleUI.GetAppANSIText(), title, staff);
                            consoleUI.PrintOrderBorderLine();
                            foreach (Order order in orders[currentPage])
                            {
                                consoleUI.PrintOrderInfo(order);
                            }
                            consoleUI.GetFooterPagination(currentPage, countPage);
                            do
                            {
                                input = Console.ReadKey();
                                if (currentPage <= countPage)
                                {
                                    if (input.Key == ConsoleKey.RightArrow)
                                    {
                                        if (currentPage <= countPage - 1)
                                            currentPage++;
                                        currentPageDetails = currentPage;
                                        Console.Clear();
                                        break;
                                    }
                                    else if (input.Key == ConsoleKey.LeftArrow)
                                    {
                                        if (currentPage > 1)
                                            currentPage--;
                                        currentPageDetails = currentPage;
                                        Console.Clear();
                                        break;
                                    }
                                    else if (input.Key == ConsoleKey.B)
                                    {
                                        Console.Clear();
                                        return false;
                                    }
                                    else if (input.Key == ConsoleKey.Spacebar)
                                    {
                                        return true;
                                    }
                                }
                            } while (input.Key != ConsoleKey.RightArrow || input.Key != ConsoleKey.LeftArrow || input.Key != ConsoleKey.B || input.Key != ConsoleKey.Spacebar);
                        }
                    }
                }
            }
            return null;
        }

        public Dictionary<int, List<Phone>> PhoneMenuPaginationHandle(List<Phone> phoneList)
        {
            List<Phone> sList = new List<Phone>();
            Dictionary<int, List<Phone>> menuTab = new Dictionary<int, List<Phone>>();
            int phoneQuantity = phoneList.Count(), itemInTab = 4, count = 1, secondCount = 1, idTab = 0;

            foreach (Phone phone in phoneList)
            {
                if ((count - 1) == itemInTab)
                {
                    sList = new List<Phone>();
                    count = 1;
                }
                sList.Add(phone);
                if (sList.Count() == itemInTab)
                {
                    idTab++;
                    menuTab.Add(idTab, sList);
                }
                else if (sList.Count() < itemInTab && secondCount == phoneQuantity)
                {
                    idTab++;
                    menuTab.Add(idTab, sList);
                }
                secondCount++;
                count++;
            }
            return menuTab;
        }

        public Dictionary<int, List<Order>> OrderMenuPaginationHandle(List<Order> orderList)
        {
            List<Order> sList = new List<Order>();
            Dictionary<int, List<Order>> menuTab = new Dictionary<int, List<Order>>();
            int orderQuantity = orderList.Count(), itemInTab = 4, count = 1, secondCount = 1, idTab = 0;

            foreach (Order order in orderList)
            {
                if ((count - 1) == itemInTab)
                {
                    sList = new List<Order>();
                    count = 1;
                }
                sList.Add(order);
                if (sList.Count() == itemInTab)
                {
                    idTab++;
                    menuTab.Add(idTab, sList);
                }
                else if (sList.Count() < itemInTab && secondCount == orderQuantity)
                {
                    idTab++;
                    menuTab.Add(idTab, sList);
                }
                secondCount++;
                count++;
            }
            return menuTab;
        }
        public bool ListPhonePagination(List<Phone> listPhone, string[] phases, int itemCount, int currentPhase, Staff loggedInStaff)
        {
            string title = consoleUI.GetAddPhoneToOrderANSIText();
            if (listPhone != null)
            {
                bool active = true;
                Dictionary<int, List<Phone>> phones = new Dictionary<int, List<Phone>>();
                phones = PhoneMenuPaginationHandle(listPhone);
                listAllPhones = phones;
                int countPage = phones.Count(), currentPage = 1;
                ConsoleKeyInfo input = new ConsoleKeyInfo();
                while (true)
                {
                    consoleUI.PrintTitle(consoleUI.GetAppANSIText(), title, loggedInStaff);
                    while (active)
                    {
                        Console.Clear();
                        consoleUI.PrintTimeLine(phases, itemCount, currentPhase);
                        consoleUI.PrintTitle(consoleUI.GetAppANSIText(), title, loggedInStaff);
                        consoleUI.PrintPhoneBorderLine();
                        foreach (Phone phone in phones[currentPage])
                        {
                            consoleUI.PrintPhoneInfo(phone);
                        }
                        consoleUI.GetFooterPagination(currentPage, countPage);
                        do
                        {
                            input = Console.ReadKey();
                            if (currentPage <= countPage)
                            {
                                if (input.Key == ConsoleKey.RightArrow)
                                {
                                    if (currentPage <= countPage - 1) currentPage++;
                                    currentPageDetails = currentPage;
                                    Console.Clear();
                                    break;
                                }
                                else if (input.Key == ConsoleKey.LeftArrow)
                                {
                                    if (currentPage > 1)
                                        currentPage--;
                                    currentPageDetails = currentPage;
                                    Console.Clear();
                                    break;
                                }
                                else if (input.Key == ConsoleKey.B)
                                {
                                    Console.Clear();
                                    return false;
                                }
                                else if (input.Key == ConsoleKey.Spacebar)
                                {
                                    return true;
                                }
                            }
                        } while (input.Key != ConsoleKey.RightArrow || input.Key != ConsoleKey.LeftArrow || input.Key != ConsoleKey.B || input.Key != ConsoleKey.Spacebar);
                    }
                }
            }
            else
            {
                Alert(ConsoleEnum.Alert.Warning, "Phone Not Found");
                PressEnterTo("Back To Previous Menu");
            }
            return false;
        }


        public void Alert(ConsoleEnum.Alert alertType, string msg)
        {
            int centeredPosition = (Console.WindowWidth - msg.Length) / 2;
            string spaces = new string(' ', centeredPosition);
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
            string spaces = new string(' ', centeredPosition);
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
            string spaces = new string(' ', centeredPosition);
            string userName = GetInputString(spaces + "👉 User Name");
            return userName;
        }

        public string GetPassword()
        {
            int centeredPosition = (Console.WindowWidth - "|--------------------------------------------------------------------------------------------|".Length) / 2;
            string spaces = new string(' ', centeredPosition);
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
            int centeredPosition = (Console.WindowWidth - "|--------------------------------------------------------------------------------------------|".Length) / 2;
            string spaces = new string(' ', centeredPosition);
            string customerName = GetInputString($"{spaces}Customer Name");
            string phoneNumber = GetInputString($"{spaces}Phone Number");
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
        public int InputIDValidation(int maximumValue, string requestToEnter, string errorMessage)
        {
            int intValue = 0;
            do
            {
                intValue = GetInputInt(requestToEnter);
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
                    Console.WriteLine($"Please choose an id in list {listofid}");
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Invalid input! Please input a number!");
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
        public void PrintColumnChart(Dictionary<int, decimal> data)
        {
            Console.WriteLine();
            int centeredPosition = (Console.WindowWidth - "|==================================================================================================================================================|".Length) / 2;
            string spaces = new string(' ', centeredPosition);
            PhoneBL phoneBL = new PhoneBL();
            decimal maxValue = data.Values.Max();
            foreach (var value in data)
            {
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
            }
        }
        public Dictionary<int, decimal> DataToPrintChartHandle(List<Order> orders)
        {
            Dictionary<int, decimal> phoneIdToTotalSold = new Dictionary<int, decimal>();

            foreach (var order in orders)
            {
                foreach (var phone in order.PhoneDetails)
                {
                    if(phoneIdToTotalSold.Count() == 5) return phoneIdToTotalSold;
                    if (phoneIdToTotalSold.ContainsKey(phone.PhoneDetailID))
                    {
                        phoneIdToTotalSold[phone.PhoneDetailID] += phone.Quantity * phone.Price;
                    }
                    else
                    {
                        phoneIdToTotalSold[phone.PhoneDetailID] = phone.Quantity * phone.Price;
                    }

                }
            }
            return phoneIdToTotalSold;
        }
    }

}
