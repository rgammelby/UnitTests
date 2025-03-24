using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTests.Customers;
using UnitTests.Bank;
using System.Security.Cryptography;
namespace UnitTests
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Environment.GetEnvironmentVariable("JWT_SECRET"));
        }
    }
}
