using Model;
using BL;
using BusinessEnum;
using GUIEnum;
using System.Diagnostics;
using System;
using System.Reflection.Metadata;

namespace Ults
{
    class Ultilities
    {
        private PhoneBL phoneBL = new PhoneBL();
        private ConsoleUlts ConsoleUlts = new ConsoleUlts();
        private StaffBL StaffBL = new StaffBL();
        // private CustomerBL customerBL = new CustomerBL();
        private OrderBL orderBL = new OrderBL();
        public Staff? OrderStaff = null;
        public Dictionary<int, List<Phone>> listAllPhones = null;
        int currentPageDetails = 1;
        public int MenuHandle(string? title, string? subTitle, string[] menuItem)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            ConsoleKeyInfo keyInfo;
            string iconBackhand = "👉";
            bool activeSelectedMenu = true;
            int currentChoice = 1;
            while (activeSelectedMenu)
            {
                ConsoleUlts.Title(title, subTitle);
                ShowStaffNameAndID();
                if (currentChoice <= (menuItem.Count() + 1) && currentChoice >= 1)
                {
                    for (int i = 0; i < menuItem.Count(); i++)
                        Console.WriteLine(((currentChoice - 1 == i) ? (iconBackhand + " ") : "") + " " + menuItem[i] + $" ({i + 1})");
                    Console.WriteLine("\n================================================================================================");
                    Console.WriteLine("*press 'down arrow' to next choice or 'up arrow' to previous choice and press 'enter to confirm'");
                    Console.WriteLine("================================================================================================");
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
                }
            }
            return currentChoice;
        }
        public StaffEnum.Role? LoginUlt()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            ConsoleUlts.Title(null, @"
                            ██╗      ██████╗  ██████╗ ██╗███╗   ██╗
                            ██║     ██╔═══██╗██╔════╝ ██║████╗  ██║
                            ██║     ██║   ██║██║  ███╗██║██╔██╗ ██║
                            ██║     ██║   ██║██║   ██║██║██║╚██╗██║
                            ███████╗╚██████╔╝╚██████╔╝██║██║ ╚████║
                            ╚══════╝ ╚═════╝  ╚═════╝ ╚═╝╚═╝  ╚═══╝");
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
            OrderStaff = StaffBL.Authenticate(userName, pass);
            if (OrderStaff != null) return OrderStaff.Role;
            else return null;
        }
        public void ShowLoginSuccessAlert()
        {
            Console.Clear();
            ConsoleUlts.ConsoleForegroundColor(ConsoleEnum.Color.Green);
            Console.WriteLine("-- Login Success! --------------------------------");
            ConsoleUlts.ConsoleForegroundColor(ConsoleEnum.Color.White);
            Console.WriteLine("\n   Hello, " + OrderStaff.StaffName + "!");
            PressEnterTo("Continue");
        }
        public void ShowStaffNameAndID()
        {
            if (OrderStaff != null)
            {
                ConsoleUlts.ConsoleForegroundColor(ConsoleEnum.Color.Green);
                Console.WriteLine("                                                              Staff: " + OrderStaff.StaffName + " - ID: " + OrderStaff.StaffID);
                ConsoleUlts.ConsoleForegroundColor(ConsoleEnum.Color.White);
            }
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
                switch (MenuHandle(
                    null, @"
                          ███████╗███████╗██╗     ██╗     ███████╗██████╗ 
                          ██╔════╝██╔════╝██║     ██║     ██╔════╝██╔══██╗
                          ███████╗█████╗  ██║     ██║     █████╗  ██████╔╝
                          ╚════██║██╔══╝  ██║     ██║     ██╔══╝  ██╔══██╗
                          ███████║███████╗███████╗███████╗███████╗██║  ██║
                          ╚══════╝╚══════╝╚══════╝╚══════╝╚══════╝╚═╝  ╚═╝
                                                ", menuItem))
                {
                    case 1:
                        CreateOrder();
                        break;
                    case 2:
                        HandleResult = HandleOrder();
                        if (HandleResult == -1) ConsoleUlts.Alert(GUIEnum.ConsoleEnum.Alert.Error, "No Order exist");
                        else if (HandleResult == 0) ConsoleUlts.Alert(GUIEnum.ConsoleEnum.Alert.Warning, "Cancel Order Completed");
                        else if (HandleResult == 1) ConsoleUlts.Alert(GUIEnum.ConsoleEnum.Alert.Success, "Handle Order Completed");
                        else if (HandleResult == 2) PressEnterTo("Back Previous Menu");
                        break;
                    case 3:
                        active = false; result = 1;
                        break;
                    default:
                        break;
                }
            }
            return result;
        }

        public int AccountantMenu()
        {
            int result = 0;
            bool active = true;
            Ultilities ultilities = new Ultilities();
            string[] menuItem = { "Payment", "Revenue Report", "Log Out" };
            while (active)
            {
                switch (ultilities.MenuHandle(null,
                @"
         █████╗  ██████╗ ██████╗ ██████╗ ██╗   ██╗███╗   ██╗████████╗ █████╗ ███╗   ██╗████████╗
        ██╔══██╗██╔════╝██╔════╝██╔═══██╗██║   ██║████╗  ██║╚══██╔══╝██╔══██╗████╗  ██║╚══██╔══╝
        ███████║██║     ██║     ██║   ██║██║   ██║██╔██╗ ██║   ██║   ███████║██╔██╗ ██║   ██║   
        ██╔══██║██║     ██║     ██║   ██║██║   ██║██║╚██╗██║   ██║   ██╔══██║██║╚██╗██║   ██║   
        ██║  ██║╚██████╗╚██████╗╚██████╔╝╚██████╔╝██║ ╚████║   ██║   ██║  ██║██║ ╚████║   ██║   
        ╚═╝  ╚═╝ ╚═════╝ ╚═════╝ ╚═════╝  ╚═════╝ ╚═╝  ╚═══╝   ╚═╝   ╚═╝  ╚═╝╚═╝  ╚═══╝   ╚═╝   
                                                                                        ", menuItem))
                {
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        active = false;
                        result = 1;
                        break;
                    default:
                        break;
                }
            }
            return result;
        }
        public bool ListPhonePagination(List<Phone> listPhone)
        {
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
                    Console.Clear();
                    ConsoleUlts.Title(
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
                    ShowStaffNameAndID();
                    while (active)
                    {
                        ConsoleUlts.PrintOrderAndPhoneBorder(phones, currentPage, countPage);
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
                            else if (input.Key == ConsoleKey.B) return false;
                            else if (input.Key == ConsoleKey.Spacebar) return true;
                            else Console.Clear();
                        }
                    }
                }
            }
            else
            {
                ConsoleUlts.Alert(ConsoleEnum.Alert.Warning, "Phone Not Found");
                PressEnterTo("Back To Previous Menu");
            }
            return false;
        }

        public bool? ListOrderPagination(List<Order> listOrder)
        {
            if (listOrder != null)
            {
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
                        ConsoleUlts.Title(
                            null,
                            @"
                     █████  ██      ██           ██████  ██████  ██████  ███████ ██████  ███████ 
                    ██   ██ ██      ██          ██    ██ ██   ██ ██   ██ ██      ██   ██ ██      
                    ███████ ██      ██          ██    ██ ██████  ██   ██ █████   ██████  ███████ 
                    ██   ██ ██      ██          ██    ██ ██   ██ ██   ██ ██      ██   ██      ██ 
                    ██   ██ ███████ ███████      ██████  ██   ██ ██████  ███████ ██   ██ ███████ "
                        );
                        while (active)
                        {
                            Console.WriteLine("========================================================================================================================");
                            Console.WriteLine("| {0, 10} | {1, 30} | {2, 20} | {3, 15} |", "ID", "Customer Name", "Order Date", "Status");
                            Console.WriteLine("========================================================================================================================");

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
                                return null;
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
            return false;

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
                        ███████╗███████╗ █████╗ ██████╗  ██████╗██╗  ██╗    
                        ██╔════╝██╔════╝██╔══██╗██╔══██╗██╔════╝██║  ██║
                        ███████╗█████╗  ███████║██████╔╝██║     ███████║
                        ╚════██║██╔══╝  ██╔══██║██╔══██╗██║     ██╔══██║
                        ███████║███████╗██║  ██║██║  ██║╚██████╗██║  ██║
                        ╚══════╝╚══════╝╚═╝  ╚═╝╚═╝  ╚═╝ ╚═════╝╚═╝  ╚═╝
                                                ";
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            List<PhoneDetail> Cart = new List<PhoneDetail>();
            string[] menuSearchChoice = { "Search All Phone", "Search Phone By Information", "Back To Previous Menu" };
            bool activeSearchPhone = true;
            string input = "";
            int phoneId = 0;
            int phoneModelID = 0;
            int count = 0;
            int searchChoice = 0;
            List<Imei> Imeis = new List<Imei>();
            List<int>? listAllPhonesID = new List<int>();
            bool? listPhoneSearch = false;
            List<Phone> listTemp = new List<Phone>();
            int currentPhase = 1;
            int phaseChoice = 0;
            int quantity = 0;
            string[] listPhase = { "Search Phone", "Add Phone To Cart", "Enter Quantity", "Enter Imei For Each Phone", "Add More Phone" };
            //Buoc 1: Tim va chon ra tung dien thoai muon them vao order

            do
            {
                switch (currentPhase)
                {
                    case 1:
                        do
                        {
                            ShowStaffNameAndID();
                            ConsoleUlts.PrintListPhase(listPhase, count, currentPhase);
                            searchChoice = MenuHandle(searchTitle, null, menuSearchChoice);
                            switch (searchChoice)
                            {
                                case 1:
                                    listTemp = phoneBL.GetPhonesByInformation("");
                                    break;
                                case 2:
                                    ConsoleUlts.Title(searchTitle, null);
                                    Console.Write("👌 Search Phone To Add To Cart: ");
                                    input = Console.ReadLine() ?? "";
                                    listTemp = phoneBL.GetPhonesByInformation(input);
                                    break;
                                case 3:
                                    break;
                            }
                            if (searchChoice == 3)
                            {
                                currentPhase = 6;
                                break;
                            }
                            if (listTemp.Count() == 0) activeSearchPhone = false;
                            else
                            {
                                ConsoleUlts.PrintListPhase(listPhase, count, currentPhase);
                                listPhoneSearch = ListPhonePagination(listTemp);
                                foreach (Phone item in listAllPhones[currentPageDetails])
                                {
                                    listAllPhonesID.Add(item.PhoneID);
                                }
                                Console.Write("\nEnter Phone ID To Choose Phone Model: ");
                                int.TryParse(Console.ReadLine(), out phoneId);
                                if (listAllPhonesID.IndexOf(phoneId) == -1)
                                {
                                    Console.WriteLine("\nInvalid Phone ID In This Page");
                                    ConsoleUlts.PressEnterTo("Comeback");
                                    phoneBL.GetPhonesByInformation(input);
                                    listAllPhonesID = new List<int>();
                                    currentPageDetails = 1;
                                }
                            }
                        } while (listAllPhonesID.IndexOf(phoneId) == -1);
                        currentPhase++;
                        break;
                    case 2:
                        List<PhoneDetail> phonedetails = phoneBL.GetPhoneDetailsByPhoneID(phoneId);
                        do
                        {
                            ConsoleUlts.PrintListPhase(listPhase, count, currentPhase);
                            Console.Write("Press '1' To Back Previous Phase And '2' To Continue: ");
                            int.TryParse(Console.ReadLine(), out phaseChoice);
                            if (phaseChoice == 1)
                            {
                                currentPhase--;
                                break;
                            }
                            else if (phaseChoice == 2)
                            {

                                do
                                {
                                    foreach (PhoneDetail pd in phonedetails)
                                    {
                                        ConsoleUlts.PrintPhoneDetailsInfo(pd);
                                        break;
                                    }
                                    ConsoleUlts.PrintPhoneModelTitle();
                                    foreach (PhoneDetail pd in phonedetails)
                                    {
                                        ConsoleUlts.PrintPhoneModelInfo(pd);
                                    }
                                    ConsoleUlts.TinyLine();
                                    Console.Write("Enter Phone Model ID: ");
                                    phoneModelID = Convert.ToInt32(Console.ReadLine() ?? "");
                                    if (phoneModelID <= 0 || phoneModelID > phonedetails.Count())
                                    {
                                        Console.WriteLine("\nInvalid Phone Model ID Please Choice Again");
                                        ConsoleUlts.PressEnterTo("Re-enter Phone Details ID");
                                        ConsoleUlts.PrintListPhase(listPhase, count, currentPhase);
                                    }
                                } while (phoneModelID <= 0 || phoneModelID > phonedetails.Count());
                            }
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
                                break;
                            }
                            if (phaseChoice != 1 && phaseChoice != 2)
                            {
                                ConsoleUlts.Alert(GUIEnum.ConsoleEnum.Alert.Error, "Invalid Choice");
                            }
                        } while (phaseChoice != 1 && phaseChoice != 2);

                        break;
                    case 3:
                        PhoneDetail pDetails = new PhoneBL().GetPhoneDetailByID(phoneModelID);
                        do
                        {
                            ConsoleUlts.PrintListPhase(listPhase, count, currentPhase);
                            Console.Write("Press '1' To Back Previous Phase And '2' To Continue: ");
                            int.TryParse(Console.ReadLine(), out phaseChoice);
                            if (phaseChoice == 1)
                            {
                                currentPhase--;
                                break;
                            }
                            else if (phaseChoice == 2)
                            {
                                do
                                {
                                    Console.Write("Input Quantity: ");
                                    int.TryParse(Console.ReadLine(), out quantity);
                                    if (quantity <= 0 || quantity > pDetails.Quantity)
                                        ConsoleUlts.Alert(GUIEnum.ConsoleEnum.Alert.Error, "Invalid Quantity");
                                } while ((quantity <= 0 || quantity > pDetails.Quantity) && phaseChoice != 1 && phaseChoice != 2);
                            }
                            Console.Write("Press '1' To Back Previous Phase And '2' To Create Order: ");
                            int.TryParse(Console.ReadLine(), out phaseChoice);
                            if (phaseChoice == 1)
                            {
                                currentPhase--;
                                break;
                            }
                            else if (phaseChoice == 2)
                            {
                                currentPhase++;
                                break;
                            }
                            if (phaseChoice != 1 && phaseChoice != 2)
                            {
                                Console.WriteLine("Invalid Choice");
                            }
                        } while (phaseChoice != 1 && phaseChoice != 2);
                        break;
                    case 4:
                        ConsoleUlts.PrintListPhase(listPhase, count, currentPhase);
                        Console.Write("Press '1' To Back Previous Phase And '2' To Create Order: ");
                        int.TryParse(Console.ReadLine(), out phaseChoice);
                        if (phaseChoice == 1)
                        {
                            currentPhase--;
                            break;
                        }
                        else if (phaseChoice == 2)
                        {
                            string ims = "";
                            for (var i = 0; i < quantity; i++)
                            {
                                Console.Write("Enter Imei: ");
                                ims = Console.ReadLine() ?? "";
                            }
                            // lst.Add()...
                        }
                        ConsoleUlts.PrintListPhase(listPhase, count, currentPhase);
                        Console.Write("Press '1' To Back Previous Phase And '2' To Create Order: ");
                        int.TryParse(Console.ReadLine(), out phaseChoice);
                        if (phaseChoice == 1)
                        {
                            currentPhase--;
                            break;
                        }
                        else if (phaseChoice == 2)
                        {
                            currentPhase++;
                            break;
                        }
                        break;
                    case 5:
                        ConsoleUlts.PrintListPhase(listPhase, count, currentPhase);
                        Console.Write("Press '1' To Back Previous Phase And '2' To Create Order: ");
                        int.TryParse(Console.ReadLine(), out phaseChoice);
                        if (phaseChoice == 1)
                        {
                            currentPhase--;
                            break;
                        }
                        else if (phaseChoice == 2)
                        {
                            currentPhase++;
                            break;
                        }
                        break;
                    case 6:
                        Console.WriteLine("Create Order Completed!");
                        PressEnterTo("Continue");
                        break;
                    case 7:
                        PressEnterTo("Back Previous Menu");
                        break;
                }
            } while (currentPhase != 6 && currentPhase != 7);
        }
        public void SearchPhoneMenuHandle()
        {

        }
        public int HandleOrder()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            ConsoleUlts.Title(@"
            ██╗  ██╗ █████╗ ███╗   ██╗██████╗ ██╗     ███████╗
            ██║  ██║██╔══██╗████╗  ██║██╔══██╗██║     ██╔════╝
            ███████║███████║██╔██╗ ██║██║  ██║██║     █████╗  
            ██╔══██║██╔══██║██║╚██╗██║██║  ██║██║     ██╔══╝  
            ██║  ██║██║  ██║██║ ╚████║██████╔╝███████╗███████╗
            ╚═╝  ╚═╝╚═╝  ╚═╝╚═╝  ╚═══╝╚═════╝ ╚══════╝╚══════╝
                                                  ", null);
            // Hiển thị danh sách các order có trạng thái Confirmed
            List<Order> listConfirmedOrder = new List<Order>();
            List<int> IdPattern = new List<int>();
            int orderId = 0;
            bool activeConfirmOrCancel = true;
            ConsoleKeyInfo input = new ConsoleKeyInfo();
            listConfirmedOrder = orderBL.GetOrdersInDay(OrderEnum.Status.Confirmed);
            foreach (var order in listConfirmedOrder)
            {
                IdPattern.Add(order.OrderID);
            }
            bool? temp = ListOrderPagination(listConfirmedOrder);
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

                Order order = orderBL.GetOrderById(orderId);
                Console.WriteLine(order.OrderID);
                // hiển thị thông tin của Order
                ConsoleUlts.PrintOrderDetailsInfo(order);

                // Confirm order or cancel
                while (activeConfirmOrCancel)
                {
                    ConsoleUlts.PrintOrderDetailsInfo(order);
                    Console.WriteLine("Press Y to Confirm Product or Press N to cancel Confirm");
                    input = Console.ReadKey(true);
                    if (input.Key == ConsoleKey.N)
                    {
                        activeConfirmOrCancel = false;
                        return 0;
                    }
                    else if (input.Key == ConsoleKey.Y)
                    {
                        // đổi trạng thái Order thành completed
                        if (orderBL.UpdateOrder(OrderEnum.Status.Confirmed, order) == true) return 1;
                    }
                    else Console.Clear();
                }
            }
            else if (temp == false)
            {
                return -1;
            }
            else if (temp == null)
            {
                return 2;
            }
            return 0;

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
                    Console.WriteLine($"Please choose an id in list({listofid})");
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