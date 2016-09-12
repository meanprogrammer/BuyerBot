using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BuyerRobot
{
    class Program
    {
        static void Main(string[] args)
        {
            CartHandler handler = new CartHandler();
            handler.AddToCart();
            Console.ReadLine();
        }
    }
}
