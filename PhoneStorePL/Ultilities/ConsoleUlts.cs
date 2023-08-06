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
        int currentPageDetails = 1;
        public Dictionary<int, List<Phone>> listAllPhones = null;
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
        public void Line()
        {
            int centeredPosition = (Console.WindowWidth - "|--------------------------------------------------------------------------------------------|".Length) / 2;
            string spaces = new string(' ', centeredPosition);
            Console.WriteLine(spaces + @"|--------------------------------------------------------------------------------------------|");
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
                    Title(title, subTitle, loginStaff);
                if (currentChoice <= (menuItem.Count() + 1) && currentChoice >= 1)
                {
                    for (int i = 0; i < menuItem.Count(); i++)
                        Console.WriteLine(spaces + "| {0, 50} |", (((currentChoice - 1 == i) ? (iconBackhand + " ") : "") + " " + SetTextBolder(menuItem[i]) + $" ({i + 1})").PadRight(98));
                    Line();

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
                string title = GetAllOrderANSITitle();
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
                        Title(GetAppTitle(), title, staff);
                        while (active)
                        {
                            PrintTimeLine(phases, itemCount, currentPhase);
                            Title(GetAppTitle(), title, staff);
                            PrintOrderBorderLine();
                            foreach (Order order in orders[currentPage])
                            {
                                PrintOrderInfo(order);
                            }
                            GetFooterPagination(currentPage, countPage);
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
            string title = GetAddPhoneToOrderANSITitle();
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
                    Title(GetAppTitle(), title, loggedInStaff);
                    while (active)
                    {
                        Console.Clear();
                        PrintTimeLine(phases, itemCount, currentPhase);
                        Title(GetAppTitle(), title, loggedInStaff);
                        PrintPhoneBorderLine();
                        foreach (Phone phone in phones[currentPage])
                        {
                            PrintPhoneInfo(phone);
                        }
                        GetFooterPagination(currentPage, countPage);
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
        public void PrintPhoneDetailsInfo(List<PhoneDetail> phoneDetails)
        {
            int centeredPosition = (Console.WindowWidth - "|===================================================================================================|".Length) / 2;
            string spaces = new string(' ', centeredPosition);
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
            Console.WriteLine(spaces + "|===================================================================================================|");
        }

        public void FullWidthTinyLine()
        {
            Console.WriteLine(new string('-', Console.WindowWidth));
        }

        public void Alert(ConsoleEnum.Alert alertType, string msg)
        {
            int centeredPosition = (Console.WindowWidth - msg.Length) / 2;
            string spaces = new string(' ', centeredPosition);
            switch (alertType)
            {
                case ConsoleEnum.Alert.Success:
                    ConsoleForegroundColor(ConsoleEnum.Color.Green);
                    Console.WriteLine(spaces + msg.ToUpper());
                    break;
                case ConsoleEnum.Alert.Warning:
                    ConsoleForegroundColor(ConsoleEnum.Color.Yellow);
                    Console.WriteLine(spaces + msg.ToUpper());
                    break;
                case ConsoleEnum.Alert.Error:
                    ConsoleForegroundColor(ConsoleEnum.Color.Red);
                    Console.WriteLine(spaces + msg.ToUpper());
                    break;
                default:
                    break;
            }
            ConsoleForegroundColor(ConsoleEnum.Color.White);
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

        public void PrintPhoneBorderLine()
        {
            int centeredPosition = (Console.WindowWidth - "|============================================================================================|".Length) / 2;
            string spaces = new string(' ', centeredPosition);
            Console.WriteLine(spaces + "|============================================================================================|");
            Console.WriteLine(spaces + "| {0, -10} | {1, -25} | {2, -13} | {3, -13} | {4, -17} |", "ID", "Phone Name", "Brand", "OS", "Mobile Network");
            Console.WriteLine(spaces + "|============================================================================================|");
        }

        public void PrintOrderBorderLine()
        {
            int centeredPosition = (Console.WindowWidth - "|============================================================================================|".Length) / 2;
            string spaces = new string(' ', centeredPosition);
            Console.WriteLine(spaces + "|============================================================================================|");
            Console.WriteLine(spaces + "| {0, -13} | {1, -25} | {2, -23} | {3, -20} |", "ID", "Customer Name", "Order Date", "Status");
            Console.WriteLine(spaces + "|============================================================================================|");
        }

        public void PrintPhoneInfo(Phone phone)
        {
            int centeredPosition = (Console.WindowWidth - "|============================================================================================|".Length) / 2;
            string spaces = new string(' ', centeredPosition);
            Console.WriteLine(spaces + "| {0, -10} | {1, -25} | {2, -13} | {3, -13} | {4, -17} |", phone.PhoneID, phone.PhoneName, phone.Brand.BrandName, phone.OS, phone.Connection);
        }

        public void PrintOrderInfo(Order order)
        {
            int centeredPosition = (Console.WindowWidth - "|--------------------------------------------------------------------------------------------|".Length) / 2;
            string spaces = new string(' ', centeredPosition);
            Console.WriteLine(spaces + "| {0, -13} | {1, -25} | {2, -23} | {3, -20} |", order.OrderID, order.Customer.CustomerName, order.CreateAt, order.OrderStatus);
        }
        public void PrintPhoneModelTitle()
        {
            int centeredPosition = (Console.WindowWidth - "|===================================================================================================|".Length) / 2;
            string spaces = new string(' ', centeredPosition);
            Console.WriteLine(spaces + "|===================================================================================================|");
            Console.WriteLine(spaces + "| PHONE MODEL                                                                                       |");
            Console.WriteLine(spaces + "====================================================================================================|");
            Console.WriteLine(spaces + "| {0, -10} | {1, -13} | {2, -15} | {3, -15} | {4, -15} | {5, -14} |", "Detail ID", "Phone Color", "ROM Size", "Price", "Phone Status", "Quantity");
            Console.WriteLine(spaces + "|===================================================================================================|");
        }

        public void PrintPhoneDetailsInfo(PhoneDetail phoneDetail)
        {
            int centeredPosition = (Console.WindowWidth - "|===================================================================================================|".Length) / 2;
            string spaces = new string(' ', centeredPosition);
            Console.WriteLine(spaces + "|===================================================================================================|");
            Console.WriteLine(spaces + "| PHONE DETAILS INFOMATION                                                                          |");
            Console.WriteLine(spaces + "|===================================================================================================|");
            Console.WriteLine(spaces + "| Phone Name: {0, -50} |", phoneDetail.Phone.PhoneName.PadRight(85));
            Console.WriteLine(spaces + "| Brand: {0, -50} |", phoneDetail.Phone.Brand.BrandName.PadRight(90));
            Console.WriteLine(spaces + "| Camera: {0, -50} |", phoneDetail.Phone.Camera.PadRight(89));
            Console.WriteLine(spaces + "| RAM: {0, -50} |", phoneDetail.Phone.RAM.PadRight(92));
            Console.WriteLine(spaces + "| Weight: {0, -50} |", phoneDetail.Phone.Weight.PadRight(89));
            Console.WriteLine(spaces + "| Processor: {0, -50} |", phoneDetail.Phone.Processor.PadRight(86));
            Console.WriteLine(spaces + "| Battery: {0, -50} |", phoneDetail.Phone.BatteryCapacity.PadRight(88));
            Console.WriteLine(spaces + "| OS: {0, -50} |", phoneDetail.Phone.OS.PadRight(93));
            Console.WriteLine(spaces + "| Sim Slot: {0, -50} |", phoneDetail.Phone.SimSlot.PadRight(87));
            Console.WriteLine(spaces + "| Screen : {0, -50} |", phoneDetail.Phone.Screen.PadRight(88));
            Console.WriteLine(spaces + "| Connection: {0, -50} |", phoneDetail.Phone.Connection.PadRight(85));
            Console.WriteLine(spaces + "| Charge Port: {0, -50} |", phoneDetail.Phone.ChargePort.PadRight(84));
            Console.WriteLine(spaces + "| Release Date: {0, -50} |", phoneDetail.Phone.ReleaseDate.ToString().PadRight(83));
            Console.WriteLine(spaces + "| Description: {0, -50} |", phoneDetail.Phone.Description.PadRight(84));
        }

        public void PrintTimeLine(string[] phase, int itemCount, int currentPhase)
        {
            Console.Clear();
            FullWidthTinyLine();
            foreach (string item in phase)
            {
                itemCount++;
                if (itemCount == currentPhase)
                {
                    ConsoleForegroundColor(ConsoleEnum.Color.Green);
                    Console.Write(((itemCount == currentPhase) ? " 👉 " + SetTextBolder(item) : " > " + item));
                    ConsoleForegroundColor(ConsoleEnum.Color.White);
                }
                else
                {
                    Console.Write(((itemCount == currentPhase) ? " 👉 " + SetTextBolder(item) : " > " + item));
                }
                if (itemCount == phase.Length)
                    Console.Write("\n");
            }
            FullWidthTinyLine();
            itemCount = 0;
        }
        public string SetTextBolder(string text)
        {
            return $"\x1b[1m{text}\x1b[0m";
        }
        public void PrintSellerOrder(Order ord)
        {
            int centeredPosition = (Console.WindowWidth - "|===========================================================================================================================|".Length) / 2;
            string spaces = new string(' ', centeredPosition);
            Console.WriteLine("\n" + spaces + "|===========================================================================================================================|");
            Console.WriteLine(spaces + "|------------------------------------------------------- \x1b[1mVTC Mobile\x1b[0m --------------------------------------------------------|");
            Console.WriteLine(spaces + "|===========================================================================================================================|");
            Console.WriteLine(spaces + "|                                                   Order ID: " + ord.OrderID + "                                                  |");
            Console.WriteLine(spaces + "|===========================================================================================================================|");
            Console.WriteLine(spaces + "| Website: https://vtc.edu.vn/                                                                                              |");
            Console.WriteLine(spaces + "| Address: 18 Tam Trinh, Quận Hai Bà Trưng, Thành Phố Hà Nội                                                                |");
            Console.WriteLine(spaces + "| Phone Number: 0999999999                                                                                                  |");
            Console.WriteLine(spaces + "|===========================================================================================================================|");
            Console.WriteLine(spaces + "| Order Create Time: {0, -30}|", DateTime.Now.ToString().PadRight(103));
            Console.WriteLine(spaces + "| Customer: {0, -30}|", ord.Customer.CustomerName.PadRight(112));
            Console.WriteLine(spaces + "| Address: {0, -50}|", ord.Customer.Address.PadRight(113));
            Console.WriteLine(spaces + "| Phone Number: {0, -12}|", ord.Customer.PhoneNumber.PadRight(108));
            Console.Write((ord.Accountant.StaffID != 0) ? (spaces + "| Payment Method: {0, 35} |" + "\n") : "", "Chua xu ly".PadRight(105));
            Console.WriteLine(spaces + "|===========================================================================================================================|");
            PrintOrderDetails(ord);
            Console.WriteLine(spaces + "|===========================================================================================================================|");
            if (ord.Accountant.StaffID != 0)
            {
                if (ord.DiscountPolicies.Count() != 0)
                {
                    Console.WriteLine(spaces + "| {0, 46}                                                                            |", SetTextBolder("All DiscountPolicy Be Apply For This Order Is "));
                    foreach (var dp in ord.DiscountPolicies)
                    {
                        Console.WriteLine(spaces + "| - {0, -100} |", dp.Title.PadRight(119));
                        ord.TotalDue -= dp.DiscountPrice;
                    }
                }
            }
            Console.WriteLine(spaces + "|---------------------------------------------------------------------------------------------------------------------------|");
            Console.WriteLine(spaces + "| {0, 40}{1, -26}{2, 15}|", "", "Total Due: ", SetTextBolder(FormatPrice(ord.GetTotalDue()).PadRight(56)));
            if (ord.Accountant.StaffID != 0)
            {
                Console.WriteLine(spaces + "| {0, 40}{1, -25}{2, 22}|", "", "Discount Price: ", SetTextBolder(FormatPrice(ord.TotalDue - ord.GetTotalDue()).ToString().PadRight(57)));
                Console.WriteLine(spaces + "| {0, 40}{1, -15}{2, 21}|", "", "Total Due After Discount: ", SetTextBolder(FormatPrice(ord.TotalDue).ToString().PadRight(56)));
            }
            Console.WriteLine(spaces + "| {0, 40}{1, -26}{2, 49}|", "", "To String: ", (ord.Accountant.StaffID == 0) ? SetTextBolder(ConvertNumberToWords(ord.GetTotalDue()).PadRight(56)) : SetTextBolder(ConvertNumberToWords(ord.TotalDue).PadRight(56)));
            Console.WriteLine(spaces + "|===========================================================================================================================|");
            Console.WriteLine(spaces + "|{0, 10}{1, -35} {2, -40} {3, -36}|", " ", "Customer", "Seller", "Accountant", " ");
            Console.WriteLine(spaces + "|{0, 10}{1, -35} {2, -40} {3, -36}|", " ", ord.Customer.CustomerName, ord.Seller.StaffName + " - ID: " + ord.Seller.StaffID, (ord.Accountant.StaffID == 0) ? "" : (ord.Accountant.StaffName + " - ID: " + ord.Accountant.StaffID), " ");
            Console.WriteLine(spaces + "|===========================================================================================================================|");
        }


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
            string[] ones = { "", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string[] teens = { "mười", "mười một", "mười hai", "mười ba", "mười bốn", "mười lăm", "mười sáu", "mười bảy", "mười tám", "mười chín" };
            string[] tens = { "", "", "hai mươi", "ba mươi", "bốn mươi", "năm mươi", "sáu mươi", "bảy mươi", "tám mươi", "chín mươi" };
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

        public string FormatPrice(decimal price)
        {
            CultureInfo cultureInfo = new CultureInfo("vi-VN");
            return string.Format(cultureInfo, "{0:N0} ₫", price);
        }

        public void PrintOrderDetails(Order ord)
        {
            int centeredPosition = (Console.WindowWidth - "|===========================================================================================================================|".Length) / 2;
            string spaces = new string(' ', centeredPosition);
            CultureInfo cultureInfo = new CultureInfo("vi-VN");
            int printImeiHandle = 0;
            int printImeiHandle2 = 0;
            Console.WriteLine(spaces + "| {0, -16} | {1, -30} | {2, -15} | {3, -15} | {4, -15} | {5, -15} |", "Phone Detail ID", "Phone Name", "Quantity", "Imei", "Price", "Total Price");
            Console.WriteLine(spaces + "|===========================================================================================================================|");
            foreach (var pd in ord.PhoneDetails)
            {
                printImeiHandle = pd.PhoneDetailID;
                Console.WriteLine(spaces + "|---------------------------------------------------------------------------------------------------------------------------|");
                foreach (var ims in pd.ListImei)
                {
                    Console.WriteLine(spaces + "| {0, -16} | {1, -30} | {2, -15} | {3, -15} | {4, -15} | {5, 15} |", (printImeiHandle != printImeiHandle2) ? pd.PhoneDetailID : "", (printImeiHandle != printImeiHandle2) ? pd.Phone.PhoneName : "", (printImeiHandle != printImeiHandle2) ? pd.Quantity : "", ims.PhoneImei, (printImeiHandle != printImeiHandle2) ? FormatPrice(pd.Price) : "", (printImeiHandle != printImeiHandle2) ? FormatPrice(ord.GetTotalDueForEachPhone()) : "");
                    printImeiHandle2 = printImeiHandle;
                }
            }
        }

        public void PrintDiscountPolicyDetail(DiscountPolicy discountPolicy)
        {
            Console.WriteLine($"Title: {discountPolicy.Title}");
            Console.WriteLine($"FromDate: {discountPolicy.FromDate}");
            Console.WriteLine($"ToDate: {discountPolicy.ToDate}");
            if (discountPolicy.PhoneDetail.PhoneDetailID != 0)
                Console.WriteLine($"Phone Information: {discountPolicy.PhoneDetail.Phone.PhoneName} {discountPolicy.PhoneDetail.PhoneColor.Color} {discountPolicy.PhoneDetail.ROMSize.ROM}");
            if (discountPolicy.PaymentMethod != "Not Have")
                Console.WriteLine($"Apply for Paymentmethod: {discountPolicy.PaymentMethod}");
            if (discountPolicy.MinimumPurchaseAmount > 0 && discountPolicy.MaximumPurchaseAmount > 0)
            {
                Console.WriteLine($"Maximum purchase amount: {discountPolicy.MinimumPurchaseAmount}");
                Console.WriteLine($"Minimum purchase amount: {discountPolicy.MaximumPurchaseAmount}");
            }
            if (discountPolicy.DiscountPrice != 0)
                Console.WriteLine($"DiscountPrice: {discountPolicy.DiscountPrice}");
            if (discountPolicy.MoneySupported != 0)
                Console.WriteLine($"Money supported: {discountPolicy.MoneySupported}");
        }

        public void PrintPhoneModelInfo(PhoneDetail phoneDetail)
        {
            int centeredPosition = (Console.WindowWidth - "|===================================================================================================|".Length) / 2;
            string spaces = new string(' ', centeredPosition);
            CultureInfo cultureInfo = new CultureInfo("vi-VN");
            Console.WriteLine(spaces + "| {0, -10} | {1, -13} | {2, -15} | {3, -15} | {4, -15} | {5, -14} |", phoneDetail.PhoneDetailID, phoneDetail.PhoneColor.Color, phoneDetail.ROMSize.ROM, string.Format(cultureInfo, "{0:N0} ₫", phoneDetail.Price), phoneDetail.PhoneStatusType, (phoneDetail.Quantity != 0) ? phoneDetail.Quantity : "Out Of Stock");
        }

        public void PrintOrderDetailsInfo(Order order)
        {
            int centeredPosition = (Console.WindowWidth - "|======================================================================================================================|".Length) / 2;
            string spaces = new string(' ', centeredPosition);
            if (order.PhoneDetails.Count() == 0)
            {
                Console.WriteLine(spaces + "|======================================================================================================================|");
                Console.WriteLine(spaces + "| Doesnt have any phone in cart                                                                                        |");
                Console.WriteLine(spaces + "|======================================================================================================================|");
            }
            else
            {
                Console.WriteLine(spaces + "|======================================================================================================================|");
                Console.WriteLine(spaces + "| {0, -29} | {1, -11} | {2, -15} | {3, -15} | {4, -15} | {5, -16} |", "Phone Name", "Color", "RomSize", "Quantity", "Price", "Total Price");
                Console.WriteLine(spaces + "|======================================================================================================================|");

                foreach (var phone in order.PhoneDetails)
                {
                    Console.WriteLine(spaces + "| {0, -29} | {1, -11} | {2, -15} | {3, -15} | {4, -15} | {5, -16} |", phone.Phone.PhoneName, phone.PhoneColor.Color, phone.ROMSize.ROM, phone.Quantity, FormatPrice(phone.Price), FormatPrice(order.GetTotalDueForEachPhone()));
                }
                Console.WriteLine(spaces + "|======================================================================================================================|");
                Console.WriteLine(spaces + "| Total Due: {0, -50} |", SetTextBolder(FormatPrice(order.TotalDue)).PadRight(113));
                Console.WriteLine(spaces + "|======================================================================================================================|");
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

        public void Title(string? title, string? subTitle, Staff? staffLoggedIn)
        {
            int centeredPosition = (Console.WindowWidth - "|--------------------------------------------------------------------------------------------|".Length) / 2;
            string spaces = new string(' ', centeredPosition);
            if (title != null)
            {
                Line();
                Console.WriteLine(title);
                Line();
            }
            if (subTitle != null)
            {
                Line();
                Console.WriteLine(subTitle);
                Line();
            }
            if (staffLoggedIn != null)
            {
                ConsoleForegroundColor(ConsoleEnum.Color.Green);
                Console.WriteLine(spaces + "| {0, -50} |", (((staffLoggedIn.Role == StaffEnum.Role.Accountant) ? "Accountant: " : "Seller: ") + staffLoggedIn.StaffName + " - ID: " + staffLoggedIn.StaffID).PadRight(90));
                ConsoleForegroundColor(ConsoleEnum.Color.White);
            }
            Line();
        }
        public string GetAppTitle()
        {
            int centeredPosition = (Console.WindowWidth - "|--------------------------------------------------------------------------------------------|".Length) / 2;
            string spaces = new string(' ', centeredPosition);
            return $@"{spaces}|                                                                                            |
{spaces}|                             ┌─┐┬ ┬┌─┐┌┐┌┌─┐  ┌─┐┌┬┐┌─┐┬─┐┌─┐                               |
{spaces}|                             ├─┘├─┤│ ││││├┤   └─┐ │ │ │├┬┘├┤                                |
{spaces}|                             ┴  ┴ ┴└─┘┘└┘└─┘  └─┘ ┴ └─┘┴└─└─┘                               |
{spaces}|                                                                                            |";
        }

        public void GetFooterPagination(int currentPage, int countPage)
        {
            int centeredPosition = (Console.WindowWidth - "|============================================================================================|".Length) / 2;
            string spaces = new string(' ', centeredPosition);
            Console.WriteLine(spaces + "|============================================================================================|");
            Console.WriteLine(spaces + "| {0,42}" + "< " + $"{currentPage}/{countPage}" + " >".PadRight(44) + "|", " ");
            Console.WriteLine(spaces + "|============================================================================================|");
            Console.WriteLine(spaces + "| Press 'Left Arrow' To Back Previous Page, 'Right Arror' To Next Page                       |");
            Console.WriteLine(spaces + "| Press 'Space' To Choose a phone, 'B' To Back Previous Menu                                 |");
            Console.WriteLine(spaces + "|============================================================================================|");

        }

        public string GetSearchANSIText()
        {
            int centeredPosition = (Console.WindowWidth - "|--------------------------------------------------------------------------------------------|".Length) / 2;
            string spaces = new string(' ', centeredPosition);
            return $@"{spaces}|                                                                                            |
{spaces}|                                     ┌─┐┌─┐┌─┐┬─┐┌─┐┬ ┬                                     |
{spaces}|                                     └─┐├┤ ├─┤├┬┘│  ├─┤                                     |
{spaces}|                                     └─┘└─┘┴ ┴┴└─└─┘┴ ┴                                     |
{spaces}|                                                                                            |";
        }
        public string GetHandleOrderANSIText()
        {
            int centeredPosition = (Console.WindowWidth - "|--------------------------------------------------------------------------------------------|".Length) / 2;
            string spaces = new string(' ', centeredPosition);
            return $@"{spaces}|                                                                                            |
{spaces}|                          ┬ ┬┌─┐┌┐┌┌┬┐┬  ┌─┐  ┌─┐┬─┐┌┬┐┌─┐┬─┐┌─┐                            |
{spaces}|                          ├─┤├─┤│││ │││  ├┤   │ │├┬┘ ││├┤ ├┬┘└─┐                            |
{spaces}|                          ┴ ┴┴ ┴┘└┘─┴┘┴─┘└─┘  └─┘┴└──┴┘└─┘┴└─└─┘                            |
{spaces}|                                                                                            |";
        }

        public string[] GetMenuItemSearch()
        {
            return new string[] { "Search All Phone", "Search Phone By Information", "Back To Previous Menu" };
        }

        public string[] GetCreateOrderTimeLine()
        {
            return new string[] { "Search Phone", "Add Phone To Order", "Add More Phone?", "Enter Customer Info", "Confirm Order" };
        }

        public string GetCustomerInfoANSITitle()
        {
            int centeredPosition = (Console.WindowWidth - "|--------------------------------------------------------------------------------------------|".Length) / 2;
            string spaces = new string(' ', centeredPosition);
            string title =
                $@"{spaces}|                                                                                            |                                                                      
{spaces}|          ┌─┐┌┐┌┌┬┐┌─┐┬─┐  ┌─┐┬ ┬┌─┐┌┬┐┌─┐┌┬┐┌─┐┬─┐  ┬┌┐┌┌─┐┌─┐┬─┐┌┬┐┌─┐┌┬┐┬┌─┐┌┐┌          |
{spaces}|          ├┤ │││ │ ├┤ ├┬┘  │  │ │└─┐ │ │ ││││├┤ ├┬┘  ││││├┤ │ │├┬┘│││├─┤ │ ││ ││││          |
{spaces}|          └─┘┘└┘ ┴ └─┘┴└─  └─┘└─┘└─┘ ┴ └─┘┴ ┴└─┘┴└─  ┴┘└┘└  └─┘┴└─┴ ┴┴ ┴ ┴ ┴└─┘┘└┘          |
{spaces}|                                                                                            |";
            return title;
        }

        public string GetAddPhoneToOrderANSITitle()
        {
            int centeredPosition = (Console.WindowWidth - "|--------------------------------------------------------------------------------------------|".Length) / 2;
            string spaces = new string(' ', centeredPosition);
            return
            $@"{spaces}|                                                                                            |
{spaces}|                    ┌─┐┌┬┐┌┬┐  ┌─┐┬ ┬┌─┐┌┐┌┌─┐  ┌┬┐┌─┐  ┌─┐┬─┐┌┬┐┌─┐┬─┐                     |
{spaces}|                    ├─┤ ││ ││  ├─┘├─┤│ ││││├┤    │ │ │  │ │├┬┘ ││├┤ ├┬┘                     |
{spaces}|                    ┴ ┴─┴┘─┴┘  ┴  ┴ ┴└─┘┘└┘└─┘   ┴ └─┘  └─┘┴└──┴┘└─┘┴└─                     |
{spaces}|                                                                                            |";
        }
        public string GetAllOrderANSITitle()
        {
            int centeredPosition = (Console.WindowWidth - "|--------------------------------------------------------------------------------------------|".Length) / 2;
            string spaces = new string(' ', centeredPosition);
            return $@"{spaces}|                                                                                            |
{spaces}|                                ┌─┐┬  ┬    ┌─┐┬─┐┌┬┐┌─┐┬─┐┌─┐                               |
{spaces}|                                ├─┤│  │    │ │├┬┘ ││├┤ ├┬┘└─┐                               |
{spaces}|                                ┴ ┴┴─┘┴─┘  └─┘┴└──┴┘└─┘┴└─└─┘                               |
{spaces}|                                                                                            |";
        }
        public string GetLoginANSITitle()
        {
            int centeredPosition = (Console.WindowWidth - "|--------------------------------------------------------------------------------------------|".Length) / 2;
            string spaces = new string(' ', centeredPosition);
            return $@"{spaces}|                                                                                            |
{spaces}|                                       ┬  ┌─┐┌─┐┬┌┐┌                                        |
{spaces}|                                       │  │ ││ ┬││││                                        |
{spaces}|                                       ┴─┘└─┘└─┘┴┘└┘                                        |
{spaces}|                                                                                            |";
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
    }
}
