using System;
using System.Collections.Generic;
using Model;
using Ults;
using BusinessEnum;
using GUIEnum;
using BL;
using DAL;
using UI;




namespace PhoneStoreUI
{
    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            StaffBL staffBL = new StaffBL();
            Ultilities Ults = new Ultilities(staffBL);
            ConsoleUlts consoleUlts = new ConsoleUlts();
            ConsoleUI consoleUI = new ConsoleUI();
            StaffEnum.Role? loginAccount = null;
            bool active = true;
            int mainChoice = 0, SellerAccount = 0, AccountantAccount = 0;
            do
            {
                Console.Clear();
                Console.OutputEncoding = System.Text.Encoding.UTF8;
                consoleUI.PrintTitle(consoleUI.GetAppANSIText(), consoleUI.GetLoginANSIText(), staffBL.LoggedInStaff);
                bool isSuccess = staffBL.Login(consoleUlts.GetUserName(), consoleUlts.GetPassword());
                if (isSuccess)
                {
                    consoleUlts.Alert(GUIEnum.ConsoleEnum.Alert.Success, "Login Successfully!");
                    if (staffBL.LoggedInStaff.Role == StaffEnum.Role.Seller)
                    {
                        int result = 0;
                        bool activeSellerMenu = true;
                        string[] menuItem = { "Create Order", "Handle Order", "Log Out" };
                        int HandleResult = 0;
                        while (activeSellerMenu)
                        {
                            switch (consoleUlts.MenuHandle(consoleUI.GetAppANSIText(), null, menuItem, staffBL.LoggedInStaff))
                            {
                                case 1:
                                    Ults.CreateOrder();
                                    break;
                                case 2:
                                    HandleResult = Ults.HandleOrder();
                                    if (HandleResult == -1)
                                        consoleUlts.Alert(GUIEnum.ConsoleEnum.Alert.Error, "No Order exist");
                                    else if (HandleResult == 0)
                                        consoleUlts.Alert(GUIEnum.ConsoleEnum.Alert.Warning, "Cancel Order Completed");
                                    else if (HandleResult == 1)
                                        consoleUlts.Alert(GUIEnum.ConsoleEnum.Alert.Success, "Handle Order Completed");
                                    break;
                                case 3:
                                    activeSellerMenu = false;
                                    result = 1;
                                    staffBL.Logout();
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    else if (staffBL.LoggedInStaff.Role == StaffEnum.Role.Accountant)
                    {
                        int result = 0;
                        bool activeAccountantMenu = true;
                        string[] menuItem = { "Payment", "Revenue Report","TradeIn", "Log Out" };
                        while (activeAccountantMenu)
                        {
                            switch (consoleUlts.MenuHandle(consoleUI.GetAppANSIText(), null, menuItem, staffBL.LoggedInStaff))
                            {
                                case 1:
                                    Ults.Payment();
                                    break;
                                case 2:
                                    Ults.CreateReport();
                                    break;
                                case 3:
                                    Ults.TradeIn();
                                    break;
                                case 4:
                                    activeAccountantMenu = false;
                                    result = 1;
                                    staffBL.Logout();
                                    break;
                                
                                default:
                                    break;
                            }
                        }
                    }
                }
                else
                {
                    consoleUlts.Alert(ConsoleEnum.Alert.Error, "Invalid Username Or Password");
                    Main();
                }
            } while (active);
        }
    }
}

