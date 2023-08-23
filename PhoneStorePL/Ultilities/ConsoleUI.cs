using System;
using GUIEnum;
using BusinessEnum;
using Model;
using System.Globalization; // thu vien format tien
using Ults;
using BL;

namespace UI;
class ConsoleUI
{
    public void ConsoleForegroundColor(ConsoleEnum.Color colorEnum)
    {
        switch (colorEnum)
        {
            case ConsoleEnum.Color.Red:
                Console.ForegroundColor = ConsoleColor.Red;
                break;
            case ConsoleEnum.Color.Green:
                Console.ForegroundColor = ConsoleColor.Green;
                break;
            case ConsoleEnum.Color.Blue:
                Console.ForegroundColor = ConsoleColor.Blue;
                break;
            case ConsoleEnum.Color.Yellow:
                Console.ForegroundColor = ConsoleColor.Yellow;
                break;
            case ConsoleEnum.Color.White:
                Console.ForegroundColor = ConsoleColor.White;
                break;
            default:
                break;
        }
    }
    public void PrintLine()
    {
        int centeredPosition = (Console.WindowWidth - "|--------------------------------------------------------------------------------------------|".Length) / 2;
        string spaces = centeredPosition > 0 ? new string(' ', centeredPosition) : "";
        Console.WriteLine(spaces + @"|--------------------------------------------------------------------------------------------|");
    }
    public void FullWidthTinyLine()
    {
        Console.WriteLine(new string('-', Console.WindowWidth));
    }
    public void PrintPhoneBorderLine()
    {
        int centeredPosition = (Console.WindowWidth - "|============================================================================================|".Length) / 2;
        string spaces = centeredPosition > 0 ? new string(' ', centeredPosition) : "";
        Console.WriteLine(spaces + "|============================================================================================|");
        Console.WriteLine(spaces + "| {0, -10} | {1, -25} | {2, -13} | {3, -13} | {4, -17} |", "ID", "Phone Name", "Brand", "OS", "Mobile Network");
        Console.WriteLine(spaces + "|============================================================================================|");
    }
    public void PrintOrderBorderLine()
    {
        int centeredPosition = (Console.WindowWidth - "|============================================================================================|".Length) / 2;
        string spaces = centeredPosition > 0 ? new string(' ', centeredPosition) : "";
        Console.WriteLine(spaces + "|============================================================================================|");
        Console.WriteLine(spaces + "| {0, -13} | {1, -25} | {2, -23} | {3, -20} |", "ID", "Customer Name", "Order Date", "Status");
        Console.WriteLine(spaces + "|============================================================================================|");
    }
    public void PrintPhoneInfo(Phone phone)
    {
        int centeredPosition = (Console.WindowWidth - "|============================================================================================|".Length) / 2;
        string spaces = centeredPosition > 0 ? new string(' ', centeredPosition) : "";
        Console.WriteLine(spaces + "| {0, -10} | {1, -25} | {2, -13} | {3, -13} | {4, -17} |", phone.PhoneID, phone.PhoneName, phone.Brand.BrandName, phone.OS, phone.Connection);
    }
    public void PrintOrderInfo(Order order)
    {
        int centeredPosition = (Console.WindowWidth - "|--------------------------------------------------------------------------------------------|".Length) / 2;
        string spaces = centeredPosition > 0 ? new string(' ', centeredPosition) : "";
        Console.WriteLine(spaces + "| {0, -13} | {1, -25} | {2, -23} | {3, -20} |", order.OrderID, order.Customer.CustomerName, order.CreateAt, order.OrderStatus);
    }
    public void PrintPhoneModelTitle()
    {
        string spaces = AlignCenter("|============================================================================================|");
        Console.WriteLine(spaces + "|============================================================================================|");
        Console.WriteLine(spaces + "| PHONE MODEL                                                                                |");
        Console.WriteLine(spaces + "|============================================================================================|");
        Console.WriteLine(spaces + "| {0, -10} | {1, -38} | {2, 13} | {3, 20} |", "Detail ID", "Phone Model", "Quantity", "Price");
        Console.WriteLine(spaces + "|============================================================================================|");
    }
    public void PrintPhoneTradeInDetailInfo(PhoneDetail phoneDetail)
    {
        string spaces = AlignCenter("|============================================================================================|");
        Console.WriteLine(spaces + "|============================================================================================|");
        Console.WriteLine(spaces + "| PHONE DETAILS INFOMATION                                                                   |");
        Console.WriteLine(spaces + "|============================================================================================|");
        Console.WriteLine(spaces + "| Phone Name: {0, -50} |", phoneDetail.Phone.PhoneName.PadRight(78));
        Console.WriteLine(spaces + "| Brand: {0, -50} |", phoneDetail.Phone.Brand.BrandName.PadRight(83));
        Console.WriteLine(spaces + "| Camera: {0, -50} |", phoneDetail.Phone.Camera!.PadRight(82));
        Console.WriteLine(spaces + "| RAM: {0, -50} |", phoneDetail.Phone.RAM!.PadRight(85));
        Console.WriteLine(spaces + "| Weight: {0, -50} |", phoneDetail.Phone.Weight.PadRight(82));
        Console.WriteLine(spaces + "| Processor: {0, -50} |", phoneDetail.Phone.Processor!.PadRight(79));
        Console.WriteLine(spaces + "| Battery: {0, -50} |", phoneDetail.Phone.BatteryCapacity.PadRight(81));
        Console.WriteLine(spaces + "| OS: {0, -50} |", phoneDetail.Phone.OS.PadRight(86));
        Console.WriteLine(spaces + "| Sim Slot: {0, -50} |", phoneDetail.Phone.SimSlot.PadRight(80));
        Console.WriteLine(spaces + "| Screen : {0, -50} |", phoneDetail.Phone.Screen.PadRight(81));
        Console.WriteLine(spaces + "| Connection: {0, -50} |", phoneDetail.Phone.Connection.PadRight(78));
        Console.WriteLine(spaces + "| Charge Port: {0, -50} |", phoneDetail.Phone.ChargePort.PadRight(77));
        Console.WriteLine(spaces + "| Release Date: {0, -50} |", phoneDetail.Phone.ReleaseDate.ToString().PadRight(76));
        Console.WriteLine(spaces + "| Description: {0, -50} |", phoneDetail.Phone.Description.PadRight(77));
        Console.WriteLine(spaces + "| Phone Status Type: {0, -50} |", phoneDetail.PhoneStatusType.ToString().PadRight(71));
        Console.WriteLine(spaces + "| Color: {0, -50} |", phoneDetail.PhoneColor.Color.PadRight(83));
        Console.WriteLine(spaces + "| ROMSize: {0, -50} |", phoneDetail.ROMSize.ROM.PadRight(81));
        Console.WriteLine(spaces + "|============================================================================================|");

    }
    public void PrintTimeLine(string[] phase, int currentPhase)
    {
        int itemCount = 0;
        Console.Clear();
        FullWidthTinyLine();
        foreach (string item in phase)
        {
            itemCount++;
            if (itemCount == currentPhase)
            {
                ConsoleForegroundColor(ConsoleEnum.Color.Green);
                Console.Write(((itemCount == currentPhase) ? " > " + item : " > " + item));
                ConsoleForegroundColor(ConsoleEnum.Color.White);
            }
            else
            {
                Console.Write(((itemCount == currentPhase) ? " > " + item : " > " + item));
            }
            if (itemCount == phase.Length)
                Console.Write("\n");
        }
        FullWidthTinyLine();
        itemCount = 0;
    }
    public void PrintOrder(Order ord)
    {
        Dictionary<DiscountPolicy, int> DiscountAndRepeatTime = new Dictionary<DiscountPolicy, int>();
        // Xu li lan lap Discount cua TradeInPolicy
        List<DiscountPolicy> ListTemp = new List<DiscountPolicy>();
        foreach (var discount in ord.DiscountPolicies)
        {
            int count = 0;
            foreach (var discountTemp in ListTemp)
            {
                if (discount.Title == discountTemp.Title && discount.PhoneDetail.PhoneDetailID != 0 && discount.MoneySupported == discountTemp.MoneySupported) count++;
            }
            if (count == 0) ListTemp.Add(discount);
        }
        foreach (var discountTemp in ListTemp)
        {
            int RepeatTime = 0;
            foreach (var discount in ord.DiscountPolicies)
            {
                if (discountTemp.Title == discount.Title && discountTemp.MoneySupported == discount.MoneySupported) RepeatTime++;
            }
            DiscountAndRepeatTime.Add(discountTemp, RepeatTime);
        }
        int centeredPosition = (Console.WindowWidth - "|===========================================================================================================================|".Length) / 2;
        string spaces = centeredPosition > 0 ? new string(' ', centeredPosition) : "";
        Console.WriteLine("\n" + spaces + "|===========================================================================================================================|");
        Console.WriteLine(spaces + "|------------------------------------------------------- VTC Mobile --------------------------------------------------------|");
        Console.WriteLine(spaces + "|===========================================================================================================================|");
        Console.WriteLine(spaces + "|                                                   Order ID: " + ord.OrderID + "                                                  |");
        Console.WriteLine(spaces + "|===========================================================================================================================|");
        Console.WriteLine(spaces + "| Website: https://vtc.edu.vn/                                                                                              |");
        Console.WriteLine(spaces + "| Address: 18 Tam Trinh, Quận Hai Bà Trưng, Thành Phố Hà Nội                                                                |");
        Console.WriteLine(spaces + "| Phone Number: 0999999999                                                                                                  |");
        Console.WriteLine(spaces + "|===========================================================================================================================|");
        Console.WriteLine(spaces + "| Order Create Time: {0, -30}|", DateTime.Now.ToString().PadRight(103));
        if (ord.Customer.CustomerName != "" && ord.Customer.PhoneNumber != "")
        {
            Console.WriteLine(spaces + "| Customer: {0, -30}|", ord.Customer.CustomerName.PadRight(112));
            Console.WriteLine(spaces + "| Address: {0, -50}|", ord.Customer.Address.PadRight(113));
            Console.WriteLine(spaces + "| Phone Number: {0, -12}|", ord.Customer.PhoneNumber.PadRight(108));
        }
        Console.Write((ord.Accountant.StaffID != 0) ? (spaces + "| Payment Method: {0, 35} |" + "\n") : "", ord.PaymentMethod.PadRight(105));
        Console.WriteLine(spaces + "|===========================================================================================================================|");
        PrintOrderDetails(ord);
        Console.WriteLine(spaces + "|===========================================================================================================================|");
        if (ord.DiscountPolicies.Count() != 0)
        {

            Console.WriteLine(spaces + "| {0, 46}                                                                            |", SetTextBolder("All DiscountPolicy Be Apply For This Order Is "));
            foreach (var dp in DiscountAndRepeatTime)
            {
                if (dp.Key.DiscountPrice != 0) Console.WriteLine(spaces + "| - {0, -100} |", (dp.Key.Title + (" " + "(" + dp.Value + "x" + ")") + ": " + SetTextBolder(FormatPrice(dp.Key.DiscountPrice))).PadRight(119));
                if (dp.Key.MoneySupported != 0) Console.WriteLine(spaces + "| - {0, -100} |", (dp.Key.Title + (" " + "(" + dp.Value + "x" + ")") + ": " + SetTextBolder(FormatPrice(dp.Key.MoneySupported))).PadRight(119));
                if (ord.OrderStatus == OrderEnum.Status.Pending || ord.OrderStatus == OrderEnum.Status.Confirmed) ord.TotalDue -= dp.Key.DiscountPrice;
            }
        }
        Console.WriteLine(spaces + "| {0, 40}{1, -26}{2, 15}|", "", "Total Due: ", SetTextBolder(FormatPrice(ord.GetTotalDue()).PadRight(56)));
        if (ord.Accountant.StaffID != 0)
        {
            Console.WriteLine(spaces + "| {0, 40}{1, -26}{2, 23}|", "", "Discount Price: ", SetTextBolder(FormatPrice(ord.TotalDue - ord.GetTotalDue()).ToString().PadRight(56)));
            Console.WriteLine(spaces + "| {0, 40}{1, -15}{2, 21}|", "", "Total Due After Discount: ", SetTextBolder(FormatPrice(ord.TotalDue).ToString().PadRight(56)));
        }


        Console.WriteLine(spaces + "| {0, 40}{1, -26}{2, 49}|", "", "To String: ", (ord.Accountant.StaffID != 0) ? SetTextBolder(ConvertNumberToWords(ord.TotalDue).PadRight(56)) : SetTextBolder(ConvertNumberToWords(ord.GetTotalDue()).PadRight(56)));
        Console.WriteLine(spaces + "|---------------------------------------------------------------------------------------------------------------------------|");
        Console.WriteLine(spaces + "|===========================================================================================================================|");
        Console.WriteLine(spaces + "|{0, 10}{1, -35} {2, -40} {3, -36}|", " ", "Customer", "Seller", "Accountant", " ");
        Console.WriteLine(spaces + "|{0, 10}{1, -35} {2, -40} {3, -36}|", " ", (ord.Customer.CustomerName != "") ? ord.Customer.CustomerName : "", ord.Seller.StaffName + " - ID: " + ord.Seller.StaffID, (ord.Accountant.StaffID == 0) ? "" : (ord.Accountant.StaffName + " - ID: " + ord.Accountant.StaffID), " ");
        Console.WriteLine(spaces + "|===========================================================================================================================|");
        ord.TotalDue = ord.GetTotalDue();
    }
    public void PrintOrderDetails(Order ord)
    {
        List<PhoneDetail> ListTemp = new List<PhoneDetail>();
        foreach (var imei in ord.ListImeiInOrder)
        {
            ListTemp.Add(imei.PhoneDetail);
        }
        List<PhoneDetail> ListPhoneInOrder = new List<PhoneDetail>();
        foreach (var phone in ListTemp)
        {
            bool checkRepeate = false;
            foreach (var phone1 in ListPhoneInOrder)
            {
                if (phone.PhoneDetailID == phone1.PhoneDetailID) checkRepeate = true;
            }
            if (!checkRepeate) ListPhoneInOrder.Add(phone);
        }
        Dictionary<PhoneDetail, int> ListPhoneAndQuantity = new Dictionary<PhoneDetail, int>();
        foreach (var phone in ListPhoneInOrder)
        {
            int count = 0;
            foreach (var temp in ListTemp)
            {
                if (temp.PhoneDetailID == phone.PhoneDetailID) count++;
            }
            ListPhoneAndQuantity.Add(phone, count);
        }
        int centeredPosition = (Console.WindowWidth - "|===========================================================================================================================|".Length) / 2;
        string spaces = centeredPosition > 0 ? new string(' ', centeredPosition) : "";
        CultureInfo cultureInfo = new CultureInfo("vi-VN");
        int printImeiHandle = 0;
        int printImeiHandle2 = 0;
        Console.WriteLine(spaces + "| {0, -16} | {1, -30} | {2, -15} | {3, 15} | {4, 15} | {5, 15} |", "Phone Detail ID", "Phone Model", "Imei", "Quantity", "Price", "Total Price");
        Console.WriteLine(spaces + "|===========================================================================================================================|");
        foreach (var imei in ListPhoneAndQuantity)
        {
            int quan = 0;
            printImeiHandle = imei.Key.PhoneDetailID;
            Console.WriteLine(spaces + "|---------------------------------------------------------------------------------------------------------------------------|");
            foreach (var imei1 in ord.ListImeiInOrder)
            {
                if (imei.Key.PhoneDetailID == imei1.PhoneDetail.PhoneDetailID)
                {
                    quan += 1;
                    Console.WriteLine(spaces + "| {0, -16} | {1, -30} | {2, -15} | {3, 15} | {4, 15} | {5, 15} |", (printImeiHandle != printImeiHandle2) ? imei1.PhoneDetail.PhoneDetailID : "", (printImeiHandle != printImeiHandle2) ? (imei1.PhoneDetail.Phone.PhoneName + " " + imei1.PhoneDetail.PhoneColor.Color + " " + imei1.PhoneDetail.ROMSize.ROM + $" ({imei1.PhoneDetail.PhoneStatusType})") : "", imei1.PhoneImei, (printImeiHandle != printImeiHandle2) ? imei.Value : "", (printImeiHandle != printImeiHandle2) ? FormatPrice(imei1.PhoneDetail.Price) : "", (printImeiHandle != printImeiHandle2) ? FormatPrice(ord.GetTotalDueForEachPhone(imei1.PhoneDetail.PhoneDetailID)) : "");
                    printImeiHandle2 = printImeiHandle;
                }
            }

        }
    }

    public void PrintPhoneModelInfo(PhoneDetail phoneDetail)
    {
        string spaces = AlignCenter("|============================================================================================|");
        CultureInfo cultureInfo = new CultureInfo("vi-VN");
        Console.WriteLine(spaces + "| {0, -10} | {1, -38} | {2, 13} | {3, 20} |", phoneDetail.PhoneDetailID, phoneDetail.Phone.PhoneName + " " + phoneDetail.PhoneColor.Color + "/" + phoneDetail.ROMSize.ROM + " (" + phoneDetail.PhoneStatusType + ")", (phoneDetail.Quantity != 0) ? phoneDetail.Quantity : "Out Of Stock", FormatPrice(phoneDetail.Price));
    }
    public void PrintTitle(string? title, string? subTitle, Staff? staffLoggedIn)
    {
        int centeredPosition = (Console.WindowWidth - "|--------------------------------------------------------------------------------------------|".Length) / 2;
        string spaces = centeredPosition > 0 ? new string(' ', centeredPosition) : "";
        if (title != null)
        {
            PrintLine();
            Console.WriteLine(title);
            PrintLine();
        }
        if (subTitle != null)
        {
            PrintLine();
            Console.WriteLine(subTitle);
            PrintLine();
        }
        if (staffLoggedIn != null)
        {
            ConsoleForegroundColor(ConsoleEnum.Color.Green);
            Console.WriteLine(spaces + "| {0, -50} |", (((staffLoggedIn.Role == StaffEnum.Role.Accountant) ? "Accountant: " : "Seller: ") + staffLoggedIn.StaffName + " - ID: " + staffLoggedIn.StaffID).PadRight(90));
            ConsoleForegroundColor(ConsoleEnum.Color.White);
        }
        PrintLine();
    }
    public string GetChoosePaymentMethodText()
    {
        int centeredPosition = (Console.WindowWidth - "|--------------------------------------------------------------------------------------------|".Length) / 2;
        string spaces = centeredPosition > 0 ? new string(' ', centeredPosition) : "";
        return $@"{spaces}|                                                                                            |
{spaces}|                 ┌─┐┬ ┬┌─┐┌─┐┌─┐┌─┐  ┌─┐┌─┐┬ ┬┌┬┐┌─┐┌┐┌┌┬┐  ┌┬┐┌─┐┌┬┐┬ ┬┌─┐┌┬┐              |
{spaces}|                 │  ├─┤│ ││ │└─┐├┤   ├─┘├─┤└┬┘│││├┤ │││ │   │││├┤  │ ├─┤│ │ ││              |
{spaces}|                 └─┘┴ ┴└─┘└─┘└─┘└─┘  ┴  ┴ ┴ ┴ ┴ ┴└─┘┘└┘ ┴   ┴ ┴└─┘ ┴ ┴ ┴└─┘─┴┘              |
{spaces}|                                                                                            |       ";
    }
    public string GetChooseDiscountPolicyText()
    {
        int centeredPosition = (Console.WindowWidth - "|--------------------------------------------------------------------------------------------|".Length) / 2;
        string spaces = centeredPosition > 0 ? new string(' ', centeredPosition) : "";
        return $@"{spaces}|                                                                                            |
{spaces}|                   ┌─┐┬ ┬┌─┐┌─┐┌─┐┌─┐  ┌┬┐┬┌─┐┌─┐┌─┐┬ ┬┌┐┌┌┬┐  ┌─┐┌─┐┬  ┬┌─┐┬ ┬             |
{spaces}|                   │  ├─┤│ ││ │└─┐├┤    │││└─┐│  │ ││ ││││ │   ├─┘│ ││  ││  └┬┘             |
{spaces}|                   └─┘┴ ┴└─┘└─┘└─┘└─┘  ─┴┘┴└─┘└─┘└─┘└─┘┘└┘ ┴   ┴  └─┘┴─┘┴└─┘ ┴              |
{spaces}|                                                                                            | ";
    }
    public string GetAppANSIText()
    {
        int centeredPosition = (Console.WindowWidth - "|--------------------------------------------------------------------------------------------|".Length) / 2;
        string spaces = centeredPosition > 0 ? new string(' ', centeredPosition) : "";
        return $@"{spaces}|                                                                                            |
{spaces}|                             ┌─┐┬ ┬┌─┐┌┐┌┌─┐  ┌─┐┌┬┐┌─┐┬─┐┌─┐                               |
{spaces}|                             ├─┘├─┤│ ││││├┤   └─┐ │ │ │├┬┘├┤                                |
{spaces}|                             ┴  ┴ ┴└─┘┘└┘└─┘  └─┘ ┴ └─┘┴└─└─┘                               |
{spaces}|                                                                                            |";
    }
    public string GetTradeInANSIText()
    {
        int centeredPosition = (Console.WindowWidth - "|--------------------------------------------------------------------------------------------|".Length) / 2;
        string spaces = centeredPosition > 0 ? new string(' ', centeredPosition) : "";
        return $@"{spaces}|                                                                                            |
{spaces}|                                  ┌┬┐┬─┐┌─┐┌┬┐┌─┐  ┬┌┐┌                                     |
{spaces}|                                   │ ├┬┘├─┤ ││├┤   ││││                                     |
{spaces}|                                   ┴ ┴└─┴ ┴─┴┘└─┘  ┴┘└┘                                     |
{spaces}|                                                                                            |";


    }
    public string GetPaymentANSIText()
    {
        int centeredPosition = (Console.WindowWidth - "|--------------------------------------------------------------------------------------------|".Length) / 2;
        string spaces = centeredPosition > 0 ? new string(' ', centeredPosition) : "";
        return $@"{spaces}|                                                                                            |
{spaces}|                                    ┌─┐┌─┐┬ ┬┌┬┐┌─┐┌┐┌┌┬┐                                   |
{spaces}|                                    ├─┘├─┤└┬┘│││├┤ │││ │                                    |
{spaces}|                                    ┴  ┴ ┴ ┴ ┴ ┴└─┘┘└┘ ┴                                    |
{spaces}|                                                                                            |";
    }

    public void GetFooterPagination(int currentPage, int countPage)
    {
        int centeredPosition = (Console.WindowWidth - "|============================================================================================|".Length) / 2;
        string spaces = centeredPosition > 0 ? new string(' ', centeredPosition) : "";
        Console.WriteLine(spaces + "|============================================================================================|");
        Console.WriteLine(spaces + "| {0,42}" + "< " + $"{currentPage}/{countPage}" + " >".PadRight(44) + "|", " ");
        Console.WriteLine(spaces + "|============================================================================================|");
        Console.WriteLine(spaces + "| Press 'Left Arrow' To Back Previous Page, 'Right Arror' To Next Page                       |");
        Console.WriteLine(spaces + "| Press 'Space' To Choose a phone, 'B' To Back Previous Menu                                 |");
        Console.WriteLine(spaces + "|============================================================================================|");

    }
    public string GetSearchANSIText()
    {
        int centeredPosition = (Console.WindowWidth - "|--------------------------------------------------------------------------------------------|".Length) / 2;
        string spaces = centeredPosition > 0 ? new string(' ', centeredPosition) : "";
        return $@"{spaces}|                                                                                            |
{spaces}|                                     ┌─┐┌─┐┌─┐┬─┐┌─┐┬ ┬                                     |
{spaces}|                                     └─┐├┤ ├─┤├┬┘│  ├─┤                                     |
{spaces}|                                     └─┘└─┘┴ ┴┴└─└─┘┴ ┴                                     |
{spaces}|                                                                                            |";
    }
    public string GetHandleOrderANSIText()
    {
        int centeredPosition = (Console.WindowWidth - "|--------------------------------------------------------------------------------------------|".Length) / 2;
        string spaces = centeredPosition > 0 ? new string(' ', centeredPosition) : "";
        return $@"{spaces}|                                                                                            |
{spaces}|                          ┬ ┬┌─┐┌┐┌┌┬┐┬  ┌─┐  ┌─┐┬─┐┌┬┐┌─┐┬─┐┌─┐                            |
{spaces}|                          ├─┤├─┤│││ │││  ├┤   │ │├┬┘ ││├┤ ├┬┘└─┐                            |
{spaces}|                          ┴ ┴┴ ┴┘└┘─┴┘┴─┘└─┘  └─┘┴└──┴┘└─┘┴└─└─┘                            |
{spaces}|                                                                                            |";
    }
    public string[] GetMenuItemSearch()
    {
        return new string[] { "Search All Phone", "Search Phone By Information", "Back To Previous Menu" };
    }
    // public string[] GetMenuReportItem()
    // {
    //     return new string[] { "Add Phone Model To Report", "Add Revenue Growth Rate To Report", "Back To Previous Menu" };
    // }
    public string[] GetCreateOrderTimeLine()
    {
        return new string[] { "Search Phone", "Add Phone To Order", "Add More Phone?", "Enter Customer Info", "Confirm Order" };
    }
    public string[] GetPaymentTimeLine()
    {
        return new string[] { "Choose an Order", "Choose Paymentmethod", "Enter Money", "Confirm or Cancel or Skip Payment" };
    }
    public string[] GetTradeInTimeLine()
    {
        return new string[] { "Choose an Order", "Check and Add Customer's Phone", "Confirm or Cancel TradeIn" };
    }
    public string[] GetHandleOrderTimeLine()
    {
        return new string[] { "Show orders", "Confirm Handle" };
    }
    public string GetCustomerInfoANSIText()
    {
        int centeredPosition = (Console.WindowWidth - "|--------------------------------------------------------------------------------------------|".Length) / 2;
        string spaces = centeredPosition > 0 ? new string(' ', centeredPosition) : "";
        string title =
            $@"{spaces}|                                                                                            |                                                                      
{spaces}|          ┌─┐┌┐┌┌┬┐┌─┐┬─┐  ┌─┐┬ ┬┌─┐┌┬┐┌─┐┌┬┐┌─┐┬─┐  ┬┌┐┌┌─┐┌─┐┬─┐┌┬┐┌─┐┌┬┐┬┌─┐┌┐┌          |
{spaces}|          ├┤ │││ │ ├┤ ├┬┘  │  │ │└─┐ │ │ ││││├┤ ├┬┘  ││││├┤ │ │├┬┘│││├─┤ │ ││ ││││          |
{spaces}|          └─┘┘└┘ ┴ └─┘┴└─  └─┘└─┘└─┘ ┴ └─┘┴ ┴└─┘┴└─  ┴┘└┘└  └─┘┴└─┴ ┴┴ ┴ ┴ ┴└─┘┘└┘          |
{spaces}|                                                                                            |";
        return title;
    }
    public string GetAddPhoneToOrderANSIText()
    {
        int centeredPosition = (Console.WindowWidth - "|--------------------------------------------------------------------------------------------|".Length) / 2;
        string spaces = centeredPosition > 0 ? new string(' ', centeredPosition) : "";
        return
        $@"{spaces}|                                                                                            |
{spaces}|                    ┌─┐┌┬┐┌┬┐  ┌─┐┬ ┬┌─┐┌┐┌┌─┐  ┌┬┐┌─┐  ┌─┐┬─┐┌┬┐┌─┐┬─┐                     |
{spaces}|                    ├─┤ ││ ││  ├─┘├─┤│ ││││├┤    │ │ │  │ │├┬┘ ││├┤ ├┬┘                     |
{spaces}|                    ┴ ┴─┴┘─┴┘  ┴  ┴ ┴└─┘┘└┘└─┘   ┴ └─┘  └─┘┴└──┴┘└─┘┴└─                     |
{spaces}|                                                                                            |";
    }
    public string GetCheckCustomerPhoneANSIText()
    {
        int centeredPosition = (Console.WindowWidth - "|--------------------------------------------------------------------------------------------|".Length) / 2;
        string spaces = centeredPosition > 0 ? new string(' ', centeredPosition) : "";
        return $@"{spaces}|                                                                                            |
{spaces}|                ╔═╗┬ ┬┌─┐┌─┐┬┌─  ╔═╗┬ ┬┌─┐┌┬┐┌─┐┌┬┐┌─┐┬─┐┌─┐  ╔═╗┬ ┬┌─┐┌┐┌┌─┐               |
{spaces}|                ║  ├─┤├┤ │  ├┴┐  ║  │ │└─┐ │ │ ││││├┤ ├┬┘└─┐  ╠═╝├─┤│ ││││├┤                |
{spaces}|                ╚═╝┴ ┴└─┘└─┘┴ ┴  ╚═╝└─┘└─┘ ┴ └─┘┴ ┴└─┘┴└─└─┘  ╩  ┴ ┴└─┘┘└┘└─┘               |
{spaces}|                                                                                            |";
    }
    public string GetLoginANSIText()
    {
        int centeredPosition = (Console.WindowWidth - "|--------------------------------------------------------------------------------------------|".Length) / 2;
        string spaces = centeredPosition > 0 ? new string(' ', centeredPosition) : "";
        return $@"{spaces}|                                                                                            |
{spaces}|                                       ┬  ┌─┐┌─┐┬┌┐┌                                        |
{spaces}|                                       │  │ ││ ┬││││                                        |
{spaces}|                                       ┴─┘└─┘└─┘┴┘└┘                                        |
{spaces}|                                                                                            |";
    }
    public string GetPhoneDetailANSIText()
    {
        int centeredPosition = (Console.WindowWidth - "|--------------------------------------------------------------------------------------------|".Length) / 2;
        string spaces = centeredPosition > 0 ? new string(' ', centeredPosition) : "";
        return $@"{spaces}|                                                                                            |
{spaces}|                              ┌─┐┬ ┬┌─┐┌┐┌┌─┐  ┌┬┐┌─┐┌┬┐┌─┐┬┬                               |  
{spaces}|                              ├─┘├─┤│ ││││├┤    ││├┤  │ ├─┤││                               |  
{spaces}|                              ┴  ┴ ┴└─┘┘└┘└─┘  ─┴┘└─┘ ┴ ┴ ┴┴┴─┘                             |
{spaces}|                                                                                            |";


    }
    public string SetTextBolder(string text)
    {
        return $"{text}";
    }
    public string FormatPrice(decimal price)
    {
        CultureInfo cultureInfo = new CultureInfo("vi-VN");
        return string.Format(cultureInfo, "{0:N0} VND", price);
    }
    public string ConvertToWords(long number)
    {
        string[] ones = { "", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
        string[] teens = { "mười", "mười một", "mười hai", "mười ba", "mười bốn", "mười lăm", "mười sáu", "mười bảy", "mười tám", "mười chín" };
        string[] tens = { "", "", "hai mươi", "ba mươi", "bốn mươi", "năm mươi", "sáu mươi", "bảy mươi", "tám mươi", "chín mươi" };
        if (number < 10)
        {
            return ones[number];
        }
        else if (number < 20)
        {
            return teens[number - 10];
        }
        else if (number < 100)
        {
            return tens[number / 10] + (number % 10 > 0 ? " " + ones[number % 10] : "");
        }
        else if (number < 1000)
        {
            return ones[number / 100]
                + " trăm"
                + (number % 100 > 0 ? " " + ConvertToWords(number % 100) : "");
        }
        else if (number < 1000000)
        {
            return ConvertToWords(number / 1000)
                + " nghìn"
                + (number % 1000 > 0 ? " " + ConvertToWords(number % 1000) : "");
        }
        else if (number < 1000000000)
        {
            return ConvertToWords(number / 1000000)
                + " triệu"
                + (number % 1000000 > 0 ? " " + ConvertToWords(number % 1000000) : "");
        }
        else
        {
            throw new ArgumentException("Out of range: " + number);
        }
    }
    public string ConvertNumberToWords(decimal number)
    {
        if (number == 0)
            return "không đồng";

        long nguyen = (long)Math.Truncate(number);
        int thapPhan = (int)((number - nguyen) * 100);

        string result = ConvertToWords(nguyen) + " đồng";

        if (thapPhan > 0)
        {
            result += " và " + ConvertToWords(thapPhan) + " Hào";
        }

        return result;
    }
    public string AlignCenter(string content)
    {
        // phương thức căn giữa ra màn hình
        int centeredPosition = (Console.WindowWidth - content.Length) / 2;
        string spaces = centeredPosition > 0 ? new string(' ', centeredPosition) : "";
        return spaces;
    }
}