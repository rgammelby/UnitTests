using UnitTests.Customers;
using UnitTests.Interfaces;

namespace UnitTests.Factories
{
    public class CustomerFactory //: ICustomerFactory
    {
        public Customer Create(string customerName = "Test user", string email = "test@mail.com", string password = "1234")  // , string email = "test@test.com"
        {
            return new Customer(customerName, email, password);
        }
    }
}
