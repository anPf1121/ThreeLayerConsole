// using Model;
// using BL;
// using BusinessEnum;
// using GUIEnum;

// namespace Ults
// {
//     class Ultilities
//     {
//         // private PhoneBL phoneBL = new PhoneBL();
//         private ConsoleUlts ConsoleUlts = new ConsoleUlts();
//         private StaffBL StaffBL = new StaffBL();
//         // private CustomerBL customerBL = new CustomerBL();
//         // private OrderBL orderBL = new OrderBL();
//         private Staff? OrderStaff = null;
//         public int MenuHandle(string? title, string? subTitle, string[] menuItem)
//         {

//             ConsoleKeyInfo keyInfo;
//             string iconBackhand = "👉";
//             bool activeSelectedMenu = true;
//             int currentChoice = 1;
//             while (activeSelectedMenu)
//             {
//                 ConsoleUlts.Title(title, subTitle);
//                 if (currentChoice <= (menuItem.Count() + 1) && currentChoice >= 1)
//                 {
//                     for (int i = 0; i < menuItem.Count(); i++)
//                         Console.WriteLine(((currentChoice - 1 == i) ? (iconBackhand + " ") : "") + " " + menuItem[i] + $" ({i + 1})");
//                     Console.WriteLine("\n================================================================================================");
//                     Console.WriteLine("*press 'down arrow' to next choice or 'up arrow' to previous choice and press 'enter to confirm'");
//                     Console.WriteLine("================================================================================================");
//                     keyInfo = Console.ReadKey();

//                     if (keyInfo.Key == ConsoleKey.Enter)
//                     {
//                         Console.Clear();
//                         return currentChoice;
//                     }

//                     if (keyInfo.Key == ConsoleKey.DownArrow || keyInfo.Key == ConsoleKey.UpArrow || keyInfo.Key == ConsoleKey.Enter)
//                     {
//                         if (currentChoice >= 1 && currentChoice <= menuItem.Count())
//                             if (keyInfo.Key == ConsoleKey.DownArrow) currentChoice++;
//                             else if (keyInfo.Key == ConsoleKey.UpArrow) currentChoice--;

//                         if (currentChoice == (menuItem.Count() + 1)) currentChoice = 1;
//                         else if (currentChoice == 0) currentChoice = menuItem.Count();
//                         Console.Clear();
//                     }
//                     else
//                         ConsoleUlts.Alert(ConsoleEnum.Alert.Warning, "Please press 'down arrow' to next choice or 'up arrow' to previous choice and press 'enter to confirm'");
//                 }
//             }
//             return currentChoice;
//         }
//         public int StartMenu()
//         {
//             Console.OutputEncoding = System.Text.Encoding.UTF8;
//             string[] menuItem = { "Login", "Exit" };

//             int choice = MenuHandle(@"
//     ██████╗ ██╗  ██╗ ██████╗ ███╗   ██╗███████╗    ███████╗████████╗ ██████╗ ██████╗ ███████╗
//     ██╔══██╗██║  ██║██╔═══██╗████╗  ██║██╔════╝    ██╔════╝╚══██╔══╝██╔═══██╗██╔══██╗██╔════╝
//     ██████╔╝███████║██║   ██║██╔██╗ ██║█████╗      ███████╗   ██║   ██║   ██║██████╔╝█████╗  
//     ██╔═══╝ ██╔══██║██║   ██║██║╚██╗██║██╔══╝      ╚════██║   ██║   ██║   ██║██╔══██╗██╔══╝  
//     ██║     ██║  ██║╚██████╔╝██║ ╚████║███████╗    ███████║   ██║   ╚██████╔╝██║  ██║███████╗
//     ╚═╝     ╚═╝  ╚═╝ ╚═════╝ ╚═╝  ╚═══╝╚══════╝    ╚══════╝   ╚═╝    ╚═════╝ ╚═╝  ╚═╝╚══════╝
//                                                                                              "
//             , null, menuItem);

//             if (choice == 1) return 1;
//             else if (choice == 2) return 2;
//             else return -1;
//         }
                     
//              public bool? ListObjectPagination(List<object> listObject)
//              {
//                  if (listObject != null)
//                  {
//                      bool active = true;
//                      bool validInput = false;
//                      Dictionary<int, List<object>> objects = new Dictionary<int, List<object>>();
//                      objects = MenuPaginationHandle(listObject);
//                      int countPage = objects.Count(), currentPage = 1;
//                      ConsoleKeyInfo input = new ConsoleKeyInfo();
//                      ConsoleKeyInfo input2 = new ConsoleKeyInfo();
//                      if(listObject is List<Phone>){
//                      while (true)
//                      {
//                          ConsoleUlts.Title(
//                              null,
//          @"
//           █████╗ ██████╗ ██████╗     ████████╗ ██████╗      ██████╗ ██████╗ ██████╗ ███████╗██████╗ 
//          ██╔══██╗██╔══██╗██╔══██╗    ╚══██╔══╝██╔═══██╗    ██╔═══██╗██╔══██╗██╔══██╗██╔════╝██╔══██╗
//          ███████║██║  ██║██║  ██║       ██║   ██║   ██║    ██║   ██║██████╔╝██║  ██║█████╗  ██████╔╝
//          ██╔══██║██║  ██║██║  ██║       ██║   ██║   ██║    ██║   ██║██╔══██╗██║  ██║██╔══╝  ██╔══██╗
//          ██║  ██║██████╔╝██████╔╝       ██║   ╚██████╔╝    ╚██████╔╝██║  ██║██████╔╝███████╗██║  ██║
//          ╚═╝  ╚═╝╚═════╝ ╚═════╝        ╚═╝    ╚═════╝      ╚═════╝ ╚═╝  ╚═╝╚═════╝ ╚══════╝╚═╝  ╚═╝
//                                                                                                  "
//                          );
//                          while (active)
//                          {
//                              Console.WriteLine("========================================================================================================================");
//                              Console.WriteLine("| {0, 10} | {1, 30} | {2, 15} | {3, 15} | {4, 15} | {5, 15}  |", "ID", "Phone Name", "Brand", "Price", "OS", "Quantity");
//                              Console.WriteLine("========================================================================================================================");
//                              foreach (Phone phone in objects[currentPage])
//                              {
//                                  ConsoleUlts.PrintPhoneInfo(phone);
//                              }
//                              Console.WriteLine("========================================================================================================================");
//                              Console.WriteLine("{0,55}" + "< " + $"{currentPage}/{countPage}" + " >", " ");
//                              Console.WriteLine("========================================================================================================================");
//                              Console.WriteLine("Press 'Left Arrow' To Back Previous Page, 'Right Arror' To Next Page");
//                              Console.Write("Press 'Space' To View Phone Details, 'B' To Back Previous Menu");
//                              input = Console.ReadKey();
//                              if (currentPage <= countPage)
//                              {
//                                  if (input.Key == ConsoleKey.RightArrow)
//                                  {
//                                      if (currentPage <= countPage - 1) currentPage++;
//                                      Console.Clear();
//                                  }
//                                  else if (input.Key == ConsoleKey.LeftArrow)
//                                  {
//                                      if (currentPage > 1) currentPage--;
//                                      Console.Clear();
//                                  }

//                                  else if (input.Key == ConsoleKey.B)
//                                  {
//                                      return null;
//                                  }

//                                  else if (input.Key == ConsoleKey.Spacebar)
//                                  {

//                                      Console.Clear();
//                                      Console.WriteLine("========================================================================================================================");
//                                      Console.WriteLine("| {0, 10} | {1, 30} | {2, 15} | {3, 15} | {4, 15} | {5, 15}  |", "ID", "Phone Name", "Brand", "Price", "OS", "Quantity");
//                                      Console.WriteLine("========================================================================================================================");

//                                      foreach (Phone phone in objects[currentPage])
//                                      {
//                                          ConsoleUlts.PrintPhoneInfo(phone);
//                                      }
//                                      Console.WriteLine("========================================================================================================================");
//                                      Console.WriteLine("{0,55}" + "< " + $"{currentPage}/{countPage}" + " >", " ");
//                                      Console.WriteLine("========================================================================================================================");
//                                      do
//                                      {
//                                          validInput = false;
//                                          Console.WriteLine("Continue to select phone to view details or change the phone list page ? Press Y to continue or press N to switch pages(Y / N):");
//                                          input2 = Console.ReadKey(true);
//                                          if (input2.Key == ConsoleKey.Y)
//                                          {
//                                              validInput = true;
//                                              active = false;
//                                              return true;
//                                          }
//                                          else if (input2.Key == ConsoleKey.N)
//                                          {
//                                              validInput = true;
//                                              Console.Clear();
//                                          }
//                                          else
//                                          {
//                                              ConsoleUlts.Alert(E.Feature.Alert.Error, "Invalid Input (Y/N)");
//                                          }
//                                      } while (!validInput);
//                                  }
//                                  else
//                                  {
//                                      Console.Clear();
//                                  }
//                              }
//                          }
//                      }
//                      }
//                      else if(listObject is List<Order>){
//                         while (true)
//                      {
//                          ConsoleUlts.Title(
//                              null,
//                              @"
//           █████╗ ██╗     ██╗          ██████╗ ██████╗ ██████╗ ███████╗██████╗ ███████╗
//          ██╔══██╗██║     ██║         ██╔═══██╗██╔══██╗██╔══██╗██╔════╝██╔══██╗██╔════╝
//          ███████║██║     ██║         ██║   ██║██████╔╝██║  ██║█████╗  ██████╔╝███████╗
//          ██╔══██║██║     ██║         ██║   ██║██╔══██╗██║  ██║██╔══╝  ██╔══██╗╚════██║
//          ██║  ██║███████╗███████╗    ╚██████╔╝██║  ██║██████╔╝███████╗██║  ██║███████║
//          ╚═╝  ╚═╝╚══════╝╚══════╝     ╚═════╝ ╚═╝  ╚═╝╚═════╝ ╚══════╝╚═╝  ╚═╝╚══════╝
//                                                                                      "
//                          );
//                          while (active)
//                          {
//                              Console.WriteLine("========================================================================================================================");
//                              Console.WriteLine("| {0, 10} | {1, 30} | {2, 20} | {3, 15} |", "ID", "Customer Name", "Order Date", "Status");
//                              Console.WriteLine("========================================================================================================================");
//                              foreach (Order order in objects[currentPage])
//                              {
//                                  ConsoleUlts.PrintOrderInfo(order);
//                              }
//                              Console.WriteLine("========================================================================================================================");
//                              Console.WriteLine("{0,55}" + "< " + $"{currentPage}/{countPage}" + " >", " ");
//                              Console.WriteLine("========================================================================================================================");
//                              Console.WriteLine("Press 'Left Arrow' To Back Previous Page, 'Right Arrow' To Next Page");
//                              Console.Write("Press 'Space' To View Order Details, 'B' To Back Previous Menu");
//                              input = Console.ReadKey();
//                              if (input.Key == ConsoleKey.RightArrow)
//                              {
//                                  if (currentPage <= countPage - 1) currentPage++;
//                                  Console.Clear();
//                              }
//                              else if (input.Key == ConsoleKey.LeftArrow)
//                              {
//                                  if (currentPage > 1) currentPage--;
//                                  Console.Clear();
//                              }

//                              else if (input.Key == ConsoleKey.B)
//                              {
//                                  return null;
//                              }
//                              else if (input.Key == ConsoleKey.Spacebar)
//                              {
//                                  return true;
//                              }
//                              else
//                                  Console.Clear();

//                          }
//                      }
//                      }
//                  }
//                  else
//                  {
//                      ConsoleUlts.Alert(E.Feature.Alert.Warning, "Doesnt have any object to set Page");
//                      PressEnterTo("Back To Previous Menu");
//                  }
//                  return false;
//              }
             
//              public Dictionary<int, List<object>> MenuPaginationHandle(List<object> objectList)
//              {
//                  List<object> sList = new List<object>();
//                  Dictionary<int, List<object>> menuTab = new Dictionary<int, List<object>>();
//                  int objectQuantity = objectList.Count(), itemInTab = 4, numberOfTab = 0, count = 1, secondCount = 1, idTab = 0;

//                  if (objectQuantity % itemInTab != 0) numberOfTab = (objectQuantity / itemInTab) + 1;
//                  else numberOfTab = objectQuantity / itemInTab;

//                  foreach (var item in objectList)
//                  {
//                      if ((count - 1) == itemInTab)
//                      {
//                          sList = new List<object>();
//                          count = 1;
//                      }
//                      sList.Add(item);
//                      if (sList.Count() == itemInTab)
//                      {
//                          idTab++;
//                          menuTab.Add(idTab, sList);
//                      }
//                      else if (sList.Count() < itemInTab && secondCount == objectQuantity)
//                      {
//                          idTab++;
//                          menuTab.Add(idTab, sList);
//                      }
//                      secondCount++;
//                      count++;
//                  }
//                  return menuTab;
//              }
             
//         public StaffEnum.Role? LoginUlt()
//         {
//             Console.OutputEncoding = System.Text.Encoding.UTF8;
//             ConsoleUlts.Title(null, @"
//                             ██╗      ██████╗  ██████╗ ██╗███╗   ██╗
//                             ██║     ██╔═══██╗██╔════╝ ██║████╗  ██║
//                             ██║     ██║   ██║██║  ███╗██║██╔██╗ ██║
//                             ██║     ██║   ██║██║   ██║██║██║╚██╗██║
//                             ███████╗╚██████╔╝╚██████╔╝██║██║ ╚████║
//                             ╚══════╝ ╚═════╝  ╚═════╝ ╚═╝╚═╝  ╚═══╝");
//             Console.Write("\n👉 User Name: ");
//             string userName = Console.ReadLine() ?? "";
//             Console.Write("\n👉 Password: ");
//             string pass = "";
//             ConsoleKeyInfo key;
//             do
//             {
//                 key = Console.ReadKey(true);
//                 if (key.Key != ConsoleKey.Backspace)
//                 {
//                     pass += key.KeyChar;
//                     if (key.Key != ConsoleKey.Enter) Console.Write("*");
//                 }
//                 else if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
//                 {
//                     // Xóa ký tự cuối cùng trong chuỗi pass khi người dùng nhấn phím Backspace
//                     pass = pass.Substring(0, pass.Length - 1);
//                     Console.Write("\b \b");
//                 }
//             }
//             while (key.Key != ConsoleKey.Enter);
//             pass = pass.Substring(0, pass.Length - 1);
//             OrderStaff = StaffBL.Authenticate(userName, pass);
//             if (OrderStaff != null)
//                 return OrderStaff.Role;
//             else return null;
//         }
//         public void PressEnterTo(string? action)
//         {
//             if (action != null)
//             {
//                 Console.Write($"\n👉 Press Enter To {action}...");
//             }
//             ConsoleKeyInfo key = Console.ReadKey();
//             if (key.Key == ConsoleKey.Enter)
//             {
//                 Console.Clear();
//                 return;
//             }
//             else PressEnterTo(null);
//         }
//         public int SellerMenu()
//         {
//             int result = 0;
//             bool active = true;
//             string[] menuItem = { "Create Order", "Handle Order", "Log Out" };
//             while (active)
//             {
//                 switch (MenuHandle(
//                     null, @"
//                             ███████╗███████╗██╗     ██╗     ███████╗██████╗ 
//                             ██╔════╝██╔════╝██║     ██║     ██╔════╝██╔══██╗
//                             ███████╗█████╗  ██║     ██║     █████╗  ██████╔╝
//                             ╚════██║██╔══╝  ██║     ██║     ██╔══╝  ██╔══██╗
//                             ███████║███████╗███████╗███████╗███████╗██║  ██║
//                             ╚══════╝╚══════╝╚══════╝╚══════╝╚══════╝╚═╝  ╚═╝
//                                                 ", menuItem))
//                 {
//                     case 1:

//                         break;
//                     case 2:
//                         break;
//                     case 3:
//                         active = false; result = 1;
//                         break;
//                     default:
//                         break;
//                 }
//             }
//             return result;
//         }

//         public int AccountantMenu()
//         {
//             int result = 0;
//             bool active = true;
//             Ultilities ultilities = new Ultilities();
//             string[] menuItem = { "Payment", "Revenue Report", "Log Out" };
//             while (active)
//             {
//                 switch (ultilities.MenuHandle(null,
//                 @"
//          █████╗  ██████╗ ██████╗ ██████╗ ██╗   ██╗███╗   ██╗████████╗ █████╗ ███╗   ██╗████████╗
//         ██╔══██╗██╔════╝██╔════╝██╔═══██╗██║   ██║████╗  ██║╚══██╔══╝██╔══██╗████╗  ██║╚══██╔══╝
//         ███████║██║     ██║     ██║   ██║██║   ██║██╔██╗ ██║   ██║   ███████║██╔██╗ ██║   ██║   
//         ██╔══██║██║     ██║     ██║   ██║██║   ██║██║╚██╗██║   ██║   ██╔══██║██║╚██╗██║   ██║   
//         ██║  ██║╚██████╗╚██████╗╚██████╔╝╚██████╔╝██║ ╚████║   ██║   ██║  ██║██║ ╚████║   ██║   
//         ╚═╝  ╚═╝ ╚═════╝ ╚═════╝ ╚═════╝  ╚═════╝ ╚═╝  ╚═══╝   ╚═╝   ╚═╝  ╚═╝╚═╝  ╚═══╝   ╚═╝   
//                                                                                         ", menuItem))
//                 {
//                     case 1:
//                         break;
//                     case 2:
//                         break;
//                     case 3:
//                         active = false;
//                         result = 1;
//                         break;
//                     default:
//                         break;
//                 }
//             }
//             return result;
//         }
//     }
// }