using System;
using GUIEnum;
using BusinessEnum;
using Model;
using System.Globalization; // thu vien format tien

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
        string spaces = new string(' ', centeredPosition);
        Console.WriteLine(spaces + @"|--------------------------------------------------------------------------------------------|");
    }
    public void PrintPhoneDetailsInfo(List<PhoneDetail> phoneDetails)
    {
        int centeredPosition = (Console.WindowWidth - "|===================================================================================================|".Length) / 2;
        string spaces = new string(' ', centeredPosition);
        foreach (PhoneDetail pd in phoneDetails)
        {
            PrintPhoneDetailsInfo(pd);
            break;
        }
        PrintPhoneModelTitle();
        foreach (PhoneDetail pd in phoneDetails)
        {
            PrintPhoneModelInfo(pd);
        }
        Console.WriteLine(spaces + "|===================================================================================================|");
    }
    public void FullWidthTinyLine()
    {
        Console.WriteLine(new string('-', Console.WindowWidth));
    }
    public void PrintPhoneBorderLine()
    {
        int centeredPosition = (Console.WindowWidth - "|============================================================================================|".Length) / 2;
        string spaces = new string(' ', centeredPosition);
        Console.WriteLine(spaces + "|============================================================================================|");
        Console.WriteLine(spaces + "| {0, -10} | {1, -25} | {2, -13} | {3, -13} | {4, -17} |", "ID", "Phone Name", "Brand", "OS", "Mobile Network");
        Console.WriteLine(spaces + "|============================================================================================|");
    }
    public void PrintOrderBorderLine()
    {
        int centeredPosition = (Console.WindowWidth - "|============================================================================================|".Length) / 2;
        string spaces = new string(' ', centeredPosition);
        Console.WriteLine(spaces + "|============================================================================================|");
        Console.WriteLine(spaces + "| {0, -13} | {1, -25} | {2, -23} | {3, -20} |", "ID", "Customer Name", "Order Date", "Status");
        Console.WriteLine(spaces + "|============================================================================================|");
    }
    public void PrintPhoneInfo(Phone phone)
    {
        int centeredPosition = (Console.WindowWidth - "|============================================================================================|".Length) / 2;
        string spaces = new string(' ', centeredPosition);
        Console.WriteLine(spaces + "| {0, -10} | {1, -25} | {2, -13} | {3, -13} | {4, -17} |", phone.PhoneID, phone.PhoneName, phone.Brand.BrandName, phone.OS, phone.Connection);
    }
    public void PrintOrderInfo(Order order)
    {
        int centeredPosition = (Console.WindowWidth - "|--------------------------------------------------------------------------------------------|".Length) / 2;
        string spaces = new string(' ', centeredPosition);
        Console.WriteLine(spaces + "| {0, -13} | {1, -25} | {2, -23} | {3, -20} |", order.OrderID, order.Customer.CustomerName, order.CreateAt, order.OrderStatus);
    }
    public void PrintPhoneModelTitle()
    {
        int centeredPosition = (Console.WindowWidth - "|===================================================================================================|".Length) / 2;
        string spaces = new string(' ', centeredPosition);
        Console.WriteLine(spaces + "|===================================================================================================|");
        Console.WriteLine(spaces + "| PHONE MODEL                                                                                       |");
        Console.WriteLine(spaces + "====================================================================================================|");
        Console.WriteLine(spaces + "| {0, -10} | {1, -13} | {2, -15} | {3, -15} | {4, -15} | {5, -14} |", "Detail ID", "Phone Color", "ROM Size", "Price", "Phone Status", "Quantity");
        Console.WriteLine(spaces + "|===================================================================================================|");
    }
    public void PrintPhoneDetailsInfo(PhoneDetail phoneDetail)
    {
        int centeredPosition = (Console.WindowWidth - "|===================================================================================================|".Length) / 2;
        string spaces = new string(' ', centeredPosition);
        Console.WriteLine(spaces + "|===================================================================================================|");
        Console.WriteLine(spaces + "| PHONE DETAILS INFOMATION                                                                          |");
        Console.WriteLine(spaces + "|===================================================================================================|");
        Console.WriteLine(spaces + "| Phone Name: {0, -50} |", phoneDetail.Phone.PhoneName.PadRight(85));
        Console.WriteLine(spaces + "| Brand: {0, -50} |", phoneDetail.Phone.Brand.BrandName.PadRight(90));
        Console.WriteLine(spaces + "| Camera: {0, -50} |", phoneDetail.Phone.Camera.PadRight(89));
        Console.WriteLine(spaces + "| RAM: {0, -50} |", phoneDetail.Phone.RAM.PadRight(92));
        Console.WriteLine(spaces + "| Weight: {0, -50} |", phoneDetail.Phone.Weight.PadRight(89));
        Console.WriteLine(spaces + "| Processor: {0, -50} |", phoneDetail.Phone.Processor.PadRight(86));
        Console.WriteLine(spaces + "| Battery: {0, -50} |", phoneDetail.Phone.BatteryCapacity.PadRight(88));
        Console.WriteLine(spaces + "| OS: {0, -50} |", phoneDetail.Phone.OS.PadRight(93));
        Console.WriteLine(spaces + "| Sim Slot: {0, -50} |", phoneDetail.Phone.SimSlot.PadRight(87));
        Console.WriteLine(spaces + "| Screen : {0, -50} |", phoneDetail.Phone.Screen.PadRight(88));
        Console.WriteLine(spaces + "| Connection: {0, -50} |", phoneDetail.Phone.Connection.PadRight(85));
        Console.WriteLine(spaces + "| Charge Port: {0, -50} |", phoneDetail.Phone.ChargePort.PadRight(84));
        Console.WriteLine(spaces + "| Release Date: {0, -50} |", phoneDetail.Phone.ReleaseDate.ToString().PadRight(83));
        Console.WriteLine(spaces + "| Description: {0, -50} |", phoneDetail.Phone.Description.PadRight(84));
    }
    public void PrintTimeLine(string[] phase, int itemCount, int currentPhase)
    {
        Console.Clear();
        FullWidthTinyLine();
        foreach (string item in phase)
        {
            itemCount++;
            if (itemCount == currentPhase)
            {
                ConsoleForegroundColor(ConsoleEnum.Color.Green);
                Console.Write(((itemCount == currentPhase) ? " 👉 " + SetTextBolder(item) : " > " + item));
                ConsoleForegroundColor(ConsoleEnum.Color.White);
            }
            else
            {
                Console.Write(((itemCount == currentPhase) ? " 👉 " + SetTextBolder(item) : " > " + item));
            }
            if (itemCount == phase.Length)
                Console.Write("\n");
        }
        FullWidthTinyLine();
        itemCount = 0;
    }
    public void PrintSellerOrderAfterPayment(Order ord)
    {
        int centeredPosition = (Console.WindowWidth - "|===========================================================================================================================|".Length) / 2;
        string spaces = new string(' ', centeredPosition);
        Console.WriteLine("\n" + spaces + "|===========================================================================================================================|");
        Console.WriteLine(spaces + "|------------------------------------------------------- \x1b[1mVTC Mobile\x1b[0m --------------------------------------------------------|");
        Console.WriteLine(spaces + "|===========================================================================================================================|");
        Console.WriteLine(spaces + "|                                                   Order ID: " + ord.OrderID + "                                                  |");
        Console.WriteLine(spaces + "|===========================================================================================================================|");
        Console.WriteLine(spaces + "| Website: https://vtc.edu.vn/                                                                                              |");
        Console.WriteLine(spaces + "| Address: 18 Tam Trinh, Quận Hai Bà Trưng, Thành Phố Hà Nội                                                                |");
        Console.WriteLine(spaces + "| Phone Number: 0999999999                                                                                                  |");
        Console.WriteLine(spaces + "|===========================================================================================================================|");
        Console.WriteLine(spaces + "| Order Create Time: {0, -30}|", DateTime.Now.ToString().PadRight(103));
        Console.WriteLine(spaces + "| Customer: {0, -30}|", ord.Customer.CustomerName.PadRight(112));
        Console.WriteLine(spaces + "| Address: {0, -50}|", ord.Customer.Address.PadRight(113));
        Console.WriteLine(spaces + "| Phone Number: {0, -12}|", ord.Customer.PhoneNumber.PadRight(108));
        Console.Write((ord.Accountant.StaffID != 0) ? (spaces + "| Payment Method: {0, 35} |" + "\n") : "", ord.PaymentMethod.PadRight(105));
        Console.WriteLine(spaces + "|===========================================================================================================================|");
        PrintOrderDetails(ord);
        Console.WriteLine(spaces + "|===========================================================================================================================|");
            if (ord.DiscountPolicies.Count() != 0)
            {
                Console.WriteLine(spaces + "| {0, 46}                                                                            |", SetTextBolder("All DiscountPolicy Be Apply For This Order Is "));
                foreach (var dp in ord.DiscountPolicies)
                {
                    Console.WriteLine(spaces + "| - {0, -100}         |", (dp.Title+": "+SetTextBolder(FormatPrice(dp.DiscountPrice).ToString().PadRight(57))).PadRight(119));
                    if(ord.OrderStatus == OrderEnum.Status.Pending)ord.TotalDue -= dp.DiscountPrice;
                }
            }
        Console.WriteLine(spaces + "|---------------------------------------------------------------------------------------------------------------------------|");
        Console.WriteLine(spaces + "| {0, 40}{1, -26}{2, 15}|", "", "Total Due: ", SetTextBolder(FormatPrice(ord.GetTotalDue()).PadRight(56)));
        if (ord.Accountant.StaffID != 0)
        {
            Console.WriteLine(spaces + "| {0, 40}{1, -25}{2, 22}|", "", "Discount Price: ", SetTextBolder(FormatPrice(ord.TotalDue - ord.GetTotalDue()).ToString().PadRight(57)));
            Console.WriteLine(spaces + "| {0, 40}{1, -15}{2, 21}|", "", "Total Due After Discount: ", SetTextBolder(FormatPrice(ord.TotalDue).ToString().PadRight(56)));
        }
        Console.WriteLine(spaces + "| {0, 40}{1, -26}{2, 49}|", "", "To String: ", (ord.Accountant.StaffID == 0) ? SetTextBolder(ConvertNumberToWords(ord.GetTotalDue()).PadRight(56)) : SetTextBolder(ConvertNumberToWords(ord.TotalDue).PadRight(56)));
        Console.WriteLine(spaces + "|===========================================================================================================================|");
        Console.WriteLine(spaces + "|{0, 10}{1, -35} {2, -40} {3, -36}|", " ", "Customer", "Seller", "Accountant", " ");
        Console.WriteLine(spaces + "|{0, 10}{1, -35} {2, -40} {3, -36}|", " ", ord.Customer.CustomerName, ord.Seller.StaffName + " - ID: " + ord.Seller.StaffID, (ord.Accountant.StaffID == 0) ? "" : (ord.Accountant.StaffName + " - ID: " + ord.Accountant.StaffID), " ");
        Console.WriteLine(spaces + "|===========================================================================================================================|");
    }
    public void PrintSellerOrderBeforePayment(Order ord)
    {
        int centeredPosition = (Console.WindowWidth - "|===========================================================================================================================|".Length) / 2;
        string spaces = new string(' ', centeredPosition);
        Console.WriteLine("\n" + spaces + "|===========================================================================================================================|");
        Console.WriteLine(spaces + "|------------------------------------------------------- \x1b[1mVTC Mobile\x1b[0m --------------------------------------------------------|");
        Console.WriteLine(spaces + "|===========================================================================================================================|");
        Console.WriteLine(spaces + "|                                                   Order ID: " + ord.OrderID + "                                                  |");
        Console.WriteLine(spaces + "|===========================================================================================================================|");
        Console.WriteLine(spaces + "| Website: https://vtc.edu.vn/                                                                                              |");
        Console.WriteLine(spaces + "| Address: 18 Tam Trinh, Quận Hai Bà Trưng, Thành Phố Hà Nội                                                                |");
        Console.WriteLine(spaces + "| Phone Number: 0999999999                                                                                                  |");
        Console.WriteLine(spaces + "|===========================================================================================================================|");
        Console.WriteLine(spaces + "| Order Create Time: {0, -30}|", DateTime.Now.ToString().PadRight(103));
        Console.WriteLine(spaces + "| Customer: {0, -30}|", ord.Customer.CustomerName.PadRight(112));
        Console.WriteLine(spaces + "| Address: {0, -50}|", ord.Customer.Address.PadRight(113));
        Console.WriteLine(spaces + "| Phone Number: {0, -12}|", ord.Customer.PhoneNumber.PadRight(108));
        PrintOrderDetailsInfo(ord);
        
        Console.WriteLine(spaces + "| {0, 40}{1, -26}{2, 15}|", "", "Total Due: ", SetTextBolder(FormatPrice(ord.GetTotalDue()).PadRight(56)));
        Console.WriteLine(spaces + "| {0, 40}{1, -26}{2, 49}|", "", "To String: ", SetTextBolder(ConvertNumberToWords(ord.GetTotalDue()).PadRight(56)));
        Console.WriteLine(spaces + "|---------------------------------------------------------------------------------------------------------------------------|");
    }
    public void PrintOrderDetails(Order ord)
    {
        int centeredPosition = (Console.WindowWidth - "|===========================================================================================================================|".Length) / 2;
        string spaces = new string(' ', centeredPosition);
        CultureInfo cultureInfo = new CultureInfo("vi-VN");
        int printImeiHandle = 0;
        int printImeiHandle2 = 0;
        Console.WriteLine(spaces + "| {0, -16} | {1, -30} | {2, -15} | {3, -15} | {4, -15} | {5, -15} |", "Phone Detail ID", "Phone Name", "Quantity", "Imei", "Price", "Total Price");
        Console.WriteLine(spaces + "|===========================================================================================================================|");
        foreach (var pd in ord.PhoneDetails)
        {
            printImeiHandle = pd.PhoneDetailID;
            Console.WriteLine(spaces + "|---------------------------------------------------------------------------------------------------------------------------|");
            foreach (var ims in pd.ListImei)
            {
                Console.WriteLine(spaces + "| {0, -16} | {1, -30} | {2, -15} | {3, -15} | {4, -15} | {5, 15} |", (printImeiHandle != printImeiHandle2) ? pd.PhoneDetailID : "", (printImeiHandle != printImeiHandle2) ? pd.Phone.PhoneName : "", (printImeiHandle != printImeiHandle2) ? pd.Quantity : "", ims.PhoneImei, (printImeiHandle != printImeiHandle2) ? FormatPrice(pd.Price) : "", (printImeiHandle != printImeiHandle2) ? FormatPrice(ord.GetTotalDueForEachPhone()) : "");
                printImeiHandle2 = printImeiHandle;
            }
        }
    }
    public void PrintDiscountPolicyDetail(DiscountPolicy discountPolicy)
    {
        Console.WriteLine($"Title: {discountPolicy.Title}");
        Console.WriteLine($"FromDate: {discountPolicy.FromDate}");
        Console.WriteLine($"ToDate: {discountPolicy.ToDate}");
        if (discountPolicy.PhoneDetail.PhoneDetailID != 0)
            Console.WriteLine($"Phone Information: {discountPolicy.PhoneDetail.Phone.PhoneName} {discountPolicy.PhoneDetail.PhoneColor.Color} {discountPolicy.PhoneDetail.ROMSize.ROM}");
        if (discountPolicy.PaymentMethod != "Not Have")
            Console.WriteLine($"Apply for Paymentmethod: {discountPolicy.PaymentMethod}");
        if (discountPolicy.MinimumPurchaseAmount > 0 && discountPolicy.MaximumPurchaseAmount > 0)
        {
            Console.WriteLine($"Maximum purchase amount: {discountPolicy.MinimumPurchaseAmount}");
            Console.WriteLine($"Minimum purchase amount: {discountPolicy.MaximumPurchaseAmount}");
        }
        if (discountPolicy.DiscountPrice != 0)
            Console.WriteLine($"DiscountPrice: {discountPolicy.DiscountPrice}");
        if (discountPolicy.MoneySupported != 0)
            Console.WriteLine($"Money supported: {discountPolicy.MoneySupported}");
    }
    public void PrintPhoneModelInfo(PhoneDetail phoneDetail)
    {
        int centeredPosition = (Console.WindowWidth - "|===================================================================================================|".Length) / 2;
        string spaces = new string(' ', centeredPosition);
        CultureInfo cultureInfo = new CultureInfo("vi-VN");
        Console.WriteLine(spaces + "| {0, -10} | {1, -13} | {2, -15} | {3, -15} | {4, -15} | {5, -14} |", phoneDetail.PhoneDetailID, phoneDetail.PhoneColor.Color, phoneDetail.ROMSize.ROM, string.Format(cultureInfo, "{0:N0} ₫", phoneDetail.Price), phoneDetail.PhoneStatusType, (phoneDetail.Quantity != 0) ? phoneDetail.Quantity : "Out Of Stock");
    }
    public void PrintOrderDetailsInfo(Order order)
    {
        int centeredPosition = (Console.WindowWidth - "|===========================================================================================================================|".Length) / 2;
        string spaces = new string(' ', centeredPosition);
        
        if (order.PhoneDetails.Count() == 0)
        {
            Console.WriteLine(spaces + "|===========================================================================================================================|");
            Console.WriteLine(spaces + "| Doesnt have any phone in cart                                                                                        |");
            Console.WriteLine(spaces + "|===========================================================================================================================|");
        }
        else
        {
            Console.WriteLine(spaces + "|===========================================================================================================================|");
            Console.WriteLine(spaces + "| {0, -34} | {1, -11} | {2, -15} | {3, -15} | {4, -15} | {5, -16} |", "Phone Name", "Color", "RomSize", "Quantity", "Price", "Total Price");
            Console.WriteLine(spaces + "|===========================================================================================================================|");

            foreach (var phone in order.PhoneDetails)
            {
                Console.WriteLine(spaces + "| {0, -34} | {1, -11} | {2, -15} | {3, -15} | {4, -15} | {5, -16} |", phone.Phone.PhoneName, phone.PhoneColor.Color, phone.ROMSize.ROM, phone.Quantity, FormatPrice(phone.Price), FormatPrice(order.GetTotalDueForEachPhone()));
            }
            Console.WriteLine(spaces + "|===========================================================================================================================|");
        }
    }
    public void PrintTitle(string? title, string? subTitle, Staff? staffLoggedIn)
    {
        int centeredPosition = (Console.WindowWidth - "|--------------------------------------------------------------------------------------------|".Length) / 2;
        string spaces = new string(' ', centeredPosition);
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
    public string GetAppANSIText()
    {
        int centeredPosition = (Console.WindowWidth - "|--------------------------------------------------------------------------------------------|".Length) / 2;
        string spaces = new string(' ', centeredPosition);
        return $@"{spaces}|                                                                                            |
{spaces}|                             ┌─┐┬ ┬┌─┐┌┐┌┌─┐  ┌─┐┌┬┐┌─┐┬─┐┌─┐                               |
{spaces}|                             ├─┘├─┤│ ││││├┤   └─┐ │ │ │├┬┘├┤                                |
{spaces}|                             ┴  ┴ ┴└─┘┘└┘└─┘  └─┘ ┴ └─┘┴└─└─┘                               |
{spaces}|                                                                                            |";
    }
    public void GetFooterPagination(int currentPage, int countPage)
    {
        int centeredPosition = (Console.WindowWidth - "|============================================================================================|".Length) / 2;
        string spaces = new string(' ', centeredPosition);
        Console.WriteLine(spaces + "|============================================================================================|");
        Console.WriteLine(spaces + "| {0,42}" + "< " + $"{currentPage}/{countPage}" + " >".PadRight(44) + "|", " ");
        Console.WriteLine(spaces + "|============================================================================================|");
        Console.WriteLine(spaces + "| Press 'Left Arrow' To Back Previous Page, 'Right Arror' To Next Page                       |");
        Console.WriteLine(spaces + "| Press 'Space' To Choose a phone, 'B' To Back Previous Menu                                 |");
        Console.WriteLine(spaces + "|============================================================================================|");

    }
    public string GetReportANSIText() {
            int secondCenteredPosition = (Console.WindowWidth - "|==================================================================================================================================================|".Length) / 2;
            string secondSpaces = new string(' ', secondCenteredPosition);
        return $@"{secondSpaces}|                                                                                                                                                  |                                  
{secondSpaces}|                                                           ┬─┐┌─┐┌─┐┌─┐┬─┐┌┬┐                                                                     |
{secondSpaces}|                                                           ├┬┘├┤ ├─┘│ │├┬┘ │                                                                      |
{secondSpaces}|                                                           ┴└─└─┘┴  └─┘┴└─ ┴                                                                      |
{secondSpaces}|                                                                                                                                                  |";
    }
    public string GetSearchANSIText()
    {
        int centeredPosition = (Console.WindowWidth - "|--------------------------------------------------------------------------------------------|".Length) / 2;
        string spaces = new string(' ', centeredPosition);
        return $@"{spaces}|                                                                                            |
{spaces}|                                     ┌─┐┌─┐┌─┐┬─┐┌─┐┬ ┬                                     |
{spaces}|                                     └─┐├┤ ├─┤├┬┘│  ├─┤                                     |
{spaces}|                                     └─┘└─┘┴ ┴┴└─└─┘┴ ┴                                     |
{spaces}|                                                                                            |";
    }
    public string GetHandleOrderANSIText()
    {
        int centeredPosition = (Console.WindowWidth - "|--------------------------------------------------------------------------------------------|".Length) / 2;
        string spaces = new string(' ', centeredPosition);
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
    public string GetCreateReportANSIText()
    {
        int centeredPosition = (Console.WindowWidth - "|--------------------------------------------------------------------------------------------|".Length) / 2;
        string spaces = new string(' ', centeredPosition);
        return $@"{spaces}|                                                                                            |
{spaces}|                           ┌─┐┬─┐┌─┐┌─┐┌┬┐┌─┐  ┬─┐┌─┐┌─┐┌─┐┬─┐┌┬┐                           |
{spaces}|                           │  ├┬┘├┤ ├─┤ │ ├┤   ├┬┘├┤ ├─┘│ │├┬┘ │                            |
{spaces}|                           └─┘┴└─└─┘┴ ┴ ┴ └─┘  ┴└─└─┘┴  └─┘┴└─ ┴                            |
{spaces}|                                                                                            | ";
    }
    public string[] GetCreateOrderTimeLine()
    {
        return new string[] { "Search Phone", "Add Phone To Order", "Add More Phone?", "Enter Customer Info", "Confirm Order" };
    }
    public string[] GetPaymentTimeLine()
    {
        return new string[] { "Choose an Order", "Choose Paymentmethod", "Choose DiscountPolicy for Paymentmethod", "Choose DiscountPolicy for Order", "Confirm or Cancel Payment" };
    }
    public string GetCustomerInfoANSIText()
    {
        int centeredPosition = (Console.WindowWidth - "|--------------------------------------------------------------------------------------------|".Length) / 2;
        string spaces = new string(' ', centeredPosition);
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
        string spaces = new string(' ', centeredPosition);
        return
        $@"{spaces}|                                                                                            |
{spaces}|                    ┌─┐┌┬┐┌┬┐  ┌─┐┬ ┬┌─┐┌┐┌┌─┐  ┌┬┐┌─┐  ┌─┐┬─┐┌┬┐┌─┐┬─┐                     |
{spaces}|                    ├─┤ ││ ││  ├─┘├─┤│ ││││├┤    │ │ │  │ │├┬┘ ││├┤ ├┬┘                     |
{spaces}|                    ┴ ┴─┴┘─┴┘  ┴  ┴ ┴└─┘┘└┘└─┘   ┴ └─┘  └─┘┴└──┴┘└─┘┴└─                     |
{spaces}|                                                                                            |";
    }
    public string GetAllOrderANSIText()
    {
        int centeredPosition = (Console.WindowWidth - "|--------------------------------------------------------------------------------------------|".Length) / 2;
        string spaces = new string(' ', centeredPosition);
        return $@"{spaces}|                                                                                            |
{spaces}|                                ┌─┐┬  ┬    ┌─┐┬─┐┌┬┐┌─┐┬─┐┌─┐                               |
{spaces}|                                ├─┤│  │    │ │├┬┘ ││├┤ ├┬┘└─┐                               |
{spaces}|                                ┴ ┴┴─┘┴─┘  └─┘┴└──┴┘└─┘┴└─└─┘                               |
{spaces}|                                                                                            |";
    }
    public string GetLoginANSIText()
    {
        int centeredPosition = (Console.WindowWidth - "|--------------------------------------------------------------------------------------------|".Length) / 2;
        string spaces = new string(' ', centeredPosition);
        return $@"{spaces}|                                                                                            |
{spaces}|                                       ┬  ┌─┐┌─┐┬┌┐┌                                        |
{spaces}|                                       │  │ ││ ┬││││                                        |
{spaces}|                                       ┴─┘└─┘└─┘┴┘└┘                                        |
{spaces}|                                                                                            |";
    }
    public string SetTextBolder(string text)
    {
        return $"\x1b[1m{text}\x1b[0m";
    }
    public string FormatPrice(decimal price)
    {
        CultureInfo cultureInfo = new CultureInfo("vi-VN");
        return string.Format(cultureInfo, "{0:N0} ₫", price);
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
    public string[] GetCreateReportTimeLine()
    {
        return new string[] { "Enter DateTime", "Confirm Report" };
    }
}