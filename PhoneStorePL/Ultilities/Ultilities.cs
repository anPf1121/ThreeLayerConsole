using Model;
using BL;
using BusinessEnum;
using GUIEnum;
using System.Diagnostics;
using System;
using System.Reflection.Metadata;
using System.Collections.Generic;

namespace Ults
{
    class Ultilities
    {
        private PhoneBL phoneBL = new PhoneBL();
        private ConsoleUlts ConsoleUlts = new ConsoleUlts();
        private StaffBL StaffBL = new StaffBL();
        // private CustomerBL customerBL = new CustomerBL();
        private OrderBL orderBL = new OrderBL();
        public Staff? orderStaff = null;
        public Dictionary<int, List<Phone>> listAllPhones = null;
        int currentPageDetails = 1;
        public int MenuHandle(string? title, string? subTitle, string[] menuItem)
        {
            Console.Clear();
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            ConsoleKeyInfo keyInfo;
            string iconBackhand = "👉";
            bool activeSelectedMenu = true;
            int currentChoice = 1;
            while (activeSelectedMenu)
            {
                if (title != null || subTitle != null)
                    Title(title, subTitle);
                if (currentChoice <= (menuItem.Count() + 1) && currentChoice >= 1)
                {
                    for (int i = 0; i < menuItem.Count(); i++)
                        Console.WriteLine(((currentChoice - 1 == i) ? (iconBackhand + " ") : "") + " " + menuItem[i] + $" ({i + 1})");
                    ConsoleUlts.Line();
                    keyInfo = Console.ReadKey();

                    if (keyInfo.Key == ConsoleKey.Enter)
                    {
                        Console.Clear();
                        return currentChoice;
                    }

                    if (keyInfo.Key == ConsoleKey.DownArrow || keyInfo.Key == ConsoleKey.UpArrow || keyInfo.Key == ConsoleKey.Enter)
                    {
                        if (currentChoice >= 1 && currentChoice <= menuItem.Count())
                            if (keyInfo.Key == ConsoleKey.DownArrow) currentChoice++;
                            else if (keyInfo.Key == ConsoleKey.UpArrow) currentChoice--;

                        if (currentChoice == (menuItem.Count() + 1)) currentChoice = 1;
                        else if (currentChoice == 0) currentChoice = menuItem.Count();
                        Console.Clear();
                    }
                    else
                        ConsoleUlts.Alert(ConsoleEnum.Alert.Warning, "Please press 'down arrow' to next choice or 'up arrow' to previous choice and press 'enter to confirm'");
                    Console.Clear();
                }
            }
            return currentChoice;
        }
        public bool PressYesOrNo(string yesAction, string noAction)
        {
            ConsoleKeyInfo input = new ConsoleKeyInfo();
            bool active = true;
            char ch = 'c';
            Console.Write($"Press 'Y' To {yesAction} Or 'N' To {noAction}");
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
            ConsoleKeyInfo input = new ConsoleKeyInfo();
            bool active = true;
            char ch = 'c';
            if (firstAction != null && secondAction != null && thirdAction != null)
            {
                Console.Write($" - Press 'Q' To {firstAction}\n - Press 'W' To {secondAction}\n - Press 'E' To {thirdAction}");
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
                    else if (input.Key == ConsoleKey.E)
                    {
                        Console.Clear();
                        return 2;
                    }
                } while (active);
            }
            if (firstAction != null && secondAction != null && thirdAction == null)
            {
                Console.Write($" - Press 'Q' To {firstAction}\n - Press 'W' To {secondAction}");
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
                } while (active);
            }
            return 0;

        }
        public StaffEnum.Role? LoginUlt()
        {
            Console.Clear();
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Title(GetAppTitle(), @"                                        ┬  ┌─┐┌─┐┬┌┐┌
                                        │  │ ││ ┬││││
                                        ┴─┘└─┘└─┘┴┘└┘");
            Console.Write("\n👉 User Name: ");
            string userName = Console.ReadLine() ?? "";
            Console.Write("\n👉 Password: ");
            string pass = "";
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.Backspace)
                {
                    pass += key.KeyChar;
                    if (key.Key != ConsoleKey.Enter) Console.Write("*");
                }
                else if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
                {
                    // Xóa ký tự cuối cùng trong chuỗi pass khi người dùng nhấn phím Backspace
                    pass = pass.Substring(0, pass.Length - 1);
                    Console.Write("\b \b");
                }
            }
            while (key.Key != ConsoleKey.Enter);
            pass = pass.Substring(0, pass.Length - 1);
            orderStaff = StaffBL.Authenticate(userName, pass);
            if (orderStaff != null) return orderStaff.Role;
            else return null;
        }
        public void ShowLoginSuccessAlert()
        {
            Console.Clear();
            ConsoleUlts.ConsoleForegroundColor(ConsoleEnum.Color.Green);
            Console.WriteLine("-- Login Success! --------------------------------");
            ConsoleUlts.ConsoleForegroundColor(ConsoleEnum.Color.White);
            Console.WriteLine("\n   Hello, " + orderStaff.StaffName + "!");
            PressEnterTo("Continue");
        }
        public void ShowStaffNameAndID()
        {
            if (orderStaff != null)
            {
                ConsoleUlts.ConsoleForegroundColor(ConsoleEnum.Color.Green);
                Console.WriteLine("                                                              " + ((orderStaff.Role == StaffEnum.Role.Accountant) ? "Accountant: " : "Seller: ") + orderStaff.StaffName + " - ID: " + orderStaff.StaffID);
                ConsoleUlts.ConsoleForegroundColor(ConsoleEnum.Color.White);
            }
        }
        public void Title(string? title, string? subTitle)
        {
            Ultilities ults = new Ultilities();
            if (title != null)
            {
                ConsoleUlts.Line();
                ConsoleUlts.ConsoleForegroundColor(ConsoleEnum.Color.Red);
                Console.WriteLine(title);
                ConsoleUlts.ConsoleForegroundColor(ConsoleEnum.Color.White);
                ConsoleUlts.Line();
            }
            if (subTitle != null)
            {
                ConsoleUlts.Line();
                ConsoleUlts.ConsoleForegroundColor(ConsoleEnum.Color.Blue);
                Console.WriteLine(subTitle);
                ConsoleUlts.ConsoleForegroundColor(ConsoleEnum.Color.White);
                ConsoleUlts.Line();
            }
            ShowStaffNameAndID();
            ConsoleUlts.Line();
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
                Console.Clear();
                return;
            }
            else PressEnterTo(null);
        }
        public int SellerMenu()
        {

            int result = 0;
            bool active = true;
            string[] menuItem = { "Create Order", "Handle Order", "Log Out" };
            int HandleResult = 0;
            while (active)
            {
                switch (MenuHandle(GetAppTitle(), null, menuItem))
                {
                    case 1:
                        CreateOrder();
                        break;
                    case 2:
                        HandleResult = HandleOrder();
                        if (HandleResult == -1) ConsoleUlts.Alert(GUIEnum.ConsoleEnum.Alert.Error, "No Order exist");
                        else if (HandleResult == 0) ConsoleUlts.Alert(GUIEnum.ConsoleEnum.Alert.Warning, "Cancel Order Completed");
                        else if (HandleResult == 1) ConsoleUlts.Alert(GUIEnum.ConsoleEnum.Alert.Success, "Handle Order Completed");
                        break;
                    case 3:
                        active = false; result = 1; orderStaff = null;
                        break;
                    default:
                        break;
                }
            }
            return result;
        }
        public string GetAppTitle()
        {
            return @"
                              ┌─┐┬ ┬┌─┐┌┐┌┌─┐  ┌─┐┌┬┐┌─┐┬─┐┌─┐
                              ├─┘├─┤│ ││││├┤   └─┐ │ │ │├┬┘├┤ 
                              ┴  ┴ ┴└─┘┘└┘└─┘  └─┘ ┴ └─┘┴└─└─┘
                              ";
        }
        public int AccountantMenu()
        {
            int result = 0;
            bool active = true;
            Ultilities ultilities = new Ultilities();
            string[] menuItem = { "Payment", "Revenue Report", "Log Out" };
            while (active)
            {
                switch (ultilities.MenuHandle(GetAppTitle(), null, menuItem))
                {
                    case 1:
                        Payment();
                        break;
                    case 2:
                        break;
                    case 3:
                        active = false; result = 1; orderStaff = null;
                        break;
                    default:
                        break;
                }
            }
            return result;
        }
        public bool ListPhonePagination(List<Phone> listPhone, string[] phases, int itemCount, int currentPhase)
        {
            string title = @"
                        ┌─┐┌┬┐┌┬┐  ┌─┐┬ ┬┌─┐┌┐┌┌─┐  ┌┬┐┌─┐  ┌─┐┬─┐┌┬┐┌─┐┬─┐
                        ├─┤ ││ ││  ├─┘├─┤│ ││││├┤    │ │ │  │ │├┬┘ ││├┤ ├┬┘
                        ┴ ┴─┴┘─┴┘  ┴  ┴ ┴└─┘┘└┘└─┘   ┴ └─┘  └─┘┴└──┴┘└─┘┴└─
                        ";
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
                    Title(null, title);
                    while (active)
                    {
                        Console.Clear();
                        ConsoleUlts.PrintListPhase(phases, itemCount, currentPhase);
                        Title(null, title);
                        ConsoleUlts.PrintPhoneBorderLine();
                        foreach (Phone phone in phones[currentPage])
                        {
                            ConsoleUlts.PrintPhoneInfo(phone);
                        }
                        Console.WriteLine("================================================================================================");
                        Console.WriteLine("{0,45}" + "< " + $"{currentPage}/{countPage}" + " >", " ");
                        Console.WriteLine("================================================================================================");
                        Console.WriteLine("Press 'Left Arrow' To Back Previous Page, 'Right Arror' To Next Page");
                        Console.Write("Press 'Space' To Choose a phone, 'B' To Back Previous Menu");
                        input = Console.ReadKey();
                        if (currentPage <= countPage)
                        {
                            if (input.Key == ConsoleKey.RightArrow)
                            {
                                if (currentPage <= countPage - 1) currentPage++;
                                currentPageDetails = currentPage;
                                Console.Clear();
                            }
                            else if (input.Key == ConsoleKey.LeftArrow)
                            {
                                if (currentPage > 1) currentPage--;
                                currentPageDetails = currentPage;
                                Console.Clear();
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
                            else Console.Clear();
                        }
                    }
                    Console.Clear();
                }
            }
            else
            {
                ConsoleUlts.Alert(ConsoleEnum.Alert.Warning, "Phone Not Found");
                PressEnterTo("Back To Previous Menu");
            }
            return false;
        }

        public bool? ListOrderPagination(List<Order> listOrder, string[] phases, int itemCount, int currentPhase)
        {
            if (listOrder != null)
            {
                string title = @"
                            ┌─┐┬  ┬    ┌─┐┬─┐┌┬┐┌─┐┬─┐┌─┐
                            ├─┤│  │    │ │├┬┘ ││├┤ ├┬┘└─┐
                            ┴ ┴┴─┘┴─┘  └─┘┴└──┴┘└─┘┴└─└─┘
                            ";
                bool active = true;
                Dictionary<int, List<Order>> orders = new Dictionary<int, List<Order>>();
                orders = OrderMenuPaginationHandle(listOrder);
                if (orders != null && orders.Count > 0)
                {
                    int countPage = orders.Count(), currentPage = 1;
                    ConsoleKeyInfo input = new ConsoleKeyInfo();
                    while (true)
                    {
                        Console.Clear();
                        Title(null, title);
                        while (active)
                        {
                            ConsoleUlts.PrintListPhase(phases, itemCount, currentPhase);
                            Title(null, title);
                            ConsoleUlts.PrintOrderBorderLine();
                            foreach (Order order in orders[currentPage])
                            {
                                ConsoleUlts.PrintOrderInfo(order);
                            }
                            Console.WriteLine("========================================================================================================================");
                            Console.WriteLine("{0,55}" + "< " + $"{currentPage}/{countPage}" + " >", " ");
                            Console.WriteLine("========================================================================================================================");
                            Console.WriteLine("Press 'Left Arrow' To Back Previous Page, 'Right Arrow' To Next Page");
                            Console.Write("Press 'Space' To View Order Details, 'B' To Back Previous Menu");
                            input = Console.ReadKey(true);
                            if (input.Key == ConsoleKey.RightArrow)
                            {
                                if (currentPage <= countPage - 1) currentPage++;
                                Console.Clear();
                            }
                            else if (input.Key == ConsoleKey.LeftArrow)
                            {
                                if (currentPage > 1) currentPage--;
                                Console.Clear();
                            }

                            else if (input.Key == ConsoleKey.B)
                            {
                                return false;
                            }
                            else if (input.Key == ConsoleKey.Spacebar)
                            {
                                return true;
                            }
                            else
                                Console.Clear();

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
            int phoneQuantity = phoneList.Count(), itemInTab = 4, numberOfTab = 0, count = 1, secondCount = 1, idTab = 0;

            if (phoneQuantity % itemInTab != 0) numberOfTab = (phoneQuantity / itemInTab) + 1;
            else numberOfTab = phoneQuantity / itemInTab;

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
            int orderQuantity = orderList.Count(), itemInTab = 4, numberOfTab = 0, count = 1, secondCount = 1, idTab = 0;

            if (orderQuantity % itemInTab != 0) numberOfTab = (orderQuantity / itemInTab) + 1;
            else numberOfTab = orderQuantity / itemInTab;

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

        public void CreateOrder()
        {
            string searchTitle = @"
                                      ┌─┐┌─┐┌─┐┬─┐┌─┐┬ ┬
                                      └─┐├┤ ├─┤├┬┘│  ├─┤
                                      └─┘└─┘┴ ┴┴└─└─┘┴ ┴
                                      ";
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            List<PhoneDetail> cart = new List<PhoneDetail>();
            string[] menuSearchChoice = { "Search All Phone", "Search Phone By Information", "Back To Previous Menu" };
            bool activeSearchPhone = true;
            string input = "";
            int phoneId = 0;
            int phoneModelID = 0;
            int count = 0;
            int searchChoice = 0;
            List<Imei>? imeis = null;
            List<int>? listAllPhonesID = new List<int>();
            bool listPhoneSearch = false;
            List<Phone> listTemp = new List<Phone>();
            List<PhoneDetail> phonedetails = new List<PhoneDetail>();
            PhoneDetail? pDetails = null;
            Customer? customer = null;
            int currentPhase = 1;
            int phaseChoice = 0;
            int quantity = 0;
            string[] listPhase = { "Search Phone", "Add Phone To Order", "Enter Quantity", "Enter Imei", "Add More Phone", "Enter Customer Info", "Confirm Order" };
            //Buoc 1: Tim va chon ra tung dien thoai muon them vao order

            do
            {
                switch (currentPhase)
                {
                    case 1:
                        ConsoleUlts.PrintListPhase(listPhase, count, currentPhase);
                        Title(GetAppTitle(), searchTitle);
                        searchChoice = PressCharacterTo("Search All Phone", "Search Phone By Information", "Back To Previous Menu");
                        switch (searchChoice)
                        {
                            case 0:
                                listTemp = phoneBL.GetPhonesByInformation("");
                                break;
                            case 1:
                                ConsoleUlts.PrintListPhase(listPhase, count, currentPhase);
                                Title(GetAppTitle(), searchTitle);
                                Console.Write("👌 Search For A Phone To Add To The Cart: ");
                                input = Console.ReadLine() ?? "";
                                listTemp = phoneBL.GetPhonesByInformation(input);
                                break;
                            case 2:
                                break;
                        }
                        if (searchChoice == 2) return;
                        if (listTemp.Count() == 0) activeSearchPhone = false;
                        else
                        {
                            do
                            {
                                listPhoneSearch = ListPhonePagination(listTemp, listPhase, count, currentPhase);
                                if (listPhoneSearch)
                                {
                                    Console.Write("\nEnter Phone ID To View Details: ");
                                    int.TryParse(Console.ReadLine(), out phoneId);
                                    if (phoneId <= 0 || phoneId > phoneBL.GetAllPhone().Count())
                                    {
                                        Console.WriteLine("\nInvalid Phone ID In This Page");
                                        ConsoleUlts.PressEnterTo("Comeback");
                                        phoneBL.GetPhonesByInformation(input);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                else break;
                            } while (phoneId <= 0 || phoneId > phoneBL.GetAllPhone().Count());
                        }
                        if (!listPhoneSearch) break;
                        if (!PressYesOrNo("Back Previous Phase", "Choose Phone Model"))
                            currentPhase++;
                        break;
                    case 2:
                        phonedetails = phoneBL.GetPhoneDetailsByPhoneID(phoneId);
                        ConsoleUlts.PrintListPhase(listPhase, count, currentPhase);
                        List<int> listPhoneDetailID = new List<int>();
                        foreach (PhoneDetail item in phonedetails)
                        {
                            listPhoneDetailID.Add(item.PhoneDetailID);
                        }
                        do
                        {
                            ConsoleUlts.PrintPhoneDetailsInfo(phonedetails);

                            Console.Write("Enter Phone Model ID: ");
                            int.TryParse(Console.ReadLine(), out phoneModelID);

                            if (listPhoneDetailID.IndexOf(phoneModelID) == -1)
                            {
                                ConsoleUlts.Alert(GUIEnum.ConsoleEnum.Alert.Error, "Invalid Phone Model ID Please Choice Again");
                                ConsoleUlts.PrintListPhase(listPhase, count, currentPhase);
                            }
                        } while (listPhoneDetailID.IndexOf(phoneModelID) == -1);
                        if (phoneBL.GetPhoneDetailByID(phoneModelID).Quantity == 0)
                        {
                            currentPhase--;
                            ConsoleUlts.Alert(GUIEnum.ConsoleEnum.Alert.Warning, "This Phone Model Is Out Of Stock");
                            break;
                        }
                        if (PressYesOrNo("Back Previous Phase", "Enter Quantity"))
                            currentPhase--;
                        else
                            currentPhase++;
                        break;
                    case 3:
                        pDetails = new PhoneBL().GetPhoneDetailByID(phoneModelID);
                        ConsoleUlts.PrintListPhase(listPhase, count, currentPhase);
                        do
                        {
                            Console.Clear();
                            ConsoleUlts.PrintPhoneDetailsInfo(phonedetails);
                            Console.WriteLine("Phone Model ID: " + phoneModelID);
                            Console.Write("Input Quantity: ");
                            int.TryParse(Console.ReadLine(), out quantity);
                            if (quantity <= 0 || quantity > pDetails.Quantity)
                            {
                                ConsoleUlts.Alert(GUIEnum.ConsoleEnum.Alert.Error, "Invalid Quantity");
                            }
                            else
                            {
                                ConsoleUlts.Alert(GUIEnum.ConsoleEnum.Alert.Success, "Quantity Successfully Added");
                                pDetails.Quantity = quantity;
                            }
                        } while ((quantity <= 0 || quantity > pDetails.Quantity));

                        if (PressYesOrNo("Back Previous Phase", "Enter Imeis"))
                            currentPhase--;
                        else
                            currentPhase++;
                        break;
                    case 4:
                        imeis = new List<Imei>();
                        bool isDuplicateImei = false;
                        do
                        {
                            ConsoleUlts.PrintListPhase(listPhase, count, currentPhase);
                            int idImei = 1;
                            ConsoleUlts.PrintPhoneDetailsInfo(phonedetails);
                            Console.WriteLine("Phone Model ID: " + phoneModelID);
                            Console.WriteLine("Quantity: " + quantity);
                            foreach (var item in imeis)
                            {
                                Console.WriteLine("Imei " + (idImei) + ": " + item.PhoneImei);
                                idImei++;
                            }
                            for (int i = 0; i < quantity; i++)
                            {
                                Imei imei = new Imei("", BusinessEnum.PhoneEnum.ImeiStatus.NotExport);
                                do
                                {
                                    isDuplicateImei = false;
                                    Console.Write($"Enter Imei {i + 1}: ");
                                    imei.PhoneImei = Console.ReadLine() ?? "";
                                    foreach (PhoneDetail itemInCart in cart)
                                    {
                                        foreach (Imei j in itemInCart.ListImei)
                                        {
                                            if (j.PhoneImei == imei.PhoneImei)
                                            {
                                                isDuplicateImei = true;
                                            }
                                        }
                                    }
                                    if (!phoneBL.CheckImeiExist(imei, phoneModelID) || isDuplicateImei)
                                    {
                                        if (!phoneBL.CheckImeiExist(imei, phoneModelID))
                                            ConsoleUlts.Alert(GUIEnum.ConsoleEnum.Alert.Error, "Imei Not Found");
                                        else if (isDuplicateImei)
                                            ConsoleUlts.Alert(GUIEnum.ConsoleEnum.Alert.Error, "You Just Entered This Imei Before, Please Re-enter It");
                                        ConsoleUlts.PrintPhoneDetailsInfo(phonedetails);
                                        Console.WriteLine("Phone Model ID: " + phoneModelID);
                                        Console.WriteLine("Quantity: " + quantity);
                                        idImei = 1;
                                        foreach (var item in imeis)
                                        {
                                            Console.WriteLine("Imei " + (idImei) + ": " + item.PhoneImei);
                                            idImei++;
                                        }
                                    }
                                    else
                                    {
                                        ConsoleUlts.Alert(GUIEnum.ConsoleEnum.Alert.Success, "Imei Successfully Added");
                                        imeis.Add(imei);
                                    }
                                } while (!phoneBL.CheckImeiExist(imei, phoneModelID) || isDuplicateImei);
                            }
                            if (PressYesOrNo("Back Previous Phase", "Continue"))
                                currentPhase--;
                            else
                                currentPhase++;
                            break;
                        } while (phaseChoice != 1 && phaseChoice != 2);
                        break;
                    case 5:
                        bool isDuplicate = false;
                        pDetails.ListImei = imeis;
                        foreach (PhoneDetail pd in cart)
                        {
                            if (pDetails.PhoneDetailID == pd.PhoneDetailID)
                            {
                                pd.Quantity += pDetails.Quantity;
                                pd.ListImei.AddRange(imeis);
                                isDuplicate = true;
                            }
                        }
                        if (!isDuplicate)
                            cart.Add(pDetails);

                        Order ord = new Order(0, DateTime.Now, orderStaff, new Staff(0, "", "", "", "", "", StaffEnum.Role.Accountant, StaffEnum.Status.Active), new Customer(0, "", "", ""), cart, OrderEnum.Status.Pending, new List<DiscountPolicy>(), "", 0);
                        ConsoleUlts.PrintListPhase(listPhase, count, currentPhase);
                        Console.WriteLine("=======================================================================================================================");
                        Console.WriteLine("                                               LIST PHONE IN ORDER");
                        ConsoleUlts.PrintOrderDetails(ord);
                        ConsoleUlts.Line();
                        phaseChoice = PressCharacterTo("Back Previous Phase", "Enter Customer Info", "Add More Phone");
                        if (phaseChoice == 0)
                            currentPhase--;
                        else if (phaseChoice == 1)
                            currentPhase++;
                        else if (phaseChoice == 2)
                            currentPhase = 1;
                        break;
                    case 6:
                        ConsoleUlts.PrintListPhase(listPhase, count, currentPhase);
                        Title(GetAppTitle(), @"                    
            ┌─┐┌┐┌┌┬┐┌─┐┬─┐  ┌─┐┬ ┬┌─┐┌┬┐┌─┐┌┬┐┌─┐┬─┐  ┬┌┐┌┌─┐┌─┐┬─┐┌┬┐┌─┐┌┬┐┬┌─┐┌┐┌
            ├┤ │││ │ ├┤ ├┬┘  │  │ │└─┐ │ │ ││││├┤ ├┬┘  ││││├┤ │ │├┬┘│││├─┤ │ ││ ││││
            └─┘┘└┘ ┴ └─┘┴└─  └─┘└─┘└─┘ ┴ └─┘┴ ┴└─┘┴└─  ┴┘└┘└  └─┘┴└─┴ ┴┴ ┴ ┴ ┴└─┘┘└┘");
                        Console.Write(" Customer Name: ");
                        string customerName = Console.ReadLine() ?? "";
                        Console.Write(" Phone Number: ");
                        string phoneNumber = Console.ReadLine() ?? "";
                        Console.Write(" Address: ");
                        string address = Console.ReadLine() ?? "";
                        customer = new Customer(0, customerName, phoneNumber, address);
                        if (PressYesOrNo("Back Previous Phase", "Confirm Order"))
                            currentPhase--;
                        else
                            currentPhase++;
                        break;
                    case 7:
                        ConsoleUlts.PrintListPhase(listPhase, count, currentPhase);
                        Order order = new Order(0, DateTime.Now, orderStaff, new Staff(0, "", "", "", "", "", StaffEnum.Role.Accountant, StaffEnum.Status.Active), customer, cart, OrderEnum.Status.Pending, new List<DiscountPolicy>(), "", 0);

                        ConsoleUlts.PrintSellerOrder(order);
                        if (PressYesOrNo("Create Order", "Cancel Order"))
                        {
                            orderBL.CreateOrder(order);
                            ConsoleUlts.Alert(GUIEnum.ConsoleEnum.Alert.Success, "Create Order Completed!");
                        }
                        else
                            ConsoleUlts.Alert(GUIEnum.ConsoleEnum.Alert.Success, "Cancel Order Completed!");

                        currentPhase++;
                        break;
                }
            } while (currentPhase != 8);
        }
        public void Payment()
        {
            List<Order> ListOrderPending = new OrderBL().GetOrdersPendingInday();
            string input = "";
            int count = 0;
            int currentPhase = 1;
            string[] listPhase = { "Choose an Order", "Choose Paymentmethod", "Choose DiscountPolicy for Paymentmethod", "Choose DiscountPolicy for Order", "Confirm or Cancel Payment" };
            List<DiscountPolicy> ListDiscountPolicyValidToOrder = new List<DiscountPolicy>();
            List<int> choicePattern = new List<int>();
            Dictionary<int, string> ListPaymentMethod = new Dictionary<int, string>();
            ListPaymentMethod.Add(1, "VNPay");
            ListPaymentMethod.Add(2, "Banking");
            ListPaymentMethod.Add(3, "Cash");
            int choice = 0;
            ConsoleKeyInfo keyInfo = new ConsoleKeyInfo();

            bool dontKnowHowtoCall = true;
            do
            {
                currentPhase = 1;
                bool dontKnowHowtoCall1 = false;
                bool? showOrderList = ListOrderPagination(ListOrderPending, listPhase, count, currentPhase);
                Console.WriteLine(showOrderList);
                if (showOrderList == null)
                {
                    Console.WriteLine("Doesnt have any Order in Pending status in day!");
                    Console.WriteLine("Press any key to back to previous menu");
                    Console.ReadKey();
                    break;
                }
                else if (showOrderList == true)
                {
                    Console.WriteLine();
                    Console.Write("Choose an order id to Payment: ");
                    foreach (var order in ListOrderPending)
                    {
                        choicePattern.Add(order.OrderID);
                    }
                    input = Console.ReadLine() ?? "";
                    while (!CheckInputIDValid(input, choicePattern))
                    {
                        Console.Write("Choose again: ");
                        input = Console.ReadLine() ?? "";
                    }
                    choice = Convert.ToInt32(input);
                    Order orderWantToPayment = new OrderBL().GetOrderById(choice);
                    //Wait to display orderdetail
                    ConsoleUlts.PrintOrderDetailsInfo(orderWantToPayment);
                    if (orderWantToPayment.PhoneDetails.Count() == 0)
                    {
                        Console.WriteLine("Cant Payment! This Order doesnt have any phone!");
                        Console.WriteLine("Press any key to back to previous menu");
                        Console.ReadKey();
                        break;
                    }
                    Console.Write("Press Enter to keep doing payment OR Any Key to choose order again.");
                    keyInfo = Console.ReadKey(true);
                    if (keyInfo.Key == ConsoleKey.Enter)
                    {
                        dontKnowHowtoCall1 = true;
                    }
                    else
                    {
                        continue;
                    }
                    if (dontKnowHowtoCall1 == true)
                    {
                        choicePattern = new List<int>();
                        bool dontKnowHowtoCall2 = false;
                        do
                        {
                            currentPhase = 2;
                            ConsoleUlts.PrintListPhase(listPhase, count, currentPhase);
                            Console.WriteLine("Choose a payment method");
                            foreach (var payment in ListPaymentMethod)
                            {
                                Console.WriteLine(payment.Key + ". " + payment.Value);
                                choicePattern.Add(payment.Key);
                            }
                            Console.Write("Your choice: ");
                            input = Console.ReadLine() ?? "";
                            while (!CheckInputIDValid(input, choicePattern))
                            {
                                Console.Write("Choose again: ");
                                input = Console.ReadLine() ?? "";
                            }
                            choice = Convert.ToInt32(input);
                            foreach (var payment in ListPaymentMethod)
                            {
                                if (payment.Key == choice) orderWantToPayment.PaymentMethod = payment.Value;
                            }
                            ListDiscountPolicyValidToOrder = new DiscountPolicyBL().GetDiscountValidToOrder(orderWantToPayment);
                            Console.Write("Press Enter to finish choose PaymentMethod OR Any Key to choose PaymentMethod again.");
                            keyInfo = Console.ReadKey(true);
                            if (keyInfo.Key == ConsoleKey.Enter)
                            {
                                dontKnowHowtoCall2 = true;
                            }
                            else
                            {
                                continue;
                            }
                            if (dontKnowHowtoCall2 == true)
                            {
                                choicePattern = new List<int>();
                                bool dontKnowHowtoCall3 = false;
                                do
                                {
                                    Console.WriteLine();
                                    currentPhase = 3;
                                    ConsoleUlts.PrintListPhase(listPhase, count, currentPhase);
                                    Console.WriteLine("Choose discount policy for PaymentMethod");
                                    foreach (var discount in ListDiscountPolicyValidToOrder)
                                    {
                                        if (orderWantToPayment.PaymentMethod.Equals(discount.PaymentMethod))
                                        {
                                            choicePattern.Add(discount.PolicyID);
                                            Console.WriteLine(discount.PolicyID + ". " + discount.Title);
                                        }

                                    }
                                    if (choicePattern.Count() != 0)
                                    {
                                        Console.Write("Your choice: ");
                                        input = Console.ReadLine() ?? "";
                                        while (!CheckInputIDValid(input, choicePattern))
                                        {
                                            Console.Write("Choose again: ");
                                            input = Console.ReadLine() ?? "";
                                        }
                                        choice = Convert.ToInt32(input);
                                        Console.WriteLine("Show Discount Policy Detail");
                                        ConsoleUlts.PrintDiscountPolicyDetail(new DiscountPolicyBL().GetDiscountPolicyByID(choice));
                                    }
                                    else
                                    {
                                        Console.WriteLine("Doesnt have any discount policy valid to this Payment method !");
                                    }
                                    Console.WriteLine("Press Enter to finish choose discount policy OR Any key to choose again.");
                                    keyInfo = Console.ReadKey(true);
                                    if (keyInfo.Key == ConsoleKey.Enter)
                                    {
                                        dontKnowHowtoCall3 = true;
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                    if (dontKnowHowtoCall3 == true)
                                    {
                                        orderWantToPayment.DiscountPolicies.Add(new DiscountPolicyBL().GetDiscountPolicyByID(choice));
                                        choicePattern = new List<int>();
                                        bool dontKnowHowtoCall4 = false;
                                        do
                                        {
                                            currentPhase = 4;
                                            ConsoleUlts.PrintListPhase(listPhase, count, currentPhase);
                                            Console.WriteLine("Choose discount Policy for order");
                                            foreach (var discount in ListDiscountPolicyValidToOrder)
                                            {
                                                if (discount.MinimumPurchaseAmount > 0)
                                                {
                                                    if (orderWantToPayment.TotalDue > discount.MinimumPurchaseAmount && discount.PaymentMethod == "Not Have")
                                                    {
                                                        Console.WriteLine(discount.PolicyID + ". " + discount.Title);
                                                        choicePattern.Add(discount.PolicyID);
                                                    }
                                                }
                                            }
                                            if (choicePattern.Count() != 0)
                                            {
                                                Console.Write("Your choice: ");
                                                input = Console.ReadLine() ?? "";
                                                while (!CheckInputIDValid(input, choicePattern))
                                                {
                                                    Console.Write("Choose again: ");
                                                    input = Console.ReadLine() ?? "";
                                                }
                                                choice = Convert.ToInt32(input);
                                                Console.WriteLine("Show Discount Policy Detail");
                                                ConsoleUlts.PrintDiscountPolicyDetail(new DiscountPolicyBL().GetDiscountPolicyByID(choice));
                                                orderWantToPayment.DiscountPolicies.Add(new DiscountPolicyBL().GetDiscountPolicyByID(choice));
                                            }
                                            else
                                            {
                                                Console.WriteLine("Doesnt have any discount policy valid to this order !");
                                            }
                                            Console.WriteLine("Press Enter to finish choose discount policy OR Any key to choose again.");
                                            if (keyInfo.Key == ConsoleKey.Enter)
                                            {
                                                dontKnowHowtoCall4 = true;
                                            }
                                            else
                                            {
                                                continue;
                                            }
                                            if (dontKnowHowtoCall4 == true)
                                            {
                                                currentPhase = 5;
                                                ConsoleUlts.PrintListPhase(listPhase, count, currentPhase);
                                                ConsoleUlts.PrintOrderDetailsInfo(orderWantToPayment);
                                                Console.WriteLine("Press Enter to Confirm order OR Any key to Cancel order.");
                                                if (keyInfo.Key == ConsoleKey.Enter)
                                                {
                                                    orderWantToPayment.Accountant = this.orderStaff;
                                                    orderBL.Payment(orderWantToPayment);
                                                    Console.WriteLine("Executing Payment...");
                                                    System.Threading.Thread.Sleep(3000);
                                                    Console.WriteLine("Payment Completed! Press Any Key to Back to previous Menu");
                                                    Console.ReadKey();

                                                }
                                                else
                                                {
                                                    orderWantToPayment.Accountant = this.orderStaff;
                                                    orderBL.CancelPayment(orderWantToPayment);
                                                    Console.WriteLine("Executing...");
                                                    System.Threading.Thread.Sleep(3000);
                                                    Console.WriteLine("Cancel Completed !Press Any Key to Back to previous Menu");
                                                    Console.ReadKey();
                                                }
                                            }
                                        } while (dontKnowHowtoCall4 == false);
                                    }
                                } while (dontKnowHowtoCall3 == false);

                            }
                        } while (dontKnowHowtoCall2 == false);
                    }
                }
                else break;
            } while (dontKnowHowtoCall == false);
        }
        public int HandleOrder()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            string handleTitle = @"
                            ┬ ┬┌─┐┌┐┌┌┬┐┬  ┌─┐  ┌─┐┬─┐┌┬┐┌─┐┬─┐┌─┐
                            ├─┤├─┤│││ │││  ├┤   │ │├┬┘ ││├┤ ├┬┘└─┐
                            ┴ ┴┴ ┴┘└┘─┴┘┴─┘└─┘  └─┘┴└──┴┘└─┘┴└─└─┘
";
            string[] listPhase = { "Show orders", "Show order details", "Confirm Handle" };
            int currentPhase = 1;
            int phaseChoice = 0;
            int count = 0;
            int handleChoice = 0;
            bool activeHandleOrder = true;
            bool activeConfirmOrCancel = true;
            // danh sách chứa tạm các order lấy được trong database
            List<Order> listOrderTemp = new List<Order>();
            // danh sách chứa các id để check id
            List<int> IdPattern = new List<int>();
            ConsoleKeyInfo input = new ConsoleKeyInfo();
            int orderId = 0;
            Order orderdetails = new Order();
            bool? temp = false;
            do
            {
                switch (currentPhase)
                {
                    case 1:
                        ConsoleUlts.PrintListPhase(listPhase, count, currentPhase);
                        Title(GetAppTitle(), handleTitle);
                        handleChoice = PressCharacterTo("Show orders have confirmed status in day", "Back To Previous Menu", null);
                        switch (handleChoice)
                        {
                            case 0:
                                listOrderTemp = orderBL.GetOrdersInDay(OrderEnum.Status.Confirmed);
                                break;
                            case 1:
                                break;
                        }
                        if (handleChoice == 1) return 2;

                        foreach (var item in listOrderTemp)
                        {
                            IdPattern.Add(item.OrderID);
                        }
                        ConsoleUlts.PrintListPhase(listPhase, count, currentPhase);
                        temp = ListOrderPagination(listOrderTemp, listPhase, count, currentPhase);
                        if (temp == true)
                        {
                            // nhập Id order để xem
                            Console.Write("\n👉 Enter Order ID:");
                            string inputOrderId = Console.ReadLine() ?? "";
                            while (!CheckInputIDValid(inputOrderId, IdPattern))
                            {
                                Console.Write("👉 Enter Order ID:");
                                inputOrderId = Console.ReadLine() ?? "";
                            }
                            orderId = Convert.ToInt32(inputOrderId);

                        }
                        else if (temp == false)
                        {
                            break;
                        }
                        else if (temp == null)
                        {
                            return -1;
                        }
                        currentPhase++;
                        break;
                    case 2:
                        Order order = orderBL.GetOrderById(orderId);
                        orderdetails = order;
                        do
                        {
                            ConsoleUlts.PrintListPhase(listPhase, count, currentPhase);
                            ConsoleUlts.PrintOrderDetailsInfo(order);
                            Console.Write("Press '1' To Back Previous Phase And '2' To Continue: ");
                            int.TryParse(Console.ReadLine(), out phaseChoice);
                            if (phaseChoice == 1)
                            {
                                currentPhase--;
                                break;
                            }
                            else if (phaseChoice == 2)
                            {
                                currentPhase++;
                            }
                            else ConsoleUlts.Alert(GUIEnum.ConsoleEnum.Alert.Error, "Invalid Phase Choice");
                        } while (phaseChoice != 1 && phaseChoice != 2);
                        break;
                    case 3:
                        while (activeConfirmOrCancel)
                        {
                            ConsoleUlts.PrintListPhase(listPhase, count, currentPhase);
                            ConsoleUlts.PrintOrderDetailsInfo(orderdetails);
                            Console.WriteLine("Press Y to Confirm Product or Press N to cancel Confirm");
                            input = Console.ReadKey(true);
                            if (input.Key == ConsoleKey.N)
                            {
                                activeConfirmOrCancel = false;
                                if (orderBL.UpdateOrder(OrderEnum.Status.Canceled, orderdetails) == true)
                                {
                                    currentPhase++;
                                    return 0;
                                }
                            }
                            else if (input.Key == ConsoleKey.Y)
                            {
                                // đổi trạng thái Order thành completed
                                if (orderBL.UpdateOrder(OrderEnum.Status.Confirmed, orderdetails) == true)
                                {
                                    currentPhase++;
                                    return 1;
                                }
                            }
                            else ConsoleUlts.Alert(GUIEnum.ConsoleEnum.Alert.Error, "Invalid Choice");
                        }
                        break;
                }
            } while (currentPhase != 4);
            return 1;
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

    }

}