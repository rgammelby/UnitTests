using UnitTests.Customers;

namespace UnitTests.Interfaces
{
    public interface ICustomerRepository
    {
        public Customer Create(Customer c);
    }
}
