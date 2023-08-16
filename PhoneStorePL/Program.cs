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
            bool active = true;
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
                                    staffBL.Logout();
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    else if (staffBL.LoggedInStaff.Role == StaffEnum.Role.Accountant)
                    {
                        bool activeAccountantMenu = true;
                        string[] menuItem = { "Payment", "TradeIn", "Log Out" };
                        while (activeAccountantMenu)
                        {
                            switch (consoleUlts.MenuHandle(consoleUI.GetAppANSIText(), null, menuItem, staffBL.LoggedInStaff))
                            {
                                case 1:
                                    Ults.Payment();
                                    break;
                                case 2:
                                    Ults.TradeIn();
                                    break;
                                case 3:
                                    activeAccountantMenu = false;
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

