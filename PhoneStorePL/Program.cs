// using System;
// using System.Collections.Generic;
// using Model;
// using Ults;
// using BusinessEnum;
// using GUIEnum;
// using BL;
// using DAL;

// namespace PhoneStoreUI
// {
//     class Program
//     {
//         static void Main()
//         {
//             Console.OutputEncoding = System.Text.Encoding.UTF8;
//             Ultilities Ults = new Ultilities();
//             ConsoleUlts ConsoleUlts = new ConsoleUlts();
//             StaffEnum.Role? loginAccount = null;
//             int mainChoice = 0, SellerAccount = 0, AccountantAccount = 0;
//             bool active = true;
//             do
//             {
//                 loginAccount = Ults.LoginUlt();
//                 if (loginAccount == StaffEnum.Role.Seller)
//                 {
//                     Ults.ShowLoginSuccessAlert();
//                     SellerAccount = Ults.SellerMenu();
//                 }
//                 else if (loginAccount == StaffEnum.Role.Accountant)
//                 {
//                     Ults.ShowLoginSuccessAlert();
//                     AccountantAccount = Ults.AccountantMenu();
//                 }
//                 else
//                 {
//                     Console.WriteLine();
//                     ConsoleUlts.Alert(ConsoleEnum.Alert.Error, "Invalid Username Or Password");
//                     Main();
//                 }
//             } while (active);
//         }
//     }
// }

