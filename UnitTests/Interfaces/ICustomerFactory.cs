using UnitTests.Customers;

namespace UnitTests.Interfaces
{
    public interface ICustomerFactory
    {
        public Customer Create(string name);
    }
}
