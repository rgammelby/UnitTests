
using UnitTests.Bank;
using UnitTests.Customers;
using UnitTests.Interfaces;

namespace UnitTests.Factories
{
    public class BankAccountFactory : IBankFactory
    {
        public BankAccount Create()
        {
            return new BankAccount();
        }
    }
}
