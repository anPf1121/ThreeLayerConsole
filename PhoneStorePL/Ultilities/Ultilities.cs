using Model;
using BL;
using BusinessEnum;
using GUIEnum;
using System.Diagnostics;
using System;

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
                Dictionary<int, List<Order>> orders = OrderMenuPaginationHandle(listOrder);
                int countPage = orders.Count(), currentPage = 1;
                ConsoleKeyInfo input = new ConsoleKeyInfo();
                while (true)
                {
                    ConsoleUlts.Title(
                        null,
                        @"
                     █████  ██      ██           ██████  ██████  ██████  ███████ ██████  ███████ 
                    ██   ██ ██      ██          ██    ██ ██   ██ ██   ██ ██      ██   ██ ██      
                    ███████ ██      ██          ██    ██ ██████  ██   ██ █████   ██████  ███████ 
                    ██   ██ ██      ██          ██    ██ ██   ██ ██   ██ ██      ██   ██      ██ 
                    ██   ██ ███████ ███████      ██████  ██   ██ ██████  ███████ ██   ██ ███████ "
                    );
                    ShowStaffNameAndID();
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
                        input = Console.ReadKey();
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
            else
                ConsoleUlts.Alert(ConsoleEnum.Alert.Warning, "No orders exist");
            PressEnterTo("Back To Previous Menu");
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
        public void GreenText(string text)
        {
            ConsoleUlts.ConsoleForegroundColor(ConsoleEnum.Color.Green);
            Console.Write(text);
            ConsoleUlts.ConsoleForegroundColor(ConsoleEnum.Color.White);
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
            int phonedetailId = 0;
            int count = 0;
            int searchChoice = 0;
            List<Imei> Imeis = new List<Imei>();
            List<int> listPhoneDetailsID = new List<int>();
            List<int>? listAllPhonesID = new List<int>();
            bool? listPhoneSearch = false;
            List<Phone> listTemp = new List<Phone>();
            int currentPhase = 1;
            int phaseChoice = 0;
            string[] listPhase = { "Search Phone", "View Phone Details", "Choose Phone Model", "Enter Quantity", "Enter Imei For Each Phone", "Add More Phone?" };
            //Buoc 1: Tim va chon ra tung dien thoai muon them vao order
            do
            {
                do
                {
                    switch (currentPhase)
                    {
                        case 1:
                            do
                            {
                                ShowStaffNameAndID();
                                ConsoleUlts.TinyLine();
                                foreach (string item in listPhase)
                                {
                                    count++;
                                    Console.Write(((count == currentPhase) ? " 👉 " + item : " > " + item));
                                }
                                ConsoleUlts.TinyLine();
                                count = 0;
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
                                if (searchChoice == 3) break;
                                if (listTemp.Count() == 0) activeSearchPhone = false;
                                else
                                {
                                    ConsoleUlts.TinyLine();
                                    foreach (string item in listPhase)
                                    {
                                        count++;
                                        Console.Write(((count == currentPhase) ? " 👉 " + item : " > " + item));
                                    }
                                    ConsoleUlts.TinyLine();
                                    count = 0;
                                    listPhoneSearch = ListPhonePagination(listTemp);
                                    foreach (Phone item in listAllPhones[currentPageDetails])
                                    {
                                        listAllPhonesID.Add(item.PhoneID);
                                    }
                                    Console.Write("\nEnter Phone ID To View Details: ");
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
                            ConsoleUlts.TinyLine();
                            foreach (string item in listPhase)
                            {
                                count++;
                                Console.Write(((count == currentPhase) ? " 👉 " + item : " > " + item));
                            }
                            ConsoleUlts.TinyLine();
                            count = 0;
                            List<PhoneDetail> phonedetails = phoneBL.GetPhoneDetailsByPhoneID(phoneId);
                            foreach (PhoneDetail pd in phonedetails)
                            {
                                listPhoneDetailsID.Add(pd.PhoneDetailID);
                                ConsoleUlts.PrintPhoneDetailsInfo(pd);
                                break;
                            }
                            do
                            {
                                ConsoleUlts.TinyLine();
                                foreach (string item in listPhase)
                                {
                                    count++;
                                    Console.Write(((count == currentPhase) ? " 👉 " + item : " > " + item));
                                }
                                ConsoleUlts.TinyLine();
                                count = 0;
                                ConsoleUlts.PrintPhoneDetailsInfoTitle();
                                foreach (PhoneDetail pd in phonedetails)
                                {
                                    ConsoleUlts.PrintPhoneDetailsType(pd);
                                }
                                ConsoleUlts.TinyLine();
                                Console.Write("Enter Phone Detail ID You Want To Choose: ");
                                phonedetailId = Convert.ToInt32(Console.ReadLine() ?? "");
                                if (listPhoneDetailsID.IndexOf(phonedetailId) == -1)
                                {
                                    Console.WriteLine("\nInvalid Phone Details ID Please Choice Again");
                                    ConsoleUlts.PressEnterTo("Re-enter Phone Details ID");
                                }
                            } while (listPhoneDetailsID.IndexOf(phonedetailId) == -1);
                            Console.Write("Press 1 To Create Order Or 2 To Back Previous Phase: ");
                            int.TryParse(Console.ReadLine(), out phaseChoice);
                            switch (phaseChoice)
                            {
                                case 1:
                                    currentPhase++;
                                    break;
                                case 2:
                                    currentPhase--;
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case 3:
                            Console.Write("Input Quantity: ");
                            Console.WriteLine("phase 3");
                            Console.ReadKey();
                            break;
                        case 4:
                            break;
                        case 5:
                            break;
                    }
                } while (currentPhase != 4);


                // int quantity = 0;
                // if (phonedt.Quantity > 0)
                // {
                //     Console.Write("Input quantity: ");
                //     while (!(int.TryParse(Console.ReadLine() ?? "", out quantity) && quantity <= phonedt.Quantity))
                //     {
                //         Console.Write("Out of stock OR Invalid input! \nPlease input again: ");
                //     }
                //     phonedt.Quantity = quantity;
                // }
                // else
                // {
                //     Console.WriteLine("Phone is out of stock. Please choose another phone!");
                //     ConsoleUlts.PrintOrderAndPhoneBorder(listAllPhones, listAllPhones.Count(), currentPageDetails);

                // }
                // bool check = false;

                // while (check == false)
                // {
                //     for (int i = 0; i < quantity; i++)
                //     {
                //         Console.Write($"Imei number {i + 1} for {phonedt.Phone.PhoneName}: ");
                //         string imei = Console.ReadLine() ?? "";
                //         Imeis.Add(new Imei(imei, PhoneEnum.ImeiStatus.NotExport));
                //     }
                //     if (new PhoneBL().CheckImeisExits(phonedt, Imeis))
                //     {
                //         Cart.Add(phonedt);
                //         check = true;

                //     }
                //     else
                //     {
                //         Console.Write("Wrong imeis! Choose re-enter Imeis or Back to choose phone.\nYour choice is(write 'stop imeis' to back to previous, any word to re-input imeis): ");
                //         string answer = Console.ReadLine() ?? "";
                //         if (answer == "stop imeis")
                //         {
                //             break;
                //         }
                //     }
                // }
                // Console.WriteLine("Add Phone to order completed! Keep Adding Press any key OR Stop adding write 'stop' !");
                // Console.Write("Your choice: ");
                // input = Console.ReadLine() ?? "";
                // Console.Clear();

                // // foreach(var phonedetail in Cart){
                // //     phonedetail.
                // // }

                // //Buoc 2: Insert thong tin cua customer va seller
                // Console.WriteLine("Customer Information");
                // Console.Write("Customer name: ");
                // string cusname = Console.ReadLine() ?? "";
                // Console.Write("Phone number: ");
                // string phonenumber = Console.ReadLine() ?? "";
                // Console.Write("Address: ");
                // string address = Console.ReadLine() ?? "";

                // Customer newcustomer = new Customer(0, cusname, phonenumber, address);
                // //Buoc 3: Insert vao trong database

                // Order neworder = new Order(0, new DateTime(), OrderStaff, new Staff(2, "not have", "not have", "not have", "not have", "not have", StaffEnum.Role.Accountant, StaffEnum.Status.InActive), newcustomer, Cart, OrderEnum.Status.Pending, new List<DiscountPolicy>());
                // Console.WriteLine(OrderStaff.StaffID);
                // if (orderBL.CreateOrder(neworder) == true)
                // {
                //     Console.WriteLine("Create Order completed");
                //     Console.WriteLine("Press Any key to continue...");
                //     Console.ReadKey();
                //     ConsoleUlts.ClearCurrentConsoleLine();
                //     break;
                // }
                // else
                // {
                //     Console.WriteLine("Create Order False");
                // }
            } while (activeSearchPhone);
        }
        public void SearchPhoneMenuHandle()
        {

        }
    }

}