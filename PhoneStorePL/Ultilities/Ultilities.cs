using Model;
using BL;
using BusinessEnum;
using GUIEnum;

namespace Ults
{
    class Ultilities
    {
        // private PhoneBL phoneBL = new PhoneBL();
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
                        {
                            if (keyInfo.Key == ConsoleKey.DownArrow) currentChoice++;

                            else if (keyInfo.Key == ConsoleKey.UpArrow) currentChoice--;
                        }

                        if (currentChoice == (menuItem.Count() + 1))
                        {
                            ConsoleUlts.Alert(ConsoleEnum.Alert.Error, "You Are In The Final Choice");
                            currentChoice = menuItem.Count();
                        }

                        else if (currentChoice == 0)
                        {
                            ConsoleUlts.Alert(ConsoleEnum.Alert.Error, "You Are In The First Choice");
                            currentChoice = 1;
                        }
                        Console.Clear();
                    }
                    else
                        ConsoleUlts.Alert(ConsoleEnum.Alert.Warning, "Please press 'down arrow' to next choice or 'up arrow' to previous choice and press 'enter to confirm'");
                }
            }
            return currentChoice;
        }
        //     public bool? ListPhonePagination(List<Phone> listPhone)
        //     {
        //         if (listPhone != null)
        //         {
        //             bool active = true;
        //             bool validInput = false;
        //             Dictionary<int, List<Phone>> phones = new Dictionary<int, List<Phone>>();
        //             phones = PhoneMenuPaginationHandle(listPhone);
        //             int countPage = phones.Count(), currentPage = 1;
        //             ConsoleKeyInfo input = new ConsoleKeyInfo();
        //             ConsoleKeyInfo input2 = new ConsoleKeyInfo();
        //             while (true)
        //             {
        //                 ConsoleUlts.Title(
        //                     null,
        // @"     █████  ██████  ██████      ████████  ██████       ██████  ██████  ██████  ███████ ██████  
        //     ██   ██ ██   ██ ██   ██        ██    ██    ██     ██    ██ ██   ██ ██   ██ ██      ██   ██ 
        //     ███████ ██   ██ ██   ██        ██    ██    ██     ██    ██ ██████  ██   ██ █████   ██████  
        //     ██   ██ ██   ██ ██   ██        ██    ██    ██     ██    ██ ██   ██ ██   ██ ██      ██   ██ 
        //     ██   ██ ██████  ██████         ██     ██████       ██████  ██   ██ ██████  ███████ ██   ██ "
        //                 );
        //                 while (active)
        //                 {
        //                     Console.WriteLine("========================================================================================================================");
        //                     Console.WriteLine("| {0, 10} | {1, 30} | {2, 15} | {3, 15} | {4, 15} | {5, 15}  |", "ID", "Phone Name", "Brand", "Price", "OS", "Quantity");
        //                     Console.WriteLine("========================================================================================================================");
        //                     foreach (Phone phone in phones[currentPage])
        //                     {
        //                         ConsoleUlts.PrintPhoneInfo(phone);
        //                     }
        //                     Console.WriteLine("========================================================================================================================");
        //                     Console.WriteLine("{0,55}" + "< " + $"{currentPage}/{countPage}" + " >", " ");
        //                     Console.WriteLine("========================================================================================================================");
        //                     Console.WriteLine("Press 'Left Arrow' To Back Previous Page, 'Right Arror' To Next Page");
        //                     Console.Write("Press 'Space' To View Phone Details, 'B' To Back Previous Menu");
        //                     input = Console.ReadKey();
        //                     if (currentPage <= countPage)
        //                     {
        //                         if (input.Key == ConsoleKey.RightArrow)
        //                         {
        //                             if (currentPage <= countPage - 1) currentPage++;
        //                             Console.Clear();
        //                         }
        //                         else if (input.Key == ConsoleKey.LeftArrow)
        //                         {
        //                             if (currentPage > 1) currentPage--;
        //                             Console.Clear();
        //                         }

        //                         else if (input.Key == ConsoleKey.B)
        //                         {
        //                             return null;
        //                         }

        //                         else if (input.Key == ConsoleKey.Spacebar)
        //                         {

        //                             Console.Clear();
        //                             Console.WriteLine("========================================================================================================================");
        //                             Console.WriteLine("| {0, 10} | {1, 30} | {2, 15} | {3, 15} | {4, 15} | {5, 15}  |", "ID", "Phone Name", "Brand", "Price", "OS", "Quantity");
        //                             Console.WriteLine("========================================================================================================================");

        //                             foreach (Phone phone in phones[currentPage])
        //                             {
        //                                 ConsoleUlts.PrintPhoneInfo(phone);
        //                             }
        //                             Console.WriteLine("========================================================================================================================");
        //                             Console.WriteLine("{0,55}" + "< " + $"{currentPage}/{countPage}" + " >", " ");
        //                             Console.WriteLine("========================================================================================================================");
        //                             do
        //                             {
        //                                 validInput = false;
        //                                 Console.WriteLine("Continue to select phone to view details or change the phone list page ? Press Y to continue or press N to switch pages(Y / N):");
        //                                 input2 = Console.ReadKey(true);
        //                                 if (input2.Key == ConsoleKey.Y)
        //                                 {
        //                                     validInput = true;
        //                                     active = false;
        //                                     return true;
        //                                 }
        //                                 else if (input2.Key == ConsoleKey.N)
        //                                 {
        //                                     validInput = true;
        //                                     Console.Clear();
        //                                 }
        //                                 else
        //                                 {
        //                                     ConsoleUlts.Alert(E.Feature.Alert.Error, "Invalid Input (Y/N)");
        //                                 }
        //                             } while (!validInput);
        //                         }
        //                         else
        //                         {
        //                             Console.Clear();
        //                         }
        //                     }
        //                 }
        //             }
        //         }
        //         else
        //         {
        //             ConsoleUlts.Alert(E.Feature.Alert.Warning, "Phone Not Found");
        //             PressEnterTo("Back To Previous Menu");
        //         }
        //         return false;
        //     }

        //     public bool? ListOrderPagination(List<Order> listOrder)
        //     {
        //         if (listOrder != null)
        //         {
        //             bool active = true;
        //             Dictionary<int, List<Order>> orders = OrderMenuPaginationHandle(listOrder);
        //             int countPage = orders.Count(), currentPage = 1;
        //             ConsoleKeyInfo input = new ConsoleKeyInfo();
        //             while (true)
        //             {
        //                 ConsoleUlts.Title(
        //                     null,
        //                     @"
        //                      █████  ██      ██           ██████  ██████  ██████  ███████ ██████  ███████ 
        //                     ██   ██ ██      ██          ██    ██ ██   ██ ██   ██ ██      ██   ██ ██      
        //                     ███████ ██      ██          ██    ██ ██████  ██   ██ █████   ██████  ███████ 
        //                     ██   ██ ██      ██          ██    ██ ██   ██ ██   ██ ██      ██   ██      ██ 
        //                     ██   ██ ███████ ███████      ██████  ██   ██ ██████  ███████ ██   ██ ███████ "
        //                 );
        //                 while (active)
        //                 {
        //                     Console.WriteLine("========================================================================================================================");
        //                     Console.WriteLine("| {0, 10} | {1, 30} | {2, 20} | {3, 15} |", "ID", "Customer Name", "Order Date", "Status");
        //                     Console.WriteLine("========================================================================================================================");
        //                     foreach (Order order in orders[currentPage])
        //                     {
        //                         ConsoleUlts.PrintOrderInfo(order);
        //                     }
        //                     Console.WriteLine("========================================================================================================================");
        //                     Console.WriteLine("{0,55}" + "< " + $"{currentPage}/{countPage}" + " >", " ");
        //                     Console.WriteLine("========================================================================================================================");
        //                     Console.WriteLine("Press 'Left Arrow' To Back Previous Page, 'Right Arrow' To Next Page");
        //                     Console.Write("Press 'Space' To View Order Details, 'B' To Back Previous Menu");
        //                     input = Console.ReadKey();
        //                     if (input.Key == ConsoleKey.RightArrow)
        //                     {
        //                         if (currentPage <= countPage - 1) currentPage++;
        //                         Console.Clear();
        //                     }
        //                     else if (input.Key == ConsoleKey.LeftArrow)
        //                     {
        //                         if (currentPage > 1) currentPage--;
        //                         Console.Clear();
        //                     }

        //                     else if (input.Key == ConsoleKey.B)
        //                     {
        //                         return null;
        //                     }
        //                     else if (input.Key == ConsoleKey.Spacebar)
        //                     {
        //                         return true;
        //                     }
        //                     else
        //                         Console.Clear();

        //                 }
        //             }
        //         }
        //         else
        //             ConsoleUlts.Alert(E.Feature.Alert.Warning, "No orders exist");
        //         PressEnterTo("Back To Previous Menu");
        //         return false;

        //     }
        //     public Dictionary<int, List<Phone>> PhoneMenuPaginationHandle(List<Phone> phoneList)
        //     {
        //         List<Phone> sList = new List<Phone>();
        //         Dictionary<int, List<Phone>> menuTab = new Dictionary<int, List<Phone>>();
        //         int phoneQuantity = phoneList.Count(), itemInTab = 4, numberOfTab = 0, count = 1, secondCount = 1, idTab = 0;

        //         if (phoneQuantity % itemInTab != 0) numberOfTab = (phoneQuantity / itemInTab) + 1;
        //         else numberOfTab = phoneQuantity / itemInTab;

        //         foreach (Phone phone in phoneList)
        //         {
        //             if ((count - 1) == itemInTab)
        //             {
        //                 sList = new List<Phone>();
        //                 count = 1;
        //             }
        //             sList.Add(phone);
        //             if (sList.Count() == itemInTab)
        //             {
        //                 idTab++;
        //                 menuTab.Add(idTab, sList);
        //             }
        //             else if (sList.Count() < itemInTab && secondCount == phoneQuantity)
        //             {
        //                 idTab++;
        //                 menuTab.Add(idTab, sList);
        //             }
        //             secondCount++;
        //             count++;
        //         }
        //         return menuTab;
        //     }
        //     public Dictionary<int, List<Order>> OrderMenuPaginationHandle(List<Order> orderList)
        //     {
        //         List<Order> sList = new List<Order>();
        //         Dictionary<int, List<Order>> menuTab = new Dictionary<int, List<Order>>();
        //         int orderQuantity = orderList.Count(), itemInTab = 4, numberOfTab = 0, count = 1, secondCount = 1, idTab = 0;

        //         if (orderQuantity % itemInTab != 0) numberOfTab = (orderQuantity / itemInTab) + 1;
        //         else numberOfTab = orderQuantity / itemInTab;

        //         foreach (Order order in orderList)
        //         {
        //             if ((count - 1) == itemInTab)
        //             {
        //                 sList = new List<Order>();
        //                 count = 1;
        //             }
        //             sList.Add(order);
        //             if (sList.Count() == itemInTab)
        //             {
        //                 idTab++;
        //                 menuTab.Add(idTab, sList);
        //             }
        //             else if (sList.Count() < itemInTab && secondCount == orderQuantity)
        //             {
        //                 idTab++;
        //                 menuTab.Add(idTab, sList);
        //             }
        //             secondCount++;
        //             count++;
        //         }
        //         return menuTab;
        //     }
        public int StartMenu()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            string[] menuItem = { "Login", "Exit" };

            int choice = MenuHandle(@"    ██████╗ ██╗  ██╗ ██████╗ ███╗   ██╗███████╗    ███████╗████████╗ ██████╗ ██████╗ ███████╗
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
            ConsoleUlts.Title(null, @"                            ██╗      ██████╗  ██████╗ ██╗███╗   ██╗
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
                    if (key.Key != ConsoleKey.Enter)
                        Console.Write("*");
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
        //     public int CreateOrderMenuHandle()
        //     {
        //         Order order = new Order();
        //         bool active = true, activeSearchPhone = true, active2 = true, activeEnterOrBack = true, activeChoseMoreOrContinue = true, activeConfirmOrCancel = true;
        //         List<Phone>? phones = phoneBL.GetAllItem();
        //         ConsoleKeyInfo input = new ConsoleKeyInfo();
        //         if (phones != null)
        //         {
        //             List<Phone>? listSearch = new List<Phone>();
        //             string searchPhone = "";
        //             string[] menuItem = { "👉 Search Phone By Information", "👉 Back To Previous Menu" };
        //             string[] menuOption = { "👉 Re-Enter Phone Information", "👉 Cancel Order" };
        //             while (active)
        //             {
        //                 switch (MenuHandle(
        //                    null,
        //     @"     ██████ ██████  ███████  █████  ████████ ███████      ██████  ██████  ██████  ███████ ██████  
        //     ██      ██   ██ ██      ██   ██    ██    ██          ██    ██ ██   ██ ██   ██ ██      ██   ██ 
        //     ██      ██████  █████   ███████    ██    █████       ██    ██ ██████  ██   ██ █████   ██████  
        //     ██      ██   ██ ██      ██   ██    ██    ██          ██    ██ ██   ██ ██   ██ ██      ██   ██ 
        //      ██████ ██   ██ ███████ ██   ██    ██    ███████      ██████  ██   ██ ██████  ███████ ██   ██ ", menuItem))
        //                 {
        //                     case 1:
        //                         do
        //                         {
        //                             ConsoleUlts.Title(null,
        //                             @"    ███████ ███████  █████  ██████   ██████ ██   ██     ██████  ██   ██  ██████  ███    ██ ███████ 
        //     ██      ██      ██   ██ ██   ██ ██      ██   ██     ██   ██ ██   ██ ██    ██ ████   ██ ██      
        //     ███████ █████   ███████ ██████  ██      ███████     ██████  ███████ ██    ██ ██ ██  ██ █████   
        //          ██ ██      ██   ██ ██   ██ ██      ██   ██     ██      ██   ██ ██    ██ ██  ██ ██ ██      
        //     ███████ ███████ ██   ██ ██   ██  ██████ ██   ██     ██      ██   ██  ██████  ██   ████ ███████ "
        //                             );
        //                             Console.Write("\nEnter Phone Information To Search: ");
        //                             searchPhone = Console.ReadLine() ?? "";
        //                             listSearch = phoneBL.SearchByPhoneInformation(searchPhone);
        //                             if (listSearch == null)
        //                             {
        //                                 ConsoleUlts.Alert(E.Feature.Alert.Warning, "Phone Not Found");
        //                                 ConsoleUlts.Title(null,
        //                                 @"    ███████ ███████  █████  ██████   ██████ ██   ██     ██████  ██   ██  ██████  ███    ██ ███████ 
        //     ██      ██      ██   ██ ██   ██ ██      ██   ██     ██   ██ ██   ██ ██    ██ ████   ██ ██      
        //     ███████ █████   ███████ ██████  ██      ███████     ██████  ███████ ██    ██ ██ ██  ██ █████   
        //          ██ ██      ██   ██ ██   ██ ██      ██   ██     ██      ██   ██ ██    ██ ██  ██ ██ ██      
        //     ███████ ███████ ██   ██ ██   ██  ██████ ██   ██     ██      ██   ██  ██████  ██   ████ ███████ "
        //                                 );
        //                                 activeSearchPhone = ReEnterOrCancel();
        //                             }
        //                             else
        //                             {
        //                                 do
        //                                 {
        //                                     bool? temp = ListPhonePagination(listSearch);
        //                                     int phoneId;
        //                                     int phoneQuantity;
        //                                     if (temp != null)
        //                                     {
        //                                         do
        //                                         {
        //                                             Console.Write("👉 Input Phone ID To view Phone Details: ");
        //                                             int.TryParse(Console.ReadLine(), out phoneId);


        //                                             if (phoneId <= 0 || phoneId > phones.Count())
        //                                                 ConsoleUlts.Alert(E.Feature.Alert.Error, "Invalid Phone ID, Please Try Again");

        //                                         } while (phoneId <= 0 || phoneId > phones.Count());
        //                                         Console.Clear();
        //                                         Phone phone = phoneBL.GetItemById(phoneId);
        //                                         if (phone != null)
        //                                         {
        //                                             activeEnterOrBack = true;
        //                                             while (activeEnterOrBack)
        //                                             {
        //                                                 ConsoleUlts.PrintPhoneDetailsInfo(phone);
        //                                                 Console.WriteLine("Press Enter To Add this phone to Order or Press B to back previous Menu: ");
        //                                                 input = Console.ReadKey(true);
        //                                                 if (input.Key == ConsoleKey.B) break;
        //                                                 else if (input.Key == ConsoleKey.Enter)
        //                                                 {
        //                                                     // input phone quantity (số lượng điện thoại)
        //                                                     do
        //                                                     {
        //                                                         Console.Write("Enter Quantity: ");
        //                                                         int.TryParse(Console.ReadLine(), out phoneQuantity);

        //                                                         if (phoneQuantity <= 0 || phoneQuantity > phone.Quantity)
        //                                                             ConsoleUlts.Alert(E.Feature.Alert.Error, "Invalid Quantity, Please Try Again");
        //                                                     } while (phoneQuantity <= 0 || phoneQuantity > phone.Quantity);

        //                                                     phone.Quantity = phoneQuantity;
        //                                                     order.ListPhone.Add(phone);
        //                                                     // hỏi người dùng có muốn chọn thêm điện thoại vào Hóa đơn được hay không?
        //                                                     while (activeChoseMoreOrContinue)
        //                                                     {
        //                                                         Console.WriteLine("Press 'Y' To Choose More Other Phone or Press 'N' To Continue Input customer Information");
        //                                                         input = Console.ReadKey(true);
        //                                                         if (input.Key == ConsoleKey.Y)
        //                                                         {
        //                                                             activeEnterOrBack = false;
        //                                                             break;
        //                                                         }
        //                                                         // NHập thông tin khách hàng mua
        //                                                         else if (input.Key == ConsoleKey.N)
        //                                                         {
        //                                                             string PatternPhone = @"^0[0-9]{9,}$";
        //                                                             Customer customer = new Customer();
        //                                                             ConsoleUlts.TinyLine();
        //                                                             Console.WriteLine("CUSTOMER INFORMATION");
        //                                                             Console.Write("Customer Name: ");
        //                                                             customer.CustomerName = Console.ReadLine() ?? "";
        //                                                             Console.Write("Phone number: ");
        //                                                             customer.PhoneNumber = Console.ReadLine() ?? "";
        //                                                             while (!(Regex.IsMatch(customer.PhoneNumber, PatternPhone, RegexOptions.IgnoreCase)))
        //                                                             {
        //                                                                 Console.WriteLine($"{customer.PhoneNumber} is not an Phone Number!");
        //                                                                 Console.Write("Phone number: ");
        //                                                                 customer.PhoneNumber = Console.ReadLine() ?? "";
        //                                                             }
        //                                                             Console.Write("Address: ");
        //                                                             customer.Address = Console.ReadLine() ?? "";
        //                                                             while (activeConfirmOrCancel)
        //                                                             {
        //                                                                 // Confirm Order
        //                                                                 Console.WriteLine("Press 'Y' To Confirm Order or Press 'N' to Cancel Order: ");
        //                                                                 input = Console.ReadKey(true);
        //                                                                 if (input.Key == ConsoleKey.Y)
        //                                                                 {
        //                                                                     order.OrderCustomer = customer;
        //                                                                     order.OrderSeller = this.OrderStaff;
        //                                                                     order.PhoneWithImei = SetImeiForeachPhone(order);
        //                                                                     orderBL.InsertOrder(order);
        //                                                                     // ở đây ta sẽ Insert Customer vào database thông qua lớp BL
        //                                                                     // Insert order vào Database thông qua lớp BL
        //                                                                     return 1;
        //                                                                 }
        //                                                                 else if (input.Key == ConsoleKey.N)
        //                                                                 {
        //                                                                     activeConfirmOrCancel = false;
        //                                                                     return 0;
        //                                                                 }
        //                                                                 else Console.Clear();
        //                                                                 ConsoleUlts.PrintPhoneDetailsInfo(phone);
        //                                                             }
        //                                                         }
        //                                                         else Console.Clear();
        //                                                         ConsoleUlts.PrintPhoneDetailsInfo(phone);
        //                                                     }

        //                                                 }
        //                                                 else Console.Clear();
        //                                             }
        //                                         }
        //                                     }
        //                                     else
        //                                         active2 = false;
        //                                     activeSearchPhone = false;
        //                                 } while (active2);
        //                             }
        //                         } while (activeSearchPhone);
        //                         break;
        //                     case 2:
        //                         active = false;
        //                         return 2;

        //                 }
        //             }
        //         }
        //         else return -1;
        //         return 1;
        //     }
        //     public Dictionary<string, Phone> SetImeiForeachPhone(Order order)
        //     {
        //         Dictionary<string, Phone> output = new Dictionary<string, Phone>();
        //         List<string> check = new List<string>();
        //         foreach (var phone in order.ListPhone)
        //         {
        //             for (int i = 0; i < phone.Quantity; i++)
        //             {
        //                 Console.Write($"Input imei for {phone.PhoneName} {i + 1}: ");
        //                 string imei = Console.ReadLine() ?? "";
        //                 while (phoneBL.CheckImeiExist(imei, phone) == false)
        //                 {
        //                     Console.Write("Imei doesnt exist or has been sold or not suitable for this phone! \nPlease Re-Enter a new phone imei: ");
        //                     imei = Console.ReadLine() ?? "";
        //                 }
        //                 output.Add(imei, phone);
        //             }
        //         }
        //         return output;
        //     }
        //     public void HandleOrderMenuHandle()
        //     {
        //         bool active = true;

        //         string[] menuItem = { "👉 Show order by paid status in day", "👉 Back To Previous Menu" };
        //         while (active)
        //         {
        //             switch (MenuHandle(
        //                 null,
        //                 @"    ██   ██  █████  ███    ██ ██████  ██      ███████      ██████  ██████  ██████  ███████ ██████  
        //     ██   ██ ██   ██ ████   ██ ██   ██ ██      ██          ██    ██ ██   ██ ██   ██ ██      ██   ██ 
        //     ███████ ███████ ██ ██  ██ ██   ██ ██      █████       ██    ██ ██████  ██   ██ █████   ██████  
        //     ██   ██ ██   ██ ██  ██ ██ ██   ██ ██      ██          ██    ██ ██   ██ ██   ██ ██      ██   ██ 
        //     ██   ██ ██   ██ ██   ████ ██████  ███████ ███████      ██████  ██   ██ ██████  ███████ ██   ██ ", menuItem))
        //             {
        //                 case 1:
        //                     ConsoleUlts.Title(null,
        //                     @"            ███████ ██   ██  ██████  ██     ██      ██████  ██████  ██████  ███████ ██████  
        //             ██      ██   ██ ██    ██ ██     ██     ██    ██ ██   ██ ██   ██ ██      ██   ██ 
        //             ███████ ███████ ██    ██ ██  █  ██     ██    ██ ██████  ██   ██ █████   ██████  
        //                  ██ ██   ██ ██    ██ ██ ███ ██     ██    ██ ██   ██ ██   ██ ██      ██   ██ 
        //             ███████ ██   ██  ██████   ███ ███       ██████  ██   ██ ██████  ███████ ██   ██ "
        //                     );
        //                     string orderId = "";
        //                     Console.Write("Input order id:");
        //                     orderId = Console.ReadLine() ?? "";
        //                     break;
        //                 case 2:
        //                     active = false;
        //                     break;
        //                 default:
        //                     break;
        //             }
        //         }
        //     }

        //     public bool ReEnterOrCancel()
        //     {
        //         int reEnterOrCancel;
        //         bool result = true;
        //         string[] menuOption = { "👉 Re-Enter Phone Information", "👉 Cancel Order" };
        //         reEnterOrCancel = MenuHandle(null, null, menuOption);
        //         switch (reEnterOrCancel)
        //         {
        //             case (int)E.Feature.SearchPhone.ReEnterPhoneInfo:
        //                 PressEnterTo("Re-Enter Phone Infomation");
        //                 break;
        //             case (int)E.Feature.SearchPhone.CancelOrder:
        //                 PressEnterTo("Back Previous Menu");
        //                 result = false;
        //                 break;
        //         }
        //         return result;

        //     }
        //     public void PaymentMenuHandle()
        //     {

        //         bool active = true;
        //         string[] paymentItem = { "👉 Show All Orders Have Processing Status", "👉 Discount", "👉 Log Out" };
        //         while (active)
        //         {
        //             List<Order> listProcessingOrder = new List<Order>();
        //             switch (MenuHandle(null, @"
        //                     ██████   █████  ██    ██ ███    ███ ███████ ███    ██ ████████ 
        //                     ██   ██ ██   ██  ██  ██  ████  ████ ██      ████   ██    ██    
        //                     ██████  ███████   ████   ██ ████ ██ █████   ██ ██  ██    ██    
        //                     ██      ██   ██    ██    ██  ██  ██ ██      ██  ██ ██    ██    
        //                     ██      ██   ██    ██    ██      ██ ███████ ██   ████    ██", paymentItem))
        //             {
        //                 case 1:
        //                     // show order có trạng thái processing
        //                     listProcessingOrder = orderBL.GetOrderHaveProcessingStatus();
        //                     bool? temp = ListOrderPagination(listProcessingOrder);
        //                     Console.Write("Choose an order by id to payment: ");
        //                     int orderid;
        //                     string id = Console.ReadLine() ?? "";
        //                     while (!int.TryParse(id, out orderid))
        //                     {
        //                         Console.Write("Invalid order id please input another: ");
        //                         id = Console.ReadLine() ?? "";
        //                     }
        //                     break;
        //                 case 2:
        //                     break;
        //                 case 3:
        //                     active = false;
        //                     break;
        //                 default:
        //                     break;
        //             }
        //         }
        //     }
        //     public void RevenueMenuHandle()
        //     {
        //         bool active = true;
        //         string[] revenueItem = { "👉 Report Revenue in week", "👉 Report Revenue In Month", "👉 Report Revenue In Day", "👉 Report Revenue Quarter Of Year", "👉 Back To Previous Menu" };
        //         while (active)
        //         {
        //             switch (MenuHandle(null, @"
        //                         ██████  ███████ ██    ██ ███████ ███    ██ ██    ██ ███████ 
        //                         ██   ██ ██      ██    ██ ██      ████   ██ ██    ██ ██      
        //                         ██████  █████   ██    ██ █████   ██ ██  ██ ██    ██ █████   
        //                         ██   ██ ██       ██  ██  ██      ██  ██ ██ ██    ██ ██      
        //                         ██   ██ ███████   ████   ███████ ██   ████  ██████  ███████ ", revenueItem))
        //             {
        //                 case 1:
        //                     Console.WriteLine("Week Revenue: 1000$");
        //                     break;
        //                 case 2:
        //                     Console.WriteLine("Month Revenue: 4000$");
        //                     break;
        //                 case 3:
        //                     Console.WriteLine("Day Revenue: 150$");
        //                     break;
        //                 case 4:
        //                     break;
        //                 case 5:
        //                     active = false;
        //                     break;
        //                 default:
        //                     break;
        //             }
        //         }
        //     }
        // }
        public int SellerMenu()
        {
            int result = 0;
            bool active = true;
            string[] menuItem = { "👉 Create Order", "👉 Handle Order", "👉 Log Out" };
            while (active)
            {
                switch (MenuHandle(
                    null
    , @"                                ███████ ███████ ██      ██      ███████ ██████  
                                        ██      ██      ██      ██      ██      ██   ██ 
                                        ███████ █████   ██      ██      █████   ██████  
                                             ██ ██      ██      ██      ██      ██   ██ 
                                        ███████ ███████ ███████ ███████ ███████ ██   ██ ", menuItem))
                {
                    case 1:

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
            string[] menuItem = { "👉 Payment", "👉 Revenue Report", "👉 Log Out" };
            while (active)
            {
                switch (ultilities.MenuHandle(null,
                @"              █████   ██████  ██████  ██████  ██    ██ ███    ██ ████████  █████  ███    ██ ████████ 
                     ██   ██ ██      ██      ██    ██ ██    ██ ████   ██    ██    ██   ██ ████   ██    ██    
                     ███████ ██      ██      ██    ██ ██    ██ ██ ██  ██    ██    ███████ ██ ██  ██    ██    
                     ██   ██ ██      ██      ██    ██ ██    ██ ██  ██ ██    ██    ██   ██ ██  ██ ██    ██    
                     ██   ██  ██████  ██████  ██████   ██████  ██   ████    ██    ██   ██ ██   ████    ██ ", menuItem))
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
    }
}