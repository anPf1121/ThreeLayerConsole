using Model;
using BL;
using BusinessEnum;
using GUIEnum;

namespace Ults
{
    class Ultilities
    {
         private PhoneBL phoneBL = new PhoneBL();
        private ConsoleUlts ConsoleUlts = new ConsoleUlts();
        private StaffBL StaffBL = new StaffBL();
        // private CustomerBL customerBL = new CustomerBL();
        // private OrderBL orderBL = new OrderBL();
        private Staff? OrderStaff = null;
        public int MenuHandle(string? title, string? subTitle, string[] menuItem)
        {

            ConsoleKeyInfo keyInfo;
            string iconBackhand = "👉";
            bool activeSelectedMenu = true;
            int currentChoice = 1;
            while (activeSelectedMenu)
            {
                ConsoleUlts.Title(title, subTitle);
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
        public int StartMenu()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            string[] menuItem = { "Login", "Exit" };

            int choice = MenuHandle(@"
    ██████╗ ██╗  ██╗ ██████╗ ███╗   ██╗███████╗    ███████╗████████╗ ██████╗ ██████╗ ███████╗
    ██╔══██╗██║  ██║██╔═══██╗████╗  ██║██╔════╝    ██╔════╝╚══██╔══╝██╔═══██╗██╔══██╗██╔════╝
    ██████╔╝███████║██║   ██║██╔██╗ ██║█████╗      ███████╗   ██║   ██║   ██║██████╔╝█████╗  
    ██╔═══╝ ██╔══██║██║   ██║██║╚██╗██║██╔══╝      ╚════██║   ██║   ██║   ██║██╔══██╗██╔══╝  
    ██║     ██║  ██║╚██████╔╝██║ ╚████║███████╗    ███████║   ██║   ╚██████╔╝██║  ██║███████╗
    ╚═╝     ╚═╝  ╚═╝ ╚═════╝ ╚═╝  ╚═══╝╚══════╝    ╚══════╝   ╚═╝    ╚═════╝ ╚═╝  ╚═╝╚══════╝
                                                                                             "
            , null, menuItem);

            if (choice == 1) return 1;
            else if (choice == 2) return 2;
            else return -1;
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
            if (OrderStaff != null)
                return OrderStaff.Role;
            else return null;
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
                        Console.WriteLine("Create Order");

                        List<PhoneDetail> Cart = new List<PhoneDetail>();
                        string input = "";
                        int phoneid = 0;
                        int phonedetailid = 0;
                        //Tim va chon ra tung dien thoai muon them vao order
                        do{
                            Console.Write("Search Phone to add to Cart: ");
                            input = Console.ReadLine()??"";
                            ListPhonePagination(phoneBL.GetPhonesByInformation(input));
                            Console.Write("Choose an id to view phone details: ");
                            phoneid = Convert.ToInt32(Console.ReadLine()??"");
                            List<PhoneDetail> phonedetails = phoneBL.GetPhoneDetailsByPhoneID(phoneid);
                            foreach(var pd in phonedetails){
                                Console.WriteLine(pd.PhoneDetailID+ " "+pd.PhoneColor.Color +" "+ pd.ROMSize.ROM+ " "+pd.Price);
                            }
                            Console.Write("Choose a version of phone by id: ");
                            phonedetailid = Convert.ToInt32(Console.ReadLine()??"");
                            PhoneDetail phonedt = phoneBL.GetPhoneDetailByID(phonedetailid);
                            Console.Write("Input quantity: ");
                            int quantity =Convert.ToInt32(Console.ReadLine()??"");
                            phonedt.Quantity = quantity;
                            Console.Write("Stop? (write 'stop' to stop adding OR write any word to keep adding): ");
                            input = Console.ReadLine()??"";
                            Cart.Add(phonedt);
                            Console.Clear();
                        }while(input != "stop");
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
        public bool? ListPhonePagination(List<Phone> listPhone)
        {
            if (listPhone != null)
            {
                bool active = true;
                bool validInput = false;
                Dictionary<int, List<Phone>> phones = new Dictionary<int, List<Phone>>();
                phones = PhoneMenuPaginationHandle(listPhone);
                int countPage = phones.Count(), currentPage = 1;
                ConsoleKeyInfo input = new ConsoleKeyInfo();
                ConsoleKeyInfo input2 = new ConsoleKeyInfo();
                while (true)
                {
                    ConsoleUlts.Title(
                        null,
    @"     █████  ██████  ██████      ████████  ██████       ██████  ██████  ██████  ███████ ██████  
    ██   ██ ██   ██ ██   ██        ██    ██    ██     ██    ██ ██   ██ ██   ██ ██      ██   ██ 
    ███████ ██   ██ ██   ██        ██    ██    ██     ██    ██ ██████  ██   ██ █████   ██████  
    ██   ██ ██   ██ ██   ██        ██    ██    ██     ██    ██ ██   ██ ██   ██ ██      ██   ██ 
    ██   ██ ██████  ██████         ██     ██████       ██████  ██   ██ ██████  ███████ ██   ██ "
                    );
                    while (active)
                    {
                        Console.WriteLine("========================================================================================================================");
                        Console.WriteLine("| {0, 10} | {1, 30} | {2, 15} | {3, 15} | {4, 15} | {5, 15}  |", "ID", "Phone Name", "Brand", "Price", "OS", "Quantity");
                        Console.WriteLine("========================================================================================================================");

                        foreach (Phone phone in phones[currentPage])
                        {

                            ConsoleUlts.PrintPhoneInfo(phone);

                        }

                        Console.WriteLine("========================================================================================================================");
                        Console.WriteLine("{0,55}" + "< " + $"{currentPage}/{countPage}" + " >", " ");
                        Console.WriteLine("========================================================================================================================");
                        Console.WriteLine("Press 'Left Arrow' To Back Previous Page, 'Right Arror' To Next Page");
                        Console.Write("Press 'Space' To Choose a phone, 'B' To Back Previous Menu");
                        input = Console.ReadKey();
                        if (currentPage <= countPage)
                        {
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

                                Console.Clear();
                                Console.WriteLine("========================================================================================================================");
                                Console.WriteLine("| {0, 10} | {1, 30} | {2, 15} | {3, 15} | {4, 15} | {5, 15}  |", "ID", "Phone Name", "Brand", "Price", "OS", "Quantity");
                                Console.WriteLine("========================================================================================================================");

                                foreach (Phone phone in phones[currentPage])
                                {
                                    ConsoleUlts.PrintPhoneInfo(phone);
                                         
                                }
          
                                Console.WriteLine("========================================================================================================================");
                                Console.WriteLine("{0,55}" + "< " + $"{currentPage}/{countPage}" + " >", " ");
                                Console.WriteLine("========================================================================================================================");
                                do
                                {
                                    validInput = false;
                                    Console.WriteLine("Continue to select phone to view details or change the phone list page ? Press Y to continue or press N to switch pages(Y / N):");
                                    input2 = Console.ReadKey(true);
                                    if (input2.Key == ConsoleKey.Y)
                                    {
                                        validInput = true;
                                        active = false;
                                        return true;
                                    }
                                    else if (input2.Key == ConsoleKey.N)
                                    {
                                        validInput = true;
                                        Console.Clear();
                                    }
                                    else
                                    {
                                        ConsoleUlts.Alert(ConsoleEnum.Alert.Error, "Invalid Input (Y/N)");
                                    }
                                } while (!validInput);
                            }
                            else
                            {
                                Console.Clear();
                            }
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
    }

}