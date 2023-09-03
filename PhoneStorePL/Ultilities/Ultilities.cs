using Model;
using BL;
using Interface;
using BusinessEnum;
using GUIEnum;
using System.Diagnostics;
using System;
using System.Reflection.Metadata;
using System.Collections.Generic;
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
        // public List<Phone> SearchPhone() {}
        public List<Phone>? SearchPhone(int currentPhase, string[] timeLine)
        {
            string spaces = consoleUI.AlignCenter("|--------------------------------------------------------------------------------------------|");
            List<Phone> result = new List<Phone>();
            int searchChoice = 0;
            string phoneInfoToSearch = String.Empty;
            consoleUI.PrintTimeLine(timeLine, currentPhase);
            consoleUI.PrintTitle(consoleUI.GetAppANSIText(), consoleUI.GetSearchANSIText(), loginManager.LoggedInStaff);
            searchChoice = ConsoleUlts.PressCharacterTo("Search All Phone", "Search Phone By Information", "Back To Previous Menu", null);
            if (searchChoice == 0) result = phoneBL.GetAllPhone();
            else if (searchChoice == 1)
            {
                consoleUI.PrintTimeLine(timeLine, currentPhase);
                consoleUI.PrintTitle(consoleUI.GetAppANSIText(), consoleUI.GetSearchANSIText(), loginManager.LoggedInStaff);
                phoneInfoToSearch = ConsoleUlts.GetInputString($"{spaces} Enter Phone Information To Search");
                result = phoneBL.GetPhonesByInformation(phoneInfoToSearch);
            }
            else if (searchChoice == 2) return null;
            return result;
        }
        public Order? CreateOrder(List<PhoneDetail> listTradeInPhone, List<Imei> imeiTemp, string[] timeLine, int phase)
        {
            string searchTitle = consoleUI.GetSearchANSIText(), orderGenerateID = ConsoleUlts.GenerateID(), spaces = consoleUI.AlignCenter("|============================================================================================|");
            string[] menuSearchChoice = consoleUI.GetMenuItemSearch(), listPhase = consoleUI.GetCreateOrderTimeLine();
            int phoneId = 0, phoneModelID = 0, currentPhase = 1, phaseChoice = 0, quantity = 0, reChooseModelAfterBackPrevPhase = 0;
            List<Imei>? imeis = new List<Imei>();
            List<int>? listAllPhonesID = new List<int>();
            bool listPhoneSearch = false;
            List<Phone>? listTemp = new List<Phone>();
            List<PhoneDetail> phonedetails = new List<PhoneDetail>(), phonesInOrder = new List<PhoneDetail>(), phoneDetailsInOrder = new List<PhoneDetail>();
            PhoneDetail? pDetails = null, phoneDetailToView = new PhoneDetail();
            Customer? customer = null;
            Order order = new Order();
            bool IsCreate = false;
            int curPhase = currentPhase;
            if (timeLine.Count() != 0 && phase != 0)
            {
                listPhase = timeLine;
                curPhase = phase;
            }
            List<Imei> imeisToInsert = new List<Imei>(), imeisHandle = new List<Imei>(), listImeiInOrder = new List<Imei>();
            do
            {
                switch (currentPhase)
                {
                    case 1:
                        if (listImeiInOrder.Count() != 0)
                            if (ConsoleUlts.PressYesOrNo("Continue To Create Order", "Search Another Phone"))
                            {
                                currentPhase = 3;
                                curPhase = currentPhase;
                                break;
                            }
                        listTemp = SearchPhone(curPhase, listPhase);
                        if (listTemp == null)
                        {
                            currentPhase = 6;
                            curPhase = currentPhase;
                            break;
                        }
                        if (listTemp.Count() == 0)
                        {
                            ConsoleUlts.Alert(ConsoleEnum.Alert.Error, "Phone not found");
                            break;
                        }
                        else
                        {
                            listPhoneSearch = ConsoleUlts.Pagination(listTemp, curPhase, listPhase, 0);
                            if (listPhoneSearch == false) break;
                            if (listPhoneSearch == true)
                            {
                                phoneId = ConsoleUlts.InputIDValidation(phoneBL.GetAllPhone().Count(), $" Enter Phone ID", "Invalid Phone ID", spaces);
                                ConsoleUlts.Alert(ConsoleEnum.Alert.Success, "Choose Phone ID Susccesfully");
                            }
                        }
                        if (ConsoleUlts.PressYesOrNo("Choose Phone Model", "Back To Previous Menu")) currentPhase++;
                        curPhase = currentPhase;
                        break;
                    case 2:
                        Dictionary<int, int> phoneDetailsIDWithQtt = new Dictionary<int, int>();
                        bool? isContinueChooseModelID = null;
                        phonedetails = phoneBL.GetPhoneDetailsByPhoneID(phoneId);
                        consoleUI.PrintTimeLine(listPhase, curPhase);
                        if (listImeiInOrder!.Count() != 0)
                        {
                            List<int> phoneDetailsID = new List<int>();
                            foreach (var item in listImeiInOrder)
                                phoneDetailsID.Add(phoneBL.GetPhoneDetailByImei(item.PhoneImei).PhoneDetailID);
                            foreach (int item in phoneDetailsID)
                            {
                                if (phoneDetailsIDWithQtt.ContainsKey(item)) phoneDetailsIDWithQtt[item]++;
                                else phoneDetailsIDWithQtt[item] = 1;
                            }
                            foreach (PhoneDetail pd in phonedetails)
                                foreach (KeyValuePair<int, int> item in phoneDetailsIDWithQtt)
                                    if (pd.PhoneDetailID == item.Key)
                                        pd.Quantity -= item.Value;
                        }
                        bool isInvalidModel = false;
                        int choicePhase1 = -1;
                        if (phonedetails.Count() <= 1)
                        {
                            foreach (PhoneDetail item in phonedetails)
                                if (item.Quantity == 0)
                                    isInvalidModel = true;
                            if (isInvalidModel == true)
                            {
                                ConsoleUlts.Alert(ConsoleEnum.Alert.Warning, "This Phone Model Is Out Of Stock");
                                if (listImeiInOrder.Count() > 0)
                                {
                                    choicePhase1 = ConsoleUlts.PressCharacterTo("Continue To Create Order", "Back To Previous Phase", null, null);
                                    if (choicePhase1 == 0)
                                    {
                                        currentPhase++;
                                        curPhase = currentPhase;
                                        break;
                                    }
                                    else if (choicePhase1 == 1)
                                    {
                                        currentPhase--;
                                        curPhase = currentPhase;
                                        break;
                                    }
                                }
                                else
                                {
                                    ConsoleUlts.PressEnterTo("Back Previous Phase");
                                    currentPhase--;
                                    curPhase = currentPhase;
                                    break;
                                }
                            }
                        }
                        int continueToCreateOrder = -1;
                        bool reEnterPhoneModelID = true;
                        List<int> listPhoneDetailID = new List<int>();
                        do
                        {
                            if (isContinueChooseModelID == false) break;
                            reChooseModelAfterBackPrevPhase = 0; reEnterPhoneModelID = true; listPhoneDetailID = new List<int>();
                            foreach (PhoneDetail item in phonedetails)
                            {
                                listPhoneDetailID.Add(item.PhoneDetailID);
                                if (item.Quantity == 0) listPhoneDetailID.Remove(item.PhoneDetailID);
                            }
                            do
                            {
                                isContinueChooseModelID = ConsoleUlts.Pagination(phonedetails, curPhase, listPhase, 0);
                                if (isContinueChooseModelID != null)
                                {
                                    if (isContinueChooseModelID == true)
                                        phoneModelID = ConsoleUlts.GetInputInt($"{spaces} Enter Phone Model ID");
                                    else break;
                                }
                                if (listPhoneDetailID.IndexOf(phoneModelID) == -1)
                                {
                                    ConsoleUlts.Alert(ConsoleEnum.Alert.Error, "Invalid Phone Model ID Please Choice Again");
                                    consoleUI.PrintTimeLine(listPhase, curPhase);
                                }
                            } while (listPhoneDetailID.IndexOf(phoneModelID) == -1);
                            if (isContinueChooseModelID == false) break;
                            ConsoleUlts.Alert(ConsoleEnum.Alert.Success, "Choose Phone Model ID Successfully");
                            int quantityAfterBackPrevPhase = 1;
                            if (isContinueChooseModelID == true)
                            {
                                if (phoneBL.GetPhoneDetailByID(phoneModelID).Quantity == 0 || quantityAfterBackPrevPhase == 0)
                                {
                                    ConsoleUlts.Alert(ConsoleEnum.Alert.Warning, "This Phone Model Is Out Of Stock");
                                    if (quantityAfterBackPrevPhase == 0)
                                    {
                                        if (listPhoneDetailID.Count() > 1)
                                        {
                                            reChooseModelAfterBackPrevPhase = ConsoleUlts.PressCharacterTo("Choose Another Phone Model", "Back Previous Phase", "Continue To Create Order", null);
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
                            }
                        } while (reEnterPhoneModelID);
                        if (reChooseModelAfterBackPrevPhase == 2)
                        {
                            currentPhase++;
                            curPhase = currentPhase;
                            break;
                        }
                        else if (reChooseModelAfterBackPrevPhase == 1 || !reEnterPhoneModelID || isContinueChooseModelID == false)
                        {
                            currentPhase--;
                            curPhase = currentPhase;
                            break;
                        }
                        int phoneDetailQuantity = 0;
                        pDetails = new PhoneBL().GetPhoneDetailByID(phoneModelID);
                        if (phonesInOrder!.Count() != 0)
                            foreach (PhoneDetail pd in phonesInOrder!)
                                if (pd.PhoneDetailID == phoneModelID)
                                    phoneDetailQuantity = pDetails.Quantity - pd.Quantity;
                                else
                                    phoneDetailQuantity = pDetails.Quantity;
                        else phoneDetailQuantity = pDetails.Quantity;
                        foreach (var item in phonedetails)
                            if (item.PhoneDetailID == phoneModelID)
                            {
                                phoneDetailToView = item;
                                phoneDetailQuantity = item.Quantity;
                            }
                        consoleUI.PrintTimeLine(listPhase, curPhase);
                        consoleUI.PrintPhoneTradeInDetailInfo(phoneDetailToView);
                        consoleUI.PrintPhoneModelTitle();
                        consoleUI.PrintPhoneModelInfo(phoneDetailToView);
                        Console.WriteLine(spaces + "|============================================================================================|");
                        bool isInvalidModelQuantity = false;
                        do
                        {
                            quantity = ConsoleUlts.GetInputInt(spaces + " Enter Phone Model Quantity");
                            if (quantity <= 0 || quantity > phoneDetailQuantity)
                            {
                                ConsoleUlts.Alert(ConsoleEnum.Alert.Error, "Invalid Phone Model Quantity");
                                isInvalidModelQuantity = true;
                                break;
                            }
                            else ConsoleUlts.Alert(ConsoleEnum.Alert.Success, "Quantity Successfully Added");
                        } while (quantity <= 0 || quantity > phoneDetailQuantity);
                        if (isInvalidModelQuantity == true)
                        {
                            if (phonesInOrder.Count() != 0)
                            {
                                continueToCreateOrder = ConsoleUlts.PressCharacterTo("Choose Another Phone Model", "Continue To Create Order", "Back To Previous Phase", null);
                                if (continueToCreateOrder == 0) break;
                                else if (continueToCreateOrder == 1)
                                {
                                    currentPhase++;
                                    curPhase = currentPhase;
                                    break;
                                }
                                else if (continueToCreateOrder == 2)
                                {
                                    currentPhase--;
                                    curPhase = currentPhase;
                                    break;
                                }
                            }
                            else
                            {
                                continueToCreateOrder = ConsoleUlts.PressCharacterTo("Choose Another Phone Model", "Back To Previous Phase", null, null);
                                if (continueToCreateOrder == 0) break;
                                else if (continueToCreateOrder == 1)
                                {
                                    currentPhase--;
                                    curPhase = currentPhase;
                                    break;
                                }
                            }
                        }
                        pDetails.Quantity = quantity;
                        phonedetails.Add(pDetails);
                        do
                        {
                            consoleUI.PrintTimeLine(listPhase, curPhase);
                            consoleUI.PrintPhoneTradeInDetailInfo(phoneDetailToView);
                            consoleUI.PrintPhoneModelTitle();
                            consoleUI.PrintPhoneModelInfo(phoneDetailToView);
                            Console.WriteLine(spaces + "|============================================================================================|");
                            Console.WriteLine($"{spaces} Phone Model ID: " + phoneModelID);
                            Console.WriteLine($"{spaces} Quantity: " + quantity);
                            List<string> listImeis = new List<string>();
                            foreach (var item in imeis)
                                listImeis.Add(item.PhoneImei);
                            imeisHandle = new List<Imei>();
                            for (int i = 0; i < quantity; i++)
                            {
                                Imei imei = new Imei(new PhoneDetail(0, new Phone(0, "", new Brand(0, "", ""), "", "", "", "", "", "", "", "", "", new DateTime(), "", new Staff(0, "", "", "", "", "", StaffEnum.Role.Seller, StaffEnum.Status.Active), new DateTime(), ""), new ROMSize(0, ""), new PhoneColor(0, ""), 0, 0, PhoneEnum.Status.Type1, new Staff(0, "", "", "", "", "", StaffEnum.Role.Seller, StaffEnum.Status.Active), new DateTime()), "", BusinessEnum.PhoneEnum.ImeiStatus.NotExport);
                                do
                                {
                                    do
                                    {
                                        imei.PhoneImei = ConsoleUlts.GetInputString($"{spaces} Enter Imei {i + 1}");
                                        if (listImeis.IndexOf(imei.PhoneImei) != -1)
                                            ConsoleUlts.Alert(ConsoleEnum.Alert.Error, "Can't Add Duplicate Imei");
                                    } while (listImeis.IndexOf(imei.PhoneImei) != -1);
                                    if (!new PhoneBL().CheckImeiExist(imei, phoneModelID))
                                    {
                                        ConsoleUlts.Alert(ConsoleEnum.Alert.Error, "Imei Not Found");
                                        Console.Clear();
                                        consoleUI.PrintTimeLine(listPhase, curPhase);
                                        consoleUI.PrintPhoneTradeInDetailInfo(phoneDetailToView);
                                        consoleUI.PrintPhoneModelTitle();
                                        consoleUI.PrintPhoneModelInfo(phoneDetailToView);
                                        Console.WriteLine(spaces + "|============================================================================================|");
                                        Console.WriteLine(spaces + " Phone Model ID: " + phoneModelID);
                                        Console.WriteLine(spaces + " Quantity: " + quantity);
                                    }
                                    else
                                    {
                                        ConsoleUlts.Alert(ConsoleEnum.Alert.Success, "Imei Successfully Added");
                                        imeis.Add(imei);
                                        imeisHandle.Add(imei);
                                    }
                                } while (!new PhoneBL().CheckImeiExist(imei, phoneModelID));
                            }
                            if (!ConsoleUlts.PressYesOrNo("Continue", "Back Previous Phase"))
                            {
                                listImeiInOrder.RemoveAll(item => imeisHandle.Contains(item));
                                currentPhase--;
                                curPhase = currentPhase;
                            }
                            else
                            {
                                listImeiInOrder = imeis;
                                currentPhase++;
                                curPhase = currentPhase;
                            }
                            break;
                        } while (phaseChoice != 1 && phaseChoice != 2);
                        if (continueToCreateOrder >= 0 && continueToCreateOrder <= 4) break;
                        break;
                    case 3:
                        if (listTradeInPhone.Count() == 0) order = new Order("", DateTime.Now, loginManager.LoggedInStaff, new Staff(0, "", "", "", "", "", StaffEnum.Role.Accountant, StaffEnum.Status.Active), new Customer(0, "", "", ""), new List<Imei>(), OrderEnum.Status.Pending, new List<DiscountPolicy>(), "", 0);
                        else
                        {
                            order = new Order("", DateTime.Now, new Staff(0, "Tran Tien Anh", "", "", "", "", StaffEnum.Role.Accountant, StaffEnum.Status.Active), loginManager.LoggedInStaff, new Customer(0, "", "", ""), new List<Imei>(), OrderEnum.Status.Pending, new List<DiscountPolicy>(), "", 0);
                        }
                        if (reChooseModelAfterBackPrevPhase != 2) order.ListImeiInOrder = imeis!;
                        order.OrderID = orderGenerateID;
                        consoleUI.PrintTimeLine(listPhase, curPhase);


                        order.ListImeiInOrder = imeis!;
                        List<Imei> listImei = new List<Imei>();
                        foreach (var item in order.ListImeiInOrder)
                        {
                            item.PhoneDetail = new PhoneBL().GetPhoneDetailByImei(item.PhoneImei);
                            item.PhoneDetail.Quantity = 1;
                        }
                        imeisToInsert = order.ListImeiInOrder;
                        foreach (Imei item in order.ListImeiInOrder)
                        {
                            var target = item.PhoneDetail.PhoneDetailID;
                            item.PhoneDetail.Quantity = order.ListImeiInOrder.Count(x => x.PhoneDetail.PhoneDetailID == target);
                        }
                        order.ListImeiInOrder.Sort((x, y) => x.PhoneDetail.PhoneDetailID.CompareTo(y.PhoneDetail.PhoneDetailID));
                        consoleUI.PrintOrder(order, listTradeInPhone, imeiTemp);
                        phaseChoice = ConsoleUlts.PressCharacterTo("Back Previous Phase", "Enter Customer Info", "Add More Phone", null);
                        if (phaseChoice == 0)
                        {
                            for (var i = 0; i < order.ListImeiInOrder.Count(); i++)
                                order.ListImeiInOrder.RemoveAll(item => imeisHandle.Contains(item));
                            foreach (PhoneDetail pd in phonesInOrder!)
                                if (pDetails!.PhoneDetailID == pd.PhoneDetailID)
                                    pd.Quantity -= pDetails.Quantity;
                            currentPhase--;
                        }
                        else if (phaseChoice == 1) currentPhase++;
                        else
                        {
                            consoleUI.PrintTimeLine(listPhase, curPhase);
                            consoleUI.PrintOrder(order, listTradeInPhone, imeiTemp);
                            currentPhase = 1;
                        }
                        curPhase = currentPhase;
                        break;
                    case 4:
                        consoleUI.PrintTimeLine(listPhase, curPhase);
                        consoleUI.PrintTitle(consoleUI.GetAppANSIText(), consoleUI.GetCustomerInfoANSIText(), loginManager.LoggedInStaff);
                        customer = ConsoleUlts.GetCustomerInfo();
                        customer.CustomerID = customerBL.CheckCustomerIsExist(customer);
                        if (customer.CustomerID == 0) ConsoleUlts.Alert(GUIEnum.ConsoleEnum.Alert.Success, $"Add customer completed");
                        else
                        {
                            customer = customerBL.GetCustomerByID(customer.CustomerID);
                            ConsoleUlts.Alert(GUIEnum.ConsoleEnum.Alert.Warning, $"Customer Phone Number Is Already Exsist, Customer ID: {customer.CustomerID}");
                        }
                        if (!ConsoleUlts.PressYesOrNo("Confirm Order", "Re-Enter Customer Information")) break;
                        else currentPhase++;
                        curPhase = currentPhase;
                        break;
                    case 5:
                        if (customer != null)
                            order.Customer = customer;
                        consoleUI.PrintTimeLine(listPhase, curPhase);
                        consoleUI.PrintOrder(order, listTradeInPhone, imeiTemp);
                        if (ConsoleUlts.PressYesOrNo("Create Order", "Cancel Order"))
                        {
                            order.ListImeiInOrder = imeisToInsert;
                            bool isCreateOrder = orderBL.CreateOrder(order);
                            ConsoleUlts.Alert(isCreateOrder ? ConsoleEnum.Alert.Success : ConsoleEnum.Alert.Error, isCreateOrder ? "Create Order Completed" : "Create Order Failed");
                            if (isCreateOrder) IsCreate = true;
                        }
                        else
                        {
                            ConsoleUlts.Alert(ConsoleEnum.Alert.Success, "Cancel Order Completed!");
                            phonesInOrder = new List<PhoneDetail>();
                            IsCreate = false;
                        }
                        currentPhase++;
                        curPhase = currentPhase;
                        break;
                }
            } while (currentPhase != 6);
            if (IsCreate) return order;
            else
            {
                return null;
            }
        }
        public List<Order>? SearchOrder(int currentPhase, OrderEnum.Status OrderStatusFilter)
        {
            List<Order>? result = null;
            if (OrderStatusFilter == OrderEnum.Status.Confirmed)
            {
                consoleUI.PrintTimeLine(consoleUI.GetHandleOrderTimeLine(), currentPhase);
                consoleUI.PrintTitle(consoleUI.GetAppANSIText(), consoleUI.GetHandleOrderANSIText(), loginManager.LoggedInStaff);
                result = orderBL.GetOrdersConfirmedInDay()!;
            }
            else if (OrderStatusFilter == OrderEnum.Status.Pending) result = orderBL.GetOrdersPendingInday();
            if (result!.Count() == 0) return null;
            return result;
        }
        public int HandleOrder()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            int currentPhase = 1, centeredPosition = (Console.WindowWidth - "|--------------------------------------------------------------------------------------------|".Length) / 2, secondcenteredPosition = (Console.WindowWidth - "|===================================================================================================|".Length) / 2;
            string handleTitle = consoleUI.GetHandleOrderANSIText(), orderId = "", spaces = centeredPosition > 0 ? new string(' ', centeredPosition) : "";
            string[] listPhase = consoleUI.GetHandleOrderTimeLine();
            List<Order>? listOrderTemp = new List<Order>(); // danh sách chứa tạm các order lấy được trong database
            List<int> IdPattern = new List<int>(); // danh sách chứa các id để check id
            Order orderdetails = new Order(), orderTemp = new Order("", new DateTime(), new Staff(0, "", "", "", "", "", StaffEnum.Role.Seller, StaffEnum.Status.Active), new Staff(0, "", "", "", "", "", StaffEnum.Role.Accountant, StaffEnum.Status.Active), new Customer(0, "", "", ""), new List<Imei>(), OrderEnum.Status.Pending, new List<DiscountPolicy>(), "", 0), order = new Order();
            bool? temp = false;
            do
            {
                switch (currentPhase)
                {
                    case 1:
                        listOrderTemp = SearchOrder(currentPhase, OrderEnum.Status.Confirmed);
                        if (listOrderTemp == null)
                        {
                            currentPhase = 3;
                            return -1;
                        }
                        else
                        {
                            consoleUI.PrintTimeLine(listPhase, currentPhase);
                            temp = ConsoleUlts.Pagination(listOrderTemp, currentPhase, consoleUI.GetHandleOrderTimeLine(), 3);
                            if (temp == true)
                            { // nhập Id order để xem
                                order = ConsoleUlts.GetAnOrder(this.loginManager);
                                order.Seller = loginManager.LoggedInStaff;
                                currentPhase++;
                            }
                            else if (temp == false) return 2;
                            else if (temp == null) return -1;
                        }
                        break;
                    case 2:
                        consoleUI.PrintTimeLine(listPhase, currentPhase);
                        consoleUI.PrintOrder(order, new List<PhoneDetail>(), new List<Imei>());
                        if (!ConsoleUlts.PressYesOrNo("Confirm Product", "Cancel Order"))
                        {
                            if (orderBL.UpdateOrder(OrderEnum.Status.Canceled, order) == true) return 0;
                        }
                        else // đổi trạng thái Order thành completed
                            if (orderBL.UpdateOrder(OrderEnum.Status.Completed, order) == true) return 1;
                        break;
                }
            } while (currentPhase != 3);
            return 1;
        }
        public void TradeIn()
        {
            int currentPhase = 1, centeredPosition = (Console.WindowWidth - "|--------------------------------------------------------------------------------------------|".Length) / 2, secondcenteredPosition = (Console.WindowWidth - "|===================================================================================================|".Length) / 2;
            string spaces = centeredPosition > 0 ? new string(' ', centeredPosition) : "", input = "";
            string[] listPhase = consoleUI.GetTradeInTimeLine();
            List<Phone> ListPhoneInformation = new List<Phone>();
            Order? orderAfterCreateForTradeIn = new Order();
            List<int> choicePattern = new List<int>();
            Order order = new Order();
            List<PhoneDetail> ListPhoneOfCustomerWantTradeIn = new List<PhoneDetail>();
            bool activeChoosePhone = false;
            List<Imei> imeis = new List<Imei>();
            do
            {
                int phoneId = 0;
                currentPhase = 1;
                List<Phone>? listPhone = SearchPhone(currentPhase, listPhase);
                if (listPhone != null)
                {
                    bool listPhoneSearch = ConsoleUlts.Pagination(listPhone, currentPhase, consoleUI.GetTradeInTimeLine(), 1);
                    if (listPhoneSearch == false) activeChoosePhone = false;
                    if (listPhoneSearch == true)
                    {
                        phoneId = ConsoleUlts.InputIDValidation(phoneBL.GetAllPhone().Count(), $" Enter Phone ID", "Invalid Phone ID", spaces);
                        List<PhoneDetail> phoneDetails = phoneBL.GetPhoneDetailsByPhoneID(phoneId);
                        List<PhoneDetail> phoneDetailsForTradeIn = new List<PhoneDetail>();
                        foreach (var phonedetail in phoneDetails)
                            if (phonedetail.PhoneStatusType != PhoneEnum.Status.New)
                            {
                                phoneDetailsForTradeIn.Add(phonedetail);
                                choicePattern.Add(phonedetail.PhoneDetailID);
                            }
                        if (phoneDetailsForTradeIn.Count() == 0)
                        {
                            Console.WriteLine(consoleUI.AlignCenter("Doesnt Have Any Model Can TradeIn Of This Phone") + "Doesnt Have Any Model Can TradeIn Of This Phone");
                            bool chooseAnotherPhoneOrBreak = ConsoleUlts.PressYesOrNo("Choose Another Phone", "Skip TradeIn");
                            if (chooseAnotherPhoneOrBreak == true) continue;
                            else
                            {
                                ConsoleUlts.Alert(ConsoleEnum.Alert.Warning, "Skip TradeIn");
                                break;
                            }
                        }
                        bool listPhoneDetailSearch = ConsoleUlts.Pagination(phoneDetailsForTradeIn, currentPhase, consoleUI.GetTradeInTimeLine(), 1);
                        if (listPhoneDetailSearch == false) activeChoosePhone = false;
                        if (listPhoneDetailSearch == true)
                        {
                            Console.Write(spaces + " Choose A Phone Model ID: ");
                            input = Console.ReadLine() ?? "";
                            while (!ConsoleUlts.CheckInputIDValid(input, choicePattern))
                            {
                                Console.Write(spaces + " Input again: ");
                                input = Console.ReadLine() ?? "";
                            }
                            consoleUI.PrintTimeLine(listPhase, 1);
                            consoleUI.PrintPhoneTradeInDetailInfo(phoneBL.GetPhoneDetailByID(Convert.ToInt32(input)));
                            Console.WriteLine(consoleUI.AlignCenter("Are You Sure All This Phone Information Are Corresponding To Customer's Phone Information?") + "Are You Sure All This Phone Information Are Corresponding To Customer's Phone Information?");
                            bool acceptOrNotChoose = ConsoleUlts.PressYesOrNo("Accept Choose Phone", "Choose another Phone");
                            if (acceptOrNotChoose == true)
                            {
                                consoleUI.PrintTimeLine(listPhase, 1);
                                consoleUI.PrintPhoneTradeInDetailInfo(phoneBL.GetPhoneDetailByID(Convert.ToInt32(input)));
                                Console.Write(spaces + " Input Quantity: ");
                                int quantity;
                                while (!int.TryParse(Console.ReadLine() ?? "", out quantity))
                                {
                                    consoleUI.PrintTimeLine(listPhase, 1);
                                    consoleUI.PrintPhoneTradeInDetailInfo(phoneBL.GetPhoneDetailByID(Convert.ToInt32(input)));
                                    Console.Write(spaces + " Input again: ");
                                }
                                PhoneDetail phonedetailTemp = phoneBL.GetPhoneDetailByID(Convert.ToInt32(input));
                                phonedetailTemp.Quantity = quantity;
                                ListPhoneOfCustomerWantTradeIn.Add(phonedetailTemp);

                                for (int i = 0; i < quantity; i++)
                                {
                                    Console.Write(spaces + $" Input imei {i + 1}: ");
                                    string imei = Console.ReadLine() ?? "";
                                    while (!ConsoleUlts.CheckImeiValid(imei))
                                    {
                                        Console.WriteLine();
                                        ConsoleUlts.Alert(ConsoleEnum.Alert.Error, "Invalid Imei");
                                        Console.Write(spaces + $" Input imei {i + 1}: ");
                                        imei = Console.ReadLine() ?? "";
                                        Console.WriteLine(spaces);
                                    }
                                    ConsoleUlts.Alert(ConsoleEnum.Alert.Success, "Enter Imei Successfully");


                                    Imei imeiValid = new Imei(new PhoneDetail(), imei, PhoneEnum.ImeiStatus.NotExport);
                                    imeis.Add(imeiValid);
                                }
                            }
                            else
                            {
                                continue;
                            }
                            Console.Clear();
                            consoleUI.PrintTimeLine(listPhase, 1);
                            consoleUI.GetTradeInTitle();
                            bool chooseMoreOrNot = ConsoleUlts.PressYesOrNo("Choose More Other Phone", "Stop Add Phone To TradeIn");
                            if (chooseMoreOrNot == true) continue;
                            else
                            {
                                bool activeChooseNewPhoneTradeIn = false;
                                do
                                {
                                    orderAfterCreateForTradeIn = CreateOrder(ListPhoneOfCustomerWantTradeIn, imeis, listPhase, 2);
                                    if (orderAfterCreateForTradeIn == null)
                                    {
                                        ConsoleUlts.Alert(ConsoleEnum.Alert.Error, "TradeIn Canceled");
                                        activeChooseNewPhoneTradeIn = true;
                                        activeChoosePhone = true;
                                    }
                                    else
                                    {
                                        bool paymentResult = Payment(ListPhoneOfCustomerWantTradeIn, imeis, orderAfterCreateForTradeIn, listPhase, listPhase.Count());
                                        ConsoleUlts.Alert(ConsoleEnum.Alert.Success, "TradeIn Completed");

                                        activeChooseNewPhoneTradeIn = true;
                                        activeChoosePhone = true;

                                    }
                                } while (activeChooseNewPhoneTradeIn == false);
                            }
                        }
                    }
                }
                else if (listPhone == null) break;
            } while (activeChoosePhone == false);
        }

        public bool Payment(List<PhoneDetail> listTradeInPhone, List<Imei> imeiTemp, Order orderForTradeIn, string[] timeLine, int phase)
        {
            bool PaymentResult = false;
            int currentPhase = 1, centeredPosition = (Console.WindowWidth - "|--------------------------------------------------------------------------------------------|".Length) / 2, secondcenteredPosition = (Console.WindowWidth - "|===================================================================================================|".Length) / 2;
            string input = "", spaces = centeredPosition > 0 ? new string(' ', centeredPosition) : "", orderID = "";
            string[] listPhase = consoleUI.GetPaymentTimeLine();
            List<Order>? ListOrderPending = null;
            List<DiscountPolicy> ListDiscountPolicyValidToOrder = new List<DiscountPolicy>();
            List<int> choicePattern = new List<int>();
            List<string> orderChoicePattern = new List<string>();
            Dictionary<int, string> ListPaymentMethod = new Dictionary<int, string>() { { 1, PaymentEnum.PaymentMethod.VNPay.ToString() }, { 2, PaymentEnum.PaymentMethod.Banking.ToString() }, { 3, PaymentEnum.PaymentMethod.Cash.ToString() } };
            Order? orderWantToPayment = new Order("", new DateTime(), new Staff(0, "", "", "", "", "", StaffEnum.Role.Seller, StaffEnum.Status.Active), new Staff(0, "", "", "", "", "", StaffEnum.Role.Accountant, StaffEnum.Status.Active), new Customer(0, "", "", ""), new List<Imei>(), OrderEnum.Status.Pending, new List<DiscountPolicy>(), "", 0);
            bool activePayment = true;
            if (timeLine.Count() != 0 && phase != 0)
            {
                listPhase = timeLine;
            }
            if (listTradeInPhone.Count() == 0)
            {
                do
                {
                    currentPhase = 1;
                    if (phase == 0) ListOrderPending = SearchOrder(currentPhase, OrderEnum.Status.Pending);
                    else ListOrderPending = SearchOrder(phase, OrderEnum.Status.Pending);
                    if (ListOrderPending == null)
                    {
                        ConsoleUlts.Alert(ConsoleEnum.Alert.Error, "NO ORDER EXIST");
                        return false;
                    }
                    else
                    {
                        bool? showOrderList = null;
                        currentPhase = 1;
                        if (phase == 0) showOrderList = ConsoleUlts.Pagination(ListOrderPending, currentPhase, listPhase, currentPhase);
                        else showOrderList = ConsoleUlts.Pagination(ListOrderPending, currentPhase, listPhase, phase);
                        if (showOrderList == true)
                        {
                            orderWantToPayment = ConsoleUlts.GetAnOrder(this.loginManager);
                            //Wait to display orderdetail
                            if (phase == 0) consoleUI.PrintTimeLine(listPhase, 1);
                            else consoleUI.PrintTimeLine(listPhase, phase);
                            consoleUI.PrintOrder(orderWantToPayment, new List<PhoneDetail>(), new List<Imei>());
                            int check = 0;
                            if (orderWantToPayment.DiscountPolicies.Count() != 0) check++;
                            if (orderWantToPayment.ListImeiInOrder.Count() == 0)
                            {
                                ConsoleUlts.Alert(ConsoleEnum.Alert.Error, "Order doesn't have any phone!");
                                break;
                            }
                            bool resultContinueOrChooseAgain = ConsoleUlts.PressYesOrNo("Continue Payment", "Choose Order Again");
                            if (!resultContinueOrChooseAgain)
                            {
                                activePayment = false;
                                continue;
                            }
                            else
                            {
                                activePayment = true;
                            }
                        }
                    }
                } while (activePayment == false);
            }
            if (listTradeInPhone.Count() != 0) orderWantToPayment = orderForTradeIn;
            choicePattern = new List<int>();
            bool activeChoosePaymentMethod = false;
            do
            {
                int inputPaymentMethodChoice = 0;
                currentPhase = 2;
                if (phase == 0) consoleUI.PrintTimeLine(listPhase, currentPhase);
                else consoleUI.PrintTimeLine(listPhase, phase);
                // hiển thị các Payment Method (phương thức thanh toán)
                consoleUI.PrintPaymentMethodTitle();
                consoleUI.PrintListPaymentMethod(ListPaymentMethod);
                do
                {
                    inputPaymentMethodChoice = ConsoleUlts.GetInputInt(spaces + "Choose a Payment Method");
                    if (inputPaymentMethodChoice <= 0 || inputPaymentMethodChoice > ListPaymentMethod.Count())
                        ConsoleUlts.Alert(ConsoleEnum.Alert.Error, "Invalid Payment Method");
                    else orderWantToPayment.PaymentMethod = ListPaymentMethod[inputPaymentMethodChoice];
                } while (inputPaymentMethodChoice <= 0 || inputPaymentMethodChoice > ListPaymentMethod.Count());

                Console.Clear();
                if (phase == 0) consoleUI.PrintTimeLine(listPhase, currentPhase);
                else consoleUI.PrintTimeLine(listPhase, phase);

                foreach (var payment in ListPaymentMethod)
                    if (payment.Key == inputPaymentMethodChoice) orderWantToPayment.PaymentMethod = payment.Value;
                decimal totalDue = orderWantToPayment.GetTotalDue();
                int checkHaveDiiscountTradeIn = 0;
                foreach (var discountinorder in orderWantToPayment.DiscountPolicies)
                    if (discountinorder.MoneySupported != 0) checkHaveDiiscountTradeIn++;

                if (checkHaveDiiscountTradeIn == 0)
                {
                    foreach (var discount in new DiscountPolicyBL().GetDiscountForPaymentMethod(orderWantToPayment))
                    {
                        if (listTradeInPhone.Count() == 0)
                        {
                            orderWantToPayment.DiscountPolicies.Add(discount);
                            if (discount.DiscountPrice != 0) totalDue -= discount.DiscountPrice;
                        }
                    }
                    DiscountPolicy discountPolicyOrder = new DiscountPolicyBL().GetDiscountForOrder(orderWantToPayment);
                    if (listTradeInPhone.Count() == 0)
                    {
                        orderWantToPayment.DiscountPolicies.Add(discountPolicyOrder);
                        totalDue -= discountPolicyOrder.DiscountPrice;
                    }
                }
                bool activeEnterMoney = false;
                if (orderWantToPayment.PaymentMethod == "Cash")
                {

                    if (orderWantToPayment.TotalDue < 0)
                    {
                        if (listTradeInPhone.Count() == 0)
                        {
                            consoleUI.PrintOrder(orderWantToPayment, listTradeInPhone, imeiTemp);
                            int PaymentOrSkipOrCancel = ConsoleUlts.PressCharacterTo("Confirm Payment", "Skip Payment", "Cancel Payment", null);
                            if (PaymentOrSkipOrCancel == 0)
                            {
                                orderBL.Payment(orderWantToPayment);
                                PaymentResult = true;
                                ConsoleUlts.Alert(ConsoleEnum.Alert.Success, "Confirm Payment");
                                activeChoosePaymentMethod = true;
                                activePayment = true;
                                break;
                            }
                            else if (PaymentOrSkipOrCancel == 1)
                            {
                                ConsoleUlts.Alert(ConsoleEnum.Alert.Success, "Skip Payment");
                                PaymentResult = false;
                                activeChoosePaymentMethod = true;
                                activePayment = true;
                                break;
                            }
                            else if (PaymentOrSkipOrCancel == 2)
                            {
                                PaymentResult = false;
                                ConsoleUlts.Alert(ConsoleEnum.Alert.Success, "Cancel Payment");
                                orderBL.CancelPayment(orderWantToPayment);
                                activeChoosePaymentMethod = true;
                                activePayment = true;
                                break;
                            }
                        }
                        else
                        {
                            consoleUI.PrintOrder(orderWantToPayment, listTradeInPhone, imeiTemp);
                            bool PaymentOrCancel = ConsoleUlts.PressYesOrNo("Confirm Payment", "Cancel Payment");
                            if (PaymentOrCancel)
                            {
                                PaymentResult = true;
                                orderBL.Payment(orderWantToPayment);
                                ConsoleUlts.Alert(ConsoleEnum.Alert.Success, "Confirm Payment");
                                activeChoosePaymentMethod = true;
                                activePayment = true;
                                break;
                            }

                            else
                            {
                                PaymentResult = false;
                                ConsoleUlts.Alert(ConsoleEnum.Alert.Success, "Cancel Payment");
                                orderBL.CancelPayment(orderWantToPayment);
                                activeChoosePaymentMethod = true;
                                activePayment = true;
                                break;
                            }
                        }
                    }



                    else
                    {
                        do
                        {
                            if (phase == 0) consoleUI.PrintTimeLine(listPhase, 3);
                            else consoleUI.PrintTimeLine(listPhase, phase);
                            consoleUI.PrintOrder(orderWantToPayment, listTradeInPhone, imeiTemp);
                            decimal moneyOfCustomerPaid = ConsoleUlts.EnterMoney();
                            Console.WriteLine();
                            if (moneyOfCustomerPaid >= totalDue)
                            {
                                int ConfirmOrCancelOrSkip = ConsoleUlts.PressCharacterTo("Confirm Payment", "Cancel Payment", "Skip Payment", null);
                                if (phase == 0) consoleUI.PrintTimeLine(listPhase, 4);
                                else consoleUI.PrintTimeLine(listPhase, phase);
                                consoleUI.PrintOrder(orderWantToPayment, new List<PhoneDetail>(), new List<Imei>());
                                if (ConfirmOrCancelOrSkip == 0)
                                {
                                    List<DiscountPolicy> ListDiscountResult = new List<DiscountPolicy>();
                                    foreach (var discount in orderWantToPayment.DiscountPolicies)
                                        if (discount.DiscountPrice != 0) ListDiscountResult.Add(discount);
                                    if (listTradeInPhone.Count() == 0) orderWantToPayment.DiscountPolicies = ListDiscountResult;
                                    Console.WriteLine(spaces + $"-> EXCESS CASH: " + consoleUI.FormatPrice(moneyOfCustomerPaid - totalDue).ToString());
                                    PaymentResult = true;
                                    orderBL.Payment(orderWantToPayment);
                                    ConsoleUlts.Alert(ConsoleEnum.Alert.Success, "Payment Completed");
                                    activeChoosePaymentMethod = true;
                                    activePayment = true;
                                    break;
                                }
                                else if (ConfirmOrCancelOrSkip == 1)
                                {
                                    Console.WriteLine(spaces + "Do You Want to Cancel Payment?");
                                    if (ConsoleUlts.PressYesOrNo("Cancel Payment", "Not Cancel"))
                                    {
                                        PaymentResult = false;
                                        orderBL.CancelPayment(orderWantToPayment);
                                        ConsoleUlts.Alert(ConsoleEnum.Alert.Warning, "Cancel Payment");
                                        activeChoosePaymentMethod = true;
                                        activePayment = true;
                                        break;
                                    }
                                    else continue;
                                }
                                else if (ConfirmOrCancelOrSkip == 2)
                                {
                                    Console.WriteLine(spaces + "Do You Want to Skip Payment?");
                                    if (ConsoleUlts.PressYesOrNo("Skip Payment", "Not Skip"))
                                    {
                                        PaymentResult = false;
                                        ConsoleUlts.Alert(ConsoleEnum.Alert.Warning, "Skip Payment");
                                        activeChoosePaymentMethod = true;
                                        activePayment = true;
                                        break;
                                    }
                                    else
                                    {

                                        continue;
                                    }
                                }
                            }
                            else if (moneyOfCustomerPaid < totalDue)
                            {
                                if (phase == 0) consoleUI.PrintTimeLine(listPhase, 4);
                                else consoleUI.PrintTimeLine(listPhase, phase);
                                consoleUI.PrintOrder(orderWantToPayment, listTradeInPhone, imeiTemp);
                                if (listTradeInPhone.Count() != 0)
                                {
                                    foreach (var phone in listTradeInPhone)
                                    {
                                        totalDue -= phone.Quantity * phone.Price;
                                    }
                                }
                                Console.WriteLine(spaces + $"Missing: " + consoleUI.FormatPrice(totalDue - moneyOfCustomerPaid).ToString());
                                int SkipOrReInputOrCancel = ConsoleUlts.PressCharacterTo("Skip Payment", "Re-Input Money", "Cancel Payment", null);
                                if (SkipOrReInputOrCancel == 0)
                                {
                                    ConsoleUlts.Alert(ConsoleEnum.Alert.Warning, "Skip Payment");
                                    activeChoosePaymentMethod = true;
                                    activePayment = true;
                                    break;
                                }
                                else if (SkipOrReInputOrCancel == 1) continue;
                                else if (SkipOrReInputOrCancel == 2)
                                {
                                    Console.WriteLine(spaces + "Are you sure want to Cancel Payment?");
                                    if (ConsoleUlts.PressYesOrNo("Cancel Payment", "Not Cancel"))
                                    {
                                        PaymentResult = false;
                                        orderBL.CancelPayment(orderWantToPayment);
                                        ConsoleUlts.Alert(ConsoleEnum.Alert.Success, "Cancel Payment");
                                        activeChoosePaymentMethod = true;
                                        activePayment = true;
                                        break;
                                    }
                                    else continue;
                                }
                            }
                            Console.ReadKey();
                        } while (activeEnterMoney == false);
                    }
                }
                else
                {
                    if (phase != 0) consoleUI.PrintTimeLine(listPhase, 4);
                    else consoleUI.PrintTimeLine(listPhase, phase);
                    consoleUI.PrintOrder(orderWantToPayment, listTradeInPhone, imeiTemp);
                    if (listTradeInPhone.Count() == 0)
                    {
                        int PaymentOrSkipOrCancel = ConsoleUlts.PressCharacterTo("Confirm Payment", "Skip Payment", "Cancel Payment", null);
                        if (PaymentOrSkipOrCancel == 0)
                        {
                            PaymentResult = true;
                            orderBL.Payment(orderWantToPayment);
                            ConsoleUlts.Alert(ConsoleEnum.Alert.Success, "Confirm Payment");
                            activeChoosePaymentMethod = true;
                            activePayment = true;
                            break;
                        }
                        else if (PaymentOrSkipOrCancel == 1)
                        {
                            PaymentResult = false;
                            ConsoleUlts.Alert(ConsoleEnum.Alert.Success, "Skip Payment");
                            activeChoosePaymentMethod = true;
                            activePayment = true;
                            break;
                        }
                        else if (PaymentOrSkipOrCancel == 2)
                        {
                            PaymentResult = false;
                            ConsoleUlts.Alert(ConsoleEnum.Alert.Success, "Cancel Payment");
                            orderBL.CancelPayment(orderWantToPayment);
                            activeChoosePaymentMethod = true;
                            activePayment = true;
                            break;
                        }
                    }
                    else
                    {
                        if (phase != 0) consoleUI.PrintTimeLine(listPhase, 4);
                        else consoleUI.PrintTimeLine(listPhase, phase);
                        consoleUI.PrintOrder(orderWantToPayment, listTradeInPhone, imeiTemp);

                        if (ConsoleUlts.PressYesOrNo("Confirm Payment", "Cancel Payment"))
                        {
                            PaymentResult = true;
                            orderBL.Payment(orderWantToPayment);
                            ConsoleUlts.Alert(ConsoleEnum.Alert.Success, "Confirm Payment");
                            activeChoosePaymentMethod = true;
                            activePayment = true;
                            break;
                        }
                        else
                        {
                            PaymentResult = false;
                            ConsoleUlts.Alert(ConsoleEnum.Alert.Success, "Cancel Payment");
                            orderBL.CancelPayment(orderWantToPayment);
                            activeChoosePaymentMethod = true;
                            activePayment = true;
                            break;
                        }


                    }
                }
            } while (activeChoosePaymentMethod == false);
            if (PaymentResult && listTradeInPhone.Count() != 0)
            {
                orderBL.HandleTradeIn(orderForTradeIn);
            }
            return PaymentResult;
        }
    }
}


