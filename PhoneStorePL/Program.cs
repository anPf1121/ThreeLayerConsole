// using System;
// using System.Collections.Generic;
// using Model;
 using Ults;
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
//                 do
//                 {
//                     mainChoice = Ults.StartMenu();
//                     if (mainChoice == 1)
//                     {
//                         loginAccount = Ults.LoginUlt();

//                         if (loginAccount == StaffEnum.Role.Seller)
//                             SellerAccount = Ults.SellerMenu();

//                         else if (loginAccount == StaffEnum.Role.Accountant)
//                             AccountantAccount = Ults.AccountantMenu();

//                         else
//                         {
//                             Console.WriteLine();
//                             ConsoleUlts.Alert(ConsoleEnum.Alert.Error, "Invalid Username Or Password");
//                             Main();
//                         }
//                     }
//                     else if (mainChoice == 2)
//                     {
//                         ConsoleUlts.Alert(ConsoleEnum.Alert.Success, "Exiting Success");
//                         Environment.Exit(0);
//                     }
//                     else
//                     {
//                         ConsoleUlts.Alert(ConsoleEnum.Alert.Error, "Invalid Choice");
//                         Main();
//                     }
//                 } while (mainChoice != 2);
//             } while (active);
//         }
//     }
// }
using Model;
using DAL;
using System.Collections.Generic;
using BL;


Ultilities a = new Ultilities();
a.LoginUlt();
a.SellerMenu();

