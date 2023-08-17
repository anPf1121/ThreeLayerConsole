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
using System.Net.Mail;

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
        private CustomerBL customerBL = new CustomerBL();
        private OrderBL orderBL = new OrderBL();
        public void CreateOrder()
        {
            int centeredPosition = (Console.WindowWidth - "|--------------------------------------------------------------------------------------------|".Length) / 2;
            string spaces = centeredPosition > 0 ? new string(' ', centeredPosition) : "";
            int secondcenteredPosition = (Console.WindowWidth - "|===================================================================================================|".Length) / 2;
            string secondspaces = secondcenteredPosition > 0 ? new string(' ', secondcenteredPosition) : "";
            string orderGenerateID = ConsoleUlts.GenerateID();
            string searchTitle = consoleUI.GetSearchANSIText(), phoneInfoToSearch = "";
            string[] menuSearchChoice = consoleUI.GetMenuItemSearch(), listPhase = consoleUI.GetCreateOrderTimeLine();
            int phoneId = 0, phoneModelID = 0, count = 0, searchChoice = 0, currentPhase = 1, phaseChoice = 0, quantity = 0, reChooseModelAfterBackPrevPhase = 0;
            List<Imei>? imeis = null;
            List<int>? listAllPhonesID = new List<int>();
            bool listPhoneSearch = false;
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

                        if (listTemp.Count() == 0)
                        {
                            ConsoleUlts.Alert(ConsoleEnum.Alert.Error, "Phone not found");
                            break;
                        }
                        else
                        {
                            listPhoneSearch = ConsoleUlts.ListPhonePagination(listTemp, listPhase, count, currentPhase, loginManager.LoggedInStaff);
                            if (listPhoneSearch == false) break;
                            if (listPhoneSearch == true)
                            {
                                phoneId = ConsoleUlts.InputIDValidation(phoneBL.GetAllPhone().Count(), $"Enter Phone ID", "Invalid Phone ID", spaces);
                            }
                        }

                        if (ConsoleUlts.PressYesOrNo("Choose Phone Model", "Back To Previous Menu")) currentPhase++;
                        break;
                    case 2:
                        phonedetails = phoneBL.GetPhoneDetailsByPhoneID(phoneId);
                        consoleUI.PrintTimeLine(listPhase, count, currentPhase);
                        if (phonesInOrder!.Count() != 0 || phonesInOrder != null)
                        {
                            Dictionary<int, int> phoneDetailsIDWithQtt = new Dictionary<int, int>();
                            foreach (PhoneDetail item in phonesInOrder!)
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
                                phoneModelID = ConsoleUlts.GetInputInt($"{spaces}Enter Phone Model ID");
                                if (listPhoneDetailID.IndexOf(phoneModelID) == -1)
                                {
                                    ConsoleUlts.Alert(ConsoleEnum.Alert.Error, "Invalid Phone Model ID Please Choice Again");
                                    consoleUI.PrintTimeLine(listPhase, count, currentPhase);
                                }
                            } while (listPhoneDetailID.IndexOf(phoneModelID) == -1);

                            int quantityAfterBackPrevPhase = 1;

                            if (phonesInOrder!.Count() != 0 || phonesInOrder != null)
                                foreach (PhoneDetail pd in phonesInOrder!)
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
                        if (reChooseModelAfterBackPrevPhase == 2)
                        {
                            currentPhase++;
                            break;
                        }
                        else if (reChooseModelAfterBackPrevPhase == 1 || !reEnterPhoneModelID)
                        {
                            currentPhase--;
                            break;
                        }

                        int phoneDetailQuantity = 0;
                        pDetails = new PhoneBL().GetPhoneDetailByID(phoneModelID);
                        if (phonesInOrder!.Count() != 0)
                        {
                            foreach (PhoneDetail pd in phonesInOrder!)
                                if (pd.PhoneDetailID == phoneModelID)
                                    phoneDetailQuantity = pDetails.Quantity - pd.Quantity;
                                else
                                    phoneDetailQuantity = pDetails.Quantity;
                        }
                        else phoneDetailQuantity = pDetails.Quantity;

                        Console.Clear();
                        consoleUI.PrintTimeLine(listPhase, count, currentPhase);
                        consoleUI.PrintPhoneDetailsInfo(phonedetails);
                        Console.WriteLine($"{spaces}Phone Model ID: " + phoneModelID);
                        quantity = ConsoleUlts.InputIDValidation(phoneDetailQuantity, $"Enter Phone Model Quantity", "Invalid Phone Model Quantity", spaces);
                        ConsoleUlts.Alert(ConsoleEnum.Alert.Success, "Quantity Successfully Added");
                        pDetails.Quantity = quantity;
                        imeis = new List<Imei>();
                        do
                        {
                            consoleUI.PrintTimeLine(listPhase, count, currentPhase);
                            int idImei = 1;
                            consoleUI.PrintPhoneDetailsInfo(phonedetails);
                            Console.WriteLine($"{spaces}Phone Model ID: " + phoneModelID);
                            Console.WriteLine($"{spaces}Quantity: " + quantity);
                            foreach (var item in imeis)
                            {
                                Console.WriteLine($"{spaces}Imei " + (idImei) + ": " + item.PhoneImei);
                                idImei++;
                            }
                            for (int i = 0; i < quantity; i++)
                            {
                                Imei imei = new Imei("", BusinessEnum.PhoneEnum.ImeiStatus.NotExport);
                                do
                                {
                                    imei.PhoneImei = ConsoleUlts.GetInputString($"{spaces}Enter Imei {i + 1}");
                                    if (!phoneBL.CheckImeiExist(imei, phoneModelID))
                                    {
                                        ConsoleUlts.Alert(ConsoleEnum.Alert.Error, "Imei Not Found");
                                        Console.Clear();
                                        consoleUI.PrintTimeLine(listPhase, count, currentPhase);
                                        ConsoleUlts.ListPhoneModelTradeInPagination(phonedetails, listPhase, 0, 2, loginManager.LoggedInStaff);
                                        Console.WriteLine(spaces + "Phone Model ID: " + phoneModelID);
                                        Console.WriteLine(spaces + "Quantity: " + quantity);
                                        idImei = 1;
                                        foreach (var item in imeis)
                                        {
                                            Console.WriteLine(spaces + "Imei " + (idImei) + ": " + item.PhoneImei);
                                            idImei++;
                                        }
                                    }
                                    else
                                    {
                                        ConsoleUlts.Alert(ConsoleEnum.Alert.Success, "Imei Successfully Added");
                                        imeis.Add(imei);
                                    }
                                } while (!phoneBL.CheckImeiExist(imei, phoneModelID));
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
                            pDetails!.ListImei = imeis!;
                            foreach (PhoneDetail pd in phonesInOrder!)
                                if (pDetails.PhoneDetailID == pd.PhoneDetailID)
                                {
                                    pd.Quantity += pDetails.Quantity;
                                    pd.ListImei.AddRange(imeis!);
                                    isDuplicate = true;
                                }
                            if (!isDuplicate) phonesInOrder.Add(pDetails);
                        }
                        Order ord = new Order("", DateTime.Now, loginManager.LoggedInStaff, new Staff(0, "", "", "", "", "", StaffEnum.Role.Accountant, StaffEnum.Status.Active), new Customer(0, "", "", ""), phonesInOrder!, OrderEnum.Status.Pending, new List<DiscountPolicy>(), "", 0);
                        ord.OrderID = orderGenerateID;
                        consoleUI.PrintTimeLine(listPhase, count, currentPhase);
                        ord.Seller = loginManager.LoggedInStaff;
                        consoleUI.PrintOrder(ord);
                        consoleUI.PrintLine();
                        phaseChoice = ConsoleUlts.PressCharacterTo("Back Previous Phase", "Enter Customer Info", "Add More Phone");
                        if (phaseChoice == 0)
                        {
                            for (var i = 0; i < phonesInOrder!.Count(); i++)
                                phonesInOrder![i].ListImei.RemoveAll(item => pDetails!.ListImei.Contains(item));
                            foreach (PhoneDetail pd in phonesInOrder!)
                                if (pDetails!.PhoneDetailID == pd.PhoneDetailID)
                                    pd.Quantity -= pDetails.Quantity;
                            currentPhase--;
                        }
                        else if (phaseChoice == 1) currentPhase++;
                        else
                        {
                            currentPhase = 1;
                        }
                        break;
                    case 4:
                        consoleUI.PrintTimeLine(listPhase, count, currentPhase);
                        consoleUI.PrintTitle(consoleUI.GetAppANSIText(), consoleUI.GetCustomerInfoANSIText(), loginManager.LoggedInStaff);
                        customer = ConsoleUlts.GetCustomerInfo();
                        customer.CustomerID = customerBL.CheckCustomerIsExist(customer);

                        if (customer.CustomerID == 0)
                            ConsoleUlts.Alert(GUIEnum.ConsoleEnum.Alert.Success, $"Add customer completed");
                        else
                        {
                            customer = customerBL.GetCustomerByID(customer.CustomerID);
                            ConsoleUlts.Alert(GUIEnum.ConsoleEnum.Alert.Warning, $"Customer Phone Number Is Already Exsist, Customer ID: {customer.CustomerID}");
                        }

                        if (!ConsoleUlts.PressYesOrNo("Confirm Order", "Back Previous Phase")) currentPhase--;

                        else currentPhase++;

                        break;
                    case 5:
                        consoleUI.PrintTimeLine(listPhase, count, currentPhase);
                        Order order = new Order(orderGenerateID, DateTime.Now, loginManager.LoggedInStaff, new Staff(0, "", "", "", "", "", StaffEnum.Role.Accountant, StaffEnum.Status.Active), customer!, phonesInOrder!, OrderEnum.Status.Pending, new List<DiscountPolicy>(), "", 0);
                        consoleUI.PrintOrder(order);
                        if (ConsoleUlts.PressYesOrNo("Create Order", "Cancel Order"))
                        {
                            if (imeis != null)
                                foreach (var ims in imeis)
                                {
                                    phoneBL.AddPhoneImeiToOrder(ims.PhoneImei);
                                }
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
            int count = 0;
            int handleChoice = 0;
            // danh sách chứa tạm các order lấy được trong database
            List<Order> listOrderTemp = new List<Order>();
            // danh sách chứa các id để check id
            List<int> IdPattern = new List<int>();
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
                                listOrderTemp = orderBL.GetOrdersInDay(OrderEnum.Status.Confirmed)!;
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
                        consoleUI.PrintOrder(order);
                        if (!ConsoleUlts.PressYesOrNo("Confirm Product", "Cancel Order"))
                        {
                            if (orderBL.UpdateOrder(OrderEnum.Status.Canceled, order) == true)
                            {
                                foreach (var phoneDetail in order.PhoneDetails)
                                {
                                    foreach (var imei in phoneDetail.ListImei)
                                    {
                                        phoneBL.ExportPhoneImei(imei.PhoneImei);
                                    }
                                }
                                return 0;
                            }
                        }
                        else
                        {
                            foreach (var phoneDetail in order.PhoneDetails)
                            {
                                foreach (var imei in phoneDetail.ListImei)
                                {
                                    phoneBL.NotExportPhoneImei(imei.PhoneImei);
                                }
                            }
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
        public void TradeIn()
        {
            int centeredPosition = (Console.WindowWidth - "|--------------------------------------------------------------------------------------------|".Length) / 2;
            string spaces = centeredPosition > 0 ? new string(' ', centeredPosition) : "";
            int secondcenteredPosition = (Console.WindowWidth - "|===================================================================================================|".Length) / 2;
            string secondspaces = secondcenteredPosition > 0 ? new string(' ', secondcenteredPosition) : "";

            string[] listPhase = consoleUI.GetTradeInTimeLine();
            string input = "";
            int count = 0;
            int currentPhase = 1;
            string orderID = "";
            List<Phone> ListPhoneInformation = new List<Phone>();
            bool activeTradeIn = true;
            List<int> choicePattern = new List<int>();
            List<Order> ListOrderPending = new OrderBL().GetOrdersPendingInday();
            Order? order = new Order("", new DateTime(), new Staff(0, "", "", "", "", "", StaffEnum.Role.Seller, StaffEnum.Status.Active), new Staff(0, "", "", "", "", "", StaffEnum.Role.Accountant, StaffEnum.Status.Active), new Customer(0, "", "", ""), new List<PhoneDetail>(), OrderEnum.Status.Pending, new List<DiscountPolicy>(), "", 0);
            do
            {
                currentPhase = 1;
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
                        orderID = ConsoleUlts.GetInputString($"{spaces}Choose An Order ID ").ToUpper();
                        order = new OrderBL().GetOrderById(orderID) ?? null;
                        if (order!.OrderID == "")
                        {
                            ConsoleUlts.Alert(ConsoleEnum.Alert.Error, "Invalid Order ID");
                        }
                        else order.Accountant = this.loginManager.LoggedInStaff;
                    } while (order.OrderID == "");
                    consoleUI.PrintTimeLine(listPhase, 0, 1);
                    consoleUI.PrintOrder(order);
                    if (order.PhoneDetails.Count() == 0)
                    {
                        ConsoleUlts.Alert(ConsoleEnum.Alert.Error, "Order doesn't have any phone!");
                        break;
                    }
                    bool resultContinueOrChooseAgain = ConsoleUlts.PressYesOrNo("Continue TradeIn", "Choose Order Again");
                    if (resultContinueOrChooseAgain == true)
                    {
                        List<PhoneDetail> ListPhoneOfCustomerWantTradeIn = new List<PhoneDetail>();
                        bool activeChoosePhone = false;
                        do
                        {
                            int phoneId = 0;
                            consoleUI.PrintTimeLine(listPhase, 0, 2);
                            Console.WriteLine(spaces + "|============================================================================================|");
                            Console.WriteLine(consoleUI.GetAppANSIText());
                            Console.WriteLine(spaces + "|============================================================================================|");
                            Console.WriteLine(consoleUI.GetTradeInANSIText());
                            Console.WriteLine(spaces + "|============================================================================================|");
                            Console.WriteLine(consoleUI.GetCheckCustomerPhoneANSIText());
                            Console.WriteLine(spaces + "|============================================================================================|");
                            Console.Write(spaces + "Search Phone By Information: ");
                            input = Console.ReadLine() ?? "";
                            ListPhoneInformation = phoneBL.GetPhonesByInformation(input);
                            if (ListPhoneInformation.Count() == 0)
                            {
                                Console.WriteLine(spaces + $"Doesnt have any result like '{input}'");
                                bool SearchAgainOrNot = ConsoleUlts.PressYesOrNo("Search again", "Skip TradeIn");
                                if (SearchAgainOrNot == true) continue;
                                else
                                {
                                    ConsoleUlts.Alert(ConsoleEnum.Alert.Warning, "Skip TradeIn");
                                }
                            }
                            bool listPhoneSearch = ConsoleUlts.ListPhonePaginationForTradeIn(phoneBL.GetPhonesByInformation(input), listPhase, 0, 2, loginManager.LoggedInStaff);
                            if (listPhoneSearch == false) break;
                            if (listPhoneSearch == true)
                            {
                                phoneId = ConsoleUlts.InputIDValidation(phoneBL.GetAllPhone().Count(), $"Enter Phone ID", "Invalid Phone ID", spaces);
                            }
                            List<PhoneDetail> phoneDetails = phoneBL.GetPhoneDetailsByPhoneID(phoneId);
                            List<PhoneDetail> phoneDetailsForTradeIn = new List<PhoneDetail>();
                            foreach (var phonedetail in phoneDetails)
                            {
                                if (phonedetail.PhoneStatusType != PhoneEnum.Status.New)
                                {
                                    phoneDetailsForTradeIn.Add(phonedetail);
                                    choicePattern.Add(phonedetail.PhoneDetailID);
                                }
                            }
                            if (phoneDetailsForTradeIn.Count() == 0)
                            {
                                Console.WriteLine(spaces + "Doesnt have any Model Can TradeIn of this Phone");
                                bool chooseAnotherPhoneOrBreak = ConsoleUlts.PressYesOrNo("Choose Another Phone", "Skip TradeIn");
                                if (chooseAnotherPhoneOrBreak == true)
                                {
                                    continue;
                                }
                                else
                                {

                                    ConsoleUlts.Alert(ConsoleEnum.Alert.Warning, "Skip TradeIn");

                                    break;
                                }
                            }
                            bool listPhoneDetailSearch = ConsoleUlts.ListPhoneModelTradeInPagination(phoneDetailsForTradeIn, listPhase, 0, 2, loginManager.LoggedInStaff);
                            if (listPhoneDetailSearch == false) break;
                            if (listPhoneDetailSearch == true)
                            {
                                Console.Write(spaces + "Choose a PhoneDetail ID: ");
                                input = Console.ReadLine() ?? "";
                                while (!ConsoleUlts.CheckInputIDValid(input, choicePattern))
                                {
                                    Console.Write(spaces + "Input again: ");
                                    input = Console.ReadLine() ?? "";
                                }
                                consoleUI.PrintTimeLine(listPhase, 0, 2);
                                consoleUI.PrintPhoneTradeInDetailInfo(phoneBL.GetPhoneDetailByID(Convert.ToInt32(input)));
                                Console.WriteLine(spaces + "Are you sure all this Phone Information are corresponding to Customer's Phone Information?");
                                bool acceptOrNotChoose = ConsoleUlts.PressYesOrNo("Accept Choose Phone", "Choose another Phone");
                                if (acceptOrNotChoose == true)
                                {
                                    consoleUI.PrintTimeLine(listPhase, 0, 2);
                                    ListPhoneOfCustomerWantTradeIn.Add(phoneBL.GetPhoneDetailByID(Convert.ToInt32(input)));
                                    Console.WriteLine(spaces + "|============================================================================================|");
                                    Console.WriteLine(consoleUI.GetAppANSIText());
                                    Console.WriteLine(spaces + "|============================================================================================|");
                                    Console.WriteLine(consoleUI.GetTradeInANSIText());
                                    Console.WriteLine(spaces + "|============================================================================================|");
                                    Console.WriteLine(consoleUI.GetCheckCustomerPhoneANSIText());
                                    Console.WriteLine(spaces + "|============================================================================================|");
                                    bool chooseMoreOrNot = ConsoleUlts.PressYesOrNo("Add more Phone of Customer to TradeIn", "Stop Add");
                                    if (chooseMoreOrNot == true)
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        List<DiscountPolicy> discountForCustomerPhones = new DiscountPolicyBL().GetDiscountTradeIn(ListPhoneOfCustomerWantTradeIn);
                                        List<DiscountPolicy> discountForPhoneInOrder = new DiscountPolicyBL().GetDiscountTradeIn(order.PhoneDetails);
                                        foreach(var TIorder in discountForPhoneInOrder){
                                            foreach(var TIcustomer in discountForCustomerPhones){
                                                if(TIorder.Title == TIcustomer.Title)order.DiscountPolicies.Add(TIcustomer);
                                            }
                                        }
                                        List<DiscountPolicy> ListCheck = order.DiscountPolicies;
                                        
                                        consoleUI.PrintOrder(order);

                                        bool TradeInOrSkip = ConsoleUlts.PressYesOrNo("TradeIn", "Skip TradeIn");
                                        if (TradeInOrSkip == true)
                                        {
                                            orderBL.TradeIn(order);
                                            ConsoleUlts.Alert(ConsoleEnum.Alert.Success, "TradeIn Completed");
                                            activeTradeIn = true;
                                            break;
                                        }
                                        else
                                        {
                                            ConsoleUlts.Alert(ConsoleEnum.Alert.Warning, "TradeIn False");
                                            activeTradeIn = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        } while (activeChoosePhone == false);
                    }
                    else
                    {
                        activeTradeIn = false;
                        continue;
                    }
                }
            } while (activeTradeIn == false);
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
            string orderID = "";
            Order? orderWantToPayment = new Order("", new DateTime(), new Staff(0, "", "", "", "", "", StaffEnum.Role.Seller, StaffEnum.Status.Active), new Staff(0, "", "", "", "", "", StaffEnum.Role.Accountant, StaffEnum.Status.Active), new Customer(0, "", "", ""), new List<PhoneDetail>(), OrderEnum.Status.Pending, new List<DiscountPolicy>(), "", 0);
            bool activePayment = true;
            do
            {
                currentPhase = 1;
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
                        if (orderWantToPayment!.OrderID == "")
                        {
                            ConsoleUlts.Alert(ConsoleEnum.Alert.Error, "Invalid Order ID");
                        }
                        else orderWantToPayment.Accountant = this.loginManager.LoggedInStaff;
                    } while (orderWantToPayment.OrderID == "");

                    //Wait to display orderdetail
                    consoleUI.PrintTimeLine(listPhase, 0, 1);
                    consoleUI.PrintOrder(orderWantToPayment);
                    int check = 0;
                    if (orderWantToPayment.DiscountPolicies.Count() != 0) check++;
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
                                Console.WriteLine(spaces + " " + payment.Key + ". " + payment.Value + $"{spaces}");
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

                            decimal totalDue = orderWantToPayment.GetTotalDue();
                            int checkHaveDiiscountTradeIn = 0;
                            foreach (var discountinorder in orderWantToPayment.DiscountPolicies)
                            {
                                if (discountinorder.MoneySupported != 0) checkHaveDiiscountTradeIn++;
                            }
                            foreach (var discount in new DiscountPolicyBL().GetDiscountForPaymentmethod(orderWantToPayment))
                            {
                                orderWantToPayment.DiscountPolicies.Add(discount);
                                if (discount.DiscountPrice != 0) totalDue -= discount.DiscountPrice;
                            }
                            if (checkHaveDiiscountTradeIn == 0)
                            {
                                DiscountPolicy discountPolicyOrder = new DiscountPolicyBL().GetDiscountForOrder(orderWantToPayment);
                                orderWantToPayment.DiscountPolicies.Add(discountPolicyOrder);
                                totalDue -= discountPolicyOrder.DiscountPrice;
                            }
                            bool activeEnterMoney = false;
                            do
                            {
                                consoleUI.PrintTimeLine(listPhase, 0, 3);
                                consoleUI.PrintOrder(orderWantToPayment);
                                Console.Write(spaces + "Enter Money: ");
                                decimal moneyOfCustomerPaid;
                                input = Console.ReadLine() ?? "";
                                while (!(decimal.TryParse(input, out moneyOfCustomerPaid) && moneyOfCustomerPaid >= 0))
                                {
                                    Console.Write(spaces + "Invalid Input! Input again: ");
                                    input = Console.ReadLine() ?? "";
                                }
                                moneyOfCustomerPaid = Convert.ToDecimal(input);
                                if (moneyOfCustomerPaid >= totalDue)
                                {
                                    int ConfirmOrCancelOrSkip = ConsoleUlts.PressCharacterTo("Confirm Payment", "Cancel Payment", "Skip Payment");
                                    consoleUI.PrintTimeLine(listPhase, 0, 4);
                                    consoleUI.PrintOrder(orderWantToPayment);
                                    if (ConfirmOrCancelOrSkip == 0)
                                    {
                                        Console.WriteLine(spaces + $"-> EXCESS CASH: " + consoleUI.FormatPrice(moneyOfCustomerPaid - totalDue).ToString());
                                        orderBL.Payment(orderWantToPayment);
                                        ConsoleUlts.Alert(ConsoleEnum.Alert.Success, "Payment Completed");
                                        activeChoosePaymentMethod = true;
                                        activePayment = true;
                                        break;
                                    }
                                    else if (ConfirmOrCancelOrSkip == 1)
                                    {
                                        Console.WriteLine(spaces + "Are you sure want to Cancel Payment?");
                                        bool YesOrNoCancel = ConsoleUlts.PressYesOrNo("Cancel Payment", "Not Cancel");
                                        if (YesOrNoCancel == true)
                                        {
                                            orderBL.CancelPayment(orderWantToPayment);
                                            ConsoleUlts.Alert(ConsoleEnum.Alert.Warning, "Cancel Payment");
                                            activeChoosePaymentMethod = true;
                                            activePayment = true;
                                            break;
                                        }
                                        else
                                        {
                                            activeChoosePaymentMethod = true;

                                            activePayment = true;
                                            break;
                                        }
                                    }
                                    else if (ConfirmOrCancelOrSkip == 2)
                                    {
                                        Console.WriteLine(spaces + "Are you sure want to Skip Payment?");
                                        bool YesOrNoSkip = ConsoleUlts.PressYesOrNo("Skip Payment", "Not Skip");
                                        if (YesOrNoSkip == true)
                                        {
                                            orderBL.CancelPayment(orderWantToPayment);
                                            ConsoleUlts.Alert(ConsoleEnum.Alert.Warning, "Skip Payment ");
                                            activeChoosePaymentMethod = true;

                                            activePayment = true;

                                            break;
                                        }
                                        else
                                        {

                                            activeChoosePaymentMethod = true;

                                            activePayment = true;
                                            break;
                                        }
                                    }

                                }
                                else if (moneyOfCustomerPaid < totalDue)
                                {
                                    consoleUI.PrintTimeLine(listPhase, 0, 4);
                                    consoleUI.PrintOrder(orderWantToPayment);
                                    Console.WriteLine(spaces + $"Missing: " + consoleUI.FormatPrice(totalDue - moneyOfCustomerPaid).ToString());
                                    int SkipOrReInputOrCancel = ConsoleUlts.PressCharacterTo("Skip Payment", "Re-Input Money", "Cancel Payment");
                                    if (SkipOrReInputOrCancel == 0)
                                    {
                                        ConsoleUlts.Alert(ConsoleEnum.Alert.Warning, "Skip Payment");
                                        activeChoosePaymentMethod = true;

                                        activePayment = true;

                                        break;
                                    }
                                    else if (SkipOrReInputOrCancel == 1)
                                    {
                                        continue;
                                    }
                                    else if (SkipOrReInputOrCancel == 2)
                                    {
                                        Console.WriteLine(spaces + "Are you sure want to Cancel Payment?");
                                        bool YesOrNoCancel = ConsoleUlts.PressYesOrNo("Cancel Payment", "Not Cancel");
                                        if (YesOrNoCancel == true)
                                        {
                                            orderBL.CancelPayment(orderWantToPayment);
                                            ConsoleUlts.Alert(ConsoleEnum.Alert.Success, "Skip Payment");
                                            Console.ReadKey();
                                            activeChoosePaymentMethod = true;
                                            activePayment = true;
                                            break;
                                        }
                                        else
                                        {
                                            activeChoosePaymentMethod = true;
                                            activePayment = true;
                                            break;
                                        }
                                    }
                                }
                                Console.ReadKey();
                            } while (activeEnterMoney == false);
                        } while (activeChoosePaymentMethod == false);
                    }
                }
            } while (activePayment == false);
        }
    }
}

