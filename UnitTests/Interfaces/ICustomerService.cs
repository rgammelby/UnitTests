using UnitTests.Customers;

namespace UnitTests.Interfaces
{
    public interface ICustomerService
    {
        public Customer Create(string json);
        public Customer GetCustomerProfile(string userId);
    }
}
