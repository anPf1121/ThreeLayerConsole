using System;
using System.Collections.Generic;
using Model;
using Ults;
using BusinessEnum;
using GUIEnum;
using BL;
using DAL;

namespace PhoneStoreUI
{
    class Program
    {
        static void Main()
        {
            // DbConfig.CreateDefaultDb();
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            StaffBL staffBL = new StaffBL();
            Ultilities Ults = new Ultilities(staffBL);
            ConsoleUlts ConsoleUlts = new ConsoleUlts();
            StaffEnum.Role? loginAccount = null;
            bool active = true;
            int mainChoice = 0,
                SellerAccount = 0,
                AccountantAccount = 0;
            do
            {
                Console.Clear();
                Console.OutputEncoding = System.Text.Encoding.UTF8;
                ConsoleUlts.Title(
                    ConsoleUlts.GetAppTitle(),
                    @"                                        ┬  ┌─┐┌─┐┬┌┐┌
                                        │  │ ││ ┬││││
                                        ┴─┘└─┘└─┘┴┘└┘",
                staffBL.LoggedInStaff);
                bool isSuccess = staffBL.Login(ConsoleUlts.GetUserName(), ConsoleUlts.GetPassword());
                if (isSuccess)
                {
                    ConsoleUlts.Alert(GUIEnum.ConsoleEnum.Alert.Success, "Login Successfully!");
                    if (staffBL.LoggedInStaff.Role == StaffEnum.Role.Seller)
                    {
                        int result = 0;
                        active = true;
                        string[] menuItem = { "Create Order", "Handle Order", "Log Out" };
                        int HandleResult = 0;
                        while (active)
                        {
                            switch (Ults.MenuHandle(ConsoleUlts.GetAppTitle(), null, menuItem))
                            {
                                case 1:
                                    Ults.CreateOrder();
                                    break;
                                case 2:
                                    // HandleResult = HandleOrder();
                                    if (HandleResult == -1)
                                        ConsoleUlts.Alert(
                                            GUIEnum.ConsoleEnum.Alert.Error,
                                            "No Order exist"
                                        );
                                    else if (HandleResult == 0)
                                        ConsoleUlts.Alert(
                                            GUIEnum.ConsoleEnum.Alert.Warning,
                                            "Cancel Order Completed"
                                        );
                                    else if (HandleResult == 1)
                                        ConsoleUlts.Alert(
                                            GUIEnum.ConsoleEnum.Alert.Success,
                                            "Handle Order Completed"
                                        );
                                    break;
                                case 3:
                                    active = false;
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
                        active = true;
                        string[] menuItem = { "Payment", "Revenue Report", "Log Out" };
                        while (active)
                        {
                            switch (Ults.MenuHandle(ConsoleUlts.GetAppTitle(), null, menuItem))
                            {
                                case 1:
                                    // Payment();
                                    break;
                                case 2:
                                    break;
                                case 3:
                                    active = false;
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
                    Console.WriteLine();
                    ConsoleUlts.Alert(ConsoleEnum.Alert.Error, "Invalid Username Or Password");
                    Main();
                }
            } while (active);
        }
    }
}
