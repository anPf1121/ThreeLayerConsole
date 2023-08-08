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
            string spaces = new string(' ', centeredPosition);
            int secondcenteredPosition = (Console.WindowWidth - "|===================================================================================================|".Length) / 2;
            string secondspaces = new string(' ', secondcenteredPosition);

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
                            phoneId = ConsoleUlts.InputIDValidation(phoneBL.GetAllPhone().Count(), $"{spaces}Enter Phone ID", "Invalid Phone ID");
                        }

                        if (!listPhoneSearch) break;

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
                            if (ConsoleUlts.PressYesOrNo("Back Previous Phase", "Continue"))
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
                        consoleUI.PrintOrderDetails(ord);
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

                        if (ConsoleUlts.PressYesOrNo("Back Previous Phase", "Confirm Order")) currentPhase--;

                        else currentPhase++;

                        break;
                    case 5:
                        consoleUI.PrintTimeLine(listPhase, count, currentPhase);
                        Order order = new Order(ConsoleUlts.GenerateID(), DateTime.Now, loginManager.LoggedInStaff, new Staff(0, "", "", "", "", "", StaffEnum.Role.Accountant, StaffEnum.Status.Active), customer, phonesInOrder, OrderEnum.Status.Pending, new List<DiscountPolicy>(), "", 0);
                        consoleUI.PrintSellerOrder(order);
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
        public void Payment()
        {
            int centeredPosition = (Console.WindowWidth - "|--------------------------------------------------------------------------------------------|".Length) / 2;
            string spaces = new string(' ', centeredPosition);
            int secondcenteredPosition = (Console.WindowWidth - "|===================================================================================================|".Length) / 2;
            string secondspaces = new string(' ', secondcenteredPosition);

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
            bool dontKnowHowtoCall = true;
            do
            {
                currentPhase = 1;
                bool dontKnowHowtoCall1 = false;
                bool? showOrderList = ConsoleUlts.ListOrderPagination(ListOrderPending, listPhase, count, currentPhase, loginManager.LoggedInStaff);
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
                        orderID = ConsoleUlts.GetInputString($"{spaces}Choose An Order ID To Payment").ToUpper();
                        orderWantToPayment = new OrderBL().GetOrderById(orderID) ?? null;
                        if (orderWantToPayment.OrderID == "")
                        {
                            ConsoleUlts.Alert(ConsoleEnum.Alert.Error, "Invalid Order ID");
                        }
                        else orderWantToPayment.Accountant = this.loginManager.LoggedInStaff;
                    } while (orderWantToPayment.OrderID == "");
                    //Wait to display orderdetail
                    consoleUI.PrintOrderDetailsInfo(orderWantToPayment);
                    if (orderWantToPayment.PhoneDetails.Count() == 0)
                    {
                        Console.WriteLine("Cant Payment! This Order doesnt have any phone!");
                        Console.WriteLine("Press any key to back to previous menu");
                        Console.ReadKey();
                        break;
                    }
                    Console.Write($"{secondspaces}Press Enter to keep doing payment OR Any Key to choose order again.");
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
                            consoleUI.PrintTimeLine(listPhase, count, currentPhase);
                            Console.WriteLine("👉 Choose a payment method");
                            foreach (var payment in ListPaymentMethod)
                            {
                                Console.WriteLine(payment.Key + ". " + payment.Value);
                                choicePattern.Add(payment.Key);
                            }
                            Console.Write("Your choice: ");
                            input = Console.ReadLine() ?? "";
                            while (!ConsoleUlts.CheckInputIDValid(input, choicePattern))
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
                                    consoleUI.PrintTimeLine(listPhase, count, currentPhase);
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
                                        while (!ConsoleUlts.CheckInputIDValid(input, choicePattern))
                                        {
                                            Console.Write("Choose again: ");
                                            input = Console.ReadLine() ?? "";
                                        }
                                        choice = Convert.ToInt32(input);
                                        Console.WriteLine("✅ Show Discount Policy Detail");
                                        consoleUI.PrintDiscountPolicyDetail(new DiscountPolicyBL().GetDiscountPolicyByID(choice));
                                    }
                                    else
                                    {
                                        Console.WriteLine(" - Doesnt have any discount policy valid to this Payment method !");
                                        Console.WriteLine(" - Or Your Order are not eligible to apply any discountpolicy !");
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
                                            consoleUI.PrintTimeLine(listPhase, count, currentPhase);
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
                                                while (!ConsoleUlts.CheckInputIDValid(input, choicePattern))
                                                {
                                                    Console.Write("Choose again: ");
                                                    input = Console.ReadLine() ?? "";
                                                }
                                                choice = Convert.ToInt32(input);
                                                Console.WriteLine("✅ Show Discount Policy Detail");
                                                consoleUI.PrintDiscountPolicyDetail(new DiscountPolicyBL().GetDiscountPolicyByID(choice));
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
                                                consoleUI.PrintTimeLine(listPhase, count, currentPhase);
                                                consoleUI.PrintSellerOrder(orderWantToPayment);
                                                Console.WriteLine("Press Enter to Confirm order - OR - ESC to Cancel order - OR - Any key for not do anything.");
                                                keyInfo = Console.ReadKey();
                                                if (keyInfo.Key == ConsoleKey.Enter)
                                                {

                                                    orderBL.Payment(orderWantToPayment);
                                                    Console.WriteLine("Executing Payment...");
                                                    System.Threading.Thread.Sleep(3000);
                                                    Console.WriteLine("Payment Completed! Press Any Key to Back to previous Menu");
                                                    Console.ReadKey();

                                                }
                                                else if (keyInfo.Key == ConsoleKey.Escape)
                                                {
                                                    orderBL.CancelPayment(orderWantToPayment);
                                                    Console.WriteLine("Executing...");
                                                    System.Threading.Thread.Sleep(3000);
                                                    Console.WriteLine("Cancel Completed !Press Any Key to Back to Previous Menu");
                                                    Console.ReadKey();
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Doesn't do anything to this order ! Press Any Key to Back to Previous Menu");
                                                    ConsoleUlts.ClearCurrentConsoleLine();
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
            int centeredPosition = (Console.WindowWidth - "|--------------------------------------------------------------------------------------------|".Length) / 2;
            string spaces = new string(' ', centeredPosition);
            int secondcenteredPosition = (Console.WindowWidth - "|===================================================================================================|".Length) / 2;
            string secondspaces = new string(' ', secondcenteredPosition);
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            string handleTitle = consoleUI.GetHandleOrderANSIText();
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
                        consoleUI.PrintTimeLine(listPhase, count, currentPhase);
                        consoleUI.PrintOrderDetailsInfo(order);

                        if (!ConsoleUlts.PressYesOrNo("Continue", "Back Previous Phase"))
                        {
                            currentPhase--;
                            break;
                        }
                        else currentPhase++;

                        break;
                    case 3:
                        consoleUI.PrintTimeLine(listPhase, count, currentPhase);
                        consoleUI.PrintSellerOrder(order);
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
            } while (currentPhase != 4);
            return 1;
        }

    }
}
