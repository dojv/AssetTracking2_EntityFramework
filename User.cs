using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetTracking2_EF
{
    public class User
    {
        public List<Office> Offices = new List<Office>(); //holds all offices, which holds all products.
        //local values due to PDF. limiting the user to theese choises for types and brands (and nr of offices)
        public string[] ProductTypes = new string[] { "Laptop", "Mobile" };
        public string[] LaptopBrands = new string[] { "MacBook", "ASUS", "Lenovo" };
        public string[] MobileBrands = new string[] { "Iphone", "Samsung", "Nokia" };

        public Context DB = new Context();
        public IDictionary<string, double> Currencies = new Dictionary<string, double>() 
        {
            {"SEK", 11.27}, {"DKK", 7.63}, {"EUR", 1.03}, 
        };
        public void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        public void Success(string message)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
            Console.Write("(press enter to continue) ");
            Console.ReadLine();
        }
        public void Stop()
        {
            Console.Write("(press enter to continue) ");
            Console.ReadLine();
        }
        public void PrintColoredDate(DateTime date)
        {
            TimeSpan atLeast33MonthsOld = DateTime.Today.AddMonths(33) - DateTime.Today; //evaluates to 33 months old
            TimeSpan atLeast30MonthsOld = DateTime.Today.AddMonths(30) - DateTime.Today;
            TimeSpan productAge = (date - DateTime.Today) * -1; //*-1 so the age is positive (because date is in the past)

            if (productAge > atLeast33MonthsOld)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(date.ToString("dd-MM-yyyy".PadRight(15)));
                Console.ResetColor();
            }
            else if (productAge > atLeast30MonthsOld)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(date.ToString("dd-MM-yyyy".PadRight(15)));
                Console.ResetColor();
            }
            else
            {
                Console.Write(date.ToString("dd-MM-yyyy".PadRight(15)));
            }
        }
    }
}
