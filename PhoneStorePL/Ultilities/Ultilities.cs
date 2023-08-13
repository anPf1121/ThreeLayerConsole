using Model;
using BL;
using Interface;
using BusinessEnum;
using GUIEnum;
using System.Diagnostics;
using System;
using System.Reflection.Metadata;
using System.Collections.Generic;
using CustomerDTO;
using UI;

namespace Ults
{
    class Ultilities
    {
        private IStaffBL loginManager;
        public Ultilities(IStaffBL loginManager)
        {
            this.loginManager = loginManager;
        }
        private PhoneBL phoneBL = new PhoneBL();
        private ConsoleUlts ConsoleUlts = new ConsoleUlts();
        private ConsoleUI consoleUI = new ConsoleUI();
        private StaffBL StaffBL = new StaffBL();
        private CustomerBL customerBL = new CustomerBL();
        private OrderBL orderBL = new OrderBL();
        public Staff? orderStaff = null;
        public void CreateOrder()
        {
            int centeredPosition = (Console.WindowWidth - "|--------------------------------------------------------------------------------------------|".Length) / 2;
            string spaces = centeredPosition > 0 ? new string(' ', centeredPosition) : "";
            int secondcenteredPosition = (Console.WindowWidth - "|===================================================================================================|".Length) / 2;
            string secondspaces = secondcenteredPosition > 0 ? new string(' ', secondcenteredPosition) : "";

            string searchTitle = consoleUI.GetSearchANSIText(), phoneInfoToSearch = "", input = "";
            string[] menuSearchChoice = consoleUI.GetMenuItemSearch(), listPhase = consoleUI.GetCreateOrderTimeLine();
            int phoneId = 0, phoneModelID = 0, count = 0, quantityAfterAddMoreHandle = 0, searchChoice = 0, currentPhase = 1, phaseChoice = 0, quantity = 0, reChooseModelAfterBackPrevPhase = 0;
            List<Imei>? imeis = null;
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
                        consoleUI.PrintTimeLine(listPhase, count, currentPhase);
                        consoleUI.PrintTitle(consoleUI.GetAppANSIText(), consoleUI.GetSearchANSIText(), loginManager.LoggedInStaff);
                        searchChoice = ConsoleUlts.PressCharacterTo("Search All Phone", "Search Phone By Information", "Back To Previous Menu");
                        if (searchChoice == 0) listTemp = phoneBL.GetAllPhone();

                        else if (searchChoice == 1)
                        {
                            consoleUI.PrintTimeLine(listPhase, count, currentPhase);
                            consoleUI.PrintTitle(consoleUI.GetAppANSIText(), searchTitle, loginManager.LoggedInStaff);
                            phoneInfoToSearch = ConsoleUlts.GetInputString($"{spaces}Enter Phone Information To Search");
                            listTemp = phoneBL.GetPhonesByInformation(phoneInfoToSearch);
                        }

                        else if (searchChoice == 2) return;

                        if (listTemp.Count() == 0) activeSearchPhone = false;
                        else
                        {
                            listPhoneSearch = ConsoleUlts.ListPhonePagination(listTemp, listPhase, count, currentPhase, loginManager.LoggedInStaff);
                            if (listPhoneSearch == false) break;
                            if (listPhoneSearch == true)
                            {
                                phoneId = ConsoleUlts.InputIDValidation(phoneBL.GetAllPhone().Count(), $"{spaces}Enter Phone ID", "Invalid Phone ID");
                            }
                        }

                        if (ConsoleUlts.PressYesOrNo("Choose Phone Model", "Back To Previous Menu")) currentPhase++;
                        break;
                    case 2:
                        phonedetails = phoneBL.GetPhoneDetailsByPhoneID(phoneId);
                        consoleUI.PrintTimeLine(listPhase, count, currentPhase);
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
                                consoleUI.PrintPhoneDetailsInfo(phonedetails);
                                phoneModelID = ConsoleUlts.GetInputInt($"{secondspaces}Enter Phone Model ID");
                                if (listPhoneDetailID.IndexOf(phoneModelID) == -1)
                                {
                                    ConsoleUlts.Alert(ConsoleEnum.Alert.Error, "Invalid Phone Model ID Please Choice Again");
                                    consoleUI.PrintTimeLine(listPhase, count, currentPhase);
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
                                        reChooseModelAfterBackPrevPhase = ConsoleUlts.PressCharacterTo("Choose Another Phone Model", "Back Previous Phase", "Continue To Create Order");
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
                                        reEnterPhoneModelID = ConsoleUlts.PressYesOrNo("Choose Another Phone Model", "Back Previous Phase");
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
                        if (reChooseModelAfterBackPrevPhase == 2) {
                            currentPhase++;
                            break;
                        } else if (reChooseModelAfterBackPrevPhase == 1 || !reEnterPhoneModelID) {
                            currentPhase--;
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
                        consoleUI.PrintTimeLine(listPhase, count, currentPhase);
                        consoleUI.PrintPhoneDetailsInfo(phonedetails);
                        Console.WriteLine($"{secondspaces}Phone Model ID: " + phoneModelID);
                        quantity = ConsoleUlts.InputIDValidation(phoneDetailQuantity, $"{secondspaces}Enter Phone Model Quantity", "Invalid Phone Model Quantity");
                        ConsoleUlts.Alert(ConsoleEnum.Alert.Success, "Quantity Successfully Added");
                        pDetails.Quantity = quantity;
                        imeis = new List<Imei>();
                        bool isDuplicateImei = false;
                        do
                        {
                            consoleUI.PrintTimeLine(listPhase, count, currentPhase);
                            int idImei = 1;
                            consoleUI.PrintPhoneDetailsInfo(phonedetails);
                            Console.WriteLine($"{secondspaces}Phone Model ID: " + phoneModelID);
                            Console.WriteLine($"{secondspaces}Quantity: " + quantity);
                            foreach (var item in imeis)
                            {
                                Console.WriteLine($"{secondspaces}Imei " + (idImei) + ": " + item.PhoneImei);
                                idImei++;
                            }
                            for (int i = 0; i < quantity; i++)
                            {
                                Imei imei = new Imei("", BusinessEnum.PhoneEnum.ImeiStatus.NotExport);
                                do
                                {
                                    isDuplicateImei = false;
                                    imei.PhoneImei = ConsoleUlts.GetInputString($"{secondspaces}Enter Imei {i + 1}");
                                    foreach (PhoneDetail item in phonesInOrder)
                                    {
                                        isDuplicateImei = phoneBL.CheckImeiIsDuplicateInOrder(imei, item.ListImei);
                                    }
                                    if (!phoneBL.CheckImeiExist(imei, phoneModelID) || isDuplicateImei)
                                    {
                                        if (!phoneBL.CheckImeiExist(imei, phoneModelID))
                                            ConsoleUlts.Alert(ConsoleEnum.Alert.Error, "Imei Not Found");
                                        else if (isDuplicateImei)
                                            ConsoleUlts.Alert(ConsoleEnum.Alert.Error, "You Just Entered This Imei Before, Please Re-enter It");
                                        Console.Clear();
                                        consoleUI.PrintTimeLine(listPhase, count, currentPhase);
                                        consoleUI.PrintPhoneDetailsInfo(phonedetails);
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
                            if (!ConsoleUlts.PressYesOrNo("Continue", "Back Previous Phase"))
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
                        Order ord = new Order("", DateTime.Now, orderStaff, new Staff(0, "", "", "", "", "", StaffEnum.Role.Accountant, StaffEnum.Status.Active), new Customer(0, "", "", ""), phonesInOrder, OrderEnum.Status.Pending, new List<DiscountPolicy>(), "", 0);
                        consoleUI.PrintTimeLine(listPhase, count, currentPhase);
                        ord.Seller = loginManager.LoggedInStaff;
                        consoleUI.PrintSellerOrderBeforePayment(ord);
                        consoleUI.PrintLine();
                        phaseChoice = ConsoleUlts.PressCharacterTo("Back Previous Phase", "Enter Customer Info", "Add More Phone");
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
                        consoleUI.PrintTimeLine(listPhase, count, currentPhase);
                        consoleUI.PrintTitle(consoleUI.GetAppANSIText(), consoleUI.GetCustomerInfoANSIText(), loginManager.LoggedInStaff);
                        customer = ConsoleUlts.GetCustomerInfo();
                        CustomerResultDTO customerWithIsDupp = customerBL.AddCustomer(customer);
                        customer.CustomerID = customerWithIsDupp.CustomerId;
                        customer = customerBL.GetCustomerByID(customer.CustomerID);
                        if (customerWithIsDupp.IsDuplicate)
                            ConsoleUlts.Alert(GUIEnum.ConsoleEnum.Alert.Success, $"Add customer completed with customer id {customer.CustomerID}");
                        else
                            ConsoleUlts.Alert(GUIEnum.ConsoleEnum.Alert.Warning, $"Customer Phone Number Is Already Exsist");

                        if (!ConsoleUlts.PressYesOrNo("Confirm Order", "Back Previous Phase")) currentPhase--;

                        else currentPhase++;

                        break;
                    case 5:
                        consoleUI.PrintTimeLine(listPhase, count, currentPhase);
                        Order order = new Order(ConsoleUlts.GenerateID(), DateTime.Now, loginManager.LoggedInStaff, new Staff(0, "", "", "", "", "", StaffEnum.Role.Accountant, StaffEnum.Status.Active), customer, phonesInOrder, OrderEnum.Status.Pending, new List<DiscountPolicy>(), "", 0);
                        consoleUI.PrintSellerOrderBeforePayment(order);
                        if (ConsoleUlts.PressYesOrNo("Create Order", "Cancel Order"))
                        {
                            bool isCreateOrder = orderBL.CreateOrder(order);
                            ConsoleUlts.Alert(isCreateOrder ? ConsoleEnum.Alert.Success : ConsoleEnum.Alert.Error, isCreateOrder ? "Create Order Completed" : "Create Order Failed");
                        }
                        else
                        {
                            ConsoleUlts.Alert(ConsoleEnum.Alert.Success, "Cancel Order Completed!");
                            phonesInOrder = new List<PhoneDetail>();
                        }
                        currentPhase++;
                        break;
                }
            } while (currentPhase != 6);
        }
        public int HandleOrder()
        {
            int centeredPosition = (Console.WindowWidth - "|--------------------------------------------------------------------------------------------|".Length) / 2;
            string spaces = centeredPosition > 0 ? new string(' ', centeredPosition) : "";
            int secondcenteredPosition = (Console.WindowWidth - "|===================================================================================================|".Length) / 2;
            string secondspaces = secondcenteredPosition > 0 ? new string(' ', secondcenteredPosition) : "";
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            string handleTitle = consoleUI.GetHandleOrderANSIText();
            string[] listPhase = { "Show orders", "Confirm Handle" };
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
                        consoleUI.PrintTimeLine(listPhase, count, currentPhase);
                        consoleUI.PrintTitle(consoleUI.GetAppANSIText(), handleTitle, loginManager.LoggedInStaff);
                        handleChoice = ConsoleUlts.PressCharacterTo("Show orders have confirmed status in day", "Back To Previous Menu", null);
                        switch (handleChoice)
                        {
                            case 0:
                                listOrderTemp = orderBL.GetOrdersInDay(OrderEnum.Status.Confirmed);
                                break;
                            case 1:
                                break;
                        }
                        if (handleChoice == 1) return 2;

                        consoleUI.PrintTimeLine(listPhase, count, currentPhase);
                        temp = ConsoleUlts.ListOrderPagination(listOrderTemp, listPhase, count, currentPhase, loginManager.LoggedInStaff);
                        if (temp == true)
                        {
                            // nhập Id order để xem
                            do
                            {
                                orderId = ConsoleUlts.GetInputString($"{spaces}Enter Order ID").ToUpper();
                                // lấy ra order bằng order ID
                                orderTemp = orderBL.GetOrderById(orderId);
                                if (orderTemp.OrderID == "")
                                {
                                    ConsoleUlts.Alert(ConsoleEnum.Alert.Error, "Invalid Order ID Please Try Again");
                                }
                            } while (orderTemp.OrderID == "");
                            order = orderTemp;
                            order.Seller = loginManager.LoggedInStaff;
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
                        consoleUI.PrintTimeLine(listPhase, count, currentPhase);
                        consoleUI.PrintOrderInfor(order);
                        if (!ConsoleUlts.PressYesOrNo("Confirm Product", "Cancel Order"))
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
            } while (currentPhase != 3);
            return 1;
        }
        public void CreateReport()
        {
            int count = 0, centeredPosition = (Console.WindowWidth - "|--------------------------------------------------------------------------------------------|".Length) / 2;
            string spaces = centeredPosition > 0 ? new string(' ', centeredPosition) : "";
            string[] timeLine = consoleUI.GetCreateReportTimeLine();
            string revenueTitle = consoleUI.GetSearchANSIText();
            string[] revenueItem = { "Revenue On Phone Model In Month", "Revenue On Phone Model In Week", "Revenue On Phone Model In Day", "Back To Previous Menu" };
            List<Phone> phones = new List<Phone>();
            List<PhoneDetail> phoneDetails = new List<PhoneDetail>();
            int phoneDetailCount = 0;
            List<Order>? orders = null;
            List<Order>? ordersStore = null;
            bool isContinue = false;
            int choice = 0, currentPhase = 1, searchChoice = 0, phoneIdToAdd = 0, phoneModelIDToAdd = 0;
            List<int> phoneDetailsIDToReport = new List<int>();
            Dictionary<int, decimal> reportDataHandle = new Dictionary<int, decimal>();
            DateTime startDate = new DateTime(), endDate = new DateTime();
            do
            {
                switch (currentPhase)
                {
                    case 1:
                        consoleUI.PrintTimeLine(timeLine, count, currentPhase);
                        consoleUI.PrintTitle(consoleUI.GetAppANSIText(), null, loginManager.LoggedInStaff);
                        int firstPhaseChoice = ConsoleUlts.PressCharacterTo("Create Report In Day", "Create Report In Week", "Create Report In Month");

                        if (firstPhaseChoice == 0)
                        {
                            orders = orderBL.GetCompletedOrderInDay(loginManager.LoggedInStaff);
                            ordersStore = orderBL.GetCompletedOrderInDay();
                        }

                        else if (firstPhaseChoice == 1)
                        {
                            orders = orderBL.GetCompletedOrderInWeek(loginManager.LoggedInStaff);
                            ordersStore = orderBL.GetCompletedOrderInWeek();
                        }

                        else
                        {
                            orders = orderBL.GetCompletedOrderInMonth(loginManager.LoggedInStaff);
                            ordersStore = orderBL.GetCompletedOrderInMonth();
                        }

                        if (orders == null || orders.Count() == 0)
                        {
                            ConsoleUlts.Alert(ConsoleEnum.Alert.Warning, "Don't Have Revenue In This Month");
                            break;
                        }
                        else currentPhase++;
                        break;
                    case 2:
                        consoleUI.PrintTimeLine(timeLine, count, currentPhase);
                        consoleUI.PrintTitle(consoleUI.GetAppANSIText(), consoleUI.GetCreateReportANSIText(), loginManager.LoggedInStaff);
                        foreach (var ord in orders)
                        {
                            foreach (var item in ord.PhoneDetails)
                            {
                                phoneDetailCount++;
                            }
                        }
                        consoleUI.PrintReportRevenue(loginManager.LoggedInStaff, orderBL.CalculateTotalRevenue(orders!), orderBL.CalculateTotalRevenue(ordersStore!), orders!.Count(), ordersStore!.Count(), ordersStore!, phoneDetailsIDToReport);

                        int reportChoice = ConsoleUlts.PressCharacterTo("Cancel Report", "Create Report", "Add More Information To Report");
                        if (reportChoice == 0) currentPhase = timeLine.Count() + 1;
                        else if (reportChoice == 1)
                        {
                            currentPhase = timeLine.Count() + 1;
                            ConsoleUlts.Alert(ConsoleEnum.Alert.Success, "Create Report Completed!");
                        }
                        else if (reportChoice == 2) currentPhase++;
                        break;
                    case 3:
                        consoleUI.PrintTimeLine(timeLine, count, currentPhase);
                        consoleUI.PrintTitle(consoleUI.GetAppANSIText(), consoleUI.GetCreateReportANSIText(), loginManager.LoggedInStaff);

                        searchChoice = ConsoleUlts.PressCharacterTo("Search All Phone", "Search Phone By Information", "Back To Previous Menu");
                        if (searchChoice == 0) phones = phoneBL.GetAllPhone();

                        else if (searchChoice == 1)
                        {
                            consoleUI.PrintTimeLine(timeLine, count, currentPhase);
                            consoleUI.PrintTitle(consoleUI.GetAppANSIText(), consoleUI.GetCreateReportANSIText(), loginManager.LoggedInStaff);
                            string phoneInfoToSearch = ConsoleUlts.GetInputString($"{spaces}Enter Phone Information To Search");
                            phones = phoneBL.GetPhonesByInformation(phoneInfoToSearch);
                        }

                        else if (searchChoice == 2) return;

                        if (phones.Count() == 0) return;
                        else
                        {
                            List<int> listPhoneDetailID = new List<int>();
                            bool listPhoneSearch = ConsoleUlts.ListPhonePagination(phones, timeLine, count, currentPhase, loginManager.LoggedInStaff);
                            phoneIdToAdd = ConsoleUlts.InputIDValidation(phoneBL.GetAllPhone().Count(), $"{spaces}Enter Phone ID", "Invalid Phone ID");
                            phoneDetails = phoneBL.GetPhoneDetailsByPhoneID(phoneIdToAdd);
                            foreach (PhoneDetail item in phoneDetails)
                                listPhoneDetailID.Add(item.PhoneDetailID);
                            do
                            {
                                Console.Clear();
                                consoleUI.PrintTimeLine(timeLine, count, currentPhase);
                                consoleUI.PrintTitle(consoleUI.GetAppANSIText(), consoleUI.GetCreateReportANSIText(), loginManager.LoggedInStaff);
                                consoleUI.PrintPhoneDetailsInfo(phoneDetails);
                                phoneModelIDToAdd = ConsoleUlts.GetInputInt($"{spaces}Enter Phone Model ID");
                                if (listPhoneDetailID.IndexOf(phoneModelIDToAdd) == -1)
                                {
                                    if (listPhoneDetailID.IndexOf(phoneModelIDToAdd) == -1)
                                        ConsoleUlts.Alert(ConsoleEnum.Alert.Error, "Invalid Phone Model ID Please Choice Again");
                                    consoleUI.PrintTimeLine(timeLine, count, currentPhase);
                                }
                                else
                                {
                                    phoneDetailsIDToReport.Add(phoneModelIDToAdd);
                                }
                            } while (listPhoneDetailID.IndexOf(phoneModelIDToAdd) == -1);
                        }
                        currentPhase--;

                        break;
                }
            } while (currentPhase != timeLine.Count() + 1);
        }
        public void TradeIn()
        {
            int centeredPosition = (Console.WindowWidth - "|========================================================================================================|".Length) / 2;
            string spaces = new string(' ', centeredPosition);
            string[] tradeInItems = { "Show Tradein Table Details", "Check Customer's Phone", "Phone Quotes", "Confirm TradeIn" };
            List<int> choicePattern = new List<int>();
            string input = "";
            ConsoleKeyInfo keyInfo = new ConsoleKeyInfo();
            bool activeShowtable = true;
            do
            {
                consoleUI.PrintTimeLine(tradeInItems, 0, 1);
                consoleUI.PrintTradeInTable();
                Console.Write(spaces + "Press any key to continue...");
                Console.ReadKey();
                if (activeShowtable == true)
                {
                    bool activeCheckPhone = false;
                    choicePattern = new List<int>();
                    do
                    {
                        consoleUI.PrintTimeLine(tradeInItems, 0, 2);
                        Console.WriteLine(spaces + "      |============================================================================================|");
                        Console.WriteLine(consoleUI.GetAppANSIText());
                        Console.WriteLine(spaces + "      |============================================================================================|");
                        Console.WriteLine(consoleUI.GetTradeInANSIText());
                        Console.WriteLine(spaces + "      |============================================================================================|");
                        Console.WriteLine(consoleUI.GetCheckCustomerPhoneANSIText());
                        Console.WriteLine(spaces + "      |============================================================================================|");
                        Console.Write(spaces + "Input Customer's Phone Information: ");
                        input = Console.ReadLine() ?? "";
                        List<Phone> phoneResult = phoneBL.GetPhonesByInformation(input);
                        foreach (var p in phoneResult)
                        {
                            choicePattern.Add(p.PhoneID);
                        }
                        bool showListPhone = ConsoleUlts.ListPhonePaginationForTradeIn(phoneResult, tradeInItems, 0, 2, this.orderStaff);
                        Console.Write(spaces + "Choose a Phone By ID: ");
                        input = Console.ReadLine() ?? "";
                        while (!ConsoleUlts.CheckInputIDValid(input, choicePattern))
                        {
                            Console.Write(spaces + "Input again: ");
                            input = Console.ReadLine() ?? "";
                        }
                        choicePattern = new List<int>();
                        Phone phoneInfo = phoneBL.GetPhoneById(Convert.ToInt32(input));
                        List<PhoneDetail> ListPhoneDetailResult = phoneBL.GetPhoneDetailsByPhoneID(phoneInfo.PhoneID);
                        foreach (var p in ListPhoneDetailResult)
                        {
                            choicePattern.Add(p.PhoneDetailID);
                        }
                        new ConsoleUlts().ListPhoneModelTradeInPagination(ListPhoneDetailResult, tradeInItems, 0, 2, this.orderStaff);
                        Console.WriteLine(spaces + "Choose a Phone Model corresponding to Customer's Phone Information");
                        Console.Write(spaces + "Choose a Phone Model By Detail ID: ");
                        input = Console.ReadLine() ?? "";
                        while (!ConsoleUlts.CheckInputIDValid(input, choicePattern))
                        {
                            Console.Write(spaces + "Input again: ");
                            input = Console.ReadLine() ?? "";
                        }
                        PhoneDetail phoneDetailForTradeIn = phoneBL.GetPhoneDetailByID(Convert.ToInt32(input));
                        Console.WriteLine(spaces + "Are you sure that all Information of this Phone are the same with Customer's Phone Information?");
                        Console.Write(spaces + "Press 'Enter' to Keep doing(All information are same) OR Any Key to Choose another Phone(Not the same).");
                        keyInfo = Console.ReadKey();
                        Console.Clear();
                        if (keyInfo.Key == ConsoleKey.Enter)
                        {
                            activeCheckPhone = true;
                        }
                        else
                        {
                            continue;
                        }
                        if (activeCheckPhone == true)
                        {
                            bool activePhoneQuotes = false;
                            do
                            {
                                consoleUI.PrintTimeLine(tradeInItems, 0, 3);
                                Console.WriteLine(spaces + "      |============================================================================================|");
                                Console.WriteLine(consoleUI.GetAppANSIText());
                                Console.WriteLine(spaces + "      |============================================================================================|");
                                Console.WriteLine(consoleUI.GetTradeInANSIText());
                                Console.WriteLine(spaces + "      |============================================================================================|");
                                Console.WriteLine(consoleUI.GetPhoneQuotesANSIText());
                                Console.WriteLine(spaces + "      |============================================================================================|");
                                Console.Write(spaces + "      " + $"|Your Phone Status is: {phoneDetailForTradeIn.PhoneStatusType}");
                                int k = ("|============================================================================================|").Length - (phoneDetailForTradeIn.PhoneStatusType + "|Your Phone Status is: ").Length;
                                for (int i = 0; i < k - 2; i++) Console.Write(" ");
                                Console.WriteLine(" |");
                                Console.Write(spaces + "      " + $"|TradeIn Price: {phoneDetailForTradeIn.Price}");
                                k = ("|============================================================================================|").Length - (phoneDetailForTradeIn.Price + "|TradeIn Price: ").Length;
                                for (int i = 0; i < k - 2; i++) Console.Write(" ");
                                Console.WriteLine(" |");
                                DiscountPolicy discountForTradeIn = new DiscountPolicyBL().GetDiscountTradeInForPhone(phoneDetailForTradeIn);
                                Console.Write(spaces + "      " + $"|Discount TradeIn: {discountForTradeIn.Title}");
                                k = ("|============================================================================================|").Length - (discountForTradeIn.Title + "|Discount TradeIn: ").Length;
                                for (int i = 0; i < k - 2; i++) Console.Write(" ");
                                Console.WriteLine(" |");
                                Console.Write(spaces + "      " + $"|Money Supported: {discountForTradeIn.MoneySupported}");
                                k = ("|============================================================================================|").Length - (discountForTradeIn.MoneySupported + "|Money Supported: ").Length;
                                for (int i = 0; i < k - 2; i++) Console.Write(" ");
                                Console.WriteLine(" |");
                                Console.Write(spaces + "      " + $"|Total Customer's Receive: {phoneDetailForTradeIn.Price + discountForTradeIn.MoneySupported}");
                                k = ("|============================================================================================|").Length - (discountForTradeIn.MoneySupported + phoneDetailForTradeIn.Price + "|Total Customer's Receive: ").Length;
                                for (int i = 0; i < k - 2; i++) Console.Write(" ");
                                Console.WriteLine(" |");
                                Console.WriteLine(spaces + "      |============================================================================================|");
                                Console.Write(spaces + "      " + "Press 'Enter' to TradeIn OR Any Key to Cancel TradeIn");
                                keyInfo = Console.ReadKey();
                                if (keyInfo.Key == ConsoleKey.Enter)
                                {
                                    Console.WriteLine("TradeIn Completed!");
                                    activePhoneQuotes = true;
                                }
                                else
                                {
                                    Console.WriteLine("TradeIn False");
                                    break;
                                }
                            } while (activePhoneQuotes == false);
                        }

                    } while (activeCheckPhone == false);
                }
            } while (activeShowtable == false);
        }
        public void Payment()
        {
            int centeredPosition = (Console.WindowWidth - "|--------------------------------------------------------------------------------------------|".Length) / 2;
            string spaces = centeredPosition > 0 ? new string(' ', centeredPosition) : "";
            int secondcenteredPosition = (Console.WindowWidth - "|===================================================================================================|".Length) / 2;
            string secondspaces = secondcenteredPosition > 0 ? new string(' ', secondcenteredPosition) : "";

            string[] listPhase = consoleUI.GetPaymentTimeLine();
            string input = "";
            int count = 0;
            int currentPhase = 1;
            List<Order> ListOrderPending = new OrderBL().GetOrdersPendingInday();
            List<DiscountPolicy> ListDiscountPolicyValidToOrder = new List<DiscountPolicy>();
            List<int> choicePattern = new List<int>();
            List<string> orderChoicePattern = new List<string>();
            Dictionary<int, string> ListPaymentMethod = new Dictionary<int, string>();
            ListPaymentMethod.Add(1, "VNPay");
            ListPaymentMethod.Add(2, "Banking");
            ListPaymentMethod.Add(3, "Cash");
            int choice = 0;
            string orderID = "";
            ConsoleKeyInfo keyInfo = new ConsoleKeyInfo();
            Order? orderWantToPayment = new Order("", new DateTime(), new Staff(0, "", "", "", "", "", StaffEnum.Role.Seller, StaffEnum.Status.Active), new Staff(0, "", "", "", "", "", StaffEnum.Role.Accountant, StaffEnum.Status.Active), new Customer(0, "", "", ""), new List<PhoneDetail>(), OrderEnum.Status.Pending, new List<DiscountPolicy>(), "", 0);
            bool activePayment = true;
            do
            {
                currentPhase = 1;
                bool dontKnowHowtoCall1 = false;
                bool? showOrderList = ConsoleUlts.ListOrderPagination(ListOrderPending, listPhase, count, currentPhase, loginManager.LoggedInStaff);
                if (showOrderList == null)
                {
                    ConsoleUlts.Alert(ConsoleEnum.Alert.Error, "NO ORDER EXIST");
                    break;
                }
                else if (showOrderList == true)
                {
                    do
                    {
                        orderID = ConsoleUlts.GetInputString($"{spaces}Choose An Order ID To Payment").ToUpper();
                        orderWantToPayment = new OrderBL().GetOrderById(orderID) ?? null;
                        if (orderWantToPayment.OrderID == "")
                        {
                            ConsoleUlts.Alert(ConsoleEnum.Alert.Error, "Invalid Order ID");
                        }
                        else orderWantToPayment.Accountant = this.loginManager.LoggedInStaff;
                    } while (orderWantToPayment.OrderID == "");

                    //Wait to display orderdetail
                    consoleUI.PrintSellerOrderBeforePayment(orderWantToPayment);
                    if (orderWantToPayment.PhoneDetails.Count() == 0)
                    {
                        ConsoleUlts.Alert(ConsoleEnum.Alert.Error, "Order doesn't have any phone!");
                        break;
                    }
                    bool resultContinueOrChooseAgain = ConsoleUlts.PressYesOrNo("Continue Payment", "Choose Order Again");
                    if (resultContinueOrChooseAgain == true)
                    {
                        choicePattern = new List<int>();
                        bool activeChoosePaymentMethod = false;
                        do
                        {
                            int inputPaymentMethodChoice = 0;
                            currentPhase = 2;
                            consoleUI.PrintTimeLine(listPhase, count, currentPhase);
                            // hiển thị các Payment Method (phương thức thanh toán)
                            Console.WriteLine(spaces + "|============================================================================================|");
                            Console.WriteLine(consoleUI.GetAppANSIText());
                            Console.WriteLine(spaces + "|============================================================================================|");
                            Console.WriteLine(consoleUI.GetPaymentANSIText());
                            Console.WriteLine(spaces + "|============================================================================================|");
                            Console.WriteLine(consoleUI.GetChoosePaymentMethodText());
                            Console.WriteLine(spaces + "|============================================================================================|");

                            foreach (var payment in ListPaymentMethod)
                            {
                                Console.Write(spaces + "|" + payment.Key + ". " + payment.Value + $"{spaces}");
                                int k = ("|============================================================================================|").Length - (spaces + payment.Key + ". " + payment.Value).Length;
                                for (int i = 0; i < k - 2; i++) Console.Write(" ");
                                Console.WriteLine("|");

                            }
                            Console.WriteLine(spaces + "|============================================================================================|");
                            do
                            {
                                inputPaymentMethodChoice = ConsoleUlts.GetInputInt(spaces + "Choose a Payment Method");
                                if (inputPaymentMethodChoice <= 0 || inputPaymentMethodChoice > ListPaymentMethod.Count())
                                {
                                    ConsoleUlts.Alert(ConsoleEnum.Alert.Error, "Invalid Payment Method");
                                }
                                else orderWantToPayment.PaymentMethod = ListPaymentMethod[inputPaymentMethodChoice];

                            } while (inputPaymentMethodChoice <= 0 || inputPaymentMethodChoice > ListPaymentMethod.Count());

                            foreach (var payment in ListPaymentMethod)
                            {
                                if (payment.Key == inputPaymentMethodChoice) orderWantToPayment.PaymentMethod = payment.Value;
                            }
                            ListDiscountPolicyValidToOrder = new DiscountPolicyBL().GetDiscountValidToOrder(orderWantToPayment);

                            bool resultContinueOrChooseMethodAgain = ConsoleUlts.PressYesOrNo("Continue", "Choose Payment Method Again");
                            if (resultContinueOrChooseMethodAgain == true)
                            {
                                activeChoosePaymentMethod = true;
                                choicePattern = new List<int>();
                                bool activeDiscountForPayment = false;
                                do
                                {
                                    int count1 = 0;
                                    currentPhase = 3;
                                    consoleUI.PrintTimeLine(listPhase, count, currentPhase);
                                    Console.WriteLine(spaces + "|============================================================================================|");
                                    Console.WriteLine(consoleUI.GetAppANSIText());
                                    Console.WriteLine(spaces + "|============================================================================================|");
                                    Console.WriteLine(consoleUI.GetPaymentANSIText());
                                    Console.WriteLine(spaces + "|============================================================================================|");
                                    Console.WriteLine(consoleUI.GetChooseDiscountPolicyText());
                                    foreach (var discount in ListDiscountPolicyValidToOrder)
                                    {
                                        if (orderWantToPayment.PaymentMethod.Equals(discount.PaymentMethod))
                                        {
                                            choicePattern.Add(discount.PolicyID);
                                            int l = ("|============================================================================================|").Length - (spaces + discount.PolicyID + ". " + discount.Title).Length;
                                            Console.Write(spaces + $"|" + discount.PolicyID + ". " + discount.Title + spaces);
                                            for (int i = 0; i < l - 2; i++) Console.Write(" ");
                                            Console.WriteLine("|");
                                            count1++;
                                        }
                                    }
                                    Console.WriteLine(spaces + "|============================================================================================|");
                                    if (choicePattern.Count() != 0)
                                    {
                                        string ChoiceDiscountPayment = "";
                                        do
                                        {
                                            ChoiceDiscountPayment = ConsoleUlts.GetInputString($"{spaces}Choose discount policy for Payment Method");
                                        } while (!ConsoleUlts.CheckInputIDValid(ChoiceDiscountPayment, choicePattern));
                                        choice = Convert.ToInt32(ChoiceDiscountPayment);
                                        Console.WriteLine(spaces + "|============================================================================================|");
                                        Console.WriteLine(consoleUI.GetAppANSIText());
                                        Console.WriteLine(spaces + "|============================================================================================|");
                                        Console.WriteLine(consoleUI.GetPaymentANSIText());
                                        Console.WriteLine(spaces + "|============================================================================================|");
                                        consoleUI.PrintTitle(consoleUI.GetDiscountPolicyDetailText(), null, loginManager.LoggedInStaff);
                                        consoleUI.PrintDiscountPolicyDetail(new DiscountPolicyBL().GetDiscountPolicyByID(choice));
                                    }
                                    else
                                    {
                                        ConsoleUlts.Alert(ConsoleEnum.Alert.Error, "No Discount Policy Valid To This Payment Method");
                                    }

                                    Console.WriteLine();
                                    bool resultDiscount = ConsoleUlts.PressYesOrNo("Continue", "Choose Discount Policy Again");

                                    if (resultDiscount == true)
                                    {
                                        activeDiscountForPayment = true;
                                        if (count1 != 0) orderWantToPayment.DiscountPolicies.Add(new DiscountPolicyBL().GetDiscountPolicyByID(choice));
                                        choicePattern = new List<int>();
                                        int count2 = 0;
                                        bool activeConfirmOrCancel = false;
                                        do
                                        {
                                            currentPhase = 4;
                                            consoleUI.PrintTimeLine(listPhase, count, currentPhase);
                                            Console.WriteLine(spaces + "|============================================================================================|");
                                            Console.WriteLine(consoleUI.GetAppANSIText());
                                            Console.WriteLine(spaces + "|============================================================================================|");
                                            Console.WriteLine(consoleUI.GetPaymentANSIText());
                                            Console.WriteLine(spaces + "|============================================================================================|");
                                            Console.WriteLine(consoleUI.GetChooseDiscountPolicyText());
                                            foreach (var discount in ListDiscountPolicyValidToOrder)
                                            {
                                                if (discount.MinimumPurchaseAmount > 0)
                                                {
                                                    if (orderWantToPayment.TotalDue >= discount.MinimumPurchaseAmount && discount.PaymentMethod == "Not Have")
                                                    {
                                                        choicePattern.Add(discount.PolicyID);
                                                        int l = ("|============================================================================================|").Length - (spaces + discount.PolicyID + ". " + discount.Title).Length;
                                                        Console.Write(spaces + $"|" + discount.PolicyID + ". " + discount.Title + spaces);
                                                        for (int i = 0; i < l - 2; i++) Console.Write(" ");
                                                        Console.WriteLine("|");
                                                        count2++;
                                                    }
                                                }
                                            }
                                            Console.WriteLine(spaces + "|============================================================================================|");
                                            if (choicePattern.Count() != 0)
                                            {
                                                string choiceDiscountOrder = "";
                                                do
                                                {
                                                    choiceDiscountOrder = ConsoleUlts.GetInputString($"{spaces} Choose Discount Policy for Order");
                                                } while (!ConsoleUlts.CheckInputIDValid(choiceDiscountOrder, choicePattern));
                                                choice = Convert.ToInt32(choiceDiscountOrder);
                                                Console.WriteLine($"{spaces}✅ Show Discount Policy Detail");
                                                consoleUI.PrintTimeLine(listPhase, count, currentPhase);
                                                consoleUI.PrintTitle(consoleUI.GetDiscountPolicyDetailText(), null, loginManager.LoggedInStaff);
                                                consoleUI.PrintDiscountPolicyDetail(new DiscountPolicyBL().GetDiscountPolicyByID(choice));
                                                if (count2 != 0) orderWantToPayment.DiscountPolicies.Add(new DiscountPolicyBL().GetDiscountPolicyByID(choice));
                                            }
                                            else
                                            {
                                                ConsoleUlts.Alert(ConsoleEnum.Alert.Error, "No Discount Policy valid to this order");
                                            }
                                            Console.WriteLine();
                                            resultDiscount = ConsoleUlts.PressYesOrNo("Continue", "Choose Discount Policy for Order Again");
                                            // if (keyInfo.Key == ConsoleKey.Enter)
                                            // {
                                            //     dontKnowHowtoCall4 = true;
                                            // }
                                            // else
                                            // {
                                            //     continue;
                                            // }
                                            if (resultDiscount == true)
                                            {
                                                activeConfirmOrCancel = true;
                                                bool choiceFinishPayment = false;
                                                currentPhase = 5;
                                                consoleUI.PrintTimeLine(listPhase, count, currentPhase);
                                                consoleUI.PrintOrderInfor(orderWantToPayment); // edit here
                                                choiceFinishPayment = ConsoleUlts.PressYesOrNo("Confirm Order", "Cancel Order");
                                                if (choiceFinishPayment == true)
                                                {
                                                    orderBL.Payment(orderWantToPayment);
                                                    Console.WriteLine($"{spaces}Executing Payment...");
                                                    System.Threading.Thread.Sleep(1000);
                                                    ConsoleUlts.Alert(ConsoleEnum.Alert.Success, "Payment Completed");

                                                }
                                                else if (keyInfo.Key == ConsoleKey.Escape)
                                                {
                                                    orderBL.CancelPayment(orderWantToPayment);
                                                    Console.WriteLine($"{spaces}Executing Cancel...");
                                                    System.Threading.Thread.Sleep(1000);
                                                    ConsoleUlts.Alert(ConsoleEnum.Alert.Success, "Cancel Order Completed");

                                                }
                                            }
                                        } while (activeDiscountForPayment == false);
                                    }
                                } while (activeDiscountForPayment == false);

                            }
                        } while (activeChoosePaymentMethod == false);
                    }
                }
                else break;
            } while (activePayment == false);
        }
    }
}
