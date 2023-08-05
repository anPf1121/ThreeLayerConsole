using Model;
using BL;
using Interface;
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
        private IStaffBL loginManager;
        public Ultilities(IStaffBL loginManager) {
            this.loginManager = loginManager;
        }

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
                    ConsoleUlts.Title(title, subTitle, loginManager.LoggedInStaff);
                if (currentChoice <= (menuItem.Count() + 1) && currentChoice >= 1)
                {
                    for (int i = 0; i < menuItem.Count(); i++)
                        Console.WriteLine("| {0, 50} |", (((currentChoice - 1 == i) ? (iconBackhand + " ") : "") + " " + ConsoleUlts.SetTextBolder(menuItem[i]) + $" ({i + 1})").PadRight(98));
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

        public bool PressYesOrNo(string yesAction, string noAction)
        {
            ConsoleKeyInfo input = new ConsoleKeyInfo();
            bool active = true;
            char ch = 'c';
            Console.Write($"Press {ConsoleUlts.SetTextBolder("Y")} To {yesAction} Or {ConsoleUlts.SetTextBolder("N")} To {noAction}");
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
            if (thirdAction != null)
                Console.Write($" Press {ConsoleUlts.SetTextBolder("Q")} To {firstAction}\n Press {ConsoleUlts.SetTextBolder("W")} To {secondAction}\n Press {ConsoleUlts.SetTextBolder("E")} To {thirdAction}");
            else
                Console.Write($" Press {ConsoleUlts.SetTextBolder("Q")} To {firstAction}\n Press {ConsoleUlts.SetTextBolder("W")} To {secondAction}");
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
        public bool ListPhonePagination(List<Phone> listPhone,string[] phases,int itemCount,int currentPhase)
        {
            string title = ConsoleUlts.GetAddPhoneToOrderANSITitle();
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
                    ConsoleUlts.Title(ConsoleUlts.GetAppTitle(), title, loginManager.LoggedInStaff);
                    while (active)
                    {
                        Console.Clear();
                        ConsoleUlts.PrintListPhase(phases, itemCount, currentPhase);
                        ConsoleUlts.Title(ConsoleUlts.GetAppTitle(), title, loginManager.LoggedInStaff);
                        ConsoleUlts.PrintPhoneBorderLine();
                        foreach (Phone phone in phones[currentPage])
                        {
                            ConsoleUlts.PrintPhoneInfo(phone);
                        }
                        ConsoleUlts.GetFooterPagination(currentPage, countPage);
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
                        } while (input.Key != ConsoleKey.RightArrow|| input.Key != ConsoleKey.LeftArrow|| input.Key != ConsoleKey.B|| input.Key != ConsoleKey.Spacebar);
                    }
                }
            }
            else
            {
                ConsoleUlts.Alert(ConsoleEnum.Alert.Warning, "Phone Not Found");
                ConsoleUlts.PressEnterTo("Back To Previous Menu");
            }
            return false;
        }

        public string GenerateID() => Guid.NewGuid().ToString("N").Substring(0, 12).ToUpper();

        public bool? ListOrderPagination(List<Order> listOrder,string[] phases,int itemCount,int currentPhase)
        {
            if (listOrder != null)
            {
                string title = ConsoleUlts.GetAllOrderANSITitle();
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
                        ConsoleUlts.Title(ConsoleUlts.GetAppTitle(), title, loginManager.LoggedInStaff);
                        while (active)
                        {
                            ConsoleUlts.PrintListPhase(phases, itemCount, currentPhase);
                            ConsoleUlts.Title(ConsoleUlts.GetAppTitle(), title, loginManager.LoggedInStaff);
                            ConsoleUlts.PrintOrderBorderLine();
                            foreach (Order order in orders[currentPage])
                            {
                                ConsoleUlts.PrintOrderInfo(order);
                            }
                            ConsoleUlts.GetFooterPagination(currentPage, countPage);
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
                            } while (input.Key != ConsoleKey.RightArrow|| input.Key != ConsoleKey.LeftArrow|| input.Key != ConsoleKey.B|| input.Key != ConsoleKey.Spacebar);
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

        public int InputIDValidation(int maximumValue, string requestToEnter, string errorMessage)
        {
            int intValue = 0;
            do
            {
                intValue = ConsoleUlts.GetInputInt(requestToEnter);
                if (intValue <= 0 || intValue > maximumValue)
                {
                    ConsoleUlts.Alert(ConsoleEnum.Alert.Error, errorMessage);
                }
            } while (intValue <= 0 || intValue > maximumValue);
            return intValue;
        }

        public void CreateOrder()
        {
            string searchTitle = ConsoleUlts.GetSearchANSIText(),phoneInfoToSearch = "",input = "";
            string[] menuSearchChoice = ConsoleUlts.GetMenuItemSearch(),listPhase = ConsoleUlts.GetCreateOrderTimeLine();
            int phoneId = 0,phoneModelID = 0,count = 0,quantityAfterAddMoreHandle = 0,searchChoice = 0,currentPhase = 1,phaseChoice = 0,quantity = 0,reChooseModelAfterBackPrevPhase = 0;
            List<Imei> imeis = new List<Imei>();
            List<int>? listAllPhonesID = new List<int>();
            bool isAddMore = false, listPhoneSearch = false, activeSearchPhone = true;
            List<Phone> listTemp = new List<Phone>();
            List<PhoneDetail> phonedetails = new List<PhoneDetail>(), phonesInOrder = new List<PhoneDetail>();
            PhoneDetail? pDetails = null;
            Customer? customer = null;
            do
            {
                switch (currentPhase)
                {
                    case 1:
                       
                        ConsoleUlts.PrintListPhase(listPhase, count, currentPhase);
                        ConsoleUlts.Title(ConsoleUlts.GetAppTitle(), ConsoleUlts.GetSearchANSIText(), loginManager.LoggedInStaff);
                        searchChoice = PressCharacterTo("Search All Phone", "Search Phone By Information", "Back To Previous Menu");

                        if (searchChoice == 0) listTemp = phoneBL.GetAllPhone();

                        else if (searchChoice == 1)
                        {
                            ConsoleUlts.PrintListPhase(listPhase, count, currentPhase);
                            ConsoleUlts.Title(ConsoleUlts.GetAppTitle(), searchTitle, loginManager.LoggedInStaff);
                            phoneInfoToSearch = ConsoleUlts.GetInputString("Enter Phone Information To Search");
                            listTemp = phoneBL.GetPhonesByInformation(phoneInfoToSearch);
                        }

                        else if (searchChoice == 2) return;

                        if (listTemp.Count() == 0) activeSearchPhone = false;
                        else
                        {
                            listPhoneSearch = ListPhonePagination(listTemp, listPhase, count, currentPhase);
                            phoneId = InputIDValidation(phoneBL.GetAllPhone().Count(), "Enter Phone ID", "Invalid Phone ID");
                        }

                        if (!listPhoneSearch) break;
                            
                        if (!PressYesOrNo("Back Previous Phase", "Choose Phone Model")) currentPhase++;
                        break;
                    case 2:
                        phonedetails = phoneBL.GetPhoneDetailsByPhoneID(phoneId);
                        ConsoleUlts.PrintListPhase(listPhase, count, currentPhase);
                        if (phonesInOrder.Count() != 0 || phonesInOrder != null)
                        {
                            Dictionary<int, int> phoneDetailsIDWithQtt = new Dictionary<int, int>();
                            
                            foreach (PhoneDetail item in phonesInOrder)
                                phoneDetailsIDWithQtt.Add(item.PhoneDetailID, item.Quantity);

                            foreach (PhoneDetail pd in phonedetails)
                                foreach (KeyValuePair<int, int> item in phoneDetailsIDWithQtt)
                                    if (pd.PhoneDetailID == item.Key)
                                        pd.Quantity -= item.Value;
                        }

                        bool reEnterPhoneModelID = true;
                        List<int> listPhoneDetailID = new List<int>();
                        do
                        {
                            reChooseModelAfterBackPrevPhase = 0; reEnterPhoneModelID = true; listPhoneDetailID = new List<int>();

                            foreach (PhoneDetail item in phonedetails)
                                listPhoneDetailID.Add(item.PhoneDetailID);

                            do
                            {
                                ConsoleUlts.PrintPhoneDetailsInfo(phonedetails);
                                phoneModelID = ConsoleUlts.GetInputInt("Enter Phone Model ID");
                                if (listPhoneDetailID.IndexOf(phoneModelID) == -1)
                                {
                                    ConsoleUlts.Alert(ConsoleEnum.Alert.Error, "Invalid Phone Model ID Please Choice Again");
                                    ConsoleUlts.PrintListPhase(listPhase, count, currentPhase);
                                }
                            } while (listPhoneDetailID.IndexOf(phoneModelID) == -1);

                            int quantityAfterBackPrevPhase = 1;

                            if (phonesInOrder.Count() != 0 || phonesInOrder != null)
                                foreach (PhoneDetail pd in phonesInOrder)
                                    if (pd.PhoneDetailID == phoneModelID)
                                        quantityAfterBackPrevPhase = phoneBL.GetPhoneDetailByID(phoneModelID).Quantity - pd.Quantity;

                            if (phoneBL.GetPhoneDetailByID(phoneModelID).Quantity == 0 || quantityAfterBackPrevPhase == 0)
                            {
                                ConsoleUlts.Alert(ConsoleEnum.Alert.Warning, "This Phone Model Is Out Of Stock");
                                if (quantityAfterBackPrevPhase == 0)
                                {
                                    if (listPhoneDetailID.Count() > 1)
                                    {
                                        reChooseModelAfterBackPrevPhase = PressCharacterTo("Choose Another Phone Model", "Back Previous Phase", "Continue To Create Order");
                                        if (reChooseModelAfterBackPrevPhase == 1)
                                        {
                                            reEnterPhoneModelID = false;
                                            break;
                                        }
                                        else if (reChooseModelAfterBackPrevPhase == 2) break;
                                    }
                                    else
                                    {
                                        reEnterPhoneModelID = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    if (listPhoneDetailID.Count() > 1)
                                    {
                                        reEnterPhoneModelID = PressYesOrNo("Choose Another Phone Model", "Back Previous Phase");
                                        if (!reEnterPhoneModelID) break;
                                    }
                                    else
                                    {
                                        reEnterPhoneModelID = false;
                                        break;
                                    }
                                }
                            }
                            else break;
                        } while (reEnterPhoneModelID);
                        if (!reEnterPhoneModelID)
                        {
                            currentPhase--;
                            break;
                        }
                        if (reChooseModelAfterBackPrevPhase == 2)
                        {
                            currentPhase++;
                            break;
                        }

                        int phoneDetailQuantity = 0;
                        pDetails = new PhoneBL().GetPhoneDetailByID(phoneModelID);
                        if (phonesInOrder.Count() != 0)
                        {
                            foreach (PhoneDetail pd in phonesInOrder)
                                if (pd.PhoneDetailID == phoneModelID)
                                    phoneDetailQuantity = pDetails.Quantity - pd.Quantity;
                                else 
                                    phoneDetailQuantity = pDetails.Quantity;
                        }
                        else phoneDetailQuantity = pDetails.Quantity;

                        Console.Clear();
                        ConsoleUlts.PrintListPhase(listPhase, count, currentPhase);
                        ConsoleUlts.PrintPhoneDetailsInfo(phonedetails);
                        Console.WriteLine("Phone Model ID: " + phoneModelID);
                        quantity = InputIDValidation(phoneDetailQuantity, "Enter Phone Model Quantity", "Invalid Phone Model Quantity");
                        ConsoleUlts.Alert(ConsoleEnum.Alert.Success, "Quantity Successfully Added");
                        pDetails.Quantity = quantity;
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
                                Imei imei = new Imei( "", BusinessEnum.PhoneEnum.ImeiStatus.NotExport );
                                do
                                {
                                    isDuplicateImei = false;
                                    imei.PhoneImei = ConsoleUlts.GetInputString($"Enter Imei {i + 1}");
                                    foreach (PhoneDetail item in phonesInOrder)
                                    {
                                        isDuplicateImei = phoneBL.CheckImeiIsDuplicateInOrder(imei, imeis);
                                    }
                                    if (!phoneBL.CheckImeiExist(imei, phoneModelID) || isDuplicateImei)
                                    {
                                        if (!phoneBL.CheckImeiExist(imei, phoneModelID))
                                            ConsoleUlts.Alert(ConsoleEnum.Alert.Error, "Imei Not Found");
                                        else if (isDuplicateImei)
                                            ConsoleUlts.Alert(ConsoleEnum.Alert.Error, "You Just Entered This Imei Before, Please Re-enter It");
                                        Console.Clear();
                                        ConsoleUlts.PrintListPhase(listPhase, count, currentPhase);
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
                                        ConsoleUlts.Alert(ConsoleEnum.Alert.Success, "Imei Successfully Added");
                                        imeis.Add(imei);
                                    }
                                } while (!phoneBL.CheckImeiExist(imei, phoneModelID) || isDuplicateImei);
                            }
                            if (PressYesOrNo("Back Previous Phase", "Continue"))
                            {
                                imeis = new List<Imei>();
                                currentPhase--;
                            }
                            else currentPhase++;
                            break;
                        } while (phaseChoice != 1 && phaseChoice != 2);
                        break;
                    case 3:
                        if (reChooseModelAfterBackPrevPhase != 2)
                        {
                            bool isDuplicate = false;
                            pDetails.ListImei = imeis;
                            foreach (PhoneDetail pd in phonesInOrder)
                                if (pDetails.PhoneDetailID == pd.PhoneDetailID)
                                {
                                    pd.Quantity += pDetails.Quantity;
                                    pd.ListImei.AddRange(imeis);
                                    isDuplicate = true;
                                }
                            if (!isDuplicate) phonesInOrder.Add(pDetails);
                        }
                        Order ord = new Order("",DateTime.Now,orderStaff,new Staff(0, "","","","","",StaffEnum.Role.Accountant,StaffEnum.Status.Active),new Customer(0, "", "", ""),phonesInOrder,OrderEnum.Status.Pending,new List<DiscountPolicy>(),"", 0);
                        ConsoleUlts.PrintListPhase(listPhase, count, currentPhase);
                        ConsoleUlts.PrintOrderDetails(ord);
                        ConsoleUlts.Line();
                        phaseChoice = PressCharacterTo("Back Previous Phase", "Enter Customer Info", "Add More Phone");
                        if (phaseChoice == 0)
                        {
                            for (var i = 0; i < phonesInOrder.Count(); i++)
                                phonesInOrder[i].ListImei.RemoveAll(item => pDetails.ListImei.Contains(item));
                            foreach (PhoneDetail pd in phonesInOrder)
                                if (pDetails.PhoneDetailID == pd.PhoneDetailID)
                                    pd.Quantity -= pDetails.Quantity;
                            currentPhase--;
                        }
                        else if (phaseChoice == 1) currentPhase++;
                        else
                        {
                            currentPhase = 1;
                            isAddMore = true;
                        }
                        break;
                    case 4:
                        ConsoleUlts.PrintListPhase(listPhase, count, currentPhase);
                        ConsoleUlts.Title(ConsoleUlts.GetAppTitle(), ConsoleUlts.GetCustomerInfoANSITitle(), loginManager.LoggedInStaff);
                        customer = ConsoleUlts.GetCustomerInfo();

                        if (PressYesOrNo("Back Previous Phase", "Confirm Order")) currentPhase--;

                        else currentPhase++;

                        break;
                    case 5:
                        ConsoleUlts.PrintListPhase(listPhase, count, currentPhase);
                        Order order = new Order(GenerateID(),DateTime.Now,loginManager.LoggedInStaff,new Staff(0,"","","","","",StaffEnum.Role.Accountant,StaffEnum.Status.Active),customer,phonesInOrder,OrderEnum.Status.Pending,new List<DiscountPolicy>(),"",0);
                        ConsoleUlts.PrintSellerOrder(order);
                        if (PressYesOrNo("Create Order", "Cancel Order")) {
                            bool isCreateOrder = orderBL.CreateOrder(order);
                            ConsoleUlts.Alert(isCreateOrder ? ConsoleEnum.Alert.Success : ConsoleEnum.Alert.Error, isCreateOrder ? "Create Order Completed" : "Create Order Failed");
                        }
                        else
                        {
                            ConsoleUlts.Alert(ConsoleEnum.Alert.Success,"Cancel Order Completed!");
                            phonesInOrder = new List<PhoneDetail>();
                        }
                        currentPhase++;
                        break;
                }
            } while (currentPhase != 6);
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
            string orderID = "";
            ConsoleKeyInfo keyInfo = new ConsoleKeyInfo();
            Order? orderWantToPayment = new Order("", new DateTime(), new Staff(0, "", "", "", "", "", StaffEnum.Role.Seller, StaffEnum.Status.Active), new Staff(0, "", "", "", "", "", StaffEnum.Role.Accountant, StaffEnum.Status.Active), new Customer(0, "", "", ""), new List<PhoneDetail>(), OrderEnum.Status.Pending, new List<DiscountPolicy>(), "", 0);
            bool dontKnowHowtoCall = true;
            do
            {
                currentPhase = 1;
                bool dontKnowHowtoCall1 = false;
                bool? showOrderList = ListOrderPagination(ListOrderPending, listPhase, count, currentPhase);
                Console.WriteLine(showOrderList);
                if (showOrderList == null)
                {
                    ConsoleUlts.Alert(ConsoleEnum.Alert.Error, "NO ORDER EXIST");
                    Console.ReadKey();
                    break;
                }
                else if (showOrderList == true)
                {
                    Console.WriteLine();
                    do
                    {
                        orderID = ConsoleUlts.GetInputString("Choose An Order ID To Payment").ToUpper();
                        orderWantToPayment = new OrderBL().GetOrderById(orderID) ?? null;
                        if(orderWantToPayment.OrderID == "") {
                            ConsoleUlts.Alert(ConsoleEnum.Alert.Error, "Invalid Order ID");
                        } else orderWantToPayment.Accountant = this.loginManager.LoggedInStaff;
                    } while (orderWantToPayment.OrderID == "");
                    
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
                            Console.WriteLine("👉 Choose a payment method");
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
                                    Console.WriteLine("👉 Choose discount policy for PaymentMethod");
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
                                        Console.WriteLine("✅ Show Discount Policy Detail");
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
                                            Console.WriteLine("👉 Choose discount Policy for order");
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
                                                Console.WriteLine("✅ Show Discount Policy Detail");
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

                                                    orderBL.Payment(orderWantToPayment);
                                                    Console.WriteLine("Executing Payment...");
                                                    System.Threading.Thread.Sleep(3000);
                                                    Console.WriteLine("Payment Completed! Press Any Key to Back to previous Menu");
                                                    Console.ReadKey();

                                                }
                                                else
                                                {
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
            string orderId = "";
            Order orderdetails = new Order();
            Order orderTemp = new Order("", new DateTime(), new Staff(0, "", "", "", "", "", StaffEnum.Role.Seller, StaffEnum.Status.Active), new Staff(0, "", "", "", "", "", StaffEnum.Role.Accountant, StaffEnum.Status.Active), new Customer(0, "", "", ""), new List<PhoneDetail>(), OrderEnum.Status.Pending, new List<DiscountPolicy>(), "", 0);
            Order order = new Order();
            bool? temp = false;
            do
            {
                switch (currentPhase)
                {
                    case 1:
                        ConsoleUlts.PrintListPhase(listPhase, count, currentPhase);
                        ConsoleUlts.Title(ConsoleUlts.GetAppTitle(), handleTitle, loginManager.LoggedInStaff);
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

                        ConsoleUlts.PrintListPhase(listPhase, count, currentPhase);
                        temp = ListOrderPagination(listOrderTemp, listPhase, count, currentPhase);
                        if (temp == true)
                        {
                            // nhập Id order để xem
                            do
                            {
                                orderId = ConsoleUlts.GetInputString("Enter Order ID").ToUpper();
                                // lấy ra order bằng order ID
                            orderTemp = orderBL.GetOrderById(orderId);
                                if (orderTemp.OrderID == "")
                                {
                                    ConsoleUlts.Alert(ConsoleEnum.Alert.Error, "Invalid Order ID Please Try Again");
                                }
                            } while (orderTemp.OrderID == "");
                            order = orderTemp;
                            currentPhase++;
                        }
                        else if (temp == false)
                        {
                            break;
                        }
                        else if (temp == null)
                        {
                            return -1;
                        }
                        break;
                    case 2:
                        if (order == null) { }
                        ConsoleUlts.PrintListPhase(listPhase, count, currentPhase);
                        ConsoleUlts.PrintOrderDetailsInfo(order);

                        if (!PressYesOrNo("Continue", "Back Previous Phase"))
                        {
                            currentPhase--;
                            break;
                        }
                        else currentPhase++;

                        break;
                    case 3:
                        ConsoleUlts.PrintListPhase(listPhase, count, currentPhase);
                        ConsoleUlts.PrintSellerOrder(order);
                        if (!PressYesOrNo("Confirm Product", "Cancel Order"))
                        {
                            if (orderBL.UpdateOrder(OrderEnum.Status.Canceled, order) == true)
                            {
                                return 0;
                            }
                        }
                        else
                        {
                            // đổi trạng thái Order thành completed
                            if (orderBL.UpdateOrder(OrderEnum.Status.Completed, order) == true)
                            {
                                return 1;
                            }
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
