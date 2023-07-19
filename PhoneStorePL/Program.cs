using System;
using System.Collections.Generic;
// using Persistence;
using Ults;
// using BL;
// using DAL;


namespace PhoneStoreUI
{
    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Ultilities Ults = new Ultilities();
            string[] items = { "select 1", "select 2", "select 3" };
            Console.WriteLine("Choice = " + Ults.MenuHandle("big title", "small title", items));
        }
    }
}


