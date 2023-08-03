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
            Ultilities Ults = new Ultilities();
            ConsoleUlts ConsoleUlts = new ConsoleUlts();
            StaffEnum.Role? loginAccount = null;
            int mainChoice = 0, SellerAccount = 0, AccountantAccount = 0;
            bool active = true;
            do
            {
                Console.Clear();
                Console.OutputEncoding = System.Text.Encoding.UTF8;
                Ults.Title(ConsoleUlts.GetAppTitle(), @"                                        ┬  ┌─┐┌─┐┬┌┐┌
                                        │  │ ││ ┬││││
                                        ┴─┘└─┘└─┘┴┘└┘");
                loginAccount = Ults.LoginUlt();
                if (loginAccount == StaffEnum.Role.Seller)
                {
                    ConsoleUlts.Alert(GUIEnum.ConsoleEnum.Alert.Success, "Login Successfully!");
                    SellerAccount = Ults.SellerMenu();
                }
                else if (loginAccount == StaffEnum.Role.Accountant)
                {
                    ConsoleUlts.Alert(GUIEnum.ConsoleEnum.Alert.Success, "Login Successfully!");
                    AccountantAccount = Ults.AccountantMenu();
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
